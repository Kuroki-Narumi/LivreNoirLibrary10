using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public readonly struct RenderArgs(
        IBitmap target, 
        FloatBitmap buffer,
        DoubleRect rect,
        FloatColor colorCorrection,
        Dictionary<object, DebugItem> totalTimes)
    {
        public readonly IBitmap RenderTarget = target;
        public readonly FloatBitmap Buffer = buffer;
        public readonly DoubleRect Rect = rect;
        public readonly FloatColor ColorCorrection = colorCorrection;
        public readonly Dictionary<object, DebugItem> TotalTimes = totalTimes;

        public RenderArgs(IBitmap target, FloatBitmap buffer, Dictionary<object, DebugItem>? totalTimes = null) :
            this(target, buffer, new(0, 0, target.Width, target.Height), FloatColor.White, totalTimes ?? [])
        { }

        public RenderArgs Descend(DoubleRect newRect, FloatColor colorCorrection)
        {
            return new(RenderTarget, Buffer, newRect, ColorCorrection * colorCorrection, TotalTimes);
        }

        public void Deconstruct(out IBitmap target, out FloatBitmap buffer, out DoubleRect parentRect, out FloatColor parentColor)
        {
            target = RenderTarget;
            buffer = Buffer;
            parentRect = Rect;
            parentColor = ColorCorrection;
        }
    }

    public readonly record struct DebugItem(string? Name, int Indent, double Time)
    {
        public DebugItem Reset() => new(Name, Indent, 0);
        public static DebugItem operator +(DebugItem left, double right) => new(left.Name, left.Indent, left.Time + right);
    }
}
