using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Media
{
    public class TextureCache : IClear
    {
        protected readonly Dictionary<string, BitmapImage?> _sourceCache = [];
        protected readonly Dictionary<TextureCacheKey, CroppedBitmap?> _croppedCache = [];

        public void Clear()
        {
            _sourceCache.Clear();
            _croppedCache.Clear();
        }

        public BitmapImage? GetSource(string key)
        {
            if (!_sourceCache.TryGetValue(key, out var bitmap))
            {
                try
                {
                    bitmap = Bitmap.GetSourceFromFile(key);
                }
                catch
                {
                    bitmap = null;
                }
                _sourceCache[key] = bitmap;
            }
            return bitmap;
        }

        public CroppedBitmap? GetBitmap(TextureCacheKey key)
        {
            if (!_croppedCache.TryGetValue(key, out var bitmap))
            {
                bitmap = new(_sourceCache[key.Path], key.Rect);
                _croppedCache.Add(key, bitmap);
            }
            return bitmap;
        }

        public CroppedBitmap? GetBitmap(in TextureData texture, int patternIndex, out TextureCacheKey key)
        {
            var (path, sx, sy, sw, sh, divX, divY, _) = texture;
            if (!string.IsNullOrEmpty(path) && GetSource(path) is { } source)
            {
                var pw = source.PixelWidth;
                var ph = source.PixelHeight;
                if (sw is <= 0)
                {
                    sw += pw;
                }
                if (sh is <= 0)
                {
                    sh += ph;
                }
                if (sx is < 0)
                {
                    sw += sx;
                    sx = 0;
                }
                if (sy is < 0)
                {
                    sh += sy;
                    sy = 0;
                }
                var maxPattern = divX * divY;
                if (divX * divY is > 1)
                {
                    var divIndex = patternIndex % maxPattern;
                    sw /= divX;
                    sh /= divY;
                    sx += sw * (divIndex % divX);
                    sy += sh * (divIndex / divX);
                }
                // 切り抜き範囲が有効な場合
                if (sx < pw && sw is > 0 && sw <= pw && sy < ph && sh is > 0 && sh <= ph)
                {
                    key = new(path, new(sx, sy, sw, sh));
                    return GetBitmap(key);
                }
            }
            key = default;
            return null;
        }
    }
}
