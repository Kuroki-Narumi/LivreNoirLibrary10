using System;
using System.Buffers;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;
using FFmpeg.AutoGen;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.FFmpeg;

namespace LivreNoirLibrary.Media.Wave
{
    public partial class WaveBuffer
    {
        /// <summary>
        /// Convert a float value in the range of -1.0 to 1.0 into a gain level in dB.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The gain level in dB.</returns>
        public static double Value2Level(float value)
        {
            return Math.Log10(Math.Abs(value)) * 20.0;
        }

        /// <summary>
        /// Convert a gain level in dB into a float value in the range of 0.0 to 1.0.
        /// </summary>
        /// <param name="level"></param>
        /// <returns>The value in float, 0.0 to 1.0.</returns>
        public static float Level2Value(double level)
        {
            return (float)Math.Pow(10.0, level / 20.0);
        }

        public static void CheckLayout(int sampleRate, int channels, int samples = 0)
        {
            FFmpegUtils.ValidateSampleRate(nameof(sampleRate), sampleRate);
            FFmpegUtils.ValidateChannels(nameof(channels), channels);
            if (samples is < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(samples), $"number of samples must be >= 0");
            }
        }

        public static void AdjustArgs(in ReadOnlySpan<float> span, ref int sampleOffset, ref int sampleCount, int channels)
        {
            var len = span.Length / channels;
            if (sampleOffset is < 0)
            {
                sampleOffset += len;
            }
            sampleOffset = Math.Clamp(sampleOffset, 0, len);
            if (sampleCount <= 0)
            {
                sampleCount += len - sampleOffset;
            }
            sampleCount = Math.Clamp(sampleCount, 0, len - sampleOffset);
        }

        public static void AdjustArgs(ReadOnlySpan<float> span, ref int sampleOffset, int channels)
        {
            var len = span.Length / channels;
            if (sampleOffset is < 0)
            {
                sampleOffset += len;
            }
            sampleOffset = Math.Clamp(sampleOffset, 0, len);
        }

        public static Span<float> Slice(Span<float> span, int sampleOffset, int sampleCount, int channels)
        {
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            return span.Slice(sampleOffset * channels, sampleCount * channels);
        }

        public static unsafe void GetChannel(ReadOnlySpan<float> source, Span<float> target, int channel, int srcOffset, int srcChannels)
        {
            if ((uint)channel >= (uint)srcChannels)
            {
                throw new IndexOutOfRangeException($"channel index must be >= 0 and < {srcChannels} (given:{channel})");
            }
            var sampleCount = target.Length;
            AdjustArgs(source, ref srcOffset, ref sampleCount, srcChannels);
            fixed (float* srcBegin = source)
            fixed (float* dstBegin = target)
            {
                var srcPtr = srcBegin + srcOffset * srcChannels + channel;
                var dstPtr = dstBegin;
                for (var i = 0; i < sampleCount; i++, srcPtr += srcChannels, dstPtr++)
                {
                    *dstPtr = *srcPtr;
                }
            }
            target[sampleCount..].Clear();
        }

        public static unsafe void GetChannelComplex(ReadOnlySpan<float> source, Span<float> target, int channel, int srcOffset, int srcChannels)
        {
            if ((uint)channel >= (uint)srcChannels)
            {
                throw new IndexOutOfRangeException($"channel index must be >= 0 and < {srcChannels} (given:{channel})");
            }
            var sampleCount = target.Length / 2;
            AdjustArgs(source, ref srcOffset, ref sampleCount, srcChannels);
            SimdOperations.Clear(target[(sampleCount * 2)..]);
            fixed (float* srcBegin = source)
            fixed (float* dstBegin = target)
            {
                var srcPtr = srcBegin + srcOffset * srcChannels + channel;
                var dstPtr = dstBegin;
                if (srcChannels is 2)
                {
                    var vecCount = Vector<ulong>.Count;
                    var v2 = vecCount * 2;
                    var mask = Vector.Create(0x0000_0000_ffff_ffffuL);
                    for (; sampleCount >= vecCount; srcPtr += v2, dstPtr += v2, sampleCount -= vecCount)
                    {
                        var buffer = (Vector<ulong>*)srcPtr;
                        *(Vector<ulong>*)dstPtr = Vector.BitwiseAnd(*buffer, mask);
                    }
                }
                for (var i = 0; i < sampleCount; i++, srcPtr += srcChannels)
                {
                    *dstPtr++ = *srcPtr;
                    *dstPtr++ = 0;
                }
            }
        }

