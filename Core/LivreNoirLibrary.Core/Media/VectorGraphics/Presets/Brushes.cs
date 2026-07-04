using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.VectorGraphics
{
    public static class Brushes
    {
        private static readonly Dictionary<string, SingleColorBrush> _brushes = [];
        private static SingleColorBrush CreateBrush(string color) => new(color);

        public static SingleColorBrush Get(string color) => _brushes.GetOrAdd(color, CreateBrush);
    }
}
