using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class IntRectPresenter : FrameworkElement
    {
        public const int MaxPattern = 8;
        public const int AnimationInterval = 80;
        public const double DefaultCornerRadius = 5;

        private static readonly Int32Animation _animation = new()
        {
            From = 0,
            To = MaxPattern,
            Duration = TimeSpan.FromMilliseconds(AnimationInterval * MaxPattern),
            RepeatBehavior = RepeatBehavior.Forever,
        };
        private static readonly DrawingBrush[] _brushes = CreateBrushes();
        private static readonly Pen? _pen = MediaUtils.GetPen(Brushes.Black, 1);

        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private int _left;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private int _top;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private int _right;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private int _bottom;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private double _cornerRadius = DefaultCornerRadius;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private double _scale;
        [DependencyProperty(AffectsRender = true)]
        private int _pattern;

        public IntRectPresenter()
        {
            BeginAnimation(PatternProperty, _animation);
        }

        public void SetRect(Int32Rect rect) => SetRect(rect.X, rect.Y, rect.Width, rect.Height);
        public void SetRect(int x, int y, int width, int height)
        {
            Left = x;
            Top = y;
            Right = x + width;
            Bottom = y + height;
        }

        private static GeometryDrawing GetGD(Brush brush, int x, int y, int w, int h) => MediaUtils.Freeze(new GeometryDrawing()
        {
            Brush = brush,
            Geometry = MediaUtils.CreateRectGeometry(new(x, y, w, h)),
        });

        private static DrawingBrush[] CreateBrushes()
        {
            var ary = new DrawingBrush[MaxPattern * 2];

            var color1 = Brushes.Black;
            var color2 = Brushes.White;

            for (int i = 0, j = 0; i < MaxPattern; i++)
            {
                // horizontal
                DrawingGroup group = new();
                var ch = group.Children;
                ch.Add(GetGD(color1, 0, 0, 8, 1));
                if (i is <= 4)
                {
                    ch.Add(GetGD(color2, i, 0, 4, 1));
                }
                else
                {
                    ch.Add(GetGD(color2, 0, 0, i - 4, 1));
                    ch.Add(GetGD(color2, i, 0, 8 - i, 1));
                }
                group.Freeze();
                ary[j++] = MediaUtils.Freeze(new DrawingBrush(group)
                {
                    Viewport = new(0, 0, 8, 1),
                    TileMode = TileMode.Tile,
                    ViewportUnits = BrushMappingMode.Absolute,
                });

                // vertical
                group = new();
                ch = group.Children;
                ch.Add(GetGD(color1, 0, 0, 1, 8));
                if (i is <= 4)
                {
                    ch.Add(GetGD(color2, 0, i, 1, 4));
                }
                else
                {
                    ch.Add(GetGD(color2, 0, 0, 1, i - 4));
                    ch.Add(GetGD(color2, 0, i, 1, 8 - i));
                }
                group.Freeze();
                ary[j++] = MediaUtils.Freeze(new DrawingBrush(group)
                {
                    Viewport = new(0, 0, 1, 8),
                    TileMode = TileMode.Tile,
                    ViewportUnits = BrushMappingMode.Absolute,
                });
            }

            return ary;
        }

        private (double, double, double, double) GetDoubleValues()
        {
            var scale = _scale;
            var left = Math.Floor(_left * scale);
            var top = Math.Floor(_top * scale);
            var right = Math.Ceiling(_right * scale);
            var bottom = Math.Ceiling(_bottom * scale);
            return (left, top, right, bottom);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var index = (_pattern % MaxPattern) * 2;
            var horz = _brushes[index];
            var vert = _brushes[index + 1];
            var (left, top, right, bottom) = GetDoubleValues();
            var w = right - left;
            var h = bottom - top;
            var pen = _pen;

            if (w is > 0)
            {
                drawingContext.DrawRectangle(horz, null, new(left, top, w, 1));
                drawingContext.DrawRectangle(horz, null, new(left, bottom - 1, w, 1));
            }
            if (h is > 0)
            {
                drawingContext.DrawRectangle(vert, null, new(left, top, 1, h));
                drawingContext.DrawRectangle(vert, null, new(right - 1, top, 1, h));
            }

            var brush = Brushes.White;
            var r = _cornerRadius;
            drawingContext.DrawEllipse(brush, pen, new(left, top), r, r);
            drawingContext.DrawEllipse(brush, pen, new(left, bottom), r, r);
            drawingContext.DrawEllipse(brush, pen, new(right, top), r, r);
            drawingContext.DrawEllipse(brush, pen, new(right, bottom), r, r);
        }

        public int GetCornerIndex(Point point)
        {
            var index = 0;
            var x = point.X;
            var y = point.Y;
            var r = _cornerRadius;
            var (left, top, right, bottom) = GetDoubleValues();
            if (x >= left - r && x < right + r && y >= top - r && y < bottom + r)
            {
                index++;
                if (x >= left + r)
                {
                    index++;
                    if (x >= right - r)
                    {
                        index++;
                    }
                }
                if (y < bottom - r)
                {
                    index += 3;
                    if (y < top + r)
                    {
                        index += 3;
                    }
                }
            }
            return index;
        }
    }
}
