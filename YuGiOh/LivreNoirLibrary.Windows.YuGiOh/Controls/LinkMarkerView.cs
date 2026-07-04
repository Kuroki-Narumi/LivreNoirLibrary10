using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.YuGiOh;
using System;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    using LivreNoirLibrary.YuGiOh.Media;
    using System.Collections.Generic;
    using System.Windows.Input;

    public partial class LinkMarkerView : FrameworkElement
    {
        public const double DefaultStrokeThickness = 1.0;
        public static readonly SolidColorBrush DefaultCheckedFill = MediaUtils.GetBrush(Icons.Link_On_Fill.Color);
        public static readonly SolidColorBrush DefaultCheckedStroke = MediaUtils.GetBrush(Icons.Link_On_Stroke.Color);
        public static readonly SolidColorBrush DefaultFill = MediaUtils.GetBrush(Icons.Link_Off_Fill.Color);
        public static readonly SolidColorBrush DefaultStroke = MediaUtils.GetBrush(Icons.Link_Off_Stroke.Color);

        static LinkMarkerView()
        {
            IsEnabledProperty.OverrideMetadata(typeof(LinkMarkerView), PropertyUtils.GetMeta(true, OnIsEnabledChanged));
        }

        static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LinkMarkerView)?.InvalidateVisual();
        }

        static readonly Dictionary<int, LinkDirection> _dir2dir = new()
        {
            [1] = LinkDirection.LowerLeft,
            [2] = LinkDirection.Lower,
            [3] = LinkDirection.LowerRight,
            [4] = LinkDirection.Left,
            [6] = LinkDirection.Right,
            [7] = LinkDirection.UpperLeft,
            [8] = LinkDirection.Upper,
            [9] = LinkDirection.UpperRight,
        };

        [DependencyProperty(AffectsRender = true)]
        private double _strokeThickness = DefaultStrokeThickness;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _checkedFill = DefaultCheckedFill;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _checkedStroke = DefaultCheckedStroke;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _fill = DefaultFill;
        [DependencyProperty(AffectsRender = true)]
        private Brush? _stroke = DefaultStroke;
        [DependencyProperty(AffectsRender = true)]
        private LinkDirection _direction;
        [DependencyProperty]
        private bool _isReadOnly;

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            _geometries.Clear();
        }

        private readonly Dictionary<LinkDirection, StreamGeometry> _geometries = [];

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var width = ActualWidth;
            var height = ActualHeight;
            drawingContext.DrawRectangle(Brushes.Transparent, null, new(0, 0, width, height));
            var th = StrokeThickness;
            var checkedFill = CheckedFill;
            var checkedStroke = MediaUtils.GetPen(CheckedStroke, th);
            var fill = Fill;
            var stroke = MediaUtils.GetPen(Stroke, th);
            var current = Direction;
            var gs = _geometries;
            if (gs.Count is 0)
            {
                foreach (var (d, g) in new LinkArrowEnumerator(width, height))
                {
                    gs[d] = MediaUtils.CreateGeometry(g);
                }
            }
            foreach (var (d, geometry) in gs)
            {
                var (brush, pen) = (d & current) switch
                {
                    0 => (fill, stroke),
                    _ => (checkedFill, checkedStroke),
                };
                drawingContext.DrawGeometry(brush, pen, geometry);
            }
        }

        private bool TryGetDirection(MouseEventArgs e, out LinkDirection direction)
        {
            if (!IsReadOnly)
            {
                var pos = e.GetPosition(this);
                var dx = ActualWidth / 5;
                var dy = ActualHeight / 5;
                var dir = 7;
                if (pos.X > dx * 2) dir += 1;
                if (pos.X >= dx * 3) dir += 1;
                if (pos.Y > dy * 2) dir -= 3;
                if (pos.Y >= dy * 3) dir -= 3;
                return _dir2dir.TryGetValue(dir, out direction);
            }
            direction = 0;
            return false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!IsReadOnly)
            {
                var on = e.LeftButton is MouseButtonState.Pressed;
                var off = e.RightButton is MouseButtonState.Pressed;
                if ((on ^ off) && TryGetDirection(e, out var direction))
                {
                    if (on)
                    {
                        Direction |= direction;
                    }
                    else
                    {
                        Direction &= ~direction;
                    }
                }
            }
            base.OnMouseMove(e);
        }
    }
}
