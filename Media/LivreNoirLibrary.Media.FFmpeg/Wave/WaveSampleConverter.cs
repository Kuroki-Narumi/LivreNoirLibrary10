using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Wave
{
    internal unsafe interface IWaveSampleConverter
    {
        public int BytesPerSample { get; }
        public void ConvertRead(byte* source, float* target, int bytes);
        public void ConvertWrite(float* source, byte* target, int totalSamples);

        public static IWaveSampleConverter? GetConverter(SampleFormat format) => format switch
        {
            SampleFormat.Float32 => new Float32WaveSampleConverter(),
            SampleFormat.UInt8 => new UInt8WaveSampleConverter(),
            SampleFormat.Int16 => new Int16WaveSampleConverter(),
            SampleFormat.Int24 => new Int24WaveSampleConverter(),
            SampleFormat.Int32 => new Int32WaveSampleConverter(),
            SampleFormat.Int48 => new Int48WaveSampleConverter(),
            SampleFormat.Int64 => new Int64WaveSampleConverter(),
            _ => null,
        };
    }

    internal sealed unsafe class Float32WaveSampleConverter : IWaveSampleConverter
    {
        public int BytesPerSample => sizeof(float);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ConvertRead(byte* source, float* target, int bytes)
        {
            SimdOperations.CopyFrom(target, (float*)source, bytes / sizeof(float));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ConvertWrite(float* source, byte* target, int totalSamples)
        {
            SimdOperations.CopyFrom((float*)target, source, totalSamples);
        }
    }

    internal abstract unsafe class IntWaveSampleConverterBase : IWaveSampleConverter
    {
        public abstract int BytesPerSample { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract float ReadSample(byte* source);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void WriteSample(byte* target, float value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ConvertRead(byte* source, float* target, int bytes)
        {
            var bps = BytesPerSample;
            for (var i = 0; i < bytes; i += bps, target++)
            {
                *target = ReadSample(source + i);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ConvertWrite(float* source, byte* target, int totalSamples)
        {
            var bps = BytesPerSample;
            var bytes = totalSamples * bps;
            for (var i = 0; i < bytes; i += bps, source++)
            {
                WriteSample(target + i, *source);
            }
        }
    }

    internal sealed unsafe class UInt8WaveSampleConverter : IntWaveSampleConverterBase
    {
        public const float Factor = sbyte.MaxValue + 1f;
        public const float FactorI = 1f / Factor;

        public override int BytesPerSample => sizeof(byte);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ReadSample(byte* source) => (*source - Factor) * FactorI;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteSample(byte* target, float value)
        {
            var v = Math.Clamp(value * Factor + Factor + 0.5f, 0, byte.MaxValue);
            *target = (byte)v;
        }
    }

    internal sealed unsafe class Int16WaveSampleConverter : IntWaveSampleConverterBase
    {
        public const float Factor = short.MaxValue + 1f;
        public const float FactorI = 1f / Factor;

        public override int BytesPerSample => sizeof(short);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ReadSample(byte* source) => *(short*)source * FactorI;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteSample(byte* target, float value)
        {
            var v = Math.Clamp(value * Factor, short.MinValue, short.MaxValue);
            *(short*)target = (short)v;
        }
    }

    internal sealed unsafe class Int24WaveSampleConverter : IntWaveSampleConverterBase
    {
        public const int MinValue = -(1 << 23);
        public const int MaxValue = -MinValue - 1;
        public const float Factor = -MinValue;
        public const float FactorI = 1f / Factor;

        public override int BytesPerSample => 3;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ReadSample(byte* source)
        {
            var v = *(ushort*)source | ((sbyte)source[2] << 16);
            return v * FactorI;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteSample(byte* target, float value)
        {
            var v = (int)Math.Clamp(value * Factor, MinValue, MaxValue);
            *(ushort*)target = (ushort)(v & 0xffff);
            target[2] = (byte)(v >> 16);
        }
    }

    internal sealed unsafe class Int32WaveSampleConverter : IntWaveSampleConverterBase
    {
        public const float Factor = int.MaxValue + 1f;
        public const float FactorI = 1f / Factor;

        public override int BytesPerSample => sizeof(int);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ReadSample(byte* source) => *(int*)source * FactorI;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteSample(byte* target, float value)
        {
            var v = Math.Clamp(value * Factor, int.MinValue, int.MaxValue);
            *(int*)target = (int)v;
        }
    }

    internal sealed unsafe class Int48WaveSampleConverter : IntWaveSampleConverterBase
    {
        public const long MinValue = -(1L << 47);
        public const long MaxValue = -MinValue - 1;
        public const float Factor = -MinValue;
        public const float FactorI = 1f / Factor;

        public override int BytesPerSample => 6;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ReadSample(byte* source)
        {
            var v = *(uint*)source | (((long)*(short*)source[4]) << 32);
            return v * FactorI;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteSample(byte* target, float value)
        {
            var v = (long)Math.Clamp(value * Factor, MinValue, MaxValue);
            *(uint*)target = (uint)(v & 0xffffffff);
            *(short*)target[4] = (short)(v >> 32);
        }
    }

    internal sealed unsafe class Int64WaveSampleConverter : IntWaveSampleConverterBase
    {
        public const float Factor = long.MaxValue + 1f;
        public const float FactorI = 1f / Factor;

        public override int BytesPerSample => sizeof(long);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ReadSample(byte* source) => *(long*)source * FactorI;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void WriteSample(byte* target, float value)
        {
            var v = Math.Clamp(value * Factor, long.MinValue, long.MaxValue);
            *(long*)target = (long)v;
        }
    }
}
