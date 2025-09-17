using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Midi
{
    public partial class Track : ITrack
    {
        protected readonly KeySwitchOption[] _keySwitch = new KeySwitchOption[128];

        public int Port { get; set =>  field = Math.Clamp(value, -1, 15); } = -1;
        public int Channel { get; set => field = Math.Clamp(value, -1, 15); } = -1;
        public string? Title { get; set; }
        public Timeline Timeline { get; } = [];
        public Span<KeySwitchOption> KeySwitchOptions => _keySwitch;
        public SortedSet<int> SideChainSources { get; } = [];

        ITimeline ITrack.Timeline => Timeline;
    }
}
