using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ColorPalette: FrameworkElement
    {
        public const double DefaultCellSize = 16;

        [DependencyProperty(AffectsArrange = true)]
        private IEnumerable<Color>? _colors;
        [DependencyProperty(AffectsArrange = true)]
        private int _columns = 16;

        private int _count;
        private INotifyCollectionChanged? _collection;

        private void OnColorsChanged(IEnumerable<Color>? value)
        {
            _collection?.CollectionChanged -= OnCollectionChanged;
            _collection = value as INotifyCollectionChanged;
            _collection?.CollectionChanged += OnCollectionChanged;
            UpdateCount();
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateCount();
            InvalidateArrange();
        }

        private void UpdateCount()
        {
            _count = Colors switch
            {
                null => 0,
                ICollection<Color> col => col.Count,
                IReadOnlyCollection<Color> col => col.Count,
                _ => Colors.Count(),
            };
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var w = Width;
            var h = Height;
            var w_infinite = !double.IsFinite(w);
            var h_infinite = !double.IsFinite(h);
            var (fw, fh) = finalSize;
            var count = _count;
            var cols = Columns;
            var rows = (count + cols - 1) / cols;
            if (w_infinite)
            {
                if (h_infinite)
                {
                    var cellSize = Math.Min(fw / cols, fh / rows);
                    finalSize = new(cellSize * cols, cellSize * rows);
                }
                else
                {
                    finalSize.Width = h / rows * cols;
                }
            }
            else
            {
                if (h_infinite)
                {
                    finalSize.Height = w / cols * rows;
                }
            }
            RenderSize = finalSize;
            return finalSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (Colors is not { } colors)
            {
                return;
            }
            var i = 0;
            var count = _count;
            var cols = Columns;
            var rows = (count + cols - 1) / cols;
            var (w, h) = RenderSize;
            w /= cols;
            h /= rows;
            foreach (var color in colors)
            {
                var x = w * (i % cols);
                var y = h * (i / cols);
                drawingContext.DrawRectangle(Media.MediaUtils.GetBrush(color), null, new(x, y, Math.Ceiling(w), Math.Ceiling(h)));
                i++;
            }
        }
    }
}
