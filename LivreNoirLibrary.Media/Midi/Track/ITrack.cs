using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public interface ITrack
    {
        public int Port { get; set; }
        public int Channel { get; set; }
        public string? Title { get; set; }
        public Timeline Timeline { get; }
        public Span<KeySwitchOption> KeySwitchOptions { get; }
    }
}
