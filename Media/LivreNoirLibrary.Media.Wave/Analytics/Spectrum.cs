using System;
using System.Buffers;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Wave
{
    public class Spectrum
    {
        private readonly IWaveBuffer _source;

        public int Channels => _source.Channels;
        public int SampleRate => _source.SampleRate;
        public double MaxLevel { get; set; } = 0.0;
        public double MinLevel { get; set; } = -90.0;
        public double Factor { get => _factor; set => _factor = Math.Clamp(value, 0, 1); }
        private double _factor = 1.0;

        private readonly int _ww;
        private readonly double[][] _fft_result;

        public ReadOnlySpan<double> Channel(int channel) => _fft_result[channel];
        public ReadOnlySpan<double> Range(int channel, long offset, long count) => new(_fft_result[channel], (int)offset, (int)count);

        public Spectrum(IWaveBuffer source, int ww = FFT.DefaultWindowWidth)
        {
            _source = source;
            _ww = ww;
            var ch = source.Channels;
            _fft_result = new double[ch][];
            var w2 = ww / 2;
            for (int c = 0; c < ch; c++)
            {
                _fft_result[c] = new double[w2];
            }
        }

        public unsafe void Update(long position)
        {
            var slen = _source.SampleLength;
            if (slen is 0)
            {
                return;
            }
            if (position >= slen) { position = slen - 1; }
            if (position is < 0) { position = 0; }
            var min = MinLevel / 20.0;
            var range = MaxLevel / 20.0 - min;
            if (range <= 0)
            {
                range = 1;
            }
            var ch = Channels;
            var ww = _ww;
            var w2 = ww / 2;
            var factor = _factor;
            var hamming = FFT.GetHammingComplex32(ww);
            var src = _source;
            var levelFactor = factor / range;
            var prevFactor = 1 - factor;
            var ary = ArrayPool<float>.Shared.Rent(ww * 2);
            try
            {
                var buffer = ary.AsSpan(0, ww * 2);
                for (int c = 0; c < ch; c++)
                {
                    src.GetChannelComplex(buffer, c, (int)position);
                    buffer.Multiply(hamming);
                    fixed (float* bufferPtr = buffer)
                    fixed (double* resultPtr = _fft_result[c])
                    {
                        FFT.FFTCore(true, bufferPtr, ww);
                        var ptr = bufferPtr;
                        for (int i = 0; i < w2; i++)
                        {
                            var value = *ptr * *ptr++ + *ptr * *ptr++;
                            var level = Math.Max(Math.Log10(value) / 2 - min, 0);
                            resultPtr[i] = level * levelFactor + resultPtr[i] * prevFactor;
                        }
                    }
                }
            }
            finally
            {
                ArrayPool<float>.Shared.Return(ary);
            }
        }
    }
}
