using System;

namespace LivreNoirLibrary.Media.VectorGraphics
{
    public class GradientBrush(GradientType type, (double, double) origin, GradientStop[] stops) : IBrush
    {
        public GradientType Type { get; } = type;
        public (double X, double Y) Origin { get; } = origin;
        public GradientStop[] Stops { get; } = stops;
    }
}
