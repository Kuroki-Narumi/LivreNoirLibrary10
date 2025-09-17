using System;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IWaveBuffer : IAudioBuffer
    {
        public Span<float> Data { get; }

        public void SetTotalSample(int size, bool clear);
        public void SetLayout(int sampleRate, int channels);
    }
}
