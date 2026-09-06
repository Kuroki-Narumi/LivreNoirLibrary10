using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace LivreNoir.WinCapture
{
    public record CapturedItem(WriteableBitmap? Bitmap, string Name);
}
