using System;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IWaveBuffer : IAudioBuffer
    {
        Span<float> Data { get; }

        void SetTotalSample(int size, bool clear);
        void SetLayout(int sampleRate, int channels);
    }
}
