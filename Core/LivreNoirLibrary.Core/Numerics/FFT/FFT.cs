using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Numerics
{
    public static unsafe partial class FFT
    {
        public static double[] Forward(ReadOnlySpan<double> spanReal)
        {
            var result = ToComplex(spanReal);
            fixed (double* ptr = result)
            {
                FFTCore(true, ptr, result.Length / 2);
            }
            return result;
        }

        public static double[] Backward(ReadOnlySpan<double> spanComplex)
        {
            var length = CalcFittingLength(spanComplex.Length) / 2;
            var result = new double[length];
            spanComplex.CopyTo(result);
            fixed (double* ptr = result)
            {
                FFTCore(false, ptr, length);
            }
            return ToReal(result);
        }

        public static float[] Forward(ReadOnlySpan<float> spanReal)
        {
            var result = ToComplex(spanReal);
            fixed (float* ptr = result)
            {
                FFTCore(true, ptr, result.Length / 2);
            }
            return result;
        }

        public static float[] ForwardComplex(ReadOnlySpan<float> spanComplex)
        {
            float[] result = [.. spanComplex];
            fixed (float* ptr = result)
            {
                FFTCore(true, ptr, result.Length / 2);
            }
            return result;
        }

        public static float[] Backward(ReadOnlySpan<float> spanComplex)
        {
            var length = CalcFittingLength(spanComplex.Length) / 2;
            var result = new float[length];
            spanComplex.CopyTo(result);
            fixed (float* ptr = result)
            {
                FFTCore(false, ptr, length);
            }
            return ToReal(result);
        }

        private static readonly ConcurrentDictionary<int, int[]> _bit_reverse = [];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<int> BitReverseTable(int m)
        {
            if (!_bit_reverse.TryGetValue(m, out var ary))
            {
                var len = 1 << m;
                ary = new int[len];
                for (var i = 0; i < len; i++)
                {
                    var rev = 0;
                    for (var j = 0; j < m; j++)
                    {
                        rev = (rev << 1) | ((i >> j) & 1);
                    }
                    ary[i] = rev;
                }
                _bit_reverse[m] = ary;
            }
            return ary;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitReverse<T>(T* buffer, int m, int len) where T : unmanaged
        {
            var table = BitReverseTable(m);
            var vecPtr = (Vector2<T>*)buffer;
            fixed (int* tablePtr = table)
            {
                for (var i = 0; i < len; i++)
                {
                    var rev = tablePtr[i];
                    if (i < rev)
                    {
                        (vecPtr[i], vecPtr[rev]) = (vecPtr[rev], vecPtr[i]);
                    }
                }
            }
        }

        private readonly struct Vector2<T>(T x, T y)
        {
            public readonly T X = x;
            public readonly T Y = y;
        }

        private static readonly ConcurrentDictionary<int, double[]> _twiddle = [];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<double> GetTwiddle(int len)
        {
            if (!_twiddle.TryGetValue(len, out var ary))
            {
                ary = new double[len * 2];
                for (var i = 0; i < len; i++)
                {
                    var angle = -2.0f * MathF.PI * i / len;
                    ary[2 * i] = MathF.Cos(angle);     // 実部
                    ary[2 * i + 1] = MathF.Sin(angle); // 虚部
                }
                _twiddle[len] = ary;
            }
            return ary;
        }

        private static readonly ConcurrentDictionary<int, float[]> _twiddle_32 = [];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<float> GetTwiddle32(int len)
        {
            if (!_twiddle_32.TryGetValue(len, out var ary))
            {
                ary = new float[len * 2];
                for (var i = 0; i < len; i++)
                {
                    var angle = -2.0f * MathF.PI * i / len;
                    ary[2 * i] = MathF.Cos(angle);     // 実部
                    ary[2 * i + 1] = MathF.Sin(angle); // 虚部
                }
                _twiddle_32[len] = ary;
            }
            return ary;
        }

        private static unsafe void FFT_Table<T>(bool forward, T* buffer, int m, int len, ReadOnlySpan<T> twiddle)
            where T : unmanaged, INumber<T>
        {
            var twiddleFactor = forward ? T.One : -T.One;
            fixed (T* twiddlePtr = twiddle)
            {
                var groupSize = 1;
                for (var stage = 0; stage < m; stage++)
                {
                    var halfGroupSize = groupSize;
                    groupSize <<= 1; // 次のステージのグループサイズ

                    var twiddleStep = ((1 << m) / groupSize) << 1;
                    for (var group = 0; group < len; group += groupSize)
                    {
                        var twiddleIndex = 0;
                        var gg = group + halfGroupSize;
                        for (var i = group; i < gg; i++)
                        {
                            // 回転因子の取得（順方向/逆方向を考慮）
                            var wReal = twiddlePtr[twiddleIndex];
                            var wImag = twiddlePtr[twiddleIndex + 1] * twiddleFactor;
                            twiddleIndex += twiddleStep;

                            // バタフライ演算
                            var i1 = i << 1;
                            var i2 = (i + halfGroupSize) << 1;
                            var tempReal = wReal * buffer[i2] - wImag * buffer[i2 + 1];
                            var tempImag = wReal * buffer[i2 + 1] + wImag * buffer[i2];
                            buffer[i2] = buffer[i1] - tempReal;
                            buffer[i2 + 1] = buffer[i1 + 1] - tempImag;
                            buffer[i1] += tempReal;
                            buffer[i1 + 1] += tempImag;
                        }
                    }
                }
            }
        }

        private static void FFT_Direct(bool forward, double* buffer, int m, int len)
        {
            var v1_1 = -1d;
            var v1_2 = 0d;
            var groupSize = 1;
            var twiddleFactor = forward ? 1d : -1d;
            for (int stage = 0; stage < m; stage++)
            {
                var halfGroupSize = groupSize;
                groupSize <<= 1;
                var v2_1 = 1d;
                var v2_2 = 0d;
                for (var group = 0; group < halfGroupSize; group++)
                {
                    for (int i = group; i < len; i += groupSize)
                    {
                        var idx = i << 1;
                        var idx2 = (i + halfGroupSize) << 1;
                        var real = v2_1 * buffer[idx2] - v2_2 * buffer[idx2 + 1];
                        var imag = v2_1 * buffer[idx2 + 1] + v2_2 * buffer[idx2];
                        buffer[idx2] = buffer[idx] - real;
                        buffer[idx2 + 1] = buffer[idx + 1] - imag;
                        buffer[idx] += real;
                        buffer[idx + 1] += imag;
                    }

                    (v2_1, v2_2) = (v2_1 * v1_1 - v2_2 * v1_2, v2_1 * v1_2 + v2_2 * v1_1);
                }

                v1_2 = Math.Sqrt((1d - v1_1) * 0.5d) * twiddleFactor;
                v1_1 = Math.Sqrt((1d + v1_1) * 0.5d);
            }
        }

        private static void FFT_Direct(bool forward, float* buffer, int m, int len)
        {
            var v1_1 = -1f;
            var v1_2 = 0f;
            var groupSize = 1;
            var twiddleFactor = forward ? 1f : -1f;
            for (int stage = 0; stage < m; stage++)
            {
                var halfGroupSize = groupSize;
                groupSize <<= 1;
                var v2_1 = 1f;
                var v2_2 = 0f;
                for (var group = 0; group < halfGroupSize; group++)
                {
                    for (int i = group; i < len; i += groupSize)
                    {
                        var idx = i << 1;
                        var idx2 = (i + halfGroupSize) << 1;
                        var real = v2_1 * buffer[idx2] - v2_2 * buffer[idx2 + 1];
                        var imag = v2_1 * buffer[idx2 + 1] + v2_2 * buffer[idx2];
                        buffer[idx2] = buffer[idx] - real;
                        buffer[idx2 + 1] = buffer[idx + 1] - imag;
                        buffer[idx] += real;
                        buffer[idx + 1] += imag;
                    }

                    (v2_1, v2_2) = (v2_1 * v1_1 - v2_2 * v1_2, v2_1 * v1_2 + v2_2 * v1_1);
                }

                v1_2 = MathF.Sqrt((1f - v1_1) * 0.5f) * twiddleFactor;
                v1_1 = MathF.Sqrt((1f + v1_1) * 0.5f);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FFTCore(bool forward, double* buffer, int len)
        {
            var m = int.Log2(len);
            BitReverse(buffer, m, len);
            if (len is > 4096)
            {
                FFT_Table(forward, buffer, m, len, GetTwiddle(len));
            }
            else
            {
                FFT_Direct(forward, buffer, m, len);
            }
            if (forward)
            {
                new Span<double>(buffer, len * 2).Multiply(1d / len);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void FFTCore(bool forward, float* buffer, int len)
        {
            var m = int.Log2(len);
            BitReverse(buffer, m, len);
            if (len is > 4096)
            {
                FFT_Table(forward, buffer, m, len, GetTwiddle32(len));
            }
            else
            {
                FFT_Direct(forward, buffer, m, len);
            }
            if (forward)
            {
                new Span<float>(buffer, len * 2).Multiply(1f / len);
            }
        }
    }
}
