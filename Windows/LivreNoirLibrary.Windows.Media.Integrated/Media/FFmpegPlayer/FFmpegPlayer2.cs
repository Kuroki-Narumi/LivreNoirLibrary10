using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media;
using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class FFmpegPlayer2 : FrameworkElement, IPlayer
    {
        private VideoDecoder? _video;
        private LivreNoirLibrary.Media.Wave.AudioFileReader? _audio;

        private WriteableBitmap _bitmap = Bitmap.Create(1, 1);
        private Rect _bitmapRect;

        private readonly UnmanagedArray<byte> _buffer = new();
        private readonly VideoFrameQueue _videoQueue = new();
        private long _videoTopTick = -1;
        private bool _isSeekReserved;
        private CancellationTokenSource? _updateCanceller;
        private Task? _updateTask;
        private WaveOutEvent? _waveOut;

        private bool _updating;
        private long _playStartingTick;

        [DependencyProperty]
        private string? _path;
        [DependencyProperty(SetterScope = Scope.Private)]
        private string? _fileName;
        [DependencyProperty(AffectsRender = true)]
        private double _position;
        [DependencyProperty(SetterScope = Scope.Private)]
        private double _duration;
        [DependencyProperty(SetterScope = Scope.Private)]
        private Rational _frameRate;
        [DependencyProperty(SetterScope = Scope.Private)]
        private double _secondsPerFrame;
        [DependencyProperty(SetterScope = Scope.Private)]
        private TimeSpan _positionTime;
        [DependencyProperty(SetterScope = Scope.Private)]
        private TimeSpan _durationTime;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _isPlayable;
        [DependencyProperty]
        private bool _isPlaying;
        [DependencyProperty]
        private double _saveDuration;

        public long CurrentTick => IsPlaying ? Stopwatch.GetTimestamp() - _playStartingTick : TimeUtils.Seconds2Ticks(Position);

        public FFmpegPlayer2()
        {
            Dispatcher.BeginInvoke(() => CompositionTarget.Rendering += BaseUpdate);
        }

        private void OnPathChanged(string? value)
        {
            this.Stop();
            FinishVideoUpdater();
            _video?.Dispose();
            _video = null;
            _audio?.Dispose();
            _audio = null;
            Duration = 0;
            if (System.IO.File.Exists(value))
            {
                try
                {
                    using var file = FormatContext.OpenRead(value);
                    if (file.ContainsAudio())
                    {
                        _audio = new(value);
                    }
                    if (file.ContainsVideo())
                    {
                        _video = new(value);
                        var w = _video.OutputWidth;
                        var h = _video.OutputHeight;
                        Width = w;
                        Height = h;
                        if (_bitmap.PixelWidth != w || _bitmap.PixelHeight != h)
                        {
                            _bitmap = Bitmap.Create(w, h);
                            _bitmapRect = new Rect(0, 0, w, h);
                        }
                        _buffer.EnsureSize(w * h * 4, true);
                        Duration = (double)_video.Duration;
                        FrameRate = _video.FrameRate;
                        StartVideoUpdater();
                    }
                    else
                    {
                        FrameRate = 60;
                        if (_audio is { }  audio)
                        {
                            Duration = audio.TotalSeconds;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }
            Position = 0;
            SecondsPerFrame = 1.0 / FrameRate;
            IsPlayable = Duration is > 0;
            FileName = System.IO.Path.GetFileName(value);
        }

        private void OnPositionChanged(double oldValue, double newValue)
        {
            var playing = !_updating && IsPlaying;
            if (playing)
            {
                this.Stop();
            }
            if (newValue < oldValue)
            {
                _isSeekReserved = true;
                SeekIfNeed();
            }
            PositionTime = TimeSpan.FromSeconds(newValue);
            if (playing)
            {
                this.Play();
            }
        }

        private void OnDurationChanged(double value)
        {
            DurationTime = TimeSpan.FromSeconds(value);
        }

        private bool CoerceIsPlaying(bool value) => value && IsPlayable;

        private void OnIsPlayingChanged(bool value)
        {
            if (value && IsPlayable)
            {
                if (_audio is { } audio)
                {
                    audio.SeekByTicks(TimeUtils.Seconds2Ticks(Position));
                    _waveOut = new();
                    _waveOut.Init(audio);
                }
                _playStartingTick = Stopwatch.GetTimestamp() - TimeUtils.Seconds2Ticks(Position);
                _waveOut?.Play();
            }
            else
            {
                _waveOut?.Dispose();
                _waveOut = null;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawImage(_bitmap, _bitmapRect);
        }

        public void CopyPixels(Span<byte> destination, int destWidth)
        {
            using var p = _bitmap.BeginRead();
            p.CopyTo(destination, destWidth);
        }

        private void BaseUpdate(object? sender, EventArgs e)
        {
            if (!IsPlayable)
            {
                return;
            }
            var tick = CurrentTick;
            var time = TimeUtils.Ticks2Seconds(tick);
            if (IsPlaying)
            {
                _updating = true;
                if (time >= Duration)
                {
                    this.Stop();
                    Position = Duration;
                }
                else
                {
                    Position = time;
                }
                _updating = false;
            }
            VideoFrameBacket? backet = null;
            while (_videoTopTick < tick && _videoQueue.TryDequeue(out var b))
            {
                _videoTopTick = b.Tick;
                backet = b;
            }
            backet?.CopyPixels(_bitmap);
        }

        private void FinishVideoUpdater()
        {
            Task_FinishVideoUpdater().Wait();
            _videoQueue.Clear();
        }

        private async Task Task_FinishVideoUpdater()
        {
            if (_updateCanceller is { } ca)
            {
                ca.Cancel();
                if (_updateTask is { } task)
                {
                    try
                    {
                        await task.ConfigureAwait(false);
                    }
                    catch (OperationCanceledException) { }
                }
                ca.Dispose();
                _updateCanceller = null;
                _updateTask = null;
            }
        }

        private void StartVideoUpdater()
        {
            _updateCanceller = new();
            var c = _updateCanceller.Token;
            Task.Run(async () =>
            {
                if (_updateTask is not null)
                {
                    await _updateTask;
                }
                _updateTask = UpdateVideo(c);
            }, c);
        }

        private void SeekIfNeed()
        {
            if (_isSeekReserved)
            {
                _isSeekReserved = false;
                _video?.SeekByTick(CurrentTick);
                _videoQueue.Clear();
                _videoTopTick = -1;
            }
        }

        private async Task UpdateVideo(CancellationToken c)
        {
            if (_video is VideoDecoder decoder)
            {
                var queue = _videoQueue;
                var tpf = TimeSpan.FromSeconds(SecondsPerFrame / 2);
                try
                {
                    while (!c.IsCancellationRequested)
                    {
                        SeekIfNeed();
                        c.ThrowIfCancellationRequested();

                        if (queue.Count == queue.Capacity)
                        {
                            await Task.Delay(tpf, c);
                            c.ThrowIfCancellationRequested();
                            continue;
                        }
                        c.ThrowIfCancellationRequested();

                        var tick = CurrentTick;
                        var span = _buffer.AsSpan();
                        if (decoder.GetFrame(span, out var pts, out _))
                        {
                            var input = new VideoFrameInfo(span, pts);
                            if (input.Tick < tick - decoder.MaxKeyframeInterval)
                            {
                                decoder.SeekByTick(tick);
                            }
                            else
                            {
                                queue.Enqueue(input);
                            }
                            c.ThrowIfCancellationRequested();
                        }
                        c.ThrowIfCancellationRequested();

                        if (!decoder.DecodeFrame(tick.ToRational()))
                        {
                            // end of stream
                            break;
                        }
                        c.ThrowIfCancellationRequested();
                    }
                }
                catch (Exception e)
                {
                    if (e is not (OperationCanceledException or TaskCanceledException))
                    {
                        throw;
                    }
                }
            }
            _updateTask = null;
        }
    }
}
