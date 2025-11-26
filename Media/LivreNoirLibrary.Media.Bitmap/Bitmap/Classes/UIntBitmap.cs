using LivreNoirLibrary.Collections;
using System;

namespace LivreNoirLibrary.Media
{
    public class UIntBitmap : UnmanagedBitmap<uint>
    {
        public UIntBitmap(int width, int height) : base(width, height) { }
        public UIntBitmap(UnmanagedArray<uint>? buffer, int width, int height, bool clear = true) : base(buffer, width, height, clear) { }

        public unsafe Span<TElement> AsSpan<TElement>() where TElement : unmanaged => new((void*)Pointer, Width * Height * sizeof(uint) / sizeof(TElement));
    }
}
