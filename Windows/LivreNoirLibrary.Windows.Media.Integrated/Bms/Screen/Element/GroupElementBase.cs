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
            var dw = DestWidth is < 0 ? pw : DestWidth;
            var dh = DestHeight is < 0 ? ph : DestHeight;
            var x = DestX - dw * OriginX;
            var y = DestY - dh * OriginY;
            var width = Math.Min(dw, pw - x);
            var height = Math.Min(dh, ph - y);
            RenderChildren(args.Descend(new(x + px, y + py, width, height), args.ColorCorrection * OpacityMask));
        }

        protected abstract void RenderChildren(in RenderArgs args);
    }
}
