using System;

namespace LivreNoirLibrary.Media.Wave
{
    public struct BiQuadFilterState
    {
        private double _in_1, _in_2, _out_1, _out_2;

        public readonly void Deconstruct(out double in1, out double in2, out double out1, out double out2)
        {
            in1 = _in_1;
            in2 = _in_2;
            out1 = _out_1;
            out2 = _out_2;
        }

        public void Update(double in1, double in2, double out1, double out2)
        {
            _in_1 = in1;
            _in_2 = in2;
            _out_1 = out1;
            _out_2 = out2;
        }
    }

    public readonly struct BiQuadFilter(double a0, double a1, double a2, double b0, double b1, double b2)
    {
        public readonly double A1 = a1 / a0;
        public readonly double A2 = a2 / a0;
        public readonly double B0 = b0 / a0;
        public readonly double B1 = b1 / a0;
        public readonly double B2 = b2 / a0;

        public double Apply(double input, ref BiQuadFilterState state)
        {
            var (in1, in2, out1, out2) = state;
            var output = B0 * input + B1 * in1 + B2 * in2 - A1 * out1 - A2 * out2;
            state.Update(input, in1, output, out1);
            return output;
        }

        public float Apply(float input, ref BiQuadFilterState state) => (float)Apply((double)input, ref state);

        public void Apply(ReadOnlySpan<float> input, Span<float> output, ref BiQuadFilterState state)
        {
            var count = Math.Min(input.Length, output.Length);
            var (i1, i2, o1, o2) = state;
            var a1 = A1;
            var a2 = A2;
            var b0 = B0;
            var b1 = B1;
            var b2 = B2;
            for (var k = 0; k < count; k++)
            {
                var i0 = (double)input[k];
                var o0 = b0 * i0 + b1 * i1 + b2 * i2 - a1 * o1 - a2 * o2;
                output[k] = (float)o0;
                (i1, i2) = (i0, i1);
                (o1, o2) = (o0, o1);
            }
            state.Update(i1, i2, o1, o2);
        }

        public void Apply(Span<float> span)
        {
            BiQuadFilterState state = new();
            Apply(span, span, ref state);
        }

        public void Apply(ReadOnlySpan<double> input, Span<double> output, ref BiQuadFilterState state)
        {
            var count = Math.Min(input.Length, output.Length);
            var (i1, i2, o1, o2) = state;
            var a1 = A1;
            var a2 = A2;
            var b0 = B0;
            var b1 = B1;
            var b2 = B2;
            for (var k = 0; k < count; k++)
            {
                var i0 = input[k];
                var o0 = b0 * i0 + b1 * i1 + b2 * i2 - a1 * o1 - a2 * o2;
                output[k] = o0;
                (i1, i2) = (i0, i1);
                (o1, o2) = (o0, o1);
            }
            state.Update(i1, i2, o1, o2);
        }

        public void Apply(Span<double> span)
        {
            BiQuadFilterState state = new();
            Apply(span, span, ref state);
        }

        public void ApplyMultiChannel(ReadOnlySpan<float> input, Span<float> output, Span<BiQuadFilterState> states, bool transpose = false)
        {
            var channels = states.Length;
            var sampleLength = Math.Min(input.Length, output.Length) / channels;
            var a1 = A1;
            var a2 = A2;
            var b0 = B0;
            var b1 = B1;
            var b2 = B2;
            var inputIndex = 0;
            for (var sample = 0; sample < sampleLength; sample++)
            {
                for (var channel = 0; channel < channels; channel++, inputIndex++)
                {
                    ref var state = ref states[channel];
                    var (i1, i2, o1, o2) = state;
                    var i0 = (double)input[inputIndex];
                    var o0 = b0 * i0 + b1 * i1 + b2 * i2 - a1 * o1 - a2 * o2;
                    var outputIndex = transpose ? channel * sampleLength + sample : inputIndex;
                    output[outputIndex] = (float)o0;
                    state.Update(i0, i1, o0, o1);
                }
            }
        }

        #region Presets

        public const double PI2 = 2 * Math.PI;
        public const double InvSqrt2 = 0.70710678118654752440084436210485; // 1 / sqrt(2)

        private static double Alpha(double sin, double qualityFactor) => sin * 0.5f / qualityFactor;

        public static BiQuadFilter Default { get; } = new(1, 0, 0, 1, 0, 0);

        public static BiQuadFilter LowPass(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var alpha = Alpha(sin, qualityFactor);
            return new(
                1 + alpha,
                -2 * cos,
                1 - alpha,
                0.5f - cos * 0.5f,
                1 - cos,
                0.5f - cos * 0.5f
                );
        }

        public static BiQuadFilter HighPass(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var alpha = Alpha(sin, qualityFactor);
            return new(
                1 + alpha,
                -2 * cos,
                1 - alpha,
                0.5f + cos * 0.5f,
                -1 - cos,
                0.5f + cos * 0.5f
                );
        }

        public static BiQuadFilter BandPassConstantSkirtGain(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var alpha = Alpha(sin, qualityFactor);
            return new(
                1 + alpha,
                -2 * cos,
                1 - alpha,
                sin * 0.5,
                0,
                -sin * 0.5
            );
        }

        public static BiQuadFilter BandPassConstantPeakGain(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var alpha = Alpha(sin, qualityFactor);
            return new(
                1 + alpha,
                -2 * cos,
                1 - alpha,
                alpha,
                0,
                -alpha
            );
        }

        public static BiQuadFilter BandStop(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var alpha = Alpha(sin, qualityFactor);
            return new(
                1 + alpha,
                -2 * cos,
                1 - alpha,
                1,
                -2 * cos,
                1
            );
        }

        public static BiQuadFilter LowShelf(int sampleRate, double frequency, double qualityFactor = InvSqrt2, double gain = 0)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var amp = Math.Pow(10, gain * 0.025);
            var beta = Math.Sqrt(amp) / qualityFactor;
            var ap1 = amp + 1;
            var am1 = amp - 1;
            var ap1cos = ap1 * cos;
            var am1cos = am1 * cos;
            var betasin = beta * sin;
            return new(
                ap1 + am1cos + betasin,
                -2 * (am1 + ap1cos),
                ap1 + am1cos - betasin,
                amp * (ap1 - am1cos + betasin),
                2 * amp * (am1 - ap1cos),
                amp * (ap1 - am1cos - betasin)
            );
        }

