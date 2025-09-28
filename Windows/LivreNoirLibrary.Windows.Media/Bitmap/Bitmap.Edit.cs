using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Media
{
    public static partial class Bitmap
    {
        public static bool AdjustRect(BitmapSource bitmap, ref int x, ref int y, ref int width, ref int height)
            => Structs.Adjust(ref x, ref y, ref width, ref height, bitmap.PixelWidth, bitmap.PixelHeight);

        public static unsafe void SetColor(this WriteableBitmap bitmap, ColorIndex index, byte value)
        {
            using BitmapPointer p = new(bitmap);
            BitmapOperation.SetColor(p, index, value);
        }

        public static unsafe void SetColor(this WriteableBitmap bitmap, int x, int y, int width, int height, ColorIndex index, byte value)
        {
            using BitmapPointer p = new(bitmap);
            BitmapOperation.SetColor(p, new(x, y, width, height), index, value);
        }

        public static void SetColor(this WriteableBitmap bitmap, Int32Rect rect, ColorIndex index, byte value) => SetColor(bitmap, rect.X, rect.Y, rect.Width, rect.Height, index, value);

        public static unsafe void SetColor(this WriteableBitmap bitmap, ColorIndex from, ColorIndex to)
        {
            if (to == from) { return; }
            using BitmapPointer p = new(bitmap);
            BitmapOperation.SetColor(p, from, to);
        }

        public static unsafe void SetColor(this WriteableBitmap bitmap, int x, int y, int width, int height, ColorIndex from, ColorIndex to)
        {
            if (to == from) { return; }
            using BitmapPointer p = new(bitmap);
            BitmapOperation.SetColor(p, new(x, y, width, height), from, to);
        }

        public static void SetColor(this WriteableBitmap bitmap, Int32Rect rect, ColorIndex from, ColorIndex to) => SetColor(bitmap, rect.X, rect.Y, rect.Width, rect.Height, to, from);

        public static unsafe void SwapColor(this WriteableBitmap bitmap, ColorIndex index1, ColorIndex index2)
        {
            if (index1 == index2) { return; }
            using BitmapPointer p = new(bitmap);
            BitmapOperation.SwapColor(p, index1, index2);
        }

        public static unsafe void SwapColor(this WriteableBitmap bitmap, int x, int y, int width, int height, ColorIndex index1, ColorIndex index2)
        {
            if (index1 == index2) { return; }
            using BitmapPointer p = new(bitmap);
            BitmapOperation.SwapColor(p, new(x, y, width, height), index1, index2);
        }

        public static void SwapColor(this WriteableBitmap bitmap, Int32Rect rect, ColorIndex index1, ColorIndex index2) => SwapColor(bitmap, rect.X, rect.Y, rect.Width, rect.Height, index1, index2);

        public static unsafe void InvertColor(this WriteableBitmap bitmap)
        {
            using BitmapPointer p = new(bitmap);
            BitmapOperation.InvertColor(p);
        }

        public static unsafe void InvertColor(this WriteableBitmap bitmap, int x, int y, int width, int height)
        {
            using BitmapPointer p = new(bitmap);
            BitmapOperation.InvertColor(p, new System.Drawing.Rectangle(x, y, width, height));
        }

        public static void InvertColor(this WriteableBitmap bitmap, Int32Rect rect) => InvertColor(bitmap, rect.X, rect.Y, rect.Width, rect.Height);

        public static unsafe void InvertColor(this WriteableBitmap bitmap, ColorIndex index)
        {
            using BitmapPointer p = new(bitmap);
            BitmapOperation.InvertColor(p, index);
        }

        public static unsafe void InvertColor(this WriteableBitmap bitmap, int x, int y, int width, int height, ColorIndex index)
        {
            using BitmapPointer p = new(bitmap);
            BitmapOperation.InvertColor(p, new(x, y, width, height), index);
        }

        public static void InvertColor(this WriteableBitmap bitmap, Int32Rect rect, ColorIndex index) => InvertColor(bitmap, rect.X, rect.Y, rect.Width, rect.Height, index);

        public static unsafe void SetTransparent(this WriteableBitmap bitmap, Color color)
        {
            using BitmapPointer p = new(bitmap);
            BitmapOperation.SetTransparent(p, color.ToLnColor());
        }

        public static unsafe void SetTransparent(this WriteableBitmap bitmap, int x, int y, int width, int height, Color color)
        {
            using BitmapPointer p = new(bitmap);
            BitmapOperation.SetTransparent(p, new(x, y, width, height), color.ToLnColor());
        }

        public static void SetTransparent(this WriteableBitmap bitmap, Int32Rect rect, Color color) => SetTransparent(bitmap, rect.X, rect.Y, rect.Width, rect.Height, color);

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
            if (!AdjustRect(bitmap, ref x, ref y, ref width, ref height))
            {
                return;
            }
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
            var a = AdjustRect(source, ref srcX, ref srcY, ref srcWidth, ref srcHeight);
            var b = AdjustRect(bitmap, ref dstX, ref dstY, ref srcWidth, ref srcHeight);
            if (!(a && b))
            {
                return;
            }
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
                        Buffer.MemoryCopy((void*)srcPtr, (void*)dstPtr, dstStride, dstStride);
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
            if (AdjustRect(bitmap, ref x, ref y, ref width, ref height))
            {
                var result = Create(width, height);
                CopyFrom(result, 0, 0, bitmap, x, y, width, height);
                return result;
            }
            return bitmap;
        }

        public static WriteableBitmap Crop(this WriteableBitmap bitmap, Int32Rect rect) => Crop(bitmap, rect.X, rect.Y, rect.Width, rect.Height);

        public static unsafe void MoveMemory(this WriteableBitmap bitmap, int dif, int byteLength)
        {
            var ptr = bitmap.BackBuffer;
            if (dif > 0)
            {
                Buffer.MemoryCopy((void*)ptr, (void*)(ptr + dif), byteLength, byteLength);
            }
            else
            {
                Buffer.MemoryCopy((void*)(ptr - dif), (void*)ptr, byteLength, byteLength);
            }
        }
    }
}