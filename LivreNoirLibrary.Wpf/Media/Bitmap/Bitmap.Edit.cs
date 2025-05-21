using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Windows;

namespace LivreNoirLibrary.Media
{
    public static partial class Bitmap
    {
        private static unsafe void SetColorCore<T>(WriteableBitmap bitmap, T operation)
            where T : IUnaryColorOperation
        {
            var max = bitmap.PixelWidth * bitmap.PixelHeight;
            bitmap.Lock();
            try
            {
                var ptr = GetPtr(bitmap);
                for (var i = 0; i < max; i++)
                {
                    operation.Apply(ptr);
                    ptr += 4;
                }
            }
            finally
            {
                bitmap.AddDirtyRect(GetRect(bitmap));
                bitmap.Unlock();
            }
        }

        private static unsafe void SetColorCore<T>(WriteableBitmap bitmap, T operation, int x, int y, int width, int height)
            where T : IUnaryColorOperation
        {
            AdjustRect(bitmap, ref x, ref y, ref width, ref height);
            Int32Rect rect = new(x, y, width, height);
            var stride = bitmap.BackBufferStride;
            bitmap.Lock();
            try
            {
                var offset = GetPtr(bitmap, x, y);
                for (y = 0; y < height; y++)
                {
                    var ptr = offset;
                    for (x = 0; x < width; x++)
                    {
                        operation.Apply(ptr);
                        ptr += 4;
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

        public static unsafe void SetColor(this WriteableBitmap bitmap, ColorIndex color, byte value)
        {
            if (color is ColorIndex.A)
            {
                SetColorCore(bitmap, new ColorOperation_SetAlpha(value));
            }
            else
            {
                SetColorCore(bitmap, new ColorOperation_Set((int)color, value));
            }
        }

        public static unsafe void SetColor(this WriteableBitmap bitmap, ColorIndex color, byte value, int x, int y, int width, int height)
        {
            if (color is ColorIndex.A)
            {
                SetColorCore(bitmap, new ColorOperation_SetAlpha(value), x, y, width, height);
            }
            else
            {
                SetColorCore(bitmap, new ColorOperation_Set((int)color, value), x, y, width, height);
            }
        }

        public static void SetColor(this WriteableBitmap bitmap, ColorIndex color, byte value, Int32Rect rect) => SetColor(bitmap, color, value, rect.X, rect.Y, rect.Width, rect.Height);

        public static unsafe void SetColor(this WriteableBitmap bitmap, ColorIndex to, ColorIndex from)
        {
            if (to == from) { return; }
            SetColorCore(bitmap, new ColorOperation_SetFrom((int)from, (int)to));
        }

        public static unsafe void SetColor(this WriteableBitmap bitmap, ColorIndex to, ColorIndex from, int x, int y, int width, int height)
        {
            if (to == from) { return; }
            SetColorCore(bitmap, new ColorOperation_SetFrom((int)from, (int)to), x, y, width, height);
        }

        public static void SetColor(this WriteableBitmap bitmap, ColorIndex to, ColorIndex from, Int32Rect rect) => SetColor(bitmap, to, from, rect.X, rect.Y, rect.Width, rect.Height);

        public static unsafe void SwapColor(this WriteableBitmap bitmap, ColorIndex color1, ColorIndex color2)
        {
            if (color1 == color2) { return; }
            SetColorCore(bitmap, new ColorOperation_Swap((int)color1, (int)color2));
        }

        public static unsafe void SwapColor(this WriteableBitmap bitmap, ColorIndex color1, ColorIndex color2, int x, int y, int width, int height)
        {
            if (color1 == color2) { return; }
            SetColorCore(bitmap, new ColorOperation_Swap((int)color1, (int)color2), x, y, width, height);
        }

        public static void SwapColor(this WriteableBitmap bitmap, ColorIndex color1, ColorIndex color2, Int32Rect rect) => SwapColor(bitmap, color1, color2, rect.X, rect.Y, rect.Width, rect.Height);

        public static unsafe void InvertColor(this WriteableBitmap bitmap)
        {
            SetColorCore(bitmap, new ColorOperation_InvertAll());
        }

        public static unsafe void InvertColor(this WriteableBitmap bitmap, int x, int y, int width, int height)
        {
            SetColorCore(bitmap, new ColorOperation_InvertAll(), x, y, width, height);
        }

        public static void InvertColor(this WriteableBitmap bitmap, Int32Rect rect) => InvertColor(bitmap, rect.X, rect.Y, rect.Width, rect.Height);

        public static unsafe void InvertColor(this WriteableBitmap bitmap, ColorIndex color)
        {
            if (color is ColorIndex.A)
            {
                SetColorCore(bitmap, new ColorOperation_InvertAlpha());
            }
            else
            {
                SetColorCore(bitmap, new ColorOperation_Invert((int)color));
            }
        }

        public static unsafe void InvertColor(this WriteableBitmap bitmap, ColorIndex color, int x, int y, int width, int height)
        {
            if (color is ColorIndex.A)
            {
                SetColorCore(bitmap, new ColorOperation_InvertAlpha(), x, y, width, height);
            }
            else
            {
                SetColorCore(bitmap, new ColorOperation_Invert((int)color), x, y, width, height);
            }
        }

        public static void InvertColor(this WriteableBitmap bitmap, ColorIndex color, Int32Rect rect) => InvertColor(bitmap, color, rect.X, rect.Y, rect.Width, rect.Height);

        public static unsafe void ChangeHue(this WriteableBitmap bitmap, float hue)
        {
            if (hue is not 0)
            {
                SetColorCore(bitmap, new ColorOperation_ChangeHue(hue));
            }
        }

        public static unsafe void ChangeHue(this WriteableBitmap bitmap, float hue, int x, int y, int width, int height)
        {
            if (hue is not 0)
            {
                SetColorCore(bitmap, new ColorOperation_ChangeHue(hue), x, y, width, height);
            }
        }

        public static void ChangeHue(this WriteableBitmap bitmap, float hue, Int32Rect rect) => ChangeHue(bitmap, hue, rect.X, rect.Y, rect.Width, rect.Height);

        public static unsafe void ChangeHsv(this WriteableBitmap bitmap, float hue, float saturation, float value)
        {
            if (hue is not 0 || saturation is not 0 || value is not 0)
            {
                SetColorCore(bitmap, new ColorOperation_ChangeHsv(hue, saturation, value));
            }
        }

        public static unsafe void ChangeHsv(this WriteableBitmap bitmap, float hue, float saturation, float value, int x, int y, int width, int height)
        {
            if (hue is not 0 || saturation is not 0 || value is not 0)
            {
                SetColorCore(bitmap, new ColorOperation_ChangeHsv(hue, saturation, value), x, y, width, height);
            }
        }

        public static void ChangeHsv(this WriteableBitmap bitmap, float hue, float saturation, float value, Int32Rect rect) => ChangeHsv(bitmap, hue, saturation, value, rect.X, rect.Y, rect.Width, rect.Height);

        public static unsafe void CopyFrom(this WriteableBitmap bitmap, int dstX, int dstY, WriteableBitmap source, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            AdjustRect(source, ref srcX, ref srcY, ref srcWidth, ref srcHeight);
            AdjustRect(bitmap, ref dstX, ref dstY, ref srcWidth, ref srcHeight);
            Int32Rect rect = new(dstX, dstY, srcWidth, srcHeight);
            var srcStride = source.BackBufferStride;
            var dstStride = bitmap.BackBufferStride;
            var needSafeCopy = bitmap.BackBuffer == source.BackBuffer;
            bitmap.Lock();
            try
            {
                var srcPtr = source.BackBuffer + srcX * 4 + srcY * srcStride;
                var dstPtr = bitmap.BackBuffer + dstX * 4 + dstY * dstStride;
                for (int y = 0; y < srcHeight; y++)
                {
                    if (needSafeCopy)
                    {
                        Windows.NativeMethods.Win32Api.RtlMoveMemory(dstPtr, srcPtr, dstStride);
                    }
                    else
                    {
                        SimdOperations.CopyFrom((byte*)dstPtr, (byte*)srcPtr, dstStride);
                    }
                    srcPtr += srcStride;
                    dstPtr += dstStride;
                }
            }
            finally
            {
                bitmap.AddDirtyRect(rect);
                bitmap.Unlock();
            }
        }

        public static unsafe void CopyFrom(this WriteableBitmap bitmap, WriteableBitmap source)
            => CopyFrom(bitmap, 0, 0, source, 0, 0, source.PixelWidth, source.PixelHeight);
        public static unsafe void CopyFrom(this WriteableBitmap bitmap, int dstX, int dstY, WriteableBitmap source)
            => CopyFrom(bitmap, dstX, dstY, source, 0, 0, source.PixelWidth, source.PixelHeight);
        public static unsafe void CopyFrom(this WriteableBitmap bitmap, int dstX, int dstY, WriteableBitmap source, Int32Rect srcRect)
            => CopyFrom(bitmap, dstX, dstY, source, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height);

        public static unsafe WriteableBitmap Crop(this WriteableBitmap bitmap, int x, int y, int width, int height)
        {
            AdjustRect(bitmap, ref x, ref y, ref width, ref height);
            var result = Create(width, height);
            CopyFrom(result, 0, 0, bitmap, x, y, width, height);
            return result;
        }

        public static WriteableBitmap Crop(this WriteableBitmap bitmap, Int32Rect rect) => Crop(bitmap, rect.X, rect.Y, rect.Width, rect.Height);

        public static unsafe void MoveMemory(this WriteableBitmap bitmap, int dif, int byteLength)
        {
            var ptr = bitmap.BackBuffer;
            if (dif > 0)
            {
                Windows.NativeMethods.Win32Api.RtlMoveMemory(ptr, ptr + dif, byteLength);
            }
            else
            {
                Windows.NativeMethods.Win32Api.RtlMoveMemory(ptr - dif, ptr, byteLength);
            }
        }
    }
}
