using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Media
{
    public static unsafe partial class Bitmap
    {
        extension (BitmapSource bitmap)
        {
            public Int32Rect Rect => new(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);

            public UIntBitmap ToUIntBitmap(UnmanagedArray<uint>? targetBuffer = null)
            {
                var width = bitmap.PixelWidth;
                var stride = width * 4;
                var height = bitmap.PixelHeight;
                var bufferData = new UIntBitmap(targetBuffer, width, height, false);
                if (bitmap.Format != PixelFormat)
                {
                    bitmap = new FormatConvertedBitmap(bitmap, PixelFormat, null, 0);
                }
                bitmap.CopyPixels(bitmap.Rect, bufferData.Pointer, height * stride, stride);
                return bufferData;
            }

            public void CopyPixels<T>(T target)
                where T : IBitmap
            {
                target.AssertType(false);
                var stride = target.Stride;
                bitmap.CopyPixels(bitmap.Rect, target.Pointer, target.Height * stride, stride);
            }

            public void CopyPixels<T>(T target, Int32Rect sourceRect)
                where T : IBitmap
            {
                target.AssertType(false);
                var stride = target.Stride;
                bitmap.CopyPixels(sourceRect, target.Pointer, target.Height * stride, stride);
            }

            public Int32Rect GetOpaqueRect(int margin = 0, byte transparentAlpha = 0, UnmanagedArray<uint>? buffer = null)
            {
                var rect = bitmap.ToUIntBitmap(buffer).GetOpaqueRect(margin, transparentAlpha);
                return rect.ToInt32Rect();
            }
        }

        extension (WriteableBitmap bitmap)
        {
            public BitmapPointer BeginRead() => new(bitmap, false);
            public BitmapPointer BeginWrite() => new(bitmap, true);

            public Color GetPixel(int x, int y)
            {
                if ((uint)x < (uint)bitmap.PixelWidth || (uint)y <= (uint)bitmap.PixelHeight)
                {
                    using var b = bitmap.BeginRead();
                    return MediaUtils.ToColor(*(uint*)b.Offset(x, y));
                }
                else
                {
                    return Colors.Transparent;
                }
            }

            public void SetPixel(int x, int y, Color color)
            {
                if ((uint)x < (uint)bitmap.PixelWidth || (uint)y < (uint)bitmap.PixelHeight)
                {
                    using var b = bitmap.BeginWrite();
                    *(uint*)b.Offset(x, y) = color.ToUInt();
                }
            }

            public void Clear()
            {
                using var b = bitmap.BeginWrite();
                b.Clear();
            }

            public void Clear(Int32Rect rect)
            {
                using var b = bitmap.BeginWrite();
                b.Clear(rect.ToDrawingRect());
            }

            public Int32Rect GetOpaqueRect(int margin = 0, byte transparentAlpha = 0)
            {
                using var p = bitmap.BeginRead();
                return p.GetOpaqueRect(margin, transparentAlpha).ToInt32Rect();
            }
        }
    }
}
