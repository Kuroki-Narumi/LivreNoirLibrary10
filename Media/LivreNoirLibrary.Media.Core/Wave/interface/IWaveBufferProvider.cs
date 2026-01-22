using LivreNoirLibrary.ObjectModel;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IWaveBufferProvider<TKey> : IClear
    {
        int SampleRate { get; set; }
        int Channels { get; set; }

        bool TryGetWaveBuffer(TKey key, [MaybeNullWhen(false)] out IWaveBuffer waveBuffer);
    }
}
