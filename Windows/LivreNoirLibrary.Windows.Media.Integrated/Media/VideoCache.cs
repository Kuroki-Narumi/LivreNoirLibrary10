using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.ObjectModel;
using System;

namespace LivreNoirLibrary.Windows.Media
{
    public unsafe class VideoCache : DisposableBase
    {
        private VideoDecoder _decoder;
        private bool _needInitialize;
        private bool _canRead;
        private double _lastPosition;
        private double _nextPosition;

        public int Width { get; }
        public int Height { get; }

        public VideoCache(string path)
        {
            var decoder = new VideoDecoder(path);
            _decoder = decoder;
            var width = decoder.OutputWidth;
            var height = decoder.OutputHeight;
            Width = width;
            Height = height;
            Rewind();
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            _decoder.Dispose();
            _decoder = null!;
            _canRead = false;
        }

        private void Rewind()
        {
            _decoder?.SeekByTick(0);
            _needInitialize = true;
            _canRead = _decoder is not null;
            _lastPosition = 0;
            _nextPosition = 0;
        }

        public void GetBitmap(double time, Span<byte> span)
        {
            if (time < _lastPosition)
            {
                Rewind();
            }
            if (_canRead)
            {
                while (_needInitialize || (_nextPosition < time))
                {
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
