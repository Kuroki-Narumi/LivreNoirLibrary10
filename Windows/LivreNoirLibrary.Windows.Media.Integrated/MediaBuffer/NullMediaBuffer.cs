using System;
using System.Windows;
using System.Windows.Media.Imaging;
using DrRect = System.Drawing.Rectangle;

namespace LivreNoirLibrary.Windows.Media
{
    public sealed class NullMediaBuffer : MediaBuffer
    {
        public override void RefreshRect(string path, in DrRect requiredRect) { }
        public override (WriteableBitmap?, Rect) GetBitmap(long ticks) => (null, default);
    }
}
