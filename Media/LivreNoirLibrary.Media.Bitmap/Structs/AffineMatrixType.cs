using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media
{
    [Flags]
    public enum AffineMatrixType : byte
    {
        Identity = 0,
        Offset = 1,
        Scale = 2,
        OffsetAndScale = Offset | Scale,
        Unknown = 4,
    }
}
