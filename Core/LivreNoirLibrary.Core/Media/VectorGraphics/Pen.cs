using System;

namespace LivreNoirLibrary.Media.VectorGraphics
{
    public class Pen(IBrush brush, double thickness)
    {
        public IBrush Brush { get; } = brush;
        public double Thickness { get; } = thickness;
    }
}
