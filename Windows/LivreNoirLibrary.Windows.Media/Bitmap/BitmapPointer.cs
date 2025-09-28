using System;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public readonly unsafe struct BitmapPointer : IDisposable
    {
        private readonly WriteableBitmap _bitmap;
        private readonly byte* Pointer;
        private readonly int SpanLength;
        private readonly int Width;
        private readonly int Height;

        public BitmapPointer(WriteableBitmap bitmap)
        {
            _bitmap = bitmap;
            Pointer = (byte*)bitmap.BackBuffer;
            Width = bitmap.PixelWidth;
            Height = bitmap.PixelHeight;
            SpanLength = bitmap.BackBufferStride * bitmap.PixelHeight;
            bitmap.Lock();
        }

        public void Dispose()
        {
            _bitmap.AddDirtyRect(_bitmap.GetRect());
            _bitmap.Unlock();
        }

        public Span<byte> AsSpan() => new(Pointer, SpanLength);
        public LnBitmapData ToBitmapData() => new(Pointer, Width, Height);
        public static implicit operator LnBitmapData(BitmapPointer value) => value.ToBitmapData();
    }
}
