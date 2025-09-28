using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using DrRect = System.Drawing.Rectangle;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Media
{
    public class MediaBufferCollection
    {
        private readonly Dictionary<string, MediaBuffer> _data = [];

        public DrRect RequiredRect
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    foreach (var (path, buffer) in _data)
                    {
                        buffer.RefreshRect(path, value);
                    }
                }
            }
        }

        public void Clear()
        {
            foreach (var (_, buffer) in _data)
            {
                buffer.Dispose();
            }
            _data.Clear();
        }

        public (WriteableBitmap?, Rect) GetBitmap(string path, long ticks)
        {
            var image = _data.GetOrAdd(path, p => MediaBuffer.Create(p, RequiredRect));
            return image.GetBitmap(ticks);
        }
    }
}
