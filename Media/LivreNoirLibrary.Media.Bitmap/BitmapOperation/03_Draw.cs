using System;
using System.Drawing;
using System.Buffers;
using System.Threading.Tasks;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public static unsafe partial class BitmapOperation
    {
        public static void FillRect(this LnBitmapData bitmap, Rectangle rect, LnColor color)
        {
            if (bitmap.IsValid && Structs.Adjust(ref rect, bitmap))
            {
                var c = (uint)color;
                var w = rect.Width;
                foreach (var p in bitmap.EnumerateLines(rect))
                {
                    SimdOperations.CopyFrom(p, c, w);
                }
            }
        }

        public static void FillRect(this LnBitmapData bitmap, Rectangle rect, LnColor color1, LnColor color2, bool vertical = false)
        {
            // 同じ色が指定されている場合は単色用のメソッドを呼ぶ
            if (color1 == color2)
            {
                FillRect(bitmap, rect, color1);
            }
            else if (bitmap.IsValid && Structs.Adjust(ref rect, bitmap))
            {
                var w = rect.Width;
                if (vertical)
                {
                    var y = 0;
                    GradientColorGetter g = new(color1, color2, rect.Height);
                    foreach (var p in bitmap.EnumerateLines(rect))
                    {
                        SimdOperations.CopyFrom(p, g.Get(y), w);
                        y++;
                    }
                }
                else
                {
                    GradientColorGetter g = new(color1, color2, w);
                    foreach (var p in bitmap.EnumerateLines(rect))
                    {
                        for (var x = 0; x < w; x++)
                        {
                            p[x] = g.Get(x);
                        }
                    }
                }
            }
        }

        public static void FillTriangle(this LnBitmapData bitmap, Triangle triangle, LnColor color)
        {
            var (p, w, h) = bitmap;
            var c = (uint)color;
            foreach (var (left, right, y) in triangle)
            {
                if ((uint)y < (uint)h)
                {
                    var x = Math.Clamp(left, 0, w - 1);
                    var width = Math.Clamp(right, 0, w - 1) - x + 1;
                    SimdOperations.CopyFrom(p + y * w + x, c, width);
                }
            }
        }

        public static void FillTriangle(this LnBitmapData bitmap, Triangle triangle, LnColor color1, LnColor color2, bool radial = false)
        {
            // 同じ色が指定されている場合は単色用のメソッドを呼ぶ
            if (color1 == color2)
            {
                FillTriangle(bitmap, triangle, color1);
            }
            else
            {
                var (p, w, h) = bitmap;
                var (x0, y0, x1, y1, x2, y2) = triangle;
                if (radial)
                {
                    // 基準直線
                    // (y2 - y1) * x + (x1 - x2) * y + x2 * y1 - x1 * y2
                    var dx = x1 - x2;
                    var dy = y2 - y1;
                    var cross = x2 * y1 - x1 * y2;
                    var den2 = MathF.ReciprocalSqrtEstimate(dx * dx + dy + dy) * 2f;
                    var den = MathF.Abs(dy * x0 + dx * y0 + cross) * den2;
                    GradientColorGetter g = new(color1, color2, den);
                    foreach (var (left, right, y) in triangle)
                    {
                        if ((uint)y < (uint)h)
                        {
                            for (var x = left; x <= right; x++)
                            {
                                if ((uint)x < (uint)w)
                                {
                                    var d = den - Math.Abs(dy * x + dx * y + cross) * den2;
                                    p[x + y * w] = (uint)g.Get(d.RoundToInt());
                                }
                            }
                        }
                    }
                }
                else
                {
                    var dx = x2 - x1;
                    var dy = y2 - y1;
                    var cross = x2 * y1 - x1 * y2;
                    var (vertical, den) = Math.Abs(dx) >= Math.Abs(dy) ? (false, dx) : (true, dy);
                    GradientColorGetter g = new(color1, color2, den);
                    foreach (var (left, right, y) in triangle)
                    {
                        if ((uint)y < (uint)h)
                        {
                            var d = y - y0;
                            for (var x = left; x <= right; x++)
                            {
                                if ((uint)x < (uint)w)
                                {
                                    var e = (double)x - x0;
                                    var f = (double)x * y0 - x0 * y;
                                    var xx = (f * dx - cross * e) / (e * dy - d * dx);
                                    int dif;
                                    if (vertical)
                                    {
                                        double yy;
                                        if (dx == 0)
                                        {
                                            yy = (d * xx + f) / e;
                                        }
                                        else
                                        {
                                            yy = (dy * xx + cross) / dx;
                                        }
                                        dif = (yy - y1).RoundToInt();
                                    }
                                    else
                                    {
                                        dif = (xx - x1).RoundToInt();
                                    }
                                    p[x + y * w] = (uint)g.Get(dif);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void DrawBorder(this LnBitmapData bitmap, int thickness, LnColor color, bool keepSource = false)
        {
            var (p, w, h) = bitmap;
            var max = w * h;
            var buffer = ArrayPool<uint>.Shared.Rent(max);
            // アルファ値は無視
            var c = (uint)color & ~ColorUtils.Mask_A;
            try
            {
                fixed (uint* bufferPtr = buffer)
                {
                    // 円範囲のキャッシュ
                    var dxCache = stackalloc int[thickness + 1];
                    var th2 = thickness * thickness;
                    for (var dx = 0; dx <= thickness; dx++)
                    {
                        dxCache[dx] = (int)Math.Sqrt(th2 - dx * dx);
                    }
                    DetectAlphaCore(p, bufferPtr, dxCache, w, h, thickness, c);
                    if (keepSource)
                    {
                        ColorBlend.BlendCore(bufferPtr, w, p, w, w, h, ColorBlend.Alpha);
                    }
                    SimdOperations.CopyFrom(p, bufferPtr, max);
                }
            }
            finally
            {
                ArrayPool<uint>.Shared.Return(buffer);
            }
        }

        /// <summary>
        /// fixedコンテキスト内ではParallelが使えないので別メソッドに分離しておく
        /// </summary>
        private static unsafe void DetectAlphaCore(uint* source, uint* buffer, int* dxCache, int width, int height, int thickness, uint color)
        {
            // 行ごとに並列処理
            Parallel.For(0, height, y =>
            {
                var index = y * width;
                for (var x = 0; x < width; x++)
                {
                    var alpha = 0u;
                    for (var dy = 0; dy <= thickness; dy++)
                    {
                        var max = dxCache[dy];
                        var left = Math.Max(x - max, 0);
                        var dWidth = Math.Min(x + max, width - 1) - left + 1;
                        if (y - dy >= 0)
                        {
                            alpha = Math.Max(alpha, SimdOperations.Max(source + (y - dy) * width + left, dWidth));
                        }
                        if (y + dy < height)
                        {
                            alpha = Math.Max(alpha, SimdOperations.Max(source + (y + dy) * width + left, dWidth));
                        }
                    }
                    buffer[index] = color | (alpha & ColorUtils.Mask_A);
                    index++;
                }
            });
        }

        public static void DrawBorderSimple(this LnBitmapData bitmap, int thickness, LnColor color)
        {
            var (p, w, h) = bitmap;
            var max = w * h;
            var buffer = ArrayPool<uint>.Shared.Rent(max);
            // アルファ値は無視
            var c = (uint)color & ~ColorUtils.Mask_A;
            try
            {
                fixed (uint* bufferPtr = buffer)
                {
                    // 全体のコピー
                    SimdOperations.CopyFrom(bufferPtr, p, max);
                    // パス1: 水平方向のA最大値をチェック
                    for (var y = 0; y < h; y++)
                    {
                        var src = p + y * w;
                        var dst = bufferPtr + y * w;
                        for (var dx = 1; dx <= thickness; dx++)
                        {
                            var len = w - dx;
                            SimdOperations.Max(dst, src + dx, len);
                            SimdOperations.Max(dst + dx, src, len);
                        }
                    }
                    SimdOperations.CopyFrom(p, bufferPtr, max);
                    // パス2: 垂直方向のA最大値をチェック
                    for (var y = 0; y < h; y++)
                    {
                        var dst = p + y * w;
                        for (var dy = Math.Max(-y, -thickness); dy < 0; dy++)
                        {
                            SimdOperations.Max(dst, bufferPtr + (y + dy) * w, w);
                        }
                        for (var dy = Math.Min(h - y - 1, thickness); dy > 0; dy--)
                        {
                            SimdOperations.Max(dst, bufferPtr + (y + dy) * w, w);
                        }
                    }
                }
                // アルファマスクの適用
                SimdOperations.And(p, ColorUtils.Mask_A, max);
                SimdOperations.Or(p, c, max);
            }
            finally
            {
                ArrayPool<uint>.Shared.Return(buffer);
            }
        }
    }
}
