using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Media
{
    public class MediaCacheCollection
    {
        private readonly Dictionary<string, MediaCache> _data = [];

        public void Clear()
        {
            foreach (var (_, buffer) in _data)
            {
                buffer.Dispose();
            }
            _data.Clear();
        }

        public BitmapSource? GetBitmap(string path, long ticks)
        {
            var image = _data.GetOrAdd(path, MediaCache.Create);
            return image.GetBitmap(ticks);
        }
    }
}
