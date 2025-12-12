using System;

namespace LivreNoirLibrary.Media
{
    public readonly struct PointerBitmap(nint pointer, int width, int height, bool isFloat) : IBitmap
    {
        public nint Pointer { get; } = pointer;
        public int Width { get; } = width;
        public int Height { get; } = height;
        public bool IsFloat { get; } = isFloat;
    }
}
