using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Diagnostics.CodeAnalysis;
using DrRect = System.Drawing.Rectangle;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public sealed class ImageElement(Image source) : ScreenElement(source)
    {
        private readonly Image _source = source;
        private TextureData _textureData;
        private UIntBitmap? _bitmap;
        private DrRect _sourceRect;

        public override void DetermineExpressions(Skin skin, IVariableProvider? provider)
        {
            base.DetermineExpressions(skin, provider);
            IsValid = skin.TryGetTexture(_source.Texture, provider, out _textureData);
        }

        public override void Update(in UpdateArgs args)
        {
            base.Update(args);
            if (IsValid)
            {
                var index = args.Timer.GetFrameIndex(_source.SourceTimer, args.AbsoluteTime, _textureData);
                IsVisible = args.Textures.TryGetTexture(_textureData, index, out _bitmap, out _sourceRect);
            }
        }

        protected override bool TryGetBitmap([MaybeNullWhen(false)] out IBitmap bitmap, out DrRect rect, FloatBitmap buffer1, UnmanagedArray<float> buffer2)
        {
            bitmap = _bitmap;
            rect = _sourceRect;
            return bitmap is not null;
        }
    }
}
