using System;

namespace LivreNoirLibrary.Media.VectorGraphics
{
    public class GradientStop(double offset, string color)
    {
        public double Offset { get; } = offset;
        public string Color { get; } = color;
    }
}
