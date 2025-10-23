using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class ImageElement(Image source) : ScreenElementBase(source)
    {
        private readonly Image _skinInfo = source;
        private bool _isTextureValid;
        private TextureData _textureData;
        private TextureCacheKey _bitmapKey;
        private CroppedBitmap? _bitmap;

        public void LoadDestination(Skin skin, IVariableProvider? provider)
        {
            _isTextureValid = skin.TryGetTexture(_skinInfo.Texture, provider, out _textureData);
            IScreenElementExtension.LoadDestination(this, skin, provider);
        }

        public void Update(BmsTimer timer, long absoluteTick, TextureCache cache)
        {
            if (_isTextureValid)
            {
                var index = timer.GetFrameIndex(_skinInfo.SourceTimer, absoluteTick, _textureData);
                _bitmap = cache.GetBitmap(_textureData, index, out var key);
                if (ViewModel.Update(timer, absoluteTick) || _bitmapKey != key)
                {
                    _bitmapKey = key;
                    InvalidateVisual();
                }
            }
            else
            {
                ViewModel.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawImage(_bitmap, new(0, 0, Width, Height));
        }
    }
}
