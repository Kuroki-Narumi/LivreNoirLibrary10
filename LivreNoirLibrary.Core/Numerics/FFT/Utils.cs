using System;
using System.Collections.Generic;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public static partial class FFT
    {
        public const int DefaultWindowWidth = 4096;

        private static readonly Dictionary<double, double[]> _freqLists = [];
        private static readonly Dictionary<float, float[]> _freqLists32 = [];

        public static int CalcFittingLength(int length) => (int)BitOperations.RoundUpToPowerOf2((uint)length);

        public static int CalcWindowRepeats(int length, int ww = DefaultWindowWidth)
        {
            return (length + ww - 1) / ww;
        }

        public static unsafe ReadOnlySpan<double> GetFreqList(int sampleRate, int ww = DefaultWindowWidth)
        {
            var fWidth = (double)sampleRate / ww;
            if (!_freqLists.TryGetValue(fWidth, out var result))
            {
                result = new double[ww];
                fixed (double* ptr = result)
                {
                    for (int i = 1; i < ww; i++)
                    {
                        ptr[i] = i * fWidth;
                    }
                }
                _freqLists.Add(fWidth, result);
            }
            return result;
        }

        public static unsafe ReadOnlySpan<float> GetFreqList32(int sampleRate, int ww = DefaultWindowWidth)
        {
            var fWidth = (float)sampleRate / ww;
            if (!_freqLists32.TryGetValue(fWidth, out var result))
            {
                result = new float[ww];
                fixed (float* ptr = result)
                {
                    for (int i = 1; i < ww; i++)
                    {
                        ptr[i] = i * fWidth;
                    }
                }
                _freqLists32.Add(fWidth, result);
            }
            return result;
        }

        public static double[] ToComplex(ReadOnlySpan<double> spanReal)
        {
            var result = new double[CalcFittingLength(spanReal.Length) * 2];
            ToComplex(spanReal, result);
            return result;
        }

        public static unsafe void ToComplex(ReadOnlySpan<double> sourceReal, Span<double> targetComplex)
        {
            var len1 = sourceReal.Length;
            fixed (double* src = sourceReal)
            fixed (double* dstBegin = targetComplex)
            {
                var dst = dstBegin;
                for (int i = 0; i < len1; i++, dst += 2)
                {
                    *dst = src[i];
                }
            }
        }

        public static float[] ToComplex(ReadOnlySpan<float> spanComplex)
        {
            var result = new float[CalcFittingLength(spanComplex.Length) * 2];
            ToComplex(spanComplex, result);
            return result;
        }

        public static unsafe void ToComplex(ReadOnlySpan<float> sourceReal, Span<float> targetComplex)
        {
            var len1 = sourceReal.Length;
            fixed (float* src = sourceReal)
            fixed (float* dstBegin = targetComplex)
            {
                var dst = dstBegin;
                for (int i = 0; i < len1; i++, dst += 2)
                {
                    *dst = src[i];
                }
            }
        }

        public static double[] ToReal(ReadOnlySpan<double> spanComplex)
        {
            var result = new double[spanComplex.Length / 2];
            ToReal(spanComplex, result);
            return result;
        }

        public static unsafe void ToReal(ReadOnlySpan<double> sourceComplex, Span<double> targetReal)
        {
            var len1 = sourceComplex.Length / 2;
            fixed (double* srcBegin = sourceComplex)
            fixed (double* dst = targetReal)
            {
                var src = srcBegin;
                for (int i = 0; i < len1; i++, src += 2)
                {
                    dst[i] = *src;
                }
            }
        }

        public static float[] ToReal(ReadOnlySpan<float> spanComplex)
        {
            var result = new float[spanComplex.Length / 2];
            ToReal(spanComplex, result);
            return result;
        }

        public static unsafe void ToReal(ReadOnlySpan<float> sourceComplex, Span<float> targetReal)
        {
            var len1 = sourceComplex.Length / 2;
            fixed (float* srcBegin = sourceComplex)
            fixed (float* dst = targetReal)
            {
                var src = srcBegin;
                for (int i = 0; i < len1; i++, src += 2)
                {
                    dst[i] = *src;
                }
            }
        }
    }
}
