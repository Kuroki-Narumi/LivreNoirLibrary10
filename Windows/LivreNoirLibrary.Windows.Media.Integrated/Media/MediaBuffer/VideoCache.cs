using System;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.FFmpeg;

namespace LivreNoirLibrary.Windows.Media
{
    public sealed class VideoCache : MediaCache
    {
        private VideoDecoder _decoder;
        private WriteableBitmap _bitmap;
        private bool _needInitialize;
        private bool _canRead;
        private long _lastPosition;
        private long _nextPosition;

        public VideoCache(string path)
        {
            var decoder = new VideoDecoder(path);
            _decoder = decoder;
            _bitmap = Bitmap.Create(Math.Max(decoder.OutputWidth, 1), Math.Max(decoder.OutputHeight, 1));
            Rewind();
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            _decoder.Dispose();
            _decoder = null!;
            _bitmap = null!;
        }

        private void Rewind()
        {
            _decoder.SeekByTick(0);
            _bitmap.Clear();
            _needInitialize = true;
            _canRead = true;
            _lastPosition = 0;
            _nextPosition = 0;
        }

        public override WriteableBitmap? GetBitmap(long ticks)
        {
            if (ticks < _lastPosition)
            {
                Rewind();
            }
            if (_canRead)
            {
                using BitmapPointer pointer = new(_bitmap);
                while (_needInitialize || (_nextPosition < ticks))
                {
                    if (_decoder.ReadOneFrame(pointer.AsSpan(), out var pos, out var duration))
                    {
                        _needInitialize = false;
                        _lastPosition = pos.ToTicks();
                        _nextPosition = _lastPosition + duration.ToTicks();
                    }
                    else
                    {
                        _canRead = false;
                        break;
                    }
                }
            }
            return _bitmap;
        }
    }
}
