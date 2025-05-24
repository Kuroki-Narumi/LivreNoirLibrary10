using System;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Wave
{
    public partial class Analysis
    {
        public class Channel
        {
            public float[] Data { get; }
            public float[] FFTResult { get; }
            public float[] MeanSquare { get; }
            public float[] Centroids { get; }

            public Channel(int sampleCount)
            {
                Data = new float[sampleCount];
                FFTResult = new float[FFT.CalcFittingLength(sampleCount) * 2];
                MeanSquare = new float[sampleCount / 441];
                Centroids = [];
            }

            public Channel(IWaveBuffer data, int channel)
            {
                var sampleRate = data.SampleRate;
                var buffer = new float[data.SampleLength];
                data.GetChannelComplex(buffer, channel);
                Data = buffer;
                FFTResult = FFT.Forward(buffer);
                MeanSquare = CalcMeanSquare(buffer, sampleRate);
                Centroids = FFT.Centroids(buffer, sampleRate);
            }

            private static float[] CalcMeanSquare(ReadOnlySpan<float> source, int sampleRate)
            {
                var len = source.Length;
                var unit = sampleRate / 20;
                var resultLength = len / unit;
                var max = resultLength * unit;
                var remain = len - max;
                var result = new float[remain is > 0 ? resultLength + 1 : resultLength];
                var index = 0;
                for (var i = 0; index < max; i++, index += unit)
                {
                    result[i] = MathF.Sqrt(source.Slice(index, unit).MeanSquare());
                }
                if (remain is > 0)
                {
                    result[resultLength] = MathF.Sqrt(source[index..len].MeanSquare());
                }
                return result;
            }
        }
    }
}
