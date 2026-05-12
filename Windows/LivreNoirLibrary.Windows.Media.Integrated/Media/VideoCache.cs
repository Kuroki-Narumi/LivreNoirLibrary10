using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using System;

namespace LivreNoirLibrary.Windows.Media
{
    public class VideoCache : DisposableBase
    {
        private VideoDecoder _decoder;
        private bool _needInitialize;
        private bool _canRead;
        private double _seekThreshold;
        private double _lastPosition;
        private double _nextPosition;

        public int Width { get; }
        public int Height { get; }
        public double Duration { get; }
        public Rational FrameRate { get; }
        public long Bitrate { get; }

        public VideoCache(string path)
        {
            var decoder = new VideoDecoder(path);
            _decoder = decoder;
            Width = decoder.OutputWidth;
            Height = decoder.OutputHeight;
            Duration = decoder.TotalSeconds;
            FrameRate = decoder.FrameRate;
            Bitrate = decoder.Bitrate;
            _seekThreshold = TimeUtils.Ticks2Seconds(decoder.MaxKeyframeInterval);
            Seek(0);
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            _decoder.Dispose();
            _decoder = null!;
            _canRead = false;
        }

        public void Seek(double time)
        {
            _decoder?.SeekByTick(TimeUtils.Seconds2Ticks(time));
            _needInitialize = true;
            _canRead = _decoder is not null;
            _lastPosition = time;
            _nextPosition = time;
        }

        public void GetBitmap(double time, Span<byte> span)
        {
            if (time < _lastPosition)
            {
                Seek(time);
            }
            if (_canRead)
            {
                while (_needInitialize || (_nextPosition < time))
                {
                    if (time - _nextPosition > _seekThreshold)
                    {
                        _decoder.SeekByTick(TimeUtils.Seconds2Ticks(time));
                    }
                    if (_decoder.ReadOneFrame(span, out var pos, out var duration))
                    {
                        _needInitialize = false;
                        _lastPosition = (double)pos;
                        _nextPosition = _lastPosition + (double)duration;
                    }
                    else
                    {
                        _canRead = false;
                        break;
                    }
                }
            }
        }
    }
}
