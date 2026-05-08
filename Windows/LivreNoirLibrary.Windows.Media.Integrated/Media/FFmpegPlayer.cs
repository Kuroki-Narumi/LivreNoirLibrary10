using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Media;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls
{
    public unsafe partial class FFmpegPlayer : FrameworkElement, IVideoCreator<VideoSaveState>
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
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _isPlaying;

        private void OnPathChanged(string? value)
        {
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
                Stop();
                Play();
            }
        }

        private void OnDurationChanged()
        {
            DurationTime = TimeSpan.FromSeconds(Duration);
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
            fixed (byte* buffer = destination)
            {
                _bitmap.CopyPixels(new(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight), (nint)buffer, destination.Length, destWidth * 4);
            }
        }

        private long _startTime;

        public void Play()
        {
            if (_audio is { } audio)
            {
                audio.SeekByTicks(TimeUtils.Seconds2Ticks(Position));
                _waveOut = new();
                _waveOut.Init(audio);
            }
            IsPlaying = true;
            _startTime = Stopwatch.GetTimestamp() - TimeUtils.Seconds2Ticks(Position);
            CompositionTarget.Rendering += RTP_Update;
            _waveOut?.Play();
        }

        public void Stop()
        {
            IsPlaying = false;
            CompositionTarget.Rendering -= RTP_Update;
            _waveOut?.Dispose();
            _waveOut = null;
        }

        private void RTP_Update(object? sender, EventArgs e)
        {
            var time = Stopwatch.GetElapsedTime(_startTime).TotalSeconds;
            if (time >= Duration)
            {
                time = Duration;
                Stop();
            }
            _notNeedSeek = true;
            Position = time;
            _notNeedSeek = false;
        }

        public VideoSaveState CreateSaveState(ref AntiFreezeUpdater f, ProgressReporter? p, CancellationToken c)
        {
            const int kbps = 10000;
            Position = 0;

            int width, height;
            if (_video is { } video)
            {
                width = video.Width;
                height = video.Height;
            }
            else
            {
                width = 240;
                height = 240;
            }

            int rate, ch;
            if (_audio is { } audio)
            {
                rate = audio.WaveFormat.SampleRate;
                ch = audio.WaveFormat.Channels;
                audio.SeekByTicks(0);
            }
            else
            {
                rate = 44100;
                ch = 2;
            }

            var duration = Duration;
            var fps = FrameRate;
            return new()
            {
                PixelWidth = width,
                PixelHeight = height,
                AudioExists = _audio is not null,
                AudioSampleRate = rate,
                AudioChannels = ch,
                FrameRate = fps,
                TotalTime = duration,
                AbortDeadline = duration,
                ApproximateKbps = kbps,
            };
        }

        public void UpdateSaveState(VideoSaveState state, double time)
        {
            Position = time;
        }

        public void ReadSamples(Span<float> buffer)
        {
            _audio?.ReadToSpan(buffer);
        }
    }
}
