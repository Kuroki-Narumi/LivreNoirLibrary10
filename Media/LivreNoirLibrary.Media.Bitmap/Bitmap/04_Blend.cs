using LivreNoirLibrary.Collections;
using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Media
{
    public static unsafe partial class BitmapOperation
    {
        extension<T> (T bitmap) where T : IBitmap
        {
            public void Blend(Rectangle rect, BlendMode blend, LnColor color) => Blend(bitmap, rect, blend, color.ToFloatColor());
            public void Blend(BlendMode blend, LnColor color) => Blend(bitmap, bitmap.Rect, blend, color.ToFloatColor());
            public void Blend(BlendMode blend, FloatColor color) => Blend(bitmap, bitmap.Rect, blend, color);
        }

        public static void Blend<T>(this T bitmap, Rectangle rect, BlendMode blend, FloatColor color)
            where T : IBitmap
        {
            if (!Adjust(bitmap, ref rect) || !ColorBlend.TryGetBlendFunc(blend, out var func))
            {
                return;
            }
            if (bitmap.IsFloat)
            {
                ColorBlend.BlendFloat((float*)bitmap.Offset(rect.X, rect.Y), bitmap.Width, rect.Width, rect.Height, func, color);
            }
            else
            {
                ColorBlend.BlendUInt((uint*)bitmap.Offset(rect.X, rect.Y), bitmap.Width, rect.Width, rect.Height, func, color);
            }
        }

        /// <inheritdoc cref="CopyTo{TSource, TDest}(TSource, DoubleRect, TDest, DoubleRect, DoubleRect, BlendMode, FloatColor, FloatBitmap?, bool)"/>
        public static void CopyTo<TSource, TDest>(
            this TSource source, TDest destination,
            BlendMode blend, FloatColor colorCorrection,
            FloatBitmap? buffer = null, bool tweet = false)
            where TSource : IBitmap
            where TDest : IBitmap
            => CopyTo(source, source.DoubleRect, destination, destination.DoubleRect, destination.DoubleRect, blend, colorCorrection, buffer, tweet);

        /// <summary>
        /// <paramref name="source"/>から指定された矩形範囲を切り抜き、<paramref name="destination"/>へとコピーする。
        /// コピー先の矩形は<paramref name="destRect"/>で指定され、<paramref name="destValidRect"/>で規定された有効範囲内に矯正される。
        /// </summary>
        /// <param name="source">the bitmap object to copy from.</param>
        /// <param name="sourceRect">a rectangle that specified the area to crop from the <paramref name="source"/>.</param>
        /// <param name="destination">the bitmap object to copy to.</param>
        /// <param name="destValidRect">a rectangle that defines the valid range for the <paramref name="destination"/>.</param>
        /// <param name="destRect">a rectangle that specified the destination area.</param>
        /// <param name="blend">the blending mode.</param>
        /// <param name="colorCorrection">the color correction applied to the source before blending.</param>
        /// <param name="buffer">a bitmap object used to store the intermediate stretch copy result.</param>
        public static void CopyTo<TSource, TDest>(
            this TSource source, DoubleRect sourceRect,
            TDest destination, DoubleRect destValidRect, DoubleRect destRect,
            BlendMode blend, FloatColor colorCorrection,
            FloatBitmap? buffer = null, bool tweet = false)
            where TSource : IBitmap
            where TDest : IBitmap
        {
            if (tweet)
            {
                Console.WriteLine(nameof(CopyTo));
                Console.WriteLine($"  src valid={source.Rect}, src rect={sourceRect}");
                Console.WriteLine($"  dst valid={destValidRect}, dst rect={destRect}");
            }
            if (
                // 色補正のアルファが 0 (=完全な透明)の場合は何もしない
                colorCorrection.A is 0 ||
                // 範囲チェック
                !Adjust(source, sourceRect, destination, destValidRect, destRect, out var actualSourceRect, out var actualDestRect)
                )
            {
                return;
            }
            if (tweet)
            {
                Console.WriteLine($"  src actual rect={actualSourceRect}, dst actual rect={actualDestRect}");
                Console.WriteLine();
            }
            var (srcX, srcY, srcW, srcH) = actualSourceRect;
            var (destX, destY, destW, destH) = actualDestRect;
            // 拡縮の必要無し
            if (srcW == destW && srcH == destH)
            {
                BlendTo(source, srcX, srcY, destination, destX, destY, destW, destH, blend, colorCorrection);
                return;
            }

            var needDispose = buffer is null;
            buffer ??= new(0, 0);

            // 横方向の拡縮
            StretchCopy_Horizontal(source, srcX, srcY, srcW, srcH, buffer, destW);
            // 縦方向の拡縮&ブレンド
            StretchCopy_Vertical(buffer, srcH, destination, destX, destY, destW, destH, blend, colorCorrection);

            if (needDispose)
            {
                buffer.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void BlendTo<TSource, TDest>(
            TSource source, int sourceX, int sourceY,
            TDest destination, int destX, int destY,
            int width, int height,
            BlendMode blend, FloatColor colorCorrection
            )
            where TSource : IBitmap
            where TDest : IBitmap
        {
            var sourceP = source.Offset(sourceX, sourceY);
            var sourceWidth = source.Width;
            var destP = destination.Offset(destX, destY);
            var destWidth = destination.Width;
            // ブレンドメソッドが見つかった場合
            if (ColorBlend.TryGetBlendFunc(blend, out var func))
            {
                if (source.IsFloat)
                {
                    if (destination.IsFloat)
                    {
                        ColorBlend.BlendFloatToFloat((float*)sourceP, sourceWidth, (float*)destP, destWidth, width, height, func, colorCorrection);
                    }
                    else
                    {
                        ColorBlend.BlendFloatToUInt((float*)sourceP, sourceWidth, (uint*)destP, destWidth, width, height, func, colorCorrection);
                    }
                }
                else
                {
                    if (destination.IsFloat)
                    {
                        ColorBlend.BlendUIntToFloat((uint*)sourceP, sourceWidth, (float*)destP, destWidth, width, height, func, colorCorrection);
                    }
                    else
                    {
                        ColorBlend.BlendUIntToUInt((uint*)sourceP, sourceWidth, (uint*)destP, destWidth, width, height, func, colorCorrection);
                    }
                }
            }
            // 見つからなかった場合は全ピクセルを上書きする
            else
            {
                if (source.IsFloat)
                {
                    if (destination.IsFloat)
                    {
                        CopyFloatToFloat((float*)sourceP, sourceWidth, (float*)destP, destWidth, width, height, colorCorrection);
                    }
                    else
                    {
                        CopyFloatToUInt((float*)sourceP, sourceWidth, (uint*)destP, destWidth, width, height, colorCorrection);
                    }
                }
                else
                {
                    if (destination.IsFloat)
                    {
                        CopyUIntToFloat((uint*)sourceP, sourceWidth, (float*)destP, destWidth, width, height, colorCorrection);
                    }
                    else
                    {
                        CopyUIntToUInt((uint*)sourceP, sourceWidth, (uint*)destP, destWidth, width, height, colorCorrection);
                    }
                }
            }
        }

        #region Lanczos3 Kernel
        const int TableSize = 65536;
        const float Center = TableSize / 2;
        const float Scale = Center / 3 - 1;

        static readonly float[] _lanczos3Table = CreateLanczos3Table();

        static float[] CreateLanczos3Table()
        {
            var result = new float[TableSize];
            for (var i = 0; i < TableSize; i++)
            {
                var x = (i - Center) / Scale;
                if (x is 0)
                {
                    result[i] = 1;
                }
                else if (x is >= 3 or <= -3)
                {
                    result[i] = 0;
                }
                else
                {
                    var pix = MathF.PI * x;
                    result[i] = (MathF.Sin(pix) / pix) * (MathF.Sin(pix / 3) / (pix / 3));
                }
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float Lanczos3Kernel(float x)
        {
            x = x * Scale + Center;
            var index = (int)x;
            var y0 = _lanczos3Table[index];
            var y1 = _lanczos3Table[index + 1];
            return y0 + (y1 - y0) * (x - index);
        }
        #endregion

        static void CopyFloatToFloat(float* source, int sourceWidth, float* destination, int destWidth, int width, int height, Vector<float> colorCorrection)
        {
            Parallel.For(0, height, y =>
            {
                SimdOperations.CopyFrom(destination + y * destWidth * 4, source + y * sourceWidth * 4, colorCorrection, width * 4);
            });
        }

        static void CopyFloatToUInt(float* source, int sourceWidth, uint* destination, int destWidth, int width, int height, Vector<float> colorCorrection)
        {
            var count = Vector<float>.Count;
            Parallel.For(0, height, y =>
            {
                var w = width * 4;
                var srcP = (Vector<float>*)(source + y * sourceWidth);
                var dstP = (byte*)(destination + y * destWidth);
                var buffer = stackalloc float[count];
                for (; w >= count; w -= count)
                {
                    *(Vector<float>*)buffer = *srcP++ * colorCorrection;
                    BufferToByte(ref dstP, buffer, count);
                }
                if (w is > 0)
                {
                    *(Vector<float>*)buffer = FillRemain(srcP, w) * colorCorrection;
                    BufferToByte(ref dstP, buffer, w);
                }
            });
        }

        static void CopyUIntToFloat(uint* source, int sourceWidth, float* destination, int destWidth, int width, int height, Vector<float> colorCorrection)
        {
            var count = Vector<float>.Count;
            Parallel.For(0, height, y =>
            {
                var w = width * 4;
                var srcP = (byte*)(source + y * sourceWidth);
                var dstP = (Vector<float>*)(destination + y * destWidth);
                var buffer = stackalloc float[count];
                for (; w >= count; w -= count)
                {
                    ByteToBuffer(ref srcP, buffer, count);
                    *dstP++ = *(Vector<float>*)buffer * colorCorrection;
                }
                if (w is > 0)
                {
                    ByteToBuffer(ref srcP, buffer, count);
                    VectorToFloat(dstP, *(Vector<float>*)buffer * colorCorrection, w);
                }
            });
        }

        static void CopyUIntToUInt(uint* source, int sourceWidth, uint* destination, int destWidth, int width, int height, Vector<float> colorCorrection)
        {
            if (colorCorrection == Vector<float>.One)
            {
                Parallel.For(0, height, y =>
                {
                    SimdOperations.CopyFrom(destination + y * destWidth, source + y * sourceWidth, width);
                });
            }
            else
            {
                var count = Vector<float>.Count;
                Parallel.For(0, height, y =>
                {
                    var w = width * 4;
                    var srcP = (byte*)(source + y * sourceWidth);
                    var dstP = (byte*)(destination + y * destWidth);
                    var buffer = stackalloc float[count];
                    for (; w >= count; w -= count)
                    {
                        ByteToBuffer(ref srcP, buffer, count);
                        *(Vector<float>*)buffer *= colorCorrection;
                        BufferToByte(ref dstP, buffer, count);
                    }
                    if (w is > 0)
                    {
                        ByteToBuffer(ref srcP, buffer, w);
                        *(Vector<float>*)buffer *= colorCorrection;
                        BufferToByte(ref dstP, buffer, w);
                    }
                });
            }
        }

        static void StretchCopy_Horizontal<T>(T source, int srcX, int srcY, int srcW, int srcH, FloatBitmap destination, int destW)
            where T : IBitmap
        {
            // 拡大率
            var scaleX = (float)srcW / destW;
            // Lanczos3Kernelで参照する元画像の範囲
            var rangeX = 3f;
            var distanceFactorX = 1f;
            // 縮小する場合は元画像の範囲を大きくする
            if (scaleX is > 1)
            {
                rangeX *= scaleX;
                distanceFactorX /= scaleX;
            }

            // 重みを考慮したバッファサイズの決定
            var weightXCount = (int)(rangeX + 0.5f) * 2 + 1;
            destination.Resize(destW, srcH + weightXCount / 4 + 1, false);

            // 重みの事前計算
            var weightBegin = (float*)destination.Offset(srcH);
            var pointer = weightBegin;
            for (var dx = 0; dx < destW; dx++)
            {
                // 出力ピクセル中心に対応する source 座標
                var srcCenterX = (dx + 0.5f) * scaleX - 0.5f;
                // 参照範囲
                var minX = Math.Max((int)Math.Ceiling(srcCenterX - rangeX), 0);
                var maxX = Math.Min((int)Math.Floor(srcCenterX + rangeX), srcW - 1);
                *(pointer++) = minX;
                *(pointer++) = maxX - minX;
                for (var x = minX; x <= maxX; x++)
                {
                    pointer[x - minX] = Lanczos3Kernel((x - srcCenterX) * distanceFactorX);
                }
                pointer += weightXCount;
            }

            // 横拡縮
            if (source.IsFloat)
            {
                var hSrcPointer = (Vector128<float>*)source.Offset(srcX, srcY);
                var alphaSelector = Vector128.Equals(Vector128<float>.One, Vector128.Create(0, 0, 0, 1f));
                Parallel.For(0, srcH, y =>
                {
                    var destPointer = (Vector128<float>*)destination.Offset(y);
                    var fixedOffset = source.Width * y;
                    for (var dx = 0; dx < destW; dx++, destPointer++)
                    {
                        // 出力ピクセル中心に対応する source 座標
                        var srcCenterX = (dx + 0.5f) * scaleX - 0.5f;
                        // 重みキャッシュ
                        var weights = weightBegin + dx * (weightXCount + 2);
                        // 参照範囲
                        var minX = (int)*(weights++);
                        var range = (int)*(weights++);
                        var srcPointer = hSrcPointer + minX + fixedOffset;
                        var weightSum = 0f;
                        *destPointer = Vector128<float>.Zero;
                        for (var x = 0; x <= range; x++, srcPointer++, weights++)
                        {
                            var weight = *weights;
                            weightSum += weight;
                            var alpha = FloatColor.FillAlphaToAll(*srcPointer * weight);
                            var rgb = *srcPointer * alpha;
                            *destPointer += Vector128.ConditionalSelect(alphaSelector, alpha, rgb);
                        }
                        if (weightSum is > 0)
                        {
                            *destPointer /= weightSum;
                        }
                    }
                });
            }
            else
            {
                var hSrcPointer = (byte*)source.Offset(srcX, srcY);
                Parallel.For(0, srcH, y =>
                {
                    var destPointer = (float*)destination.Offset(y);
                    var fixedOffset = y * source.Width * 4;
                    for (var dx = 0; dx < destW; dx++, destPointer += 4)
                    {
                        // 出力ピクセル中心に対応する source 座標
                        var srcCenterX = (dx + 0.5f) * scaleX - 0.5f;
                        // 重みキャッシュ
                        var weights = weightBegin + dx * (weightXCount + 2);
                        // 参照範囲
                        var minX = (int)*(weights++);
                        var range = (int)*(weights++);
                        var srcPointer = hSrcPointer + minX * 4 + fixedOffset;
                        var weightSum = 0f;
                        destPointer[0] = destPointer[1] = destPointer[2] = destPointer[3] = 0;
                        for (var x = 0; x <= range; x++, srcPointer += 4, weights++)
                        {
                            var weight = *weights;
                            weightSum += weight;
                            var alpha = ColorUtils.GetFloat(srcPointer[ColorUtils.ColorIndex_A]) * weight;
                            destPointer[ColorUtils.ColorIndex_B] += ColorUtils.RgbToScRgb(srcPointer[ColorUtils.ColorIndex_B]) * alpha;
                            destPointer[ColorUtils.ColorIndex_G] += ColorUtils.RgbToScRgb(srcPointer[ColorUtils.ColorIndex_G]) * alpha;
                            destPointer[ColorUtils.ColorIndex_R] += ColorUtils.RgbToScRgb(srcPointer[ColorUtils.ColorIndex_R]) * alpha;
                            destPointer[ColorUtils.ColorIndex_A] += alpha;
                        }
                        if (weightSum is > 0)
                        {
                            *(Vector128<float>*)destPointer /= weightSum;
                        }
                    }
                });
            }
        }

        internal static void StretchCopy_Vertical<T>(FloatBitmap source, int sourceHeight, T destination, int destX, int destY, int destW, int destH, BlendMode blend, Vector<float> colorCorrection)
            where T : IBitmap
        {
            var scaleY = (float)sourceHeight / destH;
            // Lanczos3Kernelで参照する元画像の範囲
            var rangeY = 3f;
            var distanceFactorY = 1f;
            // 縮小する場合は元画像の範囲を大きくする
            if (scaleY is > 1)
            {
                rangeY *= scaleY;
                distanceFactorY /= scaleY;
            }

            // 重みを考慮したバッファサイズの決定
            var weightYCount = (int)(rangeY + 0.5f) * 2 + 1;
            source.Resize(destW, sourceHeight + weightYCount / 4 + 1, false);
            var weightBegin = (float*)source.Offset(sourceHeight);

            var count = Vector<float>.Count;
            var vSrcPointer = (Vector128<float>*)source.Pointer;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void CalculateWeightY(in int dy, out float* sourceBegin, out float* weights, out int range)
            {
                // 出力ピクセル中心に対応する source 座標
                var srcCenterY = (dy + 0.5f) * scaleY - 0.5f;
                // 参照範囲
                var minY = Math.Max((int)Math.Ceiling(srcCenterY - rangeY), 0);
                var maxY = Math.Min((int)Math.Floor(srcCenterY + rangeY), sourceHeight - 1);
                range = maxY - minY;
                // 重みの事前計算
                weights = weightBegin + dy * weightYCount;
                for (var y = minY; y <= maxY; y++)
                {
                    weights[y - minY] = Lanczos3Kernel((y - srcCenterY) * distanceFactorY);
                }
                sourceBegin = (float*)source.Offset(minY);
            }

            ColorBlend.TryGetBlendFunc(blend, out var func);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void BlendProcess(ref Vector<float> back, in int dx, in int range, in float* sourceBegin, in int stride, in float* weights)
            {
                // ブレンドする色を計算
                var front = Vector<float>.Zero;
                var weightSum = 0f;
                var sourceP = sourceBegin + dx;
                for (var y = 0; y <= range; y++, sourceP += stride)
                {
                    var weight = weights[y];
                    weightSum += weight;
                    front += *(Vector<float>*)sourceP * weight;
                }
                if (weightSum is > 0)
                {
                    front /= weightSum;
                }

                front = Vector.Clamp(front * colorCorrection, Vector<float>.Zero, Vector<float>.One);
                if (func is not null)
                {
                    ColorBlend.BlendCore(ref back, FloatColor.FillAlphaToAll(back), front, FloatColor.FillAlphaToAll(front), func);
                }
                else
                {
                    back = front;
                }
            }

            if (destination.IsFloat)
            {
                Parallel.For(0, destH, dy =>
                {
                    CalculateWeightY(dy, out var sourceBegin, out var weights, out var range);
                    var backP = (Vector<float>*)destination.Offset(destX, dy + destY);

                    var w = destW * 4;
                    var dx = 0;
                    var stride = destW * 4;
                    for (; w >= count; w -= count, dx += count, backP++)
                    {
                        var back = *backP;
                        BlendProcess(ref back, dx, range, sourceBegin, stride, weights);
                        *backP = back;
                    }
                    if (w is > 0)
                    {
                        var back = FillRemain(backP, w);
                        BlendProcess(ref back, dx, range, sourceBegin, stride, weights);
                        VectorToFloat(backP, back, w);
                    }
                });
            }
            else
            {
                Parallel.For(0, destH, dy =>
                {
                    CalculateWeightY(dy, out var sourceBegin, out var weights, out var range);
                    // float変換用の作業バッファ
                    var backP = (byte*)destination.Offset(destX, dy + destY);
                    var backBuffer = stackalloc float[count];

                    var w = destW * 4;
                    var dx = 0;
                    var stride = destW * 4;
                    for (; w >= count; w -= count, dx += count)
                    {
                        Process(backBuffer, count);
                    }
                    if (w is > 0)
                    {
                        Process(backBuffer, w);
                    }

                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    void Process(float* backBuffer, int count)
                    {
                        // 背景を正規化してベクトル化
                        ByteToBuffer(ref backP, backBuffer, count, true);
                        // ブレンド
                        var back = *(Vector<float>*)backBuffer;
                        BlendProcess(ref back, dx, range, sourceBegin, stride, weights);
                        // バッファへ書き戻す
                        *(Vector<float>*)backBuffer = back;
                        BufferToByte(ref backP, backBuffer, count);
                    }
                });
            }
        }
    }
}
