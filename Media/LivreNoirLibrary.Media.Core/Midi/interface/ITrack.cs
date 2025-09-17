using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Midi
{
    public interface ITrack
    {
        public int Port { get; set; }
        public int Channel { get; set; }
        public string? Title { get; set; }
        public ITimeline Timeline { get; }
        public Span<KeySwitchOption> KeySwitchOptions { get; }
        public SortedSet<int> SideChainSources { get; }
    }
}
