using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using DrRect = System.Drawing.Rectangle;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public abstract class ScreenElement(SkinElement source)
    {
        public SkinElement Source { get; } = source;
        public bool IsValid { get; protected set; } = true;
        public bool IsVisible { get; protected set; }
        public double DestX { get; protected set; }
        public double DestY { get; protected set; }
        public double DestWidth { get; protected set; }
        public double DestHeight { get; protected set; }
        public double OriginX { get; protected set; }
        public double OriginY { get; protected set; }
        public double Opacity
        {
            get;
            protected set
            {
                field = value;
                OpacityMask = new((float)value, 1, 1, 1);
            }
        }
        public FloatColor OpacityMask { get; protected set; }
        public double Angle { get; protected set; }
        public double RotateOriginX { get; protected set; }
        public double RotateOriginY { get; protected set; }
        public TimerId TimerId { get; protected set; }
        public double LoopStart { get; protected set; }
        public double LoopEnd { get; protected set; }
        public double LoopInterval { get; protected set; }
        public BlendMode BlendMode { get; protected set; }

        private readonly DoubleTimeline<double> _slopes = [];
        private readonly DestinationTimeline _dest_x = new(0);
        private readonly DestinationTimeline _dest_y = new(0);
        private readonly DestinationTimeline _dest_w = new(-1);
        private readonly DestinationTimeline _dest_h = new(-1);
        private readonly DestinationTimeline _dest_opacity = new(1);
        private readonly DestinationTimeline _dest_ox = new(0);
        private readonly DestinationTimeline _dest_oy = new(0);
        private readonly DestinationTimeline _dest_angle = new(0);
        private readonly DestinationTimeline _dest_aox = new(0.5);
        private readonly DestinationTimeline _dest_aoy = new(0.5);

        public bool IsConstantDestination { get; private set; }

        public void ClearTimeline()
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
        }

        public virtual void DetermineExpressions(Skin skin, IVariableProvider? provider)
        {
            ClearTimeline();

            double Resolve(ValueExpression? expr, double defaultValue = 0) => skin.ResolveValue(expr, provider, defaultValue);

            void SetIfNotNull(DestinationTimeline timeline, double time, ValueExpression? expr)
            {
                if (skin.TryResolveValue<double>(expr, provider, out var v))
                {
                    timeline.Set(time, v);
                }
            }

            var element = Source;
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
            var loopEnd = Resolve(element.DestLoopEnd);
            var needUpdateEnd = loopEnd is <= 0;
            foreach (var child in element.Children.AsSpan())
            {
                if (child is Destination destination)
                {
                    var time = Resolve(destination.Time);
                    slopes.Set(time, Resolve(destination.Slope, 1));
                    SetIfNotNull(x, time, destination.X);
                    SetIfNotNull(y, time, destination.Y);
                    SetIfNotNull(w, time, destination.Width);
                    SetIfNotNull(h, time, destination.Height);
                    SetIfNotNull(opacity, time, destination.Opacity);
                    SetIfNotNull(ox, time, destination.OriginX);
                    SetIfNotNull(oy, time, destination.OriginY);
                    SetIfNotNull(angle, time, destination.Angle);
                    SetIfNotNull(aox, time, destination.RotateOriginX);
                    SetIfNotNull(aoy, time, destination.RotateOriginY);
                    if (needUpdateEnd)
                    {
                        loopEnd = Math.Max(time, loopEnd);
                    }
                }
            }
            TimerId = element.DestTimer;
            LoopEnd = loopEnd;
            LoopStart = Resolve(element.DestLoopStart);
            LoopInterval = needUpdateEnd ? 0 : loopEnd - LoopStart;
            BlendMode = element.Blend;
            IsConstantDestination = slopes.Count is <= 1;
        }

        public virtual void Update(in UpdateArgs args)
        {
            if (args.Timer.TryGet(TimerId, args.AbsoluteTime, out var relativeTime) && relativeTime is >= 0)
            {
                var loopStart = LoopStart;
                var loopEnd = LoopEnd;
                var interval = LoopInterval;
                if (relativeTime > loopEnd)
                {
                    if (loopStart is < 0)
                    {
                        IsVisible = false;
                        return;
                    }
                    if (interval is > 0)
                    {
                        relativeTime = (relativeTime - loopStart) % interval + loopStart;
                    }
                }
                if (_slopes.TryGetValue(relativeTime, SearchMode.PreviousOrEqual, out _, out var slope))
                {
                    var x = _dest_x.GetBlended(relativeTime, slope);
                    var y = _dest_y.GetBlended(relativeTime, slope);
                    var w = _dest_w.GetBlended(relativeTime, slope);
                    var h = _dest_h.GetBlended(relativeTime, slope);
                    var ox = _dest_ox.GetBlended(relativeTime, slope);
                    var oy = _dest_oy.GetBlended(relativeTime, slope);
                    var op = _dest_opacity.GetBlended(relativeTime, slope);
                    var a = _dest_angle.GetBlended(relativeTime, slope);
                    var aox = _dest_aox.GetBlended(relativeTime, slope);
                    var aoy = _dest_aoy.GetBlended(relativeTime, slope);
                    DestX = x;
                    DestY = y;
                    DestWidth = w;
                    DestHeight = h;
                    OriginX = ox;
                    OriginY = oy;
                    Opacity = op;
                    Angle = a;
                    RotateOriginX = aox;
                    RotateOriginY = aoy;
                    IsVisible = op is > 0;
                    return;
                }
            }
            IsVisible = false;
        }

        public void Render(in RenderArgs args)
        {
            if (IsValid && IsVisible)
            {
                var t0 = System.Diagnostics.Stopwatch.GetTimestamp();
                RenderCore(args);
                var time = TimeUtils.Ticks2Milliseconds(System.Diagnostics.Stopwatch.GetTimestamp() - t0);
                args.TotalTimes?[this] += time;
            }
        }

        protected abstract void RenderCore(in RenderArgs args);

        public static void RenderSource(in RenderArgs args,
            IBitmap source, DrRect sourceRect, double destX, double destY, double destWidth, double destHeight,
            BlendMode blendMode, FloatColor colorCorrection)
        {
            var (target, buffer1, parentRect, _) = args;
            source.BlendWithScale(sourceRect, target, parentRect, new(destX + parentRect.X, destY + parentRect.Y, destWidth, destHeight), blendMode, colorCorrection, buffer1);
        }
    }
}
