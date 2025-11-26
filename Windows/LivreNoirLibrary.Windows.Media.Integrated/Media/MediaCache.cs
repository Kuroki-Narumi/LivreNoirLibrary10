using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace LivreNoirLibrary.Windows.Media
{
    public class MediaCache : IClear
    {
        private readonly Dictionary<string, MediaCacheItem> _data = [];

        public void Clear()
        {
            foreach (var (_, item) in _data)
            {
                item.Dispose();
            }
            _data.Clear();
        }

        public bool TryGetBitmap(string path, double time, [MaybeNullWhen(false)] out UIntBitmap bitmap) => _data.GetOrAdd(path, path => new MediaCacheItem(path)).TryGetBitmap(time, out bitmap);

        private class MediaCacheItem
        {
            private readonly UIntBitmap? _bitmap;
            private readonly VideoCache? _video;

            public unsafe MediaCacheItem(string path)
            {
                ExConsole.Write($"Create MediaBuffer from \"{path}\"");
                if (File.Exists(path))
                {
                    if (ExtRegs.Image.IsMatch(path))
                    {
                        if (Bitmap.GetSourceFromFile(path) is { } bitmap)
                        {
                            _bitmap = bitmap.ToUIntBitmap();
                        }
                    }
                    else
                    {
                        try
                        {
                            _video = new(path);
                            _bitmap = new(_video.Width, _video.Height);
                        }
                        catch
                        {
                            _video = null;
                        }
                    }
                }
            }

            public void Dispose()
            {
                _bitmap?.Dispose();
                _video?.Dispose();
            }

            public bool TryGetBitmap(double time, [MaybeNullWhen(false)] out UIntBitmap bitmap)
            {
                if (_bitmap is { } buffer)
                {
                    _video?.GetBitmap(time, buffer.AsSpan<byte>());
                    bitmap = buffer;
                    return true;
                }
                bitmap = null;
                return false;
            }
        }
    }
}