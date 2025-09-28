using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Media
{
    public static partial class Bitmap
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Rect GetRect(this BitmapSource bitmap) => new(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);

        public static unsafe byte* GetPtr(this WriteableBitmap bitmap) => (byte*)bitmap.BackBuffer;
        public static unsafe byte* GetPtr(this WriteableBitmap bitmap, int x, int y) => (byte*)bitmap.BackBuffer + x * 4 + y * bitmap.BackBufferStride;
        public static unsafe uint* GetUIntPtr(this WriteableBitmap bitmap) => (uint*)bitmap.BackBuffer;
        public static unsafe uint* GetUIntPtr(this WriteableBitmap bitmap, int x, int y) => (uint*)bitmap.BackBuffer + x + y * bitmap.PixelWidth;

        public static unsafe Color GetPixel(this WriteableBitmap bitmap, int x, int y)
        {
            if ((uint)x < (uint)bitmap.PixelWidth || (uint)y <= (uint)bitmap.PixelHeight)
            {
                var ptr = GetUIntPtr(bitmap, x, y);
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
                    var ptr = GetUIntPtr(bitmap, x, y);
                    *ptr = ColorOperation.ToUInt(color);
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
            using var ptr = new BitmapPointer(bitmap);
            SimdOperations.Clear(GetUIntPtr(bitmap), bitmap.PixelWidth * bitmap.PixelHeight);
        }

        public static unsafe void Clear(this WriteableBitmap bitmap, int x, int y, int width, int height)
        {
            using var ptr = new BitmapPointer(bitmap);
            BitmapOperation.Clear(ptr, new(x, y, width, height));
        }

        public static void Clear(this WriteableBitmap bitmap, Int32Rect rect) => Clear(bitmap, rect.X, rect.Y, rect.Width, rect.Height);

        public static unsafe Int32Rect GetOpaqueRect(this BitmapSource bitmap, int margin = 0, byte threshold = 1)
        {
            if (bitmap is WriteableBitmap w)
            {
                return BitmapOperation.GetOpaqueRect(new((void*)w.BackBuffer, w.PixelWidth, w.PixelHeight), margin, threshold).ToInt32Rect();
            }
            var stride = bitmap.PixelWidth * 4;
            var height = bitmap.PixelHeight;
            var buffer = ArrayPool<byte>.Shared.Rent(stride * height);
            try
            {
                fixed (byte* ptr = buffer)
                {
                    bitmap.CopyPixels(GetRect(bitmap), (nint)ptr, stride * height, stride);
                    return BitmapOperation.GetOpaqueRect(new(ptr, bitmap.PixelWidth, bitmap.PixelHeight), margin, threshold).ToInt32Rect();
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
}
