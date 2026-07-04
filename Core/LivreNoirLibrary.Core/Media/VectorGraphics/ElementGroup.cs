using System;

namespace LivreNoirLibrary.Media.VectorGraphics
{
    public class ElementGroup(GeometryElement[] children)
    {
        public ElementGroup(params ReadOnlySpan<GeometryElement> children) : this(children.ToArray()) { }

        public GeometryElement[] Children { get; } = children;

        public ReadOnlySpan<GeometryElement> AsSpan() => Children.AsSpan();
    }
}
