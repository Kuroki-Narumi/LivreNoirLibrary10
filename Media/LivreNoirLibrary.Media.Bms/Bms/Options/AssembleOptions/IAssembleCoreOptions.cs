using System;
using LivreNoirLibrary.Media.Wave;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IAssembleCoreOptions
    {
        public string RootDirectory { get; }
        public double Offset { get; set; }
        public double Length { get; set; }
        public double Gain { get; }
        public float KeyVolume { get; }
        public float BgmVolume { get; }
        public NormalizeMode NormalizeMode { get; }
        public bool Overlap { get; }
        public bool SetMarker { get; }
        public SampleFormat SampleFormat { get; }
    }
}
