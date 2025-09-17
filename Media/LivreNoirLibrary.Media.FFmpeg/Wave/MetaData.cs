using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Wave
{
    public abstract unsafe partial class WaveContext : IWaveMetaData
    {
        public FormatChunk Format => _format;
        public List<RiffChunk> Chunks => _chunks;
    }
}
