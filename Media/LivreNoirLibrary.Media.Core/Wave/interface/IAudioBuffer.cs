using System;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IAudioBuffer
    {
        int TotalSample { get; }
        int SampleRate { get; }
        int Channels { get; }
    }
}
