using System;

namespace LivreNoirLibrary.Media.VectorGraphics
{
    public class GeometryElement(string geometry, IBrush? fill, Pen? pen = null)
    {
        public string Geometry { get; } = geometry;
        public IBrush? Fill { get; } = fill;
        public Pen? Pen { get; } = pen;
    }
}
