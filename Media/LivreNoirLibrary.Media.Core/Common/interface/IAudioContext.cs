using System;

namespace LivreNoirLibrary.Media
{
    public interface IAudioContext : IDisposable
    {
        int InputSampleRate { get; }
        int InputChannels { get; }
        int OutputSampleRate { get; }
        int OutputChannels { get; }
    }
}
