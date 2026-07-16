using System;

namespace LivreNoirLibrary.Media.VectorGraphics
{
    public enum PathCommand : byte
    {
        None,
        Moveto,
        Lineto,
        HorizontalLineto,
        VerticalLineto,
        CurveTo,
        SmoothCurveto,
        QuadraticBezier,
        SmoothQuadratic,
        EllipticalArc,
        Closepath,
    }
}
