using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.Capture;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls
{
    public class BitmapAdapter : IImageAdapter<WriteableBitmap>
    {
        public WriteableBitmap Prepare(WriteableBitmap? previousDstImage, int width, int height)
        {
            if (previousDstImage is null || previousDstImage.PixelWidth < width || previousDstImage.PixelHeight < height)
            {
                previousDstImage = new(width, height, 96, 96, PixelFormats.Pbgra32, null);
            }
            return previousDstImage;
        }

        public unsafe void Copy(WriteableBitmap dstImage, byte* scrPtr, int width, int height, int srcStride)
        {
            using var bitmap = dstImage.BeginWrite();
            var dstPtr = (uint*)bitmap.Pointer;
            var nwidth = (nuint)width;
            var dstStride = bitmap.Width;
            for (var y = 0; y < height; y++)
            {
                SimdOperations.CopyFrom(dstPtr, (uint*)scrPtr, nwidth);
                scrPtr += srcStride;
                dstPtr += dstStride;
            }
        }
    }
}
