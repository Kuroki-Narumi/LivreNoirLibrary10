using System;

namespace LivreNoirLibrary.Media
{
    public interface IAudioContext : IDisposable
    {
        public int InputSampleRate { get; }
        public int InputChannels { get; }
        public int OutputSampleRate { get; }
        public int OutputChannels { get; }
    }
}
