using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IWaveMetaData
    {
        const double DefaultTempo = 130;

        FormatChunk Format { get; }
        List<RiffChunk> Chunks { get; }
    }
}
