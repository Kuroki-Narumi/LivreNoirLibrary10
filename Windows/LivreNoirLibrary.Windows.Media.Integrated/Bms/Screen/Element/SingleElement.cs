using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Diagnostics.CodeAnalysis;
using DrRect = System.Drawing.Rectangle;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public abstract class SingleElement(SkinElement source) : ScreenElement(source)
    {
        protected override void RenderCore(in RenderArgs args)
        {
            if (TryGetBitmap(out var source, out var sourceRect, args.Buffer))
            {
                var x = DestX - DestWidth * OriginX;
                var y = DestY - DestHeight * OriginY;
                RenderSource(args, source, sourceRect, x, y, DestWidth, DestHeight, BlendMode, args.ColorCorrection * OpacityMask);
            }
        }

        protected abstract bool TryGetBitmap([MaybeNullWhen(false)] out IBitmap bitmap, out DrRect rect, FloatBitmap buffer);
    }
}