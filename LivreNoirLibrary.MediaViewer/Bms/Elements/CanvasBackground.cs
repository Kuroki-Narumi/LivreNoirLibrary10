using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class CanvasBackground : FrameworkElement
    {
        [DependencyProperty(AffectsRender = true)]
        private Color _borderColor = Colors.LaneBorder;

        private readonly List<(double X, double Width, SolidColorBrush? Brush)> _children = [];

        internal void ClearChildren() => _children.Clear();
        internal void AddBorder(double x) => _children.Add((x, 1, null));
        internal void AddChild(double x, double width, Color color) => _children.Add((x, width, MediaUtils.GetBrush(color)));

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var h = ActualHeight;
            var borderBrush = MediaUtils.GetBrush(_borderColor);
            foreach (var child in CollectionsMarshal.AsSpan(_children))
            {
                var brush = child.Brush ?? borderBrush;
                drawingContext.DrawRectangle(brush, null, new(child.X, 0, child.Width, h));
            }
        }
    }
}
