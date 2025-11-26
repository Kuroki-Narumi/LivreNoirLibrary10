using System;

namespace LivreNoirLibrary.Media
{
    public interface IBitmap
    {
        bool IsFloat { get; }

        nint Pointer { get; }
        int Width { get; }
        int Height { get; }
    }
}
