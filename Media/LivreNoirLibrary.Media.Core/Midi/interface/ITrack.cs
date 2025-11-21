using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Midi
{
    public interface ITrack
    {
        int Port { get; set; }
        int Channel { get; set; }
        string? Title { get; set; }
        ITimeline Timeline { get; }
        Span<KeySwitchOption> KeySwitchOptions { get; }
        SortedSet<int> SideChainSources { get; }
    }
}
