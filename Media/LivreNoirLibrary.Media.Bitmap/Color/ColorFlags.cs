using System;

namespace LivreNoirLibrary.Media
{
    [Flags]
    public enum ColorFlags : byte
    {
        None = 0,
        A = 1,
        R = 2,
        G = 4,
        B = 8,

        RGB = R | G | B,
        All = A | RGB,
    }
}
