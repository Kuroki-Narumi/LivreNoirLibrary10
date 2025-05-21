using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Numerics
{
    public static unsafe partial class FFT
    {
        private static readonly Dictionary<int, double[]> _hamming = [];
        private static readonly Dictionary<int, float[]> _hamming_32 = [];
        private static readonly Dictionary<int, double[]> _hamming_c = [];
        private static readonly Dictionary<int, float[]> _hamming_c_32 = [];

        private static readonly Func<int, double[]> _create_hamming = ww =>
        {
            var ary = new double[ww];
            var w2 = 1.0 / (ww - 1);
            fixed (double* ptr = ary)
            {
                for (var i = 0; i < ww; i++)
                {
                    ptr[i] = 0.54 - 0.46 * Math.Cos(Math.PI * i * w2);
                }
            }
            return ary;
        };

        private static readonly Func<int, float[]> _create_hamming_32 = ww =>
        {
            var ary = new float[ww];
            var w2 = 1f / (ww - 1);
            fixed (float* ptr = ary)
            {
                for (var i = 0; i < ww; i++)
                {
                    ptr[i] = 0.54f - 0.46f * MathF.Cos(MathF.PI * i * w2);
                }
            }
            return ary;
        };

        private static readonly Func<int, double[]> _create_hamming_c = ww =>
        {
            var ary = new double[ww * 2];
            var w2 = 1.0 / (ww - 1);
            fixed (double* ptrBegin = ary)
            {
                var ptr = ptrBegin;
                for (var i = 0; i < ww; i++)
                {
                    var v = 0.54 - 0.46 * Math.Cos(Math.PI * i * w2);
                    *ptr++ = v;
                    *ptr++ = v;
                }
            }
            return ary;
        };

        private static readonly Func<int, float[]> _create_hamming_c_32 = ww =>
        {
            var ary = new float[ww * 2];
            var w2 = 1f / (ww - 1);
            fixed (float* ptrBegin = ary)
            {
                var ptr = ptrBegin;
                for (var i = 0; i < ww; i++)
                {
                    var v = 0.54f - 0.46f * MathF.Cos(MathF.PI * i * w2);
                    *ptr++ = v;
                    *ptr++ = v;
                }
            }
            return ary;
        };

        public static unsafe ReadOnlySpan<double> GetHamming(int ww) => _hamming.GetOrAdd(ww, _create_hamming);
        public static unsafe ReadOnlySpan<float> GetHamming32(int ww) => _hamming_32.GetOrAdd(ww, _create_hamming_32);
        public static unsafe ReadOnlySpan<double> GetHammingComplex(int ww) => _hamming_c.GetOrAdd(ww, _create_hamming_c);
        public static unsafe ReadOnlySpan<float> GetHammingComplex32(int ww) => _hamming_c_32.GetOrAdd(ww, _create_hamming_c_32);

        public static unsafe void ApplyHamming(Span<double> buffer, int ww = DefaultWindowWidth)
        {
            var hamming = GetHamming(ww);
            buffer.Multiply(hamming);
        }

        public static unsafe void ApplyHammingComplex(Span<double> buffer, int ww = DefaultWindowWidth)
        {
            var hamming = GetHammingComplex(ww);
            buffer.Multiply(hamming);
        }

        public static unsafe void ApplyHamming(Span<float> buffer, int ww = DefaultWindowWidth)
        {
            var hamming = GetHamming32(ww);
            buffer.Multiply(hamming);
        }

        public static unsafe void ApplyHammingComplex(Span<float> buffer, int ww = DefaultWindowWidth)
        {
            var hamming = GetHammingComplex32(ww);
            buffer.Multiply(hamming);
        }
    }
}
