using System;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Media
{
    public class RenderTextOption
    {
        public double X { get; set; }
        public double Y { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public bool UseDrawText { get; set; } = true;
        public Brush? Foreground { get; set; }
        public Pen? Stroke { get; set; }
    }
}
