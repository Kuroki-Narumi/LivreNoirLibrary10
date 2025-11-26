using System;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IWaveBufferProvider
    {
        public bool TryGetWaveBuffer(string path, [MaybeNullWhen(false)] out IWaveBuffer waveBuffer);
    }
}
