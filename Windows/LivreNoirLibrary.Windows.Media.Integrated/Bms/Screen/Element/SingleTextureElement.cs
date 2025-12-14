using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;
using DrRect = System.Drawing.Rectangle;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public abstract class SingleTextureElement(SkinElement source) : ScreenElement(source)
    {
        public Stretch Stretch { get; private set; }
        public double MaxWidth { get; private set;  } = double.MaxValue;
        public double MaxHeight { get; private set; } = double.MaxValue;

        public override void DetermineExpressions(Skin skin, IVariableProvider? provider)
        {
            base.DetermineExpressions(skin, provider);
            if (Source is IStretchElement source)
            {
                Stretch = source.Stretch;
                MaxWidth = skin.TryResolveValue(source.MaxWidth, provider, out double value) ? value : double.MaxValue;
                MaxHeight = skin.TryResolveValue(source.MaxHeight, provider, out value) ? value : double.MaxValue;
            }
        }

        protected override void RenderCore(in RenderArgs args)
        {
            if (TryGetBitmap(out var source, out var sourceRect, args.Buffer))
            {
                var sw = sourceRect.Width;
                var sh = sourceRect.Height;
                var dx = DestX;
                var dy = DestY;
                var dw = DestWidth;
                var dh = DestHeight;
                // 拡縮なし
                var strecth = Stretch;
                if (strecth is Stretch.None)
                {
                    dw = sw;
                    dh = sh;
                }
                else if (dw is <= 0)
                {
                    if (dh is <= 0)
                    {
                        dw = Math.Min(sw, MaxWidth);
                        dh = Math.Min(sh, MaxHeight);
                    }
                    else
                    {
                        dw = Math.Min(sw * dh / sh, MaxWidth);
                    }
                }
                else if (dh is <= 0)
                {
                    dh = Math.Min(sh * dw / sw, MaxHeight);
                }
                if (strecth is Stretch.Uniform)
                {
                    var scale = Math.Min(dw / sw, dh / sh);
                    dw = sw * scale;
                    dh = sh * scale;
                }
                else if (strecth is Stretch.UniformToFill)
                {
                    var scale = Math.Max(dw / sw, dh / sh);
                    dw = sw * scale;
                    dh = sh * scale;
                }
                var x = dx - dw * OriginX;
                var y = dy - dh * OriginY;
                RenderSource(args, source, sourceRect, x, y, dw, dh, BlendMode, args.ColorCorrection * OpacityMask);
            }
        }

        protected abstract bool TryGetBitmap([MaybeNullWhen(false)] out IBitmap bitmap, out DrRect rect, FloatBitmap buffer);
    }
}