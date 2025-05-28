using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ImageSelectionRect : FrameworkElement
    {
        public const int MaxPattern = 8;
        public const int AnimationInterval = 80;
        public const double CornerRadius = 5;

        public event ValueChangedEventHandler<Int32Rect>? RectChanged;

        private static readonly Int32Animation _animation = new()
        {
            From = 0,
            To = MaxPattern,
            Duration = TimeSpan.FromMilliseconds(AnimationInterval * MaxPattern),
            RepeatBehavior = RepeatBehavior.Forever,
        };
        private static readonly DrawingBrush[] _brushes = CreateBrushes();
        private static readonly Pen? _pen = MediaUtils.GetPen(Brushes.Black, 1);

        [DependencyProperty(BindsTwoWayByDefault = true)]
        private int _originalWidth;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private int _originalHeight;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private int _left;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private int _top;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private int _right;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private int _bottom;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private double _scale;
        [DependencyProperty(AffectsRender = true)]
        private int _pattern;

        private bool _needCoerce = true;

        public ImageSelectionRect()
        {
            BeginAnimation(PatternProperty, _animation);
        }

        private int CoerceLeft(int value) => Math.Clamp(value, 0, _needCoerce ? _right : _originalWidth);
        private int CoerceRight(int value) => Math.Clamp(value, _needCoerce ? _left : 0, _originalWidth);
        private int CoerceTop(int value) => Math.Clamp(value, 0, _needCoerce ? _bottom : _originalHeight);
        private int CoerceBottom(int value) => Math.Clamp(value, _needCoerce ? _top : 0, _originalHeight);

        public Int32Rect GetRect() => new(_left, _top, _right - _left, _bottom - _top);

        private void NotifyRectChanged()
        {
            if (_needCoerce)
            {
                RectChanged?.Invoke(this, GetRect());
            }
        }

        public void SetRect(Int32Rect rect)
        {
            _needCoerce = false;
            Left = rect.X;
            Top = rect.Y;
            Right = _left + rect.Width;
            Bottom = _top + rect.Height;
            _needCoerce = true;
            NotifyRectChanged();
        }

        private void OnLeftChanged(int value) => NotifyRectChanged();
        private void OnTopChanged(int value) => NotifyRectChanged();
        private void OnRightChanged(int value) => NotifyRectChanged();
        private void OnBottomChanged(int value) => NotifyRectChanged();

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
            var left = _left * scale;
            var top = _top * scale;
            var right = _right * scale;
            var bottom = _bottom * scale;
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

            drawingContext.DrawRectangle(horz, null, new(left, top, w, 1));
            drawingContext.DrawRectangle(vert, null, new(left, top, 1, h));
            drawingContext.DrawRectangle(horz, null, new(left, bottom - 1, w, 1));
            drawingContext.DrawRectangle(vert, null, new(right - 1, top, 1, h));

            var brush = Brushes.White;
            drawingContext.DrawEllipse(brush, pen, new(left, top), CornerRadius, CornerRadius);
            drawingContext.DrawEllipse(brush, pen, new(left, bottom), CornerRadius, CornerRadius);
            drawingContext.DrawEllipse(brush, pen, new(right, top), CornerRadius, CornerRadius);
            drawingContext.DrawEllipse(brush, pen, new(right, bottom), CornerRadius, CornerRadius);
        }

        public int GetCornerIndex(Point point)
        {
            var index = 0;
            var x = point.X;
            var y = point.Y;
            var (left, top, right, bottom) = GetDoubleValues();
            if (x >= left - CornerRadius && x < right + CornerRadius && y >= top - CornerRadius && y < bottom + CornerRadius)
            {
                index++;
                if (x >= left + CornerRadius)
                {
                    index++;
                    if (x >= right - CornerRadius)
                    {
                        index++;
                    }
                }
                if (y < bottom - CornerRadius)
                {
                    index += 3;
                    if (y < top + CornerRadius)
                    {
                        index += 3;
                    }
                }
            }
            return index;
        }

        public void OffsetHorizontal(int offset)
        {
            _needCoerce = false;
            Left = _left + offset;
            Right = _right + offset;
            _needCoerce = true;
            NotifyRectChanged();
        } 

        public void OffsetVertical(int offset)
        {
            _needCoerce = false;
            Top = _top + offset;
            Bottom = _bottom + offset;
            _needCoerce = true;
            NotifyRectChanged();
        }

        public void SetMovingHorizontal(int left, int right)
        {
            _needCoerce = false;
            (Left, Right) = (left > right) ? (right, left) : (left, right);
            _needCoerce = true;
            NotifyRectChanged();
        }

        public void SetMovingVertical(int top, int bottom)
        {
            _needCoerce = false;
            (Top, Bottom) = (top > bottom) ? (bottom, top) : (top, bottom);
            _needCoerce = true;
            NotifyRectChanged();
        }
    }
}
