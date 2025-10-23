using System;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Media
{
    public sealed class ImageCache(string path) : MediaCache
    {
        private readonly BitmapImage _bitmap = new(new Uri(path));
        public override BitmapSource? GetBitmap(long ticks) => _bitmap;
    }
}
