using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Windows;

namespace LivreNoirLibrary.Media
{
    public static partial class Bitmap
    {
        private static unsafe void BlendCore<T>(this WriteableBitmap bitmap, Color color, T operation)
            where T : IBinaryColorOperation
        {
            var max = bitmap.PixelWidth * bitmap.PixelHeight;
            var source = stackalloc byte[4];
            source[0] = color.B;
            source[1] = color.G;
            source[2] = color.R;
            source[3] = color.A;
            bitmap.Lock();
            try
            {
                var target = GetPtr(bitmap);
                for (var i = 0; i < max; i++)
                {
                    operation.Apply(source, target);
                    target += 4;
                }
            }
            finally
            {
                bitmap.AddDirtyRect(bitmap.GetRect());
                bitmap.Unlock();
            }
        }

        private static unsafe void BlendCore<T>(this WriteableBitmap bitmap, Color color, T operation, int x, int y, int width, int height)
            where T : IBinaryColorOperation
        {
            AdjustRect(bitmap, ref x, ref y, ref width, ref height);
            Int32Rect rect = new(x, y, width, height);
            var stride = bitmap.BackBufferStride;
            var source = stackalloc byte[4];
            source[0] = color.B;
            source[1] = color.G;
            source[2] = color.R;
            source[3] = color.A;
            bitmap.Lock();
            try
            {
                var offset = GetPtr(bitmap, x, y);
                for (y = 0; y < height; y++)
                {
                    var target = offset;
                    for (x = 0; x < width; x++)
                    {
                        operation.Apply(source, target);
                        target += 4;
                    }
                    offset += stride;
                }
            }
            finally
            {
                bitmap.AddDirtyRect(rect);
                bitmap.Unlock();
            }
        }

        private static unsafe void BlendCore<T>(this WriteableBitmap bitmap, int dstX, int dstY, WriteableBitmap source, int srcX, int srcY, int srcWidth, int srcHeight, T operation)
            where T : IBinaryColorOperation
        {
            AdjustRect(source, ref srcX, ref srcY, ref srcWidth, ref srcHeight);
            AdjustRect(bitmap, ref dstX, ref dstY, ref srcWidth, ref srcHeight);
            Int32Rect rect = new(dstX, dstY, srcWidth, srcHeight);
            var srcStride = source.BackBufferStride;
            var dstStride = bitmap.BackBufferStride;
            bitmap.Lock();
            try
            {
                var srcBegin = GetPtr(source, srcX, srcY);
                var dstBegin = GetPtr(bitmap, dstX, dstY);
                for (int y = 0; y < srcHeight; y++)
                {
                    var srcPtr = srcBegin;
                    var dstPtr = dstBegin;
                    for (int x = 0; x < srcWidth; x++)
                    {
                        operation.Apply(srcPtr, dstPtr);
                        srcPtr += 4;
                        dstPtr += 4;
                    }
                    srcBegin += srcStride;
                    dstBegin += dstStride;
                }
            }
            finally
            {
                bitmap.AddDirtyRect(rect);
                bitmap.Unlock();
            }
        }

        private static unsafe void BlendCore<T>(this WriteableBitmap bitmap, WriteableBitmap source, T operation)
            where T : IBinaryColorOperation
            => BlendCore(bitmap, 0, 0, source, 0, 0, source.PixelWidth, source.PixelHeight, operation);
        private static unsafe void BlendCore<T>(this WriteableBitmap bitmap, int dstX, int dstY, WriteableBitmap source, T operation)
            where T : IBinaryColorOperation
            => BlendCore(bitmap, dstX, dstY, source, 0, 0, source.PixelWidth, source.PixelHeight, operation);
        private static unsafe void BlendCore<T>(this WriteableBitmap bitmap, int dstX, int dstY, WriteableBitmap source, Int32Rect srcRect, T operation)
            where T : IBinaryColorOperation
            => BlendCore(bitmap, dstX, dstY, source, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, operation);

        public static void Multiply(this WriteableBitmap bitmap, Color color)
            => BlendCore(bitmap, color, new ColorOperation_Multiply());
        public static void Multiply(this WriteableBitmap bitmap, Color color, int x, int y, int width, int height)
            => BlendCore(bitmap, color, new ColorOperation_Multiply(), x, y, width, height);
        public static void Multiply(this WriteableBitmap bitmap, Color color, Int32Rect rect)
            => BlendCore(bitmap, color, new ColorOperation_Multiply(), rect.X, rect.Y, rect.Width, rect.Height);

        public static unsafe void Clip(this WriteableBitmap bitmap, WriteableBitmap mask, ColorIndex color = ColorIndex.A)
            => BlendCore(bitmap, mask, new ColorOperation_Mask((int)color));
        public static unsafe void Clip(this WriteableBitmap bitmap, int dstX, int dstY, WriteableBitmap mask, ColorIndex color = ColorIndex.A)
            => BlendCore(bitmap, dstX, dstY, mask, new ColorOperation_Mask((int)color));
        public static unsafe void Clip(this WriteableBitmap bitmap, int dstX, int dstY, WriteableBitmap mask, int srcX, int srcY, int srcWidth, int srcHeight, ColorIndex color = ColorIndex.A)
            => BlendCore(bitmap, dstX, dstY, mask, srcX, srcY, srcWidth, srcHeight, new ColorOperation_Mask((int)color));
        public static unsafe void Clip(this WriteableBitmap bitmap, int dstX, int dstY, WriteableBitmap mask, Int32Rect srcRect, ColorIndex color = ColorIndex.A)
            => BlendCore(bitmap, dstX, dstY, mask, srcRect, new ColorOperation_Mask((int)color));

        public static unsafe void Blt(this WriteableBitmap bitmap, WriteableBitmap source)
            => BlendCore(bitmap, source, new ColorOperation_AlphaBlend());
        public static unsafe void Blt(this WriteableBitmap bitmap, int dstX, int dstY, WriteableBitmap source)
            => BlendCore(bitmap, dstX, dstY, source, new ColorOperation_AlphaBlend());
        public static unsafe void Blt(this WriteableBitmap bitmap, int dstX, int dstY, WriteableBitmap source, int srcX, int srcY, int srcWidth, int srcHeight)
            => BlendCore(bitmap, dstX, dstY, source, srcX, srcY, srcWidth, srcHeight, new ColorOperation_AlphaBlend());
        public static unsafe void Blt(this WriteableBitmap bitmap, int dstX, int dstY, WriteableBitmap source, Int32Rect srcRect)
            => BlendCore(bitmap, dstX, dstY, source, srcRect, new ColorOperation_AlphaBlend());
    }
}
