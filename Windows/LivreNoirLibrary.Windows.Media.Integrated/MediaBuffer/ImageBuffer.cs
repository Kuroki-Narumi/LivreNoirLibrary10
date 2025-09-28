using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DrRect = System.Drawing.Rectangle;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public sealed class ImageBuffer : MediaBuffer
    {
        private readonly WriteableBitmap _bitmap;
        private Rect _rect;

        public ImageBuffer(string path, in DrRect requiredRect)
        {
            _bitmap = Bitmap.FromFile(path);
            _bitmap.SetTransparent(Color.FromRgb(0, 0, 0));
            RefreshRect(path, requiredRect);
        }

        public override void RefreshRect(string path, in DrRect requiredRect)
        {
            var (x, y, width, height) = requiredRect;
            var scale = Math.Min(width / 256.0, height / 256.0);
            var ox = x + (width - 256 * scale) / 2;
            var oy = y + (height - 256 * scale) / 2;
            _rect = new(ox, oy, _bitmap.PixelWidth * scale, _bitmap.PixelHeight * scale);
        }

        public override (WriteableBitmap?, Rect) GetBitmap(long ticks) => (_bitmap, _rect);
    }
}
