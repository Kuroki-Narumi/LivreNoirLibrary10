using System;
using System.Drawing;
using System.Buffers;
using System.Numerics;
using System.Threading.Tasks;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public static unsafe partial class BitmapOperation
    {
        extension<T> (T bitmap) where T : IBitmap
        {
            public void Fill<TElement>(params ReadOnlySpan<TElement> value)
                where TElement: unmanaged
            {
                if (bitmap.IsValid)
                {
                    FillCore(bitmap.Pointer, bitmap.Height * bitmap.Stride, VectorUtils.CreateRepeating(value));
                }
            }

            public void Fill<TElement>(Rectangle rect, params ReadOnlySpan<TElement> value)
                where TElement : unmanaged
            {
                if (bitmap.Adjust(ref rect))
                {
                    var vector = VectorUtils.CreateRepeating(value);
                    foreach (var (p, stride) in bitmap.EnumerateLines(rect))
                    {
                        FillCore(p, stride, vector);
                    }
                }
            }

            public void Fill<TElement>(Vector<TElement> value)
                where TElement: unmanaged
            {
                if (bitmap.IsValid)
                {
                    FillCore(bitmap.Pointer, bitmap.Height * bitmap.Stride, value);
                }
            }

            public void Fill<TElement>(Rectangle rect, Vector<TElement> value)
                where TElement : unmanaged
            {
                if (bitmap.Adjust(ref rect))
                {
                    foreach (var (p, stride) in bitmap.EnumerateLines(rect))
                    {
                        FillCore(p, stride, value);
                    }
                }
            }

            public void Fill(LnColor color)
            {
                if (bitmap.IsFloat)
                {
                    Fill(bitmap, color.ToFloatColor().AsVector());
                }
                else
                {
                    Fill(bitmap, (uint)color);
                }
            }

            public void Fill(Rectangle rect, LnColor color)
            {
                if (bitmap.IsFloat)
                {
                    Fill(bitmap, rect, color.ToFloatColor().AsVector());
                }
                else
                {
                    Fill(bitmap, rect, (uint)color);
                }
            }

            public void Fill(FloatColor color)
            {
                if (bitmap.IsFloat)
                {
                    Fill(bitmap, color.AsVector());
                }
                else
                {
                    Fill(bitmap, (uint)color.ToByteColor());
                }
            }

            public void Fill(Rectangle rect, FloatColor color)
            {
                if (bitmap.IsFloat)
                {
                    Fill(bitmap, rect, color.AsVector());
                }
                else
                {
                    Fill(bitmap, rect, (uint)color.ToByteColor());
                }
            }

            public void Fill(Rectangle rect, LnColor color1, LnColor color2, bool vertical = false)
            {
                // 同じ色が指定されている場合は単色用のメソッドを呼ぶ
                if (color1 == color2)
                {
                    Fill(bitmap, rect, color1);
                }
                else if (bitmap.Adjust(ref rect))
                {
                    AssertType(bitmap, false);
                    var stride = rect.Width;
                    if (vertical)
                    {
                        var y = 0;
                        GradientColorProvider g = new(color1, color2, rect.Height);
                        foreach (var (p, _) in bitmap.EnumerateLines(rect))
                        {
                            SimdOperations.CopyFrom((uint*)p, g.Get(y), stride);
                            y++;
                        }
                    }
                    else
                    {
                        GradientColorProvider g = new(color1, color2, stride);
                        // グラデ色のキャッシュ
                        var colors = stackalloc uint[stride];
                        for (var x = 0; x < stride; x++)
                        {
                            colors[x] = g.Get(x);
                        }
                        var xMin = rect.X;
                        var yMax = rect.Y + rect.Height;
                        Parallel.For(rect.Y, yMax, y =>
                        {
                            var pointer = (uint*)bitmap.Offset(xMin, y);
                            for (var x = 0; x < stride; x++, pointer++)
                            {
                                *pointer = colors[x];
                            }
                        });
                    }
                }
            }

            public void FillTriangle(Triangle triangle, LnColor color)
            {
                if (bitmap.IsValid)
                {
                    AssertType(bitmap, false);
                    var pointer = (uint*)bitmap.Pointer;
                    var width = bitmap.Width;
                    var height = bitmap.Height;
                    var c = (uint)color;
                    foreach (var (left, right, y) in triangle)
                    {
                        if ((uint)y < (uint)height)
                        {
                            var x = Math.Clamp(left, 0, width - 1);
                            var w = Math.Clamp(right, 0, width - 1) - x + 1;
                            SimdOperations.CopyFrom(pointer + x + y * width, c, w);
                        }
                    }
                }
            }

            public void FillTriangle(Triangle triangle, LnColor color1, LnColor color2, bool radial = false)
            {
                // 同じ色が指定されている場合は単色用のメソッドを呼ぶ
                if (color1 == color2)
                {
                    FillTriangle(bitmap, triangle, color1);
                }
                else
                {
                    AssertType(bitmap, false);
                    var pointer = (uint*)bitmap.Pointer;
                    var w = bitmap.Width;
                    var h = bitmap.Height;
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
                        GradientColorProvider g = new(color1, color2, den, true);
                        foreach (var (left, right, y) in triangle)
                        {
                            if ((uint)y < (uint)h)
                            {
                                for (var x = left; x <= right; x++)
                                {
                                    if ((uint)x < (uint)w)
                                    {
                                        var d = den - Math.Abs(dy * x + dx * y + cross) * den2;
                                        pointer[x + y * w] = (uint)g.Get(d.RoundToInt());
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
                        GradientColorProvider g = new(color1, color2, den, true);
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
                                        pointer[x + y * w] = (uint)g.Get(dif);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            public void DrawBorder(int thickness, LnColor color, bool keepSource = false, UnmanagedArray<uint>? buffer = null)
            {
                if (!bitmap.IsValid)
                {
                    return;
                }
                AssertType(bitmap, false);
                var pointer = (uint*)bitmap.Pointer;
                var width = bitmap.Width;
                var height = bitmap.Height;
                // アルファは無視
                var c = (uint)color & ColorUtils.GetMask(ColorFlags.RGB);

                // 透明判定の一時保存用バッファ
                var needDispose = buffer is null;
                buffer ??= new();
                buffer.EnsureSize(width * height + thickness + 1, false);
                var bufferPtr = buffer.Pointer;

                // 円範囲のキャッシュ
                var dxCache = (int*)buffer.Pointer + width * height;
                var th2 = thickness * thickness;
                for (var dx = 0; dx <= thickness; dx++)
                {
                    dxCache[dx] = (int)Math.Sqrt(th2 - dx * dx);
                }

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
                                alpha = Math.Max(alpha, SimdOperations.Max(pointer + (y - dy) * width + left, dWidth));
                            }
                            if (y + dy < height)
                            {
                                alpha = Math.Max(alpha, SimdOperations.Max(pointer + (y + dy) * width + left, dWidth));
                            }
                        }
                        bufferPtr[index] = c | (alpha & ColorUtils.GetMask(ColorFlags.A));
                        index++;
                    }
                });

                if (keepSource)
                {
                    ColorBlend.BlendUIntToUInt(bufferPtr, width, pointer, width, width, height, ColorBlend.Alpha, Vector<float>.One);
                }
                SimdOperations.CopyFrom(pointer, buffer.Pointer, width * height);

                if (needDispose)
                {
                    buffer.Dispose();
                }
            }

            public void DrawBorderSimple(int thickness, LnColor color, UnmanagedArray<uint>? buffer = null)
            {
                AssertType(bitmap, false);
                var pointer = (uint*)bitmap.Pointer;
                var width = bitmap.Width;
                var height = bitmap.Height;
                var size = width * height;
                // アルファは無視
                var c = (uint)color & ColorUtils.GetMask(ColorFlags.RGB);

                // 透明判定の一時保存用バッファ
                var needDispose = buffer is null;
                buffer ??= new();
                buffer.EnsureSize(size, false);
                var bufferPtr = buffer.Pointer;

                // 全体のコピー
                SimdOperations.CopyFrom(bufferPtr, pointer, size);
                // パス1: 水平方向のA最大値をチェック
                for (var y = 0; y < height; y++)
                {
                    var src = pointer + y * width;
                    var dst = bufferPtr + y * width;
                    for (var dx = 1; dx <= thickness; dx++)
                    {
                        var len = width - dx;
                        SimdOperations.Max(dst, src + dx, len);
                        SimdOperations.Max(dst + dx, src, len);
                    }
                }
                SimdOperations.CopyFrom(pointer, bufferPtr, size);
                // パス2: 垂直方向のA最大値をチェック
                for (var y = 0; y < height; y++)
                {
                    var dst = pointer + y * width;
                    for (var dy = Math.Max(-y, -thickness); dy < 0; dy++)
                    {
                        SimdOperations.Max(dst, bufferPtr + (y + dy) * width, width);
                    }
                    for (var dy = Math.Min(height - y - 1, thickness); dy > 0; dy--)
                    {
                        SimdOperations.Max(dst, bufferPtr + (y + dy) * width, width);
                    }
                }

                // アルファマスクの適用
                SimdOperations.And(pointer, ColorUtils.GetMask(ColorFlags.A), size);
                SimdOperations.Or(pointer, c, size);

                if (needDispose)
                {
                    buffer.Dispose();
                }
            }
        }

        private static void FillCore<TElement>(nint pointer, int byteCount, Vector<TElement> value)
            where TElement : unmanaged
        {
            var count = Vector<TElement>.Count;
            var elementCount = byteCount / sizeof(TElement);
            var vector = (Vector<TElement>*)pointer;
            for (; elementCount >= count; elementCount -= count, vector++)
            {
                *vector = value;
            }
            var ePointer = (TElement*)vector;
            for (var i = 0; i < elementCount; i++, ePointer++)
            {
                *ePointer = value[i];
            }
        }
    }
}
