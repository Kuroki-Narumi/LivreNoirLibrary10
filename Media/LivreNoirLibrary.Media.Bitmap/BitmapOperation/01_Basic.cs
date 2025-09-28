using System;
using System.Drawing;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public static unsafe partial class BitmapOperation
    {
        public static void Clear(LnBitmapData bitmap)
        {
            if (bitmap.IsValid)
            {
                SimdOperations.Clear(bitmap.Pointer, bitmap.PixelSize);
            }
        }

        public static void Clear(LnBitmapData bitmap, Rectangle rect)
        {
            if (bitmap.IsValid && Structs.Adjust(ref rect, bitmap))
            {
                var w = bitmap.Width;
                foreach (var p in bitmap.EnumerateLines(rect))
                {
                    SimdOperations.Clear(p, w);
                }
            }
        }

        public static Rectangle GetOpaqueRect(LnBitmapData bitmap, int margin, uint threshold)
        {
            if (!bitmap.IsValid)
            {
                return default;
            }
            var (p, w, h) = bitmap;
            var left = w;
            var right = 0;
            var top = -1;
            var bottom = -1;
            threshold <<= 24; // Alphaの位置にビットシフト
            for (var y = 0; y < h; y++)
            {
                var currentLeft = -1;
                var currentRight = -1;
                for (var x = 0; x < w; x++)
                {
                    if (*p >= threshold)
                    {
                        if (currentLeft is -1)
                        {
                            currentLeft = x;
                        }
                        currentRight = x;
                    }
                    p++;
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
                return default;
            }
            else
            {
                return new(Math.Max(left - margin, 0), Math.Max(top - margin, 0), Math.Min(right - left + 1 + margin, w), Math.Min(bottom - top + 1 + margin, bitmap.Height));
            }
        }
    }
}