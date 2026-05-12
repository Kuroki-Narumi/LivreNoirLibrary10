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
    public class TextElement(Media.Bms.SkinInfo.Text source) : SingleTextureElement(source)
    {
        public const double DefaultFontSize = 12;
        public const double DefaultStrokeThickness = 0;

        private readonly Media.Bms.SkinInfo.Text _source = source;
        private string? _content;
        private bool _needRefresh;
        private readonly FormattedTextOptions _options = new();
        private Pen? _pen;
        private readonly DrawingVisual _visual = new();
        private RenderTargetBitmap? _renderTarget;
        private DrRect _renderRect;
        private readonly UIntBitmap _bitmap = new(0, 0);

        public override void DetermineExpressions(Skin skin, IVariableProvider? provider)
        {
            base.DetermineExpressions(skin, provider);
            var op = _options;
            var source = _source;
            if (source.FontFamily is { } font)
            {
                op.FontFamily = font;
            }
            op.FontStyle = source.FontStyle;
            op.FontWeight = source.FontWeight;
            op.FontStretch = source.FontStretch;
            op.FontSize = skin.ResolveValue(source.FontSize, provider, DefaultFontSize);
            op.Foreground = MediaUtils.GetBrush(source.Fill.ToColor());
            _pen = MediaUtils.GetPen(source.Stroke.ToColor(), skin.ResolveValue(source.StrokeThickness, provider, DefaultStrokeThickness));
            _content = null;
            UpdateContent(skin, provider);
        }

        public override void Update(in UpdateArgs args)
        {
            base.Update(args);
            UpdateContent(args.Skin, args.VariableProvider);
        }

        private void UpdateContent(Skin skin, IVariableProvider? provider)
        {
            if ((IsValid = skin.TryResolveReflection(_source.Content, provider, out var content)) && _content != content)
            {
                _content = content;
                _needRefresh = true;
            }
        }

        protected override bool TryGetBitmap([MaybeNullWhen(false)] out IBitmap bitmap, out DrRect rect, FloatBitmap buffer)
        {
            if (_needRefresh)
            {
                RefreshText();
                _needRefresh = false;
            }
            bitmap = _bitmap;
            rect = _renderRect;
            return true;
        }

        private void RefreshText()
        {
            var ft = MediaUtils.CreateFormattedText(_content!, _options);
            using (var ctx = _visual.RenderOpen())
            {
                var geometry = ft.BuildGeometry(new System.Windows.Point(0, 0));
                if (_pen is { } pen)
                {
                    ctx.DrawGeometry(null, pen, geometry);
                }
                ctx.DrawGeometry(_options.Foreground, null, geometry);
            }
            var renderTarget = _renderTarget;
            var width = (int)Math.Ceiling(ft.Width);
            var height = (int)Math.Ceiling(ft.Height);
            if (renderTarget is null || renderTarget.PixelWidth < width || renderTarget.PixelHeight < height)
            {
                renderTarget = new RenderTargetBitmap(Math.Max(width, 1), Math.Max(height, 1), 96, 96, PixelFormats.Pbgra32);
                _renderTarget = renderTarget;
            }
            else
            {
                renderTarget.Clear();
            }
            renderTarget.Render(_visual);
            _bitmap.Resize(renderTarget.PixelWidth, renderTarget.PixelHeight);
            renderTarget.CopyPixels(_bitmap);
            _renderRect = new(0, 0, width, height);
        }
    }
}