        internal static unsafe bool ChangeLayoutCore(Span<float> buffer, int inRate, int inCh, int outRate, int outCh, [MaybeNullWhen(false)] out float[] convertBuffer, out int outTotalSample)
        {
            CheckLayout(outRate, outCh);
            if (inRate == outRate && inCh == outCh)
            {
                convertBuffer = null;
                outTotalSample = 0;
                return false;
            }
            var inSamples = buffer.Length / inCh;
            FFmpegUtils.Initialize();
            SwrContext* swr = null;
            AVChannelLayout srcCh = default;
            AVChannelLayout dstCh = default;
            ffmpeg.av_channel_layout_default(&srcCh, inCh);
            ffmpeg.av_channel_layout_default(&dstCh, outCh);

            ffmpeg.swr_alloc_set_opts2(&swr,
                &dstCh, AVSampleFormat.AV_SAMPLE_FMT_FLT, outRate,
                &srcCh, AVSampleFormat.AV_SAMPLE_FMT_FLT, inRate,
                0, null).CheckError(FFmpegUtils.ThrowInvalidOperationException);
            ffmpeg.swr_init(swr).CheckError(FFmpegUtils.ThrowInvalidOperationException);
            var outSamples = ffmpeg.swr_get_out_samples(swr, inSamples);
            outTotalSample = outSamples * outCh;
            convertBuffer = ArrayPool<float>.Shared.Rent(outTotalSample);
            fixed (float* srcPtr = buffer)
            fixed (float* dstPtr = convertBuffer)
            {
                var srcBytePtr = (byte*)srcPtr;
                var dstBytePtr = (byte*)dstPtr;
                var samples = ffmpeg.swr_convert(swr, &dstBytePtr, outSamples, &srcBytePtr, inSamples).CheckError(FFmpegUtils.ThrowInvalidOperationException);
                dstBytePtr = (byte*)(dstPtr + samples * outCh);
                ffmpeg.swr_convert(swr, &dstBytePtr, outSamples, null, 0).CheckError(FFmpegUtils.ThrowInvalidOperationException);
            }
            ffmpeg.swr_free(&swr);
            return true;
        }

        public static void SetValue(Span<float> span, float value, int sampleOffset, int sampleCount, int channels)
        {
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            span.Slice(sampleOffset * channels, sampleCount * channels).CopyFrom(value);
        }

        public static void CopyFrom(Span<float> span, ReadOnlySpan<float> src, int sampleOffset, int channels)
        {
            var sampleCount = src.Length / channels;
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            span.Slice(sampleOffset * channels, sampleCount * channels).CopyFrom(src);
        }

        public static void Add(Span<float> span, float value, int sampleOffset, int sampleCount, int channels)
        {
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            span.Slice(sampleOffset * channels, sampleCount * channels).Add(value);
        }

        public static void Add(Span<float> span, ReadOnlySpan<float> src, int sampleOffset, int channels)
        {
            var sampleCount = src.Length / channels;
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            span.Slice(sampleOffset * channels, sampleCount * channels).Add(src);
        }

        public static void Multiply(Span<float> span, float value, int sampleOffset, int sampleCount, int channels)
        {
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            span.Slice(sampleOffset * channels, sampleCount * channels).Multiply(value);
        }

        public static void Multiply(Span<float> span, ReadOnlySpan<float> src, int sampleOffset, int channels)
        {
            var sampleCount = src.Length / channels;
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            span.Slice(sampleOffset * channels, sampleCount * channels).Multiply(src);
        }

        public static void Invert(Span<float> span, int sampleOffset, int sampleCount, int channels)
        {
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            span.Slice(sampleOffset * channels, sampleCount * channels).Negate();
        }

