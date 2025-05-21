using System;
using LivreNoirLibrary.Media.FFmpeg;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IWaveBuffer
    {
        public Span<float> Data { get; }
        public int TotalSample { get; }
        public int SampleRate { get; }
        public int Channels { get; }

        public int SampleLength => TotalSample / Channels;
        public double TotalSeconds => (double)SampleLength / SampleRate;
        public TimeSpan TotalTime => TimeSpan.FromSeconds(TotalSeconds);

        public void SetTotalSample(int size, bool clear);
        public void SetLayout(int sampleRate, int channels);
        public void WriteMetaTags(IMetaTag format, IMetaTag stream) { }
    }
}
