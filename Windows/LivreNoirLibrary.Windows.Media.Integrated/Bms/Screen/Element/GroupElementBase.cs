using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public abstract class GroupElementBase(SkinElement source) : ScreenElement(source)
    {
        protected override void RenderCore(in RenderArgs args)
        {
            var (px, py, pw, ph) = args.Rect;
            var x = DestX;
            var y = DestY;
            var width = Math.Min(DestWidth, pw - x);
            var height = Math.Min(DestHeight, ph - y);
            RenderChildren(args.Descend(new(x + px, y + py, width, height), args.ColorCorrection * OpacityMask));
        }

        protected abstract void RenderChildren(in RenderArgs args);
    }
}
