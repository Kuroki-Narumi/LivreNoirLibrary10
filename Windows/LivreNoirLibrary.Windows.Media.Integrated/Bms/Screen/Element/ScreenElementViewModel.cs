using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class ScreenElementViewModel : ObservableObjectBase, IClear
    {
        public double DestX { get; set => SetValue(ref field, value); }
        public double DestY { get; set => SetValue(ref field, value); }
        public double DestWidth { get; set => SetValue(ref field, value); }
        public double DestHeight { get; set => SetValue(ref field, value); }
        public Visibility Visibility { get; set => SetValue(ref field, value); }
        public double Opacity { get; set => SetValue(ref field, value); }
        public Point RotateOrigin { get; set => SetValue(ref field, value); }
        public RotateTransform Rotation { get; } = new();

        private TimerId _timerId;
        private long _loopStart;
        private long _loopEnd;
        private long _loopInterval;
        private readonly LongTimeline<double> _slopes = [];
        private readonly DestinationTimeline _dest_x = new(0);
        private readonly DestinationTimeline _dest_y = new(0);
        private readonly DestinationTimeline _dest_w = new(0);
        private readonly DestinationTimeline _dest_h = new(0);
        private readonly DestinationTimeline _dest_opacity = new(1);
        private readonly DestinationTimeline _dest_ox = new(0);
        private readonly DestinationTimeline _dest_oy = new(0);
        private readonly DestinationTimeline _dest_angle = new(0);
        private readonly DestinationTimeline _dest_aox = new(0.5);
        private readonly DestinationTimeline _dest_aoy = new(0.5);

        public void Clear()
        {
            _slopes.Clear();
            _dest_x.Clear();
            _dest_y.Clear();
            _dest_w.Clear();
            _dest_h.Clear();
            _dest_opacity.Clear();
            _dest_ox.Clear();
            _dest_oy.Clear();
            _dest_angle.Clear();
            _dest_aox.Clear();
            _dest_aoy.Clear();
            Visibility = Visibility.Collapsed;
        }

        public void LoadDestination(Skin skin, IVariableProvider? provider, SkinElement element)
        {
            Clear();
            double Resolve(ValueExpression? expr, double defualtValue = 0)
            {
                return skin.TryResolveValue<double>(expr, provider, out var v) ? v : defualtValue;
            }
            void SetIfNotNull(DestinationTimeline timeline, long tick, ValueExpression? expr)
            {
                if (skin.TryResolveValue<double>(expr, provider, out var v))
                {
                    timeline.Set(tick, v);
                }
            }

            var slopes = _slopes;
            var x = _dest_x;
            var y = _dest_y;
            var w = _dest_w;
            var h = _dest_h;
            var opacity = _dest_opacity;
            var ox = _dest_ox;
            var oy = _dest_oy;
            var angle = _dest_angle;
            var aox = _dest_aox;
            var aoy = _dest_aoy;
            var end = Resolve(element.DestLoopEnd);
            var needUpdateEnd = end is > 0;
            var loopEnd = needUpdateEnd ? TimeUtils.Seconds2Ticks(end) : 0;
            foreach (var child in element.Children.AsSpan())
            {
                if (child is Destination destination)
                {
                    var tick = TimeUtils.Seconds2Ticks(Resolve(destination.Time));
                    slopes.Set(tick, Resolve(destination.Slope, 1));
                    SetIfNotNull(x, tick, destination.X);
                    SetIfNotNull(y, tick, destination.Y);
                    SetIfNotNull(w, tick, destination.Width);
                    SetIfNotNull(h, tick, destination.Height);
                    SetIfNotNull(opacity, tick, destination.Opacity);
                    SetIfNotNull(ox, tick, destination.OriginX);
                    SetIfNotNull(oy, tick, destination.OriginY);
                    SetIfNotNull(angle, tick, destination.Angle);
                    SetIfNotNull(aox, tick, destination.RotateOriginX);
                    SetIfNotNull(aoy, tick, destination.RotateOriginY);
                    if (needUpdateEnd)
                    {
                        loopEnd = Math.Max(tick, loopEnd);
                    }
                }
            }
            _timerId = element.DestTimer;
            _loopEnd = loopEnd;
            _loopInterval = loopEnd - (_loopStart = TimeUtils.Seconds2Ticks(Resolve(element.DestLoopStart)));
        }

        public void SetBinding(FrameworkElement element)
        {
            element.DataContext = this;
            element.SetBinding(Canvas.LeftProperty, nameof(DestX));
            element.SetBinding(Canvas.TopProperty, nameof(DestY));
            element.SetBinding(FrameworkElement.WidthProperty, nameof(DestWidth));
            element.SetBinding(FrameworkElement.HeightProperty, nameof(DestHeight));
            element.SetBinding(UIElement.VisibilityProperty, nameof(Visibility));
            element.SetBinding(UIElement.OpacityProperty, nameof(Opacity));
            element.SetBinding(UIElement.RenderTransformOriginProperty, nameof(RotateOrigin));
            element.RenderTransform = Rotation;
        }

        public bool Update(BmsTimer timer, long absoluteTick)
        {
            if (timer.TryGet(_timerId, absoluteTick, out var relativeTick) && relativeTick is >= 0)
            {
                var loopStart = _loopStart;
                var loopEnd = _loopEnd;
                var interval = _loopInterval;
                if (relativeTick > loopEnd && interval is > 0)
                {
                    if (loopStart is < 0)
                    {
                        Visibility = Visibility.Collapsed;
                        return false;
                    }
                    relativeTick = (loopEnd - loopStart) % interval + loopStart;
                }
                if (_slopes.TryGetValue(relativeTick, SearchMode.PreviousOrEqual, out _, out var slope))
                {
                    var x = _dest_x.Get(relativeTick, slope);
                    var y = _dest_y.Get(relativeTick, slope);
                    var w = _dest_w.Get(relativeTick, slope);
                    var h = _dest_h.Get(relativeTick, slope);
                    var ox = _dest_ox.Get(relativeTick, slope);
                    var oy = _dest_oy.Get(relativeTick, slope);
                    x -= w * ox;
                    y -= h * oy;
                    var op = _dest_opacity.Get(relativeTick, slope);
                    var a = _dest_angle.Get(relativeTick, slope);
                    var aox = _dest_aox.Get(relativeTick, slope);
                    var aoy = _dest_aoy.Get(relativeTick, slope);
                    Point origin = new(aox, aoy);
                    var changed = 
                        DestX != x || DestY != y || 
                        DestWidth != w || DestHeight != h || 
                        Opacity != op || 
                        RotateOrigin != origin || Rotation.Angle != a || 
                        Visibility is Visibility.Collapsed;
                    DestX = x;
                    DestY = y;
                    DestWidth = w;
                    DestHeight = h;
                    Opacity = op;
                    Rotation.Angle = a;
                    RotateOrigin = origin;
                    Visibility = Visibility.Visible;
                    return changed;
                }
            }
            Visibility = Visibility.Collapsed;
            return false;
        }
    }
}
