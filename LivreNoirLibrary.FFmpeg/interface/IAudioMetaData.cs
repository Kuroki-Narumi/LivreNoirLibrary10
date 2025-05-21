using System;

namespace LivreNoirLibrary.Media
{
    public interface IAudioMetaData
    {
        public const double DefaultTempo = 130;

        public double Tempo { get; set; }
        public bool IsTempoSet();
        public string? Genre { get; set; }
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public string? Copyright { get; set; }
    }
}
