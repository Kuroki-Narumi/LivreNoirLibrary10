using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using System;
using System.Buffers;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            AdjustRect(bitmap, ref x, ref y, ref width, ref height);
            using var ptr = new BitmapPointer(bitmap);
            for (var yy = 0; yy < height; yy++)
            {
                SimdOperations.Clear(ptr.AsUIntSpan(y + yy, x, width));
            }
        }

        public static void Clear(this WriteableBitmap bitmap, Int32Rect rect) => Clear(bitmap, rect.X, rect.Y, rect.Width, rect.Height);

        public static unsafe Int32Rect GetOpaqueRect(this BitmapSource bitmap, byte threshold = 0)
        {
            if (bitmap is WriteableBitmap w)
            {
                return GetOpaqueRect((uint*)w.BackBuffer, bitmap.PixelWidth, bitmap.PixelHeight, threshold);
            }
            var stride = bitmap.PixelWidth * 4;
            var height = bitmap.PixelHeight;
            var buffer = ArrayPool<byte>.Shared.Rent(stride * height);
            try
            {
                fixed (byte* ptr = buffer)
                {
                    bitmap.CopyPixels(GetRect(bitmap), (nint)ptr, stride * height, stride);
                    return GetOpaqueRect((uint*)ptr, bitmap.PixelWidth, bitmap.PixelHeight, threshold);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public static unsafe Int32Rect GetOpaqueRect(uint* pointer, int width, int height, uint threshold)
        {
            var left = width;
            var right = 0;
            var top = -1;
            var bottom = -1;
            threshold <<= 24; // Alphaの位置にビットシフト
            for (var y = 0; y < height; y++)
            {
                var currentLeft = -1;
                var currentRight = -1;
                for (var x = 0; x < width; x++, pointer++)
                {
                    if (*pointer > threshold)
                    {
                        if (currentLeft is -1)
                        {
                            currentLeft = x;
                        }
                        currentRight = x;
                    }
                }
                // 不透明ピクセルがあった場合
                if (currentLeft is not -1)
                {
                    // 左右端の更新
                    left = Math.Min(left, currentLeft);
                    right = Math.Max(right, currentRight);
                    // 上下端の更新
                    if (top is -1)
                    {
                        top = y;
                    }
                    bottom = y;
                }
            }
            // 全て透明
            if (top is -1)
            {
                return new(0, 0, 0, 0);
            }
            else
            {
                return new(left, top, right - left + 1, bottom - top + 1);
            }
        }
    }
}
