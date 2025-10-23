using System;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Wave
{
    public partial class BiQuadFilter
    {
        private double _a1;
        private double _a2;
        private double _b0 = 1;
        private double _b1;
        private double _b2;

        private double _in_1;
        private double _in_2;
        private double _out_1;
        private double _out_2;

        public void Setup(double a0, double a1, double a2, double b0, double b1, double b2)
        {
            _a1 = a1 / a0;
            _a2 = a2 / a0;
            _b0 = b0 / a0;
            _b1 = b1 / a0;
            _b2 = b2 / a0;
            ClearState();
        }

        public void ClearState()
        {
            _in_1 = _in_2 = _out_1 = _out_2 = 0;
        }

        public float Apply(float input)
        {
            var in0 = (double)input;
            var in1 = _in_1;
            var in2 = _in_2;
            var out1 = _out_1;
            var out2 = _out_2;

            var out0 = _b0 * in0 + _b1 * in1 * _b2 * in2 - _a1 * out1 - _a2 * out2;
            _in_1 = in0;
            _in_2 = in1;
            _out_1 = out0;
            _out_2 = out1;
            return (float)out0;
        }

        public double Apply(double input)
        {
            var in0 = input;
            var in1 = _in_1;
            var in2 = _in_2;
            var out1 = _out_1;
            var out2 = _out_2;

            var out0 = _b0 * in0 + _b1 * in1 * _b2 * in2 - _a1 * out1 - _a2 * out2;
            _in_1 = in0;
            _in_2 = in1;
            _out_1 = out0;
            _out_2 = out1;
            return out0;
        }

        public void Apply(ReadOnlySpan<float> input, Span<float> output)
        {
            var count = output.Length;
            var i1 = _in_1;
            var i2 = _in_2;
            var o1 = _out_1;
            var o2 = _out_2;
            var a1 = _a1;
            var a2 = _a2;
            var b0 = _b0;
            var b1 = _b1;
            var b2 = _b2;
            for (var k = 0; k < count; k++)
            {
                // input == output の場合に備えて入力は一時変数に格納しておく
                var i0 = (double)input[k];
                var o0 = b0 * i0 + b1 * i1 + b2 * i2 - a1 * o1 - a2 * o2;
                output[k] = (float)o0;
                (i1, i2) = (i0, i1);
                (o1, o2) = (o0, o1);
            }
            _in_1 = i1;
            _in_2 = i2;
            _out_1 = o1;
            _out_2 = o2;
        }

        public void Apply(ReadOnlySpan<double> input, Span<double> output)
        {
            var count = output.Length;
            var i1 = _in_1;
            var i2 = _in_2;
            var o1 = _out_1;
            var o2 = _out_2;
            var a1 = _a1;
            var a2 = _a2;
            var b0 = _b0;
            var b1 = _b1;
            var b2 = _b2;
            for (var k = 0; k < count; k++)
            {
                // input == output の場合に備えて入力は一時変数に格納しておく
                var i0 = input[k];
                var o0 = b0 * i0 + b1 * i1 + b2 * i2 - a1 * o1 - a2 * o2;
                output[k] = o0;
                (i1, i2) = (i0, i1);
                (o1, o2) = (o0, o1);
            }
            _in_1 = i1;
            _in_2 = i2;
            _out_1 = o1;
            _out_2 = o2;
        }

        public unsafe void Apply(float* pointer, int count)
        {
            var i1 = _in_1;
            var i2 = _in_2;
            var o1 = _out_1;
            var o2 = _out_2;
            var a1 = _a1;
            var a2 = _a2;
            var b0 = _b0;
            var b1 = _b1;
            var b2 = _b2;
            for (var k = 0; k < count; k++)
            {
                // input == output の場合に備えて入力は一時変数に格納しておく
                var i0 = (double)pointer[k];
                var o0 = b0 * i0 + b1 * i1 + b2 * i2 - a1 * o1 - a2 * o2;
                pointer[k] = (float)o0;
                (i1, i2) = (i0, i1);
                (o1, o2) = (o0, o1);
            }
            _in_1 = i1;
            _in_2 = i2;
            _out_1 = o1;
            _out_2 = o2;
        }

        public unsafe void Apply(double* pointer, int count)
        {
            var i1 = _in_1;
            var i2 = _in_2;
            var o1 = _out_1;
            var o2 = _out_2;
            var a1 = _a1;
            var a2 = _a2;
            var b0 = _b0;
            var b1 = _b1;
            var b2 = _b2;
            for (var k = 0; k < count; k++)
            {
                // input == output の場合に備えて入力は一時変数に格納しておく
                var i0 = pointer[k];
                var o0 = b0 * i0 + b1 * i1 + b2 * i2 - a1 * o1 - a2 * o2;
                pointer[k] = o0;
                (i1, i2) = (i0, i1);
                (o1, o2) = (o0, o1);
            }
            _in_1 = i1;
            _in_2 = i2;
            _out_1 = o1;
            _out_2 = o2;
        }

        public unsafe void Apply(ReadOnlySpan<float> input, Span<float> output, int channels)
        {
            if (channels is <= 1)
            {
                Apply(input, output);
                return;
            }

            var buffer = stackalloc double[channels * 4];
            SimdOperations.Clear(buffer, channels * 4);

            var count = output.Length / channels;
            var index = 0;
            var a1 = _a1;
            var a2 = _a2;
            var b0 = _b0;
            var b1 = _b1;
            var b2 = _b2;
            for (var k = 0; k < count; k++)
            {
                for (var c = 0; c < channels; c++, index++)
                {
                    var cc = buffer + (c << 2);
                    var i0 = (double)input[index];
                    var i1 = cc[0];
                    var i2 = cc[1];
                    var o1 = cc[2];
                    var o2 = cc[3];
                    var o0 = b0 * i0 + b1 * i1 + b2 * i2 - a1 * o1 - a2 * o2;
                    output[index] = (float)o0;
                    cc[0] = i0;
                    cc[1] = i1;
                    cc[2] = o0;
                    cc[3] = o1;
                }
            }
        }

        public static BiQuadFilter LowPass(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            BiQuadFilter filter = new();
            filter.SetupLowPass(sampleRate, frequency, qualityFactor);
            return filter;
        }

        public static BiQuadFilter HighPass(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            BiQuadFilter filter = new();
            filter.SetupHighPass(sampleRate, frequency, qualityFactor);
            return filter;
        }

        public static BiQuadFilter BandPassConstantSkirtGain(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            BiQuadFilter filter = new();
            filter.SetupBandPassConstantSkirtGain(sampleRate, frequency, qualityFactor);
            return filter;
        }

        public static BiQuadFilter BandPassConstantPeakGain(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            BiQuadFilter filter = new();
            filter.SetupBandPassConstantPeakGain(sampleRate, frequency, qualityFactor);
            return filter;
        }

        public static BiQuadFilter BandStop(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            BiQuadFilter filter = new();
            filter.SetupBandStop(sampleRate, frequency, qualityFactor);
            return filter;
        }

        public static BiQuadFilter LowShelf(int sampleRate, double frequency, double qualityFactor = InvSqrt2, double gain = 0)
        {
            BiQuadFilter filter = new();
            filter.SetupLowShelf(sampleRate, frequency, qualityFactor, gain);
            return filter;
        }

        public static BiQuadFilter HighShelf(int sampleRate, double frequency, double qualityFactor = InvSqrt2, double gain = 0)
        {
            BiQuadFilter filter = new();
            filter.SetupHighShelf(sampleRate, frequency, qualityFactor, gain);
            return filter;
        }

        public static BiQuadFilter Peaking(int sampleRate, double frequency, double qualityFactor = InvSqrt2, double gain = 0)
        {
            BiQuadFilter filter = new();
            filter.SetupPeaking(sampleRate, frequency, qualityFactor, gain);
            return filter;
        }

        public static BiQuadFilter AllPass(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            BiQuadFilter filter = new();
            filter.SetupAllPass(sampleRate, frequency, qualityFactor);
            return filter;
        }

        #region Setup

        public const double PI2 = 2 * Math.PI;
        public const double InvSqrt2 = 0.70710678118654752440084436210485; // 1 / sqrt(2)
        public const double HalfLog2 = 0.34657359027997265470861606072909;    // ln(2) / 2

        private static double Alpha(double sin, double qualityFactor) => sin * 0.5f / qualityFactor;
        private static double Alpha2(double omega, double sin, double qualityFactor) => sin * Math.Sinh(HalfLog2 * qualityFactor * omega / sin);

        public void Clear()
        {
            Setup(1, 0, 0, 1, 0, 0);
        }

        public void SetupLowPass(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            var omega = PI2 * frequency / sampleRate;
            var cos = Math.Cos(omega);
            var sin = Math.Sin(omega);
            var alpha = Alpha(sin, qualityFactor);
            Setup(
                1 + alpha,
                -2 * cos,
                1 - alpha,
                0.5f - cos * 0.5f,
                1 - cos,
                0.5f - cos * 0.5f
                );
        }

        public void SetupHighPass(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var alpha = Alpha(sin, qualityFactor);
            Setup(
                1 + alpha,
                -2 * cos,
                1 - alpha,
                0.5f + cos * 0.5f,
                -1 - cos,
                0.5f + cos * 0.5f
                );
        }

        public void SetupBandPassConstantSkirtGain(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var alpha = Alpha(sin, qualityFactor);
            Setup(
                1 + alpha,
                -2 * cos,
                1 - alpha,
                sin * 0.5,
                0,
                -sin * 0.5
            );
        }

        public void SetupBandPassConstantPeakGain(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var alpha = Alpha(sin, qualityFactor);
            Setup(
                1 + alpha,
                -2 * cos,
                1 - alpha,
                alpha,
                0,
                -alpha
            );
        }

        public void SetupBandStop(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var alpha = Alpha(sin, qualityFactor);
            Setup(
                1 + alpha,
                -2 * cos,
                1 - alpha,
                1,
                -2 * cos,
                1
            );
        }

        public void SetupLowShelf(int sampleRate, double frequency, double qualityFactor = InvSqrt2, double gain = 0)
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
            Setup(
                ap1 + am1cos + betasin,
                -2 * (am1 + ap1cos),
                ap1 + am1cos - betasin,
                amp * (ap1 - am1cos + betasin),
                2 * amp * (am1 - ap1cos),
                amp * (ap1 - am1cos - betasin)
            );
        }

        public void SetupHighShelf(int sampleRate, double frequency, double qualityFactor = InvSqrt2, double gain = 0)
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
            Setup(
                ap1 - am1cos + betasin,
                2 * (am1 - ap1cos),
                ap1 - am1cos - betasin,
                amp * (ap1 + am1cos + betasin),
                -2 * amp * (am1 + ap1cos),
                amp * (ap1 + am1cos - betasin)
            );
        }

        public void SetupPeaking(int sampleRate, double frequency, double qualityFactor = InvSqrt2, double gain = 0)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var alpha = Alpha(sin, qualityFactor);
            var amp = Math.Pow(10, gain * 0.025);
            var ama = alpha * amp;
            var ada = alpha / amp;
            Setup(
                1 + ada,
                -2 * cos,
                1 - ada,
                1 + ama,
                -2 * cos,
                1 - ama
            );
        }

        public void SetupAllPass(int sampleRate, double frequency, double qualityFactor = InvSqrt2)
        {
            var omega = PI2 * frequency / sampleRate;
            var sin = Math.Sin(omega);
            var cos = Math.Cos(omega);
            var alpha = Alpha(sin, qualityFactor);
            Setup(
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