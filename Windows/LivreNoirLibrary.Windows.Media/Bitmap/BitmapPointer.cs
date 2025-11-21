using System;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public readonly unsafe struct BitmapPointer : IDisposable
    {
        private readonly WriteableBitmap _bitmap;
        private readonly byte* _pointer;
        private readonly int _spanLength;
        private readonly int _width;
        private readonly int _height;

        public bool IsValid => _pointer is not null;

        public BitmapPointer(WriteableBitmap bitmap)
        {
            _bitmap = bitmap;
            _pointer = (byte*)bitmap.BackBuffer;
            _width = bitmap.PixelWidth;
            _height = bitmap.PixelHeight;
            _spanLength = bitmap.BackBufferStride * bitmap.PixelHeight;
            bitmap.Lock();
        }

        public void Dispose()
        {
            _bitmap.AddDirtyRect(_bitmap.GetRect());
            _bitmap.Unlock();
        }

        public Span<byte> AsSpan() => new(_pointer, _spanLength);
        public LnBitmapData ToBitmapData() => new(_pointer, _width, _height);
        public static implicit operator LnBitmapData(BitmapPointer value) => value.ToBitmapData();
    }
}
