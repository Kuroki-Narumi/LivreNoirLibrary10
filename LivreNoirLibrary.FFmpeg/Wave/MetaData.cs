using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Wave
{
    public abstract unsafe partial class WaveContext : IWaveMetaData, IAudioMetaData
    {
        public FormatChunk Format => _format;
        public List<RiffChunk> Chunks => _chunks;

        public double Tempo { get => this.GetTempo(); set => this.SetTempo(value); }
        public bool IsTempoSet() => IWaveMetaDataExtentions.IsTempoSet(this);
        public string? Genre { get => this.GetInfoText(ChunkIds.IGenre); set => this.SetInfoText(ChunkIds.IGenre, value); }
        public string? Title { get => this.GetInfoText(ChunkIds.IName); set => this.SetInfoText(ChunkIds.IName, value); }
        public string? Artist { get => this.GetInfoText(ChunkIds.IArtist); set => this.SetInfoText(ChunkIds.IArtist, value); }
        public string? Copyright { get => this.GetInfoText(ChunkIds.ICopyright); set => this.SetInfoText(ChunkIds.ICopyright, value); }
        public string? Comment { get => this.GetInfoText(ChunkIds.IComment); set => this.SetInfoText(ChunkIds.IComment, value); }
        public string? Software { get => this.GetInfoText(ChunkIds.ISoft); set => this.SetInfoText(ChunkIds.ISoft, value); }
    }
}
