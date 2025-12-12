using LivreNoirLibrary.ObjectModel;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IWaveBufferProvider<TKey> : IClear
    {
        public int OutputSampleRate { get; set; }
        public int OutputChannels { get; set; }

        public bool TryGetWaveBuffer(TKey key, [MaybeNullWhen(false)] out IWaveBuffer waveBuffer);
    }
}
