using System;

namespace LivreNoirLibrary.Media.Midi
{
    public partial class Track : ITrack
    {
        protected int _port = -1;
        protected int _channel = -1;
        protected readonly KeySwitchOption[] _keySwitch = new KeySwitchOption[128];

        public int Port { get => _port; set => _port = Math.Clamp(value, -1, 15); }
        public int Channel { get => _channel; set => _channel = Math.Clamp(value, -1, 15); }
        public string? Title { get; set; }
        public Timeline Timeline { get; } = [];
        public Span<KeySwitchOption> KeySwitchOptions => _keySwitch;
    }
}