        public static unsafe void FadeIn(Span<float> buffer, int sampleOffset, int sampleCount, float power, int channels)
        {
            AdjustArgs(buffer, ref sampleOffset, ref sampleCount, channels);
            fixed (float* ptrBegin = buffer)
            {
                var ptr = ptrBegin + sampleOffset * channels;
                for (var i = 0; i < sampleCount; i++)
                {
                    var ratio = MathF.Pow((float)i / sampleCount, power);
                    for (var c = 0; c < channels; c++, ptr++)
                    {
                        *ptr *= ratio;
                    }
                }
            }
        }

        public static unsafe void FadeOut(Span<float> buffer, int sampleOffset, int sampleCount, float power, int channels)
        {
            AdjustArgs(buffer, ref sampleOffset, ref sampleCount, channels);
            power = 1f / power;
            fixed (float* ptrBegin = buffer)
            {
                var ptr = ptrBegin + sampleOffset * channels;
                for (var i = 0; i < sampleCount; i++)
                {
                    var ratio = 1f - MathF.Pow((i + 1f) / sampleCount, power);
                    for (var c = 0; c < channels; c++, ptr++)
                    {
                        *ptr *= ratio;
                    }
                }
            }
        }

        public static unsafe void SetValue(Span<float> span, SetSampleFunc func, int sampleOffset, int sampleCount, int channels)
        {
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            fixed (float* ptrBegin = span)
            {
                var ptr = ptrBegin + sampleOffset * channels;
                for (var i = 0; i < sampleCount; i++)
                {
                    for (var c = 0; c < channels; c++, ptr++)
                    {
                        *ptr = func(i, c, *ptr);
                    }
                }
            }
        }

        public static float GetMaxMagnitude(Span<float> span, int sampleOffset, int sampleCount, int channels)
        {
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            var (min, max) = span.Slice(sampleOffset * channels, sampleCount * channels).MinMax();
            return Math.Max(max, -min);
        }

        public static void Clamp(Span<float> span, float value, int sampleOffset, int sampleCount, int channels)
        {
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            if (value is < 0)
            {
                value = -value;
            }
            span.Slice(sampleOffset * channels, sampleCount * channels).Clamp(-value, value);
        }

        public static unsafe int FindFirstSound(Span<float> span, float threshold, int sampleAfter, int channels)
        {
            if (sampleAfter is < 0)
            {
                sampleAfter += span.Length / channels;
            }
            fixed (float* ptrBegin = span)
            {
                var limit = span.Length / channels - 1;
                var ptr = ptrBegin + sampleAfter * channels;
                for (; sampleAfter < limit; sampleAfter++)
                {
                    for (var c = 0; c < channels; c++)
                    {
                        if (*ptr > threshold || -*ptr > threshold)
                        {
                            return sampleAfter;
                        }
                        ptr++;
                    }
                }
                return limit;
            }
        }

        public static unsafe int FindLastSound(Span<float> span, float threshold, int sampleBefore, int channels)
        {
            if (sampleBefore is <= 0)
            {
                sampleBefore += span.Length / channels;
            }
            fixed (float* ptrBegin = span)
            {
                var ptr = ptrBegin + sampleBefore * channels + channels - 1;
                for (; sampleBefore is > 0; sampleBefore--)
                {
                    for (var c = 0; c < channels; c++)
                    {
                        if (*ptr > threshold || -*ptr > threshold)
                        {
                            return sampleBefore;
                        }
                        ptr--;
                    }
                }
                return 0;
            }
        }

        public static (int, int) FindBothSilence(Span<float> span, float th1, float th2, int sliceOffset, int sliceCount, int channels)
        {
            if (th1 is < 0)
            {
                th1 = -th1;
            }
            if (th2 is < 0)
            {
                th2 = th1;
            }
            AdjustArgs(span, ref sliceOffset, ref sliceCount, channels);
            var left = FindFirstSound(span, th1, sliceOffset, channels);
            var right = FindLastSound(span, th2, sliceOffset + sliceCount - 1, channels);
            return left <= right ? (left, right) : (sliceOffset, sliceOffset);
        }

