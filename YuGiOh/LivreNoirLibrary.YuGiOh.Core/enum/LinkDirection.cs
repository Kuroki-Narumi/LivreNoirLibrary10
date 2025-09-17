using System;

namespace LivreNoirLibrary.YuGiOh
{
    [Flags]
    public enum LinkDirection
    {
        LowerLeft  = 0b0000_0001,
        Lower      = 0b0000_0010,
        LowerRight = 0b0000_0100,

        Left       = 0b0000_1000,
        Right      = 0b0001_0000,

        UpperLeft  = 0b0010_0000,
        Upper      = 0b0100_0000,
        UpperRight = 0b1000_0000,
    }
}
