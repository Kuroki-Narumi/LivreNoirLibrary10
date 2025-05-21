using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Wave
{
    public delegate float SetSampleFunc(int sample, int channel, float current);

    public static class IWaveBufferExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnsureSampleLength<T>(this T buffer, int samples, bool clear = true) where T : IWaveBuffer => EnsureTotalSample(buffer, samples * buffer.Channels, clear);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnsureTotalSample<T>(this T buffer, int size, bool clear = true)
            where T : IWaveBuffer
        {
            if (buffer.TotalSample < size)
            {
                buffer.SetTotalSample(size, clear);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSampleLength<T>(this T buffer, int samples, bool clear = true) where T : IWaveBuffer => buffer.SetTotalSample(samples * buffer.Channels, clear);

        public static Span<float> Slice<T>(this T buffer, int sampleOffset, int sampleCount = 0) where T : IWaveBuffer 
            => WaveBuffer.Slice(buffer.Data, sampleOffset, sampleCount, buffer.Channels);

        public static void GetChannel<T>(this T buffer, Span<float> target, int channel, int sampleOffset = 0) where T : IWaveBuffer
            => WaveBuffer.GetChannel(buffer.Data, target, channel, sampleOffset, buffer.Channels);

        public static void GetChannelComplex<T>(this T buffer, Span<float> target, int channel, int sampleOffset = 0) where T : IWaveBuffer
            => WaveBuffer.GetChannelComplex(buffer.Data, target, channel, sampleOffset, buffer.Channels);

        public static void InsertSilence<T>(T buffer, int sampleOffset, int sampleCount)
            where T : IWaveBuffer
        {
            if (sampleCount is <= 0)
            {
                return;
            }
            var ch = buffer.Channels;
            WaveBuffer.AdjustArgs(buffer.Data, ref sampleOffset, ch);
            var offset = sampleOffset * ch;
            var count = sampleCount * ch;
            var current = buffer.TotalSample;
            buffer.SetTotalSample(current + count, false);
            if (offset < current)
            {
                var span = buffer.Data;
                span[offset..current].CopyTo(span[(offset + count)..]);
                SimdOperations.Clear(span.Slice(offset, count));
            }
        }

        public static void RemoveRange<T>(T buffer, int sampleOffset, int sampleCount = 0)
            where T : IWaveBuffer
        {
            var ch = buffer.Channels;
            WaveBuffer.AdjustArgs(buffer.Data, ref sampleOffset, ref sampleCount, ch);
            if (sampleCount is <= 0)
            {
                return;
            }
            var offset = sampleOffset * ch;
            var count = sampleCount * ch;
            var current = buffer.TotalSample;
            var end = offset + count;
            if (end < current)
            {
                var span = buffer.Data;
                span[end..current].CopyTo(span[offset..]);
            }
            buffer.SetTotalSample(current - count, false);
        }

        public static void Splice<T>(T buffer, int sampleOffset, int sampleCount = 0)
            where T : IWaveBuffer
        {
            var ch = buffer.Channels;
            WaveBuffer.AdjustArgs(buffer.Data, ref sampleOffset, ref sampleCount, ch);
            var offset = sampleOffset * ch;
            var count = sampleCount * ch;
            if (count is > 0)
            {
                var span = buffer.Data;
                span.Slice(offset, count).CopyTo(span);
            }
            buffer.SetTotalSample(count, false);
        }

        public static void RemoveBefore<T>(T buffer, int sampleOffset) where T : IWaveBuffer => Splice(buffer, sampleOffset, 0);
        public static void RemoveAfter<T>(T buffer, int sampleOffset) where T : IWaveBuffer => Splice(buffer, 0, sampleOffset);

        public static void ChangeLayout<T>(this T buffer, int sampleRate, int channels)
            where T : IWaveBuffer
        {
            var inRate = buffer.SampleRate;
            var inCh = buffer.Channels;
            if (WaveBuffer.ChangeLayoutCore(buffer.Data, inRate, inCh, sampleRate, channels, out var convertBuffer, out var outTotalSample))
            {
                buffer.SetTotalSample(outTotalSample, false);
                //convertBuffer.AsSpan(0, outTotalSample).CopyTo(buffer.Data);
                buffer.Data.CopyFrom(convertBuffer, 0, 0, outTotalSample);
                buffer.SetLayout(sampleRate, channels);
                ArrayPool<float>.Shared.Return(convertBuffer);
            }
        }

        private delegate void ConvertProcessFunc<T>(T dst, ReadOnlySpan<float> src, int dstOffset);

        private static void ConvertProcess<T1, T2>(T1 dst, T2 src, int dstOffset, int srcOffset, int srcSamples, ConvertProcessFunc<T1> func)
            where T1 : IWaveBuffer
            where T2 : IWaveBuffer
        {
            var inRate = src.SampleRate;
            var inCh = src.Channels;
            var outRate = dst.SampleRate;
            var outCh = dst.Channels;
            WaveBuffer.AdjustArgs(src.Data, ref srcOffset, ref srcSamples, inCh);
            var srcSpan = src.Data.Slice(srcOffset * inCh, srcSamples * inCh);
            if (WaveBuffer.ChangeLayoutCore(srcSpan, inRate, inCh, outRate, outCh, out var convertBuffer, out var outTotalSample))
            {
                func(dst, convertBuffer.AsSpan(0, outTotalSample), dstOffset);
                ArrayPool<float>.Shared.Return(convertBuffer);
            }
            else
            {
                func(dst, srcSpan, dstOffset);
            }
        }

        private static void AppendCore<T>(this T buffer, ReadOnlySpan<float> source, int sampleOffset)
            where T : IWaveBuffer
        {
            var ch = buffer.Channels;
            var offset = sampleOffset * ch;
            var count = source.Length;
            if (buffer.TotalSample < offset + count)
            {
                buffer.SetTotalSample(offset + count, true);
            }
            var span = buffer.Data;
            span.Slice(offset, count).Add(source);
        }

        public static void Append<T>(this T buffer, ReadOnlySpan<float> source)
            where T : IWaveBuffer
        {
            AppendCore(buffer, source, buffer.SampleLength);
        }

        public static void AppendAt<T>(this T buffer, ReadOnlySpan<float> source, int sampleOffset)
            where T : IWaveBuffer
        {
            WaveBuffer.AdjustArgs(buffer.Data, ref sampleOffset, buffer.Channels);
            AppendCore(buffer, source, sampleOffset);
        }

        public static void Append<T1, T2>(this T1 dst, T2 src, int dstOffset = 0, int srcOffset = 0, int srcSamples = 0)
            where T1 : IWaveBuffer
            where T2 : IWaveBuffer
        {
            ConvertProcess(dst, src, dstOffset, srcOffset, srcSamples, AppendCore);
        }

        public static void SetValue<T>(this T buffer, float value, int sampleOffset = 0, int sampleCount = 0)
            where T : IWaveBuffer
        {
            WaveBuffer.SetValue(buffer.Data, value, sampleOffset, sampleCount, buffer.Channels);
        }

        public static void CopyFrom<T>(this T buffer, ReadOnlySpan<float> src, int sampleOffset = 0)
            where T : IWaveBuffer
        {
            WaveBuffer.CopyFrom(buffer.Data, src, sampleOffset, buffer.Channels);
        }

        public static void CopyFrom<T1, T2>(this T1 dst, T2 src, int dstOffset = 0, int srcOffset = 0, int srcSamples = 0)
            where T1 : IWaveBuffer
            where T2 : IWaveBuffer
        {
            ConvertProcess(dst, src, dstOffset, srcOffset, srcSamples, CopyFrom);
        }

        public static void Add<T>(this T buffer, float value, int sampleOffset = 0, int sampleCount = 0)
            where T : IWaveBuffer
        {
            WaveBuffer.Add(buffer.Data, value, sampleOffset, sampleCount, buffer.Channels);
        }

        public static void Add<T>(this T buffer, ReadOnlySpan<float> src, int sampleOffset = 0)
            where T : IWaveBuffer
        {
            WaveBuffer.Add(buffer.Data, src, sampleOffset, buffer.Channels);
        }

        public static void Add<T1, T2>(this T1 dst, T2 src, int dstOffset = 0, int srcOffset = 0, int srcSamples = 0)
            where T1 : IWaveBuffer
            where T2 : IWaveBuffer
        {
            ConvertProcess(dst, src, dstOffset, srcOffset, srcSamples, Add);
        }

        public static void Multiply<T>(this T buffer, float value, int sampleOffset = 0, int sampleCount = 0)
            where T : IWaveBuffer
        {
            WaveBuffer.Multiply(buffer.Data, value, sampleOffset, sampleCount, buffer.Channels);
        }

        public static void Multiply<T>(this T buffer, ReadOnlySpan<float> src, int sampleOffset = 0)
            where T : IWaveBuffer
        {
            WaveBuffer.Multiply(buffer.Data, src, sampleOffset, buffer.Channels);
        }

        public static void Multiply<T1, T2>(this T1 dst, T2 src, int dstOffset = 0, int srcOffset = 0, int srcSamples = 0)
            where T1 : IWaveBuffer
            where T2 : IWaveBuffer
        {
            ConvertProcess(dst, src, dstOffset, srcOffset, srcSamples, Multiply);
        }

        public static void Invert<T>(this T buffer, int sampleOffset = 0, int sampleCount = 0)
            where T : IWaveBuffer
        {
            WaveBuffer.Invert(buffer.Data, sampleOffset, sampleCount, buffer.Channels);
        }

        public static void FadeIn<T>(this T buffer, int sampleOffset = 0, int sampleCount = 0, double power = 0.5)
            where T : IWaveBuffer
        {
            WaveBuffer.FadeIn(buffer.Data, sampleOffset, sampleCount, (float)power, buffer.Channels);
        }

        public static void FadeOut<T>(this T buffer, int sampleOffset = 0, int sampleCount = 0, double power = 0.5)
            where T : IWaveBuffer
        {
            WaveBuffer.FadeOut(buffer.Data, sampleOffset, sampleCount, (float)power, buffer.Channels);
        }

        public static void SetValue<T>(this T buffer, SetSampleFunc func, int sampleOffset = 0, int sampleCount = 0)
            where T : IWaveBuffer
        {
            WaveBuffer.SetValue(buffer.Data, func, sampleOffset, sampleCount, buffer.Channels);
        }

        public static float GetMaxMagnitude<T>(this T buffer, int sampleOffset = 0, int sampleCount = 0)
            where T : IWaveBuffer
        {
            return WaveBuffer.GetMaxMagnitude(buffer.Data, sampleOffset, sampleCount, buffer.Channels);
        }

        public static void Clamp<T>(this T buffer, float value = 1, int sampleOffset = 0, int sampleCount = 0)
            where T : IWaveBuffer
        {
            WaveBuffer.Clamp(buffer.Data, value, sampleOffset, sampleCount, buffer.Channels);
        }

        public static int FindFirstSound<T>(this T buffer, float threshold, int sampleAfter = 0)
            where T : IWaveBuffer
        {
            return WaveBuffer.FindFirstSound(buffer.Data, threshold, sampleAfter, buffer.Channels);
        }

        public static int FindLastSound<T>(this T buffer, float threshold, int sampleBefore = -1)
            where T : IWaveBuffer
        {
            return WaveBuffer.FindLastSound(buffer.Data, threshold, sampleBefore, buffer.Channels);
        }

        public static (int, int) FindBothSilence<T>(this T buffer, float th1, float th2 = -1, int sliceOffset = 0, int sliceCount = 0)
            where T : IWaveBuffer
        {
            return WaveBuffer.FindBothSilence(buffer.Data, th1, th2, sliceOffset, sliceCount, buffer.Channels);
        }

        public static SliceWithCutSilenceResult SliceWithCutSilence<T>(this T buffer, int sliceOffset, int sliceCount, float th1, float th2 = -1, int marginLeft = 0, int marginRight = 0)
            where T : IWaveBuffer
        {
            return WaveBuffer.SliceWithCutSilence(buffer.Data, th1, th2, sliceOffset, sliceCount, marginLeft, marginRight, buffer.Channels);
        }

        public static void CutLeadingSilence<T>(this T buffer, float threshold, int margin)
            where T : IWaveBuffer
        {
            var offset = FindFirstSound(buffer, threshold);
            offset = Math.Max(offset - margin, 0);
            RemoveBefore(buffer, offset);
        }

        public static void CutTrailingSilence<T>(this T buffer, float threshold, int margin)
            where T : IWaveBuffer
        {
            var offset = FindLastSound(buffer, threshold);
            offset = Math.Min(offset + margin + 1, buffer.SampleLength);
            RemoveAfter(buffer, offset);
        }

        public static void CutBothSilence<T>(this T buffer, float th1, float th2 = -1, int marginLeft = 0, int marginRight = 0)
            where T : IWaveBuffer
        {
            var (left, right) = FindBothSilence(buffer, th1, th2);
            (left, right) = WaveBuffer.ApplyMargin(left, right, marginLeft, marginRight, 0, buffer.SampleLength - 1);
            Splice(buffer, left, right - left + 1);
        }

        public static float[] GetPeak<T>(this T buffer, int sampleOffset = 0, int sampleCount = 0)
            where T : IWaveBuffer
        {
            return WaveBuffer.GetPeak(buffer.Data, sampleOffset, sampleCount, buffer.Channels);
        }

        public static float[] GetRms<T>(this T buffer, int sampleOffset = 0, int sampleCount = 0)
            where T : IWaveBuffer
        {
            return WaveBuffer.GetRms(buffer.Data, sampleOffset, sampleCount, buffer.Channels);
        }
    }
}
