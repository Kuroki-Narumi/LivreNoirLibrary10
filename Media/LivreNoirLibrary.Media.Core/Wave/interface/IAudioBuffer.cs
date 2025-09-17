using System;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IAudioBuffer
    {
        public int TotalSample { get; }
        public int SampleRate { get; }
        public int Channels { get; }
    }
}