        public static BiQuadFilter HighShelf(int sampleRate, double frequency, double qualityFactor = InvSqrt2, double gain = 0)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var amp = Math.Pow(10, gain * 0.025);
            var beta = Math.Sqrt(amp) / qualityFactor;
            var ap1 = amp + 1;
            var am1 = amp - 1;
            var ap1cos = ap1 * cos;
            var am1cos = am1 * cos;
            var betasin = beta * sin;
            return new(
                ap1 - am1cos + betasin,
                2 * (am1 - ap1cos),
                ap1 - am1cos - betasin,
                amp * (ap1 + am1cos + betasin),
                -2 * amp * (am1 + ap1cos),
                amp * (ap1 + am1cos - betasin)
            );
        }

        public static BiQuadFilter Peaking(int sampleRate, double frequency, double qualityFactor = InvSqrt2, double gain = 0)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var alpha = Alpha(sin, qualityFactor);
            var amp = Math.Pow(10, gain * 0.025);
            var ama = alpha * amp;
            var ada = alpha / amp;
            return new(
                1 + ada,
                -2 * cos,
                1 - ada,
                1 + ama,
                -2 * cos,
                1 - ama
            );
        }

        public static BiQuadFilter AllPass(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var alpha = Alpha(sin, qualityFactor);
            return new(
                1 + alpha,
                -2 * cos,
                1 - alpha,
                1 - alpha,
                -2 * cos,
                1 + alpha
            );
        }

        #endregion
    }
}