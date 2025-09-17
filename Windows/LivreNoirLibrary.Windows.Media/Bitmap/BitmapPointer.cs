using LivreNoirLibrary.Collections;
using System;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public readonly unsafe struct BitmapPointer : IDisposable
    {
        private readonly WriteableBitmap _bitmap;
        private readonly byte* _buffer;
        private readonly int _bpp;
        private readonly int _stride;
        private readonly int _span_size;

        public byte* Pointer => _buffer;
        public uint* UIntPointer => (uint*)_buffer;
        public int BufferLength => _span_size;

        public uint this[int x, int y]
        {
            get => *(uint*)PointerTo(x, y);
            set => *(uint*)PointerTo(x, y) = value;
        }

        public byte this[int x, int y, ColorIndex color]
        {
            get => PointerTo(x, y)[(int)color];
            set => PointerTo(x, y)[(int)color] = value;
        }

        public BitmapPointer(WriteableBitmap bitmap)
        {
            _bitmap = bitmap;
            _buffer = (byte*)bitmap.BackBuffer;
            _bpp = bitmap.Format.BitsPerPixel / 8;
            _stride = bitmap.BackBufferStride;
            _span_size = _stride * bitmap.PixelHeight;
            bitmap.Lock();
        }

        public void Clear() => SimdOperations.Clear(_buffer, _span_size);

        public void Dispose()
        {
            _bitmap.AddDirtyRect();
            _bitmap.Unlock();
        }

        public byte* PointerTo(int y) => _buffer + _stride * y;
        public byte* PointerTo(int x, int y) => _buffer + _stride * y + _bpp * x;

        public Span<byte> AsSpan() => new(_buffer, _span_size);
        public Span<byte> AsSpan(int y) => new(_buffer + _stride * y, _stride);
        public Span<byte> AsSpan(int y, int height) => new(_buffer + _stride * y, _stride * height);
        public Span<byte> AsSpan(int y, int x, int width) => new(_buffer + _stride * y + x * _bpp, width * _bpp);

        public Span<uint> AsUIntSpan() => new(_buffer, _span_size / _bpp);
        public Span<uint> AsUIntSpan(int y) => new(_buffer + _stride * y, _stride / _bpp);
        public Span<uint> AsUIntSpan(int y, int height) => new(_buffer + _stride * y, _stride * height / _bpp);
        public Span<uint> AsUIntSpan(int y, int x, int width) => new(_buffer + _stride * y + x * _bpp, width);

        public static implicit operator Span<byte>(BitmapPointer ptr) => ptr.AsSpan();
    }
}
