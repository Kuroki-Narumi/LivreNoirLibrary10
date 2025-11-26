using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Media
{
    public class TextureCache : IClear
    {
        private readonly Dictionary<string, UIntBitmap?> _sources = [];

        public void Clear()
        {
            foreach (var (_, bitmap) in _sources)
            {
                bitmap?.Dispose();
            }
            _sources.Clear();
        }

        private UIntBitmap? CreateBitmap(string key, string path)
        {
            if (Bitmap.GetSourceFromFile(path) is { } bitmap)
            {
                var data = bitmap.ToUIntBitmap();
                _sources[key] = data;
                return data;
            }
            else
            {
                _sources[key] = null;
                return null;
            }
        }

        public void Set(string key, string? path, string basePath)
        {
            if (!string.IsNullOrEmpty(path) && FileUtils.TryGetImageFileName(Path.GetFullPath(path, basePath), out var fullPath))
            {
                CreateBitmap(key, fullPath);
            }
            else
            {
                _sources[key] = null;
            }
        }

        private bool TryGetData(string key, [MaybeNullWhen(false)] out UIntBitmap data)
        {
            if (!_sources.TryGetValue(key, out data))
            {
                data = CreateBitmap(key, key);
            }
            return data is not null;
        }

        public bool TryGetTexture(in TextureData data, int patternIndex, [MaybeNullWhen(false)] out UIntBitmap source, out System.Drawing.Rectangle sourceRect)
        {
            var (path, sx, sy, sw, sh, divX, divY, _) = data;
            if (!string.IsNullOrEmpty(path) && TryGetData(path, out var bitmap))
            {
                var pw = bitmap.Width;
                var ph = bitmap.Height;
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
                if (maxPattern is > 1)
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
                    source = bitmap;
                    sourceRect = new(sx, sy, sw, sh);
                    return true;
                }
            }
            source = default;
            sourceRect = default;
            return false;
        }
    }
}