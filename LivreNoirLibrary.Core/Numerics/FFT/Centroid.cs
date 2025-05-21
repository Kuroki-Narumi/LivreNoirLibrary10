using System;
using System.Buffers;

namespace LivreNoirLibrary.Numerics
{
    public static partial class FFT
    {
        public static unsafe double Centroid(ReadOnlySpan<double> spanComplex, int sampleRate)
        {
            var freq = GetFreqList(sampleRate, spanComplex.Length / 2);
            double num = 0, den = 0;
            fixed (double* fPtr = freq)
            fixed (double* cPtrBegin = spanComplex)
            {
                var cPtr = cPtrBegin;
                for (int i = 1; i < spanComplex.Length / 2; i++)
                {
                    var value = *cPtr * *cPtr++ + *cPtr * *cPtr++;
                    num += fPtr[i] * value;
                    den += value;
                }
            }
            return den is > 0 ? num / den : 0;
        }

        public static unsafe float Centroid(ReadOnlySpan<float> spanComplex, int sampleRate)
        {
            var freq = GetFreqList32(sampleRate, spanComplex.Length / 2);
            float num = 0, den = 0;
            fixed (float* fPtr = freq)
            fixed (float* cPtrBegin = spanComplex)
            {
                var cPtr = cPtrBegin;
                for (int i = 1; i < spanComplex.Length / 2; i++)
                {
                    var value = *cPtr * *cPtr++ + *cPtr * *cPtr++;
                    num += fPtr[i] * value;
                    den += value;
                }
            }
            return den is > 0 ? num / den : 0;
        }

        public static unsafe double[] Centroids(ReadOnlySpan<double> span, int sampleRate, int ww = DefaultWindowWidth)
        {
            var len = span.Length;
            var w2 = ww / 2;
            var result = new double[CalcWindowRepeats(len, w2)];
            var freqList = GetFreqList(sampleRate, ww);
            var array = ArrayPool<double>.Shared.Rent(ww * 2);
            var buffer = array.AsSpan();

            var resultIndex = 0;
            var index = 0;
            double num, den;
            try
            {
                fixed (double* resultPtr = result)
                fixed (double* freqPtr = freqList)
                fixed (double* bufferPtr = buffer)
                {
                    while (len > ww)
                    {
                        ToComplex(span[index..(index + ww)], buffer);
                        ApplyHammingComplex(buffer, ww);
                        FFTCore(true, bufferPtr, ww);
                        num = 0;
                        den = 0;
                        for (int j = 1; j < w2; j++)
                        {
                            var j2 = j * 2;
                            var re = bufferPtr[j2];
                            var im = bufferPtr[j2 + 1];
                            var value = re * re + im * im;
                            num += freqPtr[j] * value;
                            den += value;
                        }
                        resultPtr[resultIndex] = den is > 0 ? num / den : 0;

                        resultIndex++;
                        index += w2;
                        len -= w2;
                    }
                    buffer.Clear();
                    ToComplex(span[index..(index + len)], buffer);
                    ApplyHammingComplex(buffer, ww);
                    FFTCore(true, bufferPtr, ww);
                    num = 0;
                    den = 0;
                    for (int j = 1; j < w2; j++)
                    {
                        var j2 = j * 2;
                        var re = bufferPtr[j2];
                        var im = bufferPtr[j2 + 1];
                        var value = re * re + im * im;
                        num += freqPtr[j] * value;
                        den += value;
                    }
                    resultPtr[resultIndex] = den is > 0 ? num / den : 0;
                }
                return result;
            }
            finally
            {
                ArrayPool<double>.Shared.Return(array);
            }
        }

        public static unsafe float[] Centroids(ReadOnlySpan<float> span, int sampleRate, int ww = DefaultWindowWidth)
        {
            var len = span.Length;
            var w2 = ww / 2;
            var result = new float[CalcWindowRepeats(len, w2)];
            var freqList = GetFreqList32(sampleRate, ww);
            var array = ArrayPool<float>.Shared.Rent(ww * 2);
            var buffer = array.AsSpan(0, ww * 2);

            var resultIndex = 0;
            var index = 0;
            float num, den;
            try
            {
                fixed (float* resultPtr = result)
                fixed (float* freqPtr = freqList)
                fixed (float* bufferPtr = buffer)
                {
                    while (len > ww)
                    {
                        ToComplex(span[index..(index + ww)], buffer);
                        ApplyHammingComplex(buffer, ww);
                        FFTCore(true, bufferPtr, ww);
                        num = 0;
                        den = 0;
                        for (int j = 1; j < w2; j++)
                        {
                            var j2 = j * 2;
                            var re = bufferPtr[j2];
                            var im = bufferPtr[j2 + 1];
                            var value = re * re + im * im;
                            num += freqPtr[j] * value;
                            den += value;
                        }
                        resultPtr[resultIndex] = den is > 0 ? num / den : 0;

                        resultIndex++;
                        index += w2;
                        len -= w2;
                    }
                    buffer.Clear();
                    ToComplex(span[index..(index + len)], buffer);
                    ApplyHammingComplex(buffer, ww);
                    FFTCore(true, bufferPtr, ww);
                    num = 0;
                    den = 0;
                    for (int j = 1; j < w2; j++)
                    {
                        var j2 = j * 2;
                        var re = bufferPtr[j2];
                        var im = bufferPtr[j2 + 1];
                        var value = re * re + im * im;
                        num += freqPtr[j] * value;
                        den += value;
                    }
                    resultPtr[resultIndex] = den is > 0 ? num / den : 0;
                }
                return result;
            }
            finally
            {
                ArrayPool<float>.Shared.Return(array);
            }
        }
    }
}
