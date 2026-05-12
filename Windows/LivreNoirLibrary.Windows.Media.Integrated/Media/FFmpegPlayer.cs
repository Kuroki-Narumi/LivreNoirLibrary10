using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class FFmpegPlayer : FrameworkElement, IVideoCreator<FFmpegPlayer.SaveState>, IPlayer
    {
        private VideoCache? _video;
        private LivreNoirLibrary.Media.Wave.AudioFileReader? _audio;
        private WriteableBitmap _bitmap = Bitmap.Create(1, 1);
        private Rect _bitmapRect;
        private WaveOutEvent? _waveOut;

        private bool _needUpdateVideo;
        private bool _notNeedSeek;

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

        private void OnPathChanged(string? value)
        {
            this.Stop();
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
                        var w = _video.Width;
                        var h = _video.Height;
                        Width = w;
                        Height = h;
                        if (_bitmap.PixelWidth != w || _bitmap.PixelHeight != h)
                        {
                            _bitmap = Bitmap.Create(w, h);
                            _bitmapRect = new Rect(0, 0, w, h);
                        }
                        Duration = _video.Duration;
                        FrameRate = _video.FrameRate;
                        _needUpdateVideo = true;
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

        private void OnPositionChanged()
        {
            _needUpdateVideo = true;
            PositionTime = TimeSpan.FromSeconds(Position);
            if (!_notNeedSeek && IsPlaying)
            {
                IsPlaying = false;
                IsPlaying = true;
            }
        }

        private void OnDurationChanged()
        {
            DurationTime = TimeSpan.FromSeconds(Duration);
        }

        private bool CoerceIsPlaying(bool value) => value && IsPlayable;

        private void OnIsPlayingChanged(bool value)
        {
            if (value)
            {
                if (_audio is { } audio)
                {
                    audio.SeekByTicks(TimeUtils.Seconds2Ticks(Position));
                    _waveOut = new();
                    _waveOut.Init(audio);
                }
                _startTime = Stopwatch.GetTimestamp() - TimeUtils.Seconds2Ticks(Position);
                CompositionTarget.Rendering += RTP_Update;
                _waveOut?.Play();
            }
            else
            {
                CompositionTarget.Rendering -= RTP_Update;
                _waveOut?.Dispose();
                _waveOut = null;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            EnsureRender();
            base.OnRender(drawingContext);
            drawingContext.DrawImage(_bitmap, _bitmapRect);
        }

        private void EnsureRender()
        {
            if (_needUpdateVideo)
            {
                using var p = _bitmap.BeginWrite();
                if (_video is { } video)
                {
                    video.GetBitmap(Position, p.AsByteSpan());
                }
                else
                {
                    p.Clear();
                }
                _needUpdateVideo = false;
            }
        }

        public void CopyPixels(Span<byte> destination, int destWidth)
        {
            EnsureRender();
            using var p = _bitmap.BeginRead();
            p.CopyTo(destination, destWidth);
        }

        private long _startTime;

        private void RTP_Update(object? sender, EventArgs e)
        {
            var time = Stopwatch.GetElapsedTime(_startTime).TotalSeconds;
            if (time >= Duration)
            {
                time = Duration;
                IsPlaying = false;
            }
            _notNeedSeek = true;
            Position = time;
            _notNeedSeek = false;
        }

        public SaveState CreateSaveState(ref AntiFreezeUpdater f, ProgressReporter? p, CancellationToken c)
        {
            this.Stop();
            var pos = Position;
            int width, height, bitrate;
            if (_video is { } video)
            {
                width = video.Width;
                height = video.Height;
                bitrate = (int)video.Bitrate;
            }
            else
            {
                width = 240;
                height = 240;
                bitrate = 1000;
            }

            int rate, ch;
            if (_audio is { } audio)
            {
                rate = audio.WaveFormat.SampleRate;
                ch = audio.WaveFormat.Channels;
                audio.SeekByTicks(TimeUtils.Seconds2Ticks(pos));
            }
            else
            {
                rate = 44100;
                ch = 2;
            }

            var duration = Math.Min(Duration - pos, SaveDuration);
            return new()
            {
                PixelWidth = width,
                PixelHeight = height,
                AudioExists = _audio is not null,
                AudioSampleRate = rate,
                AudioChannels = ch,
                FrameRate = FrameRate,
                StartOffset = pos,
                TotalTime = duration,
                AbortDeadline = duration,
                VideoBitrate = bitrate,
                AudioBitrate = 224000,
            };
        }

        public void UpdateSaveState(SaveState state, double time)
        {
            Position = time + state.StartOffset;
        }

        public void ReadSamples(Span<float> buffer)
        {
            _audio?.ReadToSpan(buffer);
        }

        public class SaveState : VideoSaveState
        {
            public required double StartOffset { get; init; }
        }
    }
}
