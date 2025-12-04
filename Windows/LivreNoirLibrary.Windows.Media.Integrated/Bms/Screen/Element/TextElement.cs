using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DrRect = System.Drawing.Rectangle;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public class TextElement(Media.Bms.SkinInfo.Text source) : SingleElement(source)
    {
        public const double DefaultFontSize = 12;
        public const double DefaultStrokeThickness = 0;

        private readonly Media.Bms.SkinInfo.Text _source = source;
        private readonly FormattedTextOption _options = new();
        private readonly DrawingVisual _visual = new();
        private RenderTargetBitmap? _renderTarget;
        private readonly UIntBitmap _bitmap = new(0, 0);

        public override void DetermineExpressions(Skin skin, IVariableProvider? provider)
        {
            base.DetermineExpressions(skin, provider);
            var source = _source;
            if (IsValid = skin.TryResolveReflection(source.Content, provider, out var content))
            {
                var op = _options;
                if (source.FontFamily is { } font)
                {
                    op.FontFamily = font;
                }
                op.FontStyle = source.FontStyle;
                op.FontWeight = source.FontWeight;
                op.FontStretch = source.FontStretch;
                op.FontSize = skin.ResolveValue(source.FontSize, provider, DefaultFontSize);
                op.Foreground = MediaUtils.GetBrush(source.Fill.ToColor());
                var ft = MediaUtils.CreateFormattedText(content!, op);
                using (var ctx = _visual.RenderOpen())
                {
                    var geometry = ft.BuildGeometry(new System.Windows.Point(0, 0));
                    var pen = MediaUtils.GetPen(source.Stroke.ToColor(), skin.ResolveValue(source.StrokeThickness, provider, DefaultStrokeThickness));
                    if (pen is not null)
                    {
                        ctx.DrawGeometry(null, pen, geometry);
                    }
                    ctx.DrawGeometry(op.Foreground, null, geometry);
                }
                var renderTarget = GetRenderTarget((int)Math.Ceiling(ft.Width), (int)Math.Ceiling(ft.Height));
                renderTarget.Render(_visual);
                _bitmap.Resize(renderTarget.PixelWidth, renderTarget.PixelHeight);
                renderTarget.CopyPixels(_bitmap);
            }
        }

        private RenderTargetBitmap GetRenderTarget(int width, int height)
        {
            if (_renderTarget is null || _renderTarget.PixelWidth < width || _renderTarget.PixelHeight < height)
            {
                _renderTarget = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            }
            else
            {
                _renderTarget.Clear();
            }
            return _renderTarget;
        }

        protected override bool TryGetBitmap([MaybeNullWhen(false)] out IBitmap bitmap, out DrRect rect, FloatBitmap buffer)
        {
            bitmap = _bitmap;
            rect = bitmap.Rect;
            var w = rect.Width * DestHeight / rect.Height;
            DestWidth = Math.Min(w, DestWidth);
            return true;
        }
    }
}