        public static SliceWithCutSilenceResult SliceWithCutSilence(Span<float> span, float th1, float th2, int sliceOffset, int sliceCount, int marginLeft, int marginRight, int channels)
        {
            var (left, right) = FindBothSilence(span, th1, th2, sliceOffset, sliceCount, channels);
            (left, right) = ApplyMargin(left, right, marginLeft, marginRight, sliceOffset, sliceOffset + sliceCount);
            var actualCount = right - left;
            var newSpan = Slice(span, left, actualCount, channels);
            return new(newSpan, sliceOffset, sliceCount, left, actualCount);
        }

        internal static (int, int) ApplyMargin(int left, int right, int marginLeft, int marginRight, int leftLimit, int rightLimit)
        {
            if (marginLeft is < 0)
            {
                marginLeft = 0;
            }
            if (marginRight is <= 0)
            {
                marginRight = marginLeft;
            }
            left = Math.Clamp(left - marginLeft, leftLimit, rightLimit);
            right = Math.Clamp(right + marginRight, leftLimit, rightLimit);
            return (left, right);
        }

        public static unsafe float[] GetPeak(ReadOnlySpan<float> span, int sampleOffset, int sampleCount, int channels)
        {
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            var max = new float[channels];
            fixed (float* maxPtr = max)
            {
                var c = 0;
                foreach (var value in span.Slice(sampleOffset * channels, sampleCount * channels))
                {
                    if (value > maxPtr[c])
                    {
                        maxPtr[c] = value;
                    }
                    if (-value > maxPtr[c])
                    {
                        maxPtr[c] = -value;
                    }
                    c = (c + 1) % channels;
                }
            }
            return max;
        }

        public static unsafe float[] GetRms(ReadOnlySpan<float> span, int sampleOffset, int sampleCount, int channels)
        {
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            var sum = new float[channels];
            fixed (float* sumPtr = sum)
            {
                var c = 0;
                foreach (var value in span.Slice(sampleOffset * channels, sampleCount * channels))
                {
                    sumPtr[c] += value * value;
                    c = (c + 1) % channels;
                }
                for (c = 0; c < channels; c++)
                {
                    sumPtr[c] = MathF.Sqrt(sumPtr[c] / sampleCount);
                }
            }
            return sum;
        }

        public static unsafe float[][] SplitChannels(ReadOnlySpan<float> span, int channels, int sampleOffset = 0, int sampleCount = 0)
        {
            AdjustArgs(span, ref sampleOffset, ref sampleCount, channels);
            var result = new float[channels][];
            for (var c = 0; c < channels; c++)
            {
                result[c] = new float[sampleCount];
            }
            fixed (float* srcBegin = span)
            {
                var srcPtr = srcBegin + sampleOffset * channels;
                for (var i = 0; i < sampleCount; i++)
                {
                    for (var c = 0; c < channels; c++)
                    {
                        result[c][i] = *srcPtr++;
                    }
                }
            }
            return result;
        }

        public static unsafe float[] MergeChannels(float[][] buffer)
        {
            var channels = buffer.Length;
            var sampleLength = buffer[0].Length;
            var result = new float[channels * sampleLength];
            fixed (float* dstBegin = result)
            {
                var dstPtr = dstBegin;
                for (var i = 0; i < sampleLength; i++)
                {
                    for (var c = 0; c < channels; c++)
                    {
                        *dstPtr++ = buffer[c][i];
                    }
                }
            }
            return result;
        }
    }

    public readonly ref struct SliceWithCutSilenceResult(Span<float> span, int ro, int rl, int ao, int al)
    {
        public readonly Span<float> Span = span;
        public readonly int RequiredOffset = ro;
        public readonly int RequiredLength = rl;
        public readonly int ActualOffset = ao;
        public readonly int ActualLength = al;

        public void Deconstruct(out Span<float> span, out int requiredOffset, out int requiredLength, out int actualOffset, out int actualLength)
        {
            span = Span;
            requiredOffset = RequiredOffset;
            requiredLength = RequiredLength;
            actualOffset = ActualOffset;
            actualLength = ActualLength;
        }
    }
}
