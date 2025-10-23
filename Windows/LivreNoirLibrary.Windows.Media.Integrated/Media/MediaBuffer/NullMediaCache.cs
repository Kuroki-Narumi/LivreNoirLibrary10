using System;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Media
{
    public sealed class NullMediaCache : MediaCache
    {
        public override BitmapSource? GetBitmap(long ticks) => null;
    }
}
