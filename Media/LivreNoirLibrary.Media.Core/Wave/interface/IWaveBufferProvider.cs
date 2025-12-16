using LivreNoirLibrary.ObjectModel;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IWaveBufferProvider<TKey> : IClear
    {
        int OutputSampleRate { get; set; }
        int OutputChannels { get; set; }

        bool TryGetWaveBuffer(TKey key, [MaybeNullWhen(false)] out IWaveBuffer waveBuffer);
    }
}
