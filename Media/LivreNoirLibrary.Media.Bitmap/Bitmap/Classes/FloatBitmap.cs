using LivreNoirLibrary.Collections;
using System;

namespace LivreNoirLibrary.Media
{
    public class FloatBitmap : UnmanagedBitmap<float>
    {
        public override int ElementsPerPixel => 4;
        public override bool IsFloat => true;

        public FloatBitmap(int width, int height) : base(width, height) { }
        public FloatBitmap(UnmanagedArray<float>? buffer, int width, int height, bool clear = true) : base(buffer, width, height, clear) { }

        public unsafe Span<TElement> AsSpan<TElement>() where TElement : unmanaged => new((void*)Pointer, Width * Height * sizeof(float) * 4 / sizeof(TElement));
    }
}
