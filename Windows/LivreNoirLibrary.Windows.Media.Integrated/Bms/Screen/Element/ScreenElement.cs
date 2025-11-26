using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using DrRect = System.Drawing.Rectangle;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public abstract class ScreenElement(SkinElement source)
    {
        public SkinElement Source { get; } = source;
        public bool IsValid { get; protected set; } = true;
        public bool IsVisible { get; protected set; }
        public double DestX { get; private set; }
        public double DestY { get; private set; }
        public double DestWidth { get; private set; }
        public double DestHeight { get; private set; }
        public double Opacity
        {
            get; 
            private set
            {
                field = value;
                _opacityMask = new FloatColor((float)value, 1, 1, 1);
            }
        }
        public double Angle { get; private set; }
        public double RotateOriginX { get; private set; }
        public double RotateOriginY { get; private set; }
        public TimerId TimerId { get; private set; }
        public double LoopStart { get; private set; }
        public double LoopEnd { get; private set; }
        public double LoopInterval { get; private set; }
        public BlendMode BlendMode { get; private set; }

        private System.Numerics.Vector<float> _opacityMask;
        private readonly DoubleTimeline<double> _slopes = [];
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

            double Resolve(ValueExpression? expr, double defualtValue = 0)
            {
                return skin.TryResolveValue<double>(expr, provider, out var v) ? v : defualtValue;
            }

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
        }

        protected void UpdateParameters(BmsTimer timer, double absoluteTime)
        {
            if (timer.TryGet(TimerId, absoluteTime, out var relativeTime) && relativeTime is >= 0)
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
                        DestX = x - w * ox;
                        DestY = y - h * oy;
                        DestWidth = w;
                        DestHeight = h;
                        Opacity = op;
                        Angle = a;
                        RotateOriginX = aox;
                        RotateOriginY = aoy;
                        IsVisible = true;
                        return;
                    }
                }
            }
            IsVisible = false;
        }

        public virtual void Update(in UpdateArgs args)
        {
            UpdateParameters(args.Timer, args.AbsoluteTime);
        }

        public unsafe void Render(IBitmap target, FloatBitmap buffer1, UnmanagedArray<float> buffer2)
        {
            if (IsValid && IsVisible)
            {
                RenderCore(target, buffer1, buffer2);
            }
        }

        protected virtual void RenderCore(IBitmap target, FloatBitmap buffer1, UnmanagedArray<float> buffer2)
        {
            if (TryGetBitmap(out var source, out var sourceRect, buffer1, buffer2))
            {
                var destRect = new Rect(DestX, DestY, DestWidth, DestHeight).ToDrawingRect();
                if (sourceRect.Size == destRect.Size)
                {
                    target.Blend(source, sourceRect, destRect.Location, BlendMode, _opacityMask);
                }
                else
                {
                    buffer1.Resize(destRect.Width, destRect.Height);
                    source.StretchCopy(buffer1, sourceRect, buffer2);
                    target.Blend(buffer1, buffer1.Rect, destRect.Location, BlendMode, _opacityMask);
                }
            }
        }

        protected abstract bool TryGetBitmap([MaybeNullWhen(false)] out IBitmap bitmap, out DrRect rect, FloatBitmap buffer1, UnmanagedArray<float> buffer2);
    }
}
