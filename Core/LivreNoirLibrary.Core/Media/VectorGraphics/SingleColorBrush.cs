using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.VectorGraphics
{
    public class SingleColorBrush(string color) : IBrush
    {
        public string Color { get; } = color;
    }
}
