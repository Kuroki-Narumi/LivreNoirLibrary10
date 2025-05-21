using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media
{
    public static partial class Bitmap
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Rect GetRect(this BitmapSource bitmap) => new(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddDirtyRect(this WriteableBitmap bitmap) => bitmap.AddDirtyRect(GetRect(bitmap));

        public static void AdjustRect(this BitmapSource bitmap, ref int x, ref int y, ref int width, ref int height)
        {
            if (x is < 0)
            {
                width += x;
                x = 0;
            }
            if (y is < 0)
            {
                height += y;
                y = 0;
            }
            var max = bitmap.PixelWidth - x;
            if (width > max)
            {
                width = max;
            }
            max = bitmap.PixelHeight - y;
            if (height > max)
            {
                height = max;
            }
            if (width is <= 0 || height is <= 0)
            {
                width = 0;
                height = 0;
            }
        }

        public static unsafe byte* GetPtr(this WriteableBitmap bitmap) => (byte*)bitmap.BackBuffer;
        public static unsafe byte* GetPtr(this WriteableBitmap bitmap, int x, int y) => (byte*)bitmap.BackBuffer + x * 4 + y * bitmap.BackBufferStride;
        public static unsafe int* GetIntPtr(this WriteableBitmap bitmap) => (int*)bitmap.BackBuffer;
        public static unsafe int* GetIntPtr(this WriteableBitmap bitmap, int x, int y) => (int*)bitmap.BackBuffer + x + y * bitmap.PixelWidth;

        public static unsafe Color GetPixel(this WriteableBitmap bitmap, int x, int y)
        {
            if ((uint)x < (uint)bitmap.PixelWidth || (uint)y <= (uint)bitmap.PixelHeight)
            {
                var ptr = GetIntPtr(bitmap, x, y);
                return ColorOperation.ToColor(*ptr);
            }
            else
            {
                return Colors.Transparent;
            }
        }

        public static unsafe void SetPixel(this WriteableBitmap bitmap, int x, int y, Color color)
        {
            if ((uint)x < (uint)bitmap.PixelWidth || (uint)y < (uint)bitmap.PixelHeight)
            {
                bitmap.Lock();
                try
                {
                    var ptr = GetIntPtr(bitmap, x, y);
                    *ptr = ColorOperation.ToInt(color);
                }
                finally
                {
                    bitmap.AddDirtyRect(new(x, y, 1, 1));
                    bitmap.Unlock();
                }
            }
        }

        public static unsafe void Clear(this WriteableBitmap bitmap)
        {
            bitmap.Lock();
            try
            {
                new Span<int>(GetIntPtr(bitmap), bitmap.PixelWidth * bitmap.PixelHeight).Clear();
            }
            finally
            {
                bitmap.AddDirtyRect();
                bitmap.Unlock();
            }
        }

        public static unsafe void Clear(this WriteableBitmap bitmap, int x, int y, int width, int height)
        {
            AdjustRect(bitmap, ref x, ref y, ref width, ref height);
            bitmap.Lock();
            try
            {
                var offset = GetIntPtr(bitmap, x, y);
                for (var yy = 0; yy < height; yy++)
                {
                    new Span<int>(GetIntPtr(bitmap, x, y + yy), width).Clear();
                }
            }
            finally
            {
                bitmap.AddDirtyRect(new(x, y, width, height));
                bitmap.Unlock();
            }
        }

        public static void Clear(this WriteableBitmap bitmap, Int32Rect rect) => Clear(bitmap, rect.X, rect.Y, rect.Width, rect.Height);
    }
}
