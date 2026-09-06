using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Media
{
    partial class BitmapOperation
    {
        /// <summary>
        /// <paramref name="source"/>の指定された矩形範囲
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="destLocation"></param>
        /// <param name="source"></param>
        /// <param name="sourceRect"></param>
        /// <param name="colorIndex"></param>
        /// <param name="buffer"></param>
        public static void MaskTo<TSource, TDest>(
            this TSource source, DoubleRect sourceRect, 
            TDest destination, DoubleRect destValidRect, DoubleRect destRect, 
            ColorIndex colorIndex = ColorIndex.R, FloatBitmap? buffer = null)
            where TSource : IBitmap
            where TDest : IBitmap
        {
            AssertType(source, false);
            AssertType(destination, false);
            if (!Adjust(source, sourceRect, destination, destValidRect, destRect, out var actualSourceRect, out var actualDestRect))
            {
                return;
            }
            var (srcX, srcY, srcW, srcH) = actualSourceRect;
            var (destX, destY, destW, destH) = actualDestRect;

            var needDispose = buffer is null;
            buffer ??= new(0, 0);

            // 横方向の拡縮
            StretchCopy_Horizontal(source, srcX, srcY, srcW, srcH, buffer, destW);
            // 縦方向の拡縮&マスク適用
            StretchMask_Vertical(buffer, srcH, destination, destX, destY, destW, destH, colorIndex);

            if (needDispose)
            {
                buffer.Dispose();
            }
        }

        internal static unsafe void StretchMask_Vertical<T>(FloatBitmap source, int sourceHeight, T destination, int destX, int destY, int destW, int destH, ColorIndex colorIndex)
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
            var shuffleFunc = FloatColor.GetFillSingleElementFunc(colorIndex);

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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void BlendProcess(ref Vector<float> back, in int dx, in int range, in float* sourceBegin, in int stride, in float* weights)
            {
                // マスク(不透明度補正)の計算
                var mask = Vector<float>.Zero;
                var weightSum = 0f;
                var sourceP = sourceBegin + dx;
                for (var y = 0; y <= range; y++, sourceP += stride)
                {
                    var weight = weights[y];
                    weightSum += weight;
                    mask += *(Vector<float>*)sourceP * weight;
                }
                if (weightSum is > 0)
                {
                    mask /= weightSum;
                }
                mask = Vector.Clamp(shuffleFunc(mask), Vector<float>.Zero, Vector<float>.One);

                // 暫定実装: 対象がAlpha-Premultipliedである前提で処理する
                var alpha = FloatColor.FillAlphaToAll(back);
                var rgb = Vector.ConditionalSelect(Vector.GreaterThan(alpha, Vector<float>.Zero), back / alpha, Vector<float>.Zero);
                back = Vector.Min(rgb * mask, Vector<float>.One);
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
                        Process(ref backP, backBuffer, dx, range, sourceBegin, stride, weights, count);
                    }
                    if (w is > 0)
                    {
                        Process(ref backP, backBuffer, dx, range, sourceBegin, stride, weights, w);
                    }
                });

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                void Process(ref byte* backP, float* backBuffer, int dx, int range, float* sourceBegin, int stride, float* weights, int count)
                {
                    // 背景を正規化してベクトル化
                    ByteToFloat(ref backP, backBuffer, count, true);
                    // ブレンド
                    var back = *(Vector<float>*)backBuffer;
                    BlendProcess(ref back, dx, range, sourceBegin, stride, weights);
                    // バッファへ書き戻す
                    *(Vector<float>*)backBuffer = back;
                    FloatToByte(ref backP, backBuffer, count);
                }
            }
        }
    }
}
