using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ColorPalette: FrameworkElement
    {
        public const double DefaultCellSize = 16;

        [DependencyProperty(AffectsRender = true)]
        private Brush? _background;
        [DependencyProperty(AffectsArrange = true)]
        private IEnumerable<Color>? _colors;
        [DependencyProperty(AffectsArrange = true)]
        private int _columns = 16;

        private int _count;
        private INotifyCollectionChanged? _collection;
        private readonly List<Rect> _rects = [];
        private readonly List<double> _xList = [];
        private readonly List<double> _yList = [];

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
                    fw = cellSize * cols;
                    fh = cellSize * rows;
                }
                else
                {
                    fw = h / rows * cols;
                }
            }
            else if (h_infinite)
            {
                fh = w / cols * rows;
            }

            // 描画位置リストの更新
            Func<double, double> round = UseLayoutRounding ? Math.Round : ReturnSelf;
            var rects = _rects;
            var xs = _xList;
            var ys = _yList;
            rects.Clear();
            xs.Clear();
            ys.Clear();
            xs.Add(0);
            ys.Add(0);
            var top = 0d;
            for (var y = 0; y < rows; y++)
            {
                var bottom = round(fh * (y + 1) / rows);
                ys.Add(bottom);
                var left = 0d;
                for (var x = 0; x < cols; x++)
                {
                    var right = round(fw * (x + 1) / cols);
                    if (y is 0)
                    {
                        xs.Add(right);
                    }
                    rects.Add(new(left, top, right - left, bottom - top));
                    left = right;
                }
                top = bottom;
            }
            InvalidateVisual();

            return (RenderSize = new(fw, fh));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (Background is { } brush)
            {
                drawingContext.DrawRectangle(brush, null, new(new(0, 0), RenderSize));
            }
            if (Colors is not { } colors)
            {
                return;
            }
            var index = 0;
            var rects = _rects;
            foreach (var color in colors)
            {
                drawingContext.DrawRectangle(Media.MediaUtils.GetBrush(color), null, rects[index]);
                index++;
            }
        }

        private static double ReturnSelf(double v) => v;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(this);
            if (!e.Handled  && TryGetColorAt(pos.X, pos.Y, out var index, out var color))
            {
                RaiseClick(index, color, e.ChangedButton, e.ClickCount);
                e.Handled = true;
            }
            else
            {
                base.OnMouseDown(e);
            }
        }

        public bool TryGetColorAt(double x, double y, out int index, out Color color)
        {
            var ret = false;
            index = -1;
            color = default;
            if (_xList.TrySearch(x, SearchMode.PreviousOrEqual, out var col, out _) &&
                _yList.TrySearch(y, SearchMode.PreviousOrEqual, out var row, out _))
            {
                index = row * Columns + col;
                switch (Colors)
                {
                    case IList<Color> list:
                        if ((uint)index < (uint)list.Count)
                        {
                            ret = true;
                            color = list[index];
                        }
                        break;
                    case IReadOnlyList<Color> list:
                        if ((uint)index < (uint)list.Count)
                        {
                            ret = true;
                            color = list[index];
                        }
                        break;
                    case IEnumerable<Color> enumer:
                        foreach (var c in enumer)
                        {
                            if (index is 0)
                            {
                                ret = true;
                                color = c;
                                break;
                            }
                            index--;
                        }
                        break;
                }
            }
            return ret;
        }
    }
}
