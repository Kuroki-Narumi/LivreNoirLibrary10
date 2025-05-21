using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
/*

z = (a + bi)(c - di)
  = ac + bd + (bc - ad)i

m(z) = (ac + bd)^2 + (ad - bc)^2
     = AC + 2abcd + BD + AD - 2abcd + BC
     = AC + AD + BC + BD
     = A(C + D) + B(C + D)
     = (A + B)(C + D)
     = (a^2 + b^2)(c^2 + d^2)

 */
namespace LivreNoirLibrary.Numerics
{
    public static class LinearAlgebra
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ReadOnlySpan<T> CastToReadOnlySpan<T>(Span<T> span) => (ReadOnlySpan<T>)span;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ReadOnlySpan<T> CastToReadOnlySpan<T>(T[] array) => (ReadOnlySpan<T>)array.AsSpan();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ReadOnlySpan<T> CastToReadOnlySpan<T>(List<T> list) => (ReadOnlySpan<T>)CollectionsMarshal.AsSpan(list);

        public static double SquareComplex(this List<double> list) => SquareComplexCore(CastToReadOnlySpan(list));
        public static double SquareComplex(this double[] array) => SquareComplexCore(CastToReadOnlySpan(array));
        public static double SquareComplex(this Span<double> span) => SquareComplexCore(CastToReadOnlySpan(span));
        public static double SquareComplex(this ReadOnlySpan<double> span) => SquareComplexCore(span);

        public static float SquareComplex(this List<float> list) => SquareComplexCore(CastToReadOnlySpan(list));
        public static float SquareComplex(this float[] array) => SquareComplexCore(CastToReadOnlySpan(array));
        public static float SquareComplex(this Span<float> span) => SquareComplexCore(CastToReadOnlySpan(span));
        public static float SquareComplex(this ReadOnlySpan<float> span) => SquareComplexCore(span);

        private static unsafe double SquareComplexCore(this ReadOnlySpan<double> span)
        {
            var len = span.Length / 2;
            double sum = 0;
            fixed (double* ptrBegin = span)
            {
                var ptr = ptrBegin;
                for (int i = 0; i < len; i++)
                {
                    sum += *ptr * *ptr++ + *ptr * *ptr++;
                }
            }
            return sum;
        }

        private static unsafe float SquareComplexCore(this ReadOnlySpan<float> span)
        {
            var len = span.Length;
            float sum = 0;
            fixed (float* ptrBegin = span)
            {
                var ptr = ptrBegin;
                for (var i = 0; i < len; i++)
                {
                    sum += *ptr * *ptr++ + *ptr * *ptr++;
                }
            }
            return sum;
        }

        public static double MeanSquareComplex(this List<double> list) => MeanSquareComplex(CastToReadOnlySpan(list));
        public static double MeanSquareComplex(this double[] array) => MeanSquareComplex(CastToReadOnlySpan(array));
        public static double MeanSquareComplex(this Span<double> span) => MeanSquareComplex(CastToReadOnlySpan(span));
        public static double MeanSquareComplex(this ReadOnlySpan<double> span) => SquareComplexCore(span) * 2 / span.Length;

        public static float MeanSquareComplex(this List<float> list) => MeanSquareComplex(CastToReadOnlySpan(list));
        public static float MeanSquareComplex(this float[] array) => MeanSquareComplex(CastToReadOnlySpan(array));
        public static float MeanSquareComplex(this Span<float> span) => MeanSquareComplex(CastToReadOnlySpan(span));
        public static float MeanSquareComplex(this ReadOnlySpan<float> span) => SquareComplexCore(span) * 2 / span.Length;

        public static unsafe float Variance(this ReadOnlySpan<float> span)
        {
            float sum = 0;
            var avg = span.Average();
            Vector<float> vecSum = new();
            Vector<float> vecAvg = new(avg);
            var count = Vector<float>.Count;
            var len = span.Length;
            var clen = len / count;
            var remain = len - clen * count;
            fixed (void* ptr = span)
            {
                var vec = (Vector<float>*)ptr;
                for (var i = 0; i < clen; i++, vec++)
                {
                    var value = *vec - vecAvg;
                    vecSum += value * value;
                }
                for (var i = 0; i < count; i++)
                {
                    sum += vecSum[i];
                }
                var remPtr = (float*)vec;
                for (var i = 0; i < remain; i++, remPtr++)
                {
                    var value = *remPtr - avg;
                    sum += value * value;
                }
            }
            return sum;
        }

        /// <summary>
        /// Calculate coefficient of determination<br/>
        /// https://ja.wikipedia.org/wiki/%E6%B1%BA%E5%AE%9A%E4%BF%82%E6%95%B0
        /// </summary>
        public static unsafe float R2(ReadOnlySpan<float> span1, ReadOnlySpan<float> span2)
        {
            var len1 = span1.Length;
            var len2 = span2.Length;
            if (len1 > len2)
            {
                (len1, len2) = (len2, len1);
                var s = span1;
                span1 = span2;
                span2 = s;
            }
            var count = Vector<float>.Count;
            var avg = span1.Average();
            float num = 0, den = 0;
            fixed (void* ptr1 = span1)
            fixed (void* ptr2 = span2)
            {
                Vector<float> vecBuffer = new();
                Vector<float> vecAvg = new(avg);
                Vector<float> vecNum = new();
                Vector<float> vecDen = new();
                // 長さが一致する部分
                var clen = len1 / count;
                var remain = len1 - clen * count;
                var vec1 = (Vector<float>*)ptr1;
                var vec2 = (Vector<float>*)ptr2;
                for (var i = 0; i < clen; i++, vec1++, vec2++)
                {
                    vecBuffer = *vec1 - *vec2;
                    vecNum += vecBuffer * vecBuffer;
                    vecBuffer = *vec1 - vecAvg;
                    vecDen += vecBuffer * vecBuffer;
                }
                for (var i = 0; i < count; i++)
                {
                    num += vecNum[i];
                    den += vecDen[i];
                }
                var remain1 = (float*)vec1;
                var remain2 = (float*)vec2;
                for (var i = 0; i < remain; i++, remain1++, remain2++)
                {
                    var v = *remain1 - *remain2;
                    num += v * v;
                    v = *remain1 - avg;
                    den += v * v;
                }
                // 余り
                len1 = len2 - len1;
                clen = len1 / count;
                remain = len1 - clen * count;
                vec2 = (Vector<float>*)remain2;
                vecNum = new();
                vecDen = new();
                for (var i = 0; i < clen; i++, vec2++)
                {
                    vecNum += *vec2 * *vec2;
                    vecDen += vecAvg * vecAvg;
                }
                for (var i = 0; i < count; i++)
                {
                    num += vecNum[i];
                    den += vecDen[i];
                }
                remain2 = (float*)vec2;
                var avgS = avg * avg;
                for (var i = 0; i < remain; i++, remain2++)
                {
                    num += *remain2 * *remain2;
                    den += avgS;
                }
            }
            return den is 0 ? (num is 0 ? 1f : float.NegativeInfinity) : 1f - num / den;
        }
    }
}
