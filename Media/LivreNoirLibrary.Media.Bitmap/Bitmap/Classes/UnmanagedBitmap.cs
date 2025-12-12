using System;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media
{
    public abstract unsafe class UnmanagedBitmap<T> : DisposableBase, IBitmap
        where T : unmanaged
    {
        private readonly UnmanagedArray<T> _buffer;
        private readonly bool _needDispose;

        public nint Pointer => (nint)_buffer.Pointer;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public abstract int ElementsPerPixel { get; }
        public abstract bool IsFloat{ get; }

        public UnmanagedBitmap(int width, int height)
        {
            _buffer = new();
            _needDispose = true;
            Resize(width, height, true);
        }

        public UnmanagedBitmap(UnmanagedArray<T>? buffer, int width, int height, bool clear = true)
        {
            _needDispose = buffer is null;
            _buffer = buffer ?? new();
            Resize(width, height, clear);
        }

        public void Resize(int width, int height, bool clear = true)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(width, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(height, 0);
            _buffer.EnsureSize(width * height * ElementsPerPixel, clear);
            Width = width;
            Height = height;
        }

        public override string ToString() => $"{this.GetTypeName()}{{0x{(nint)_buffer.Pointer:X16}:{Width}x{Height}}}";

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            if (_needDispose)
            {
                _buffer.Dispose();
            }
            Width = Height = 0;
        }
    }
}
