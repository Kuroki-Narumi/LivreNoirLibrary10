using System;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public readonly unsafe struct BitmapPointer : IBitmap, IDisposable
    {
        private readonly WriteableBitmap _bitmap;
        private readonly bool _needFlush;

        public nint Pointer { get; }
        public int Width { get; }
        public int Height { get; }
        public bool IsFloat => false;

        internal BitmapPointer(WriteableBitmap bitmap, bool needFlush = true)
        {
            _bitmap = bitmap;
            _needFlush = needFlush;
            Pointer = bitmap.BackBuffer;
            Width = _bitmap.PixelWidth;
            Height = _bitmap.PixelHeight;
            bitmap.Lock();
        }

        public void Dispose()
        {
            if (_needFlush)
            {
                _bitmap.AddDirtyRect(_bitmap.Rect);
            }
            _bitmap.Unlock();
        }
    }
}
