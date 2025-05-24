using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public partial class Track : ITrack
    {
        protected int _port = -1;
        protected int _channel = -1;
        protected string? _title;
        protected Timeline _timeline = [];
        protected KeySwitchOption[] _keySwitch = new KeySwitchOption[128];

        public int Port { get => _port; set => _port = Math.Clamp(value, -1, 15); }
        public int Channel { get => _channel; set => _channel = Math.Clamp(value, -1, 15); }
        public string? Title { get => _title; set => _title = value; }
        public Timeline Timeline => _timeline;
        public Span<KeySwitchOption> KeySwitchOptions => _keySwitch;

        public void Clear()
        {
            Timeline.Clear();
            ClearKeySwitch();
        }

        public void ClearKeySwitch()
        {
            Array.Clear(_keySwitch);
        }

        public void SetKeySwitch(ReadOnlySpan<KeySwitchOption> source)
        {
            source.CopyTo(_keySwitch);
        }

        public void Update(ITrack source, bool restoreGroup = true)
        {
            var timeline = _timeline;
            SortedDictionary<int, List<NoteGroup>> groups = [];
            if (restoreGroup)
            {
                foreach (var (_, obj) in timeline)
                {
                    if (obj is NoteGroup ng)
                    {
                        var count = -ng.Count;
                        if (!groups.TryGetValue(count, out var list))
                        {
                            list = [];
                            groups.Add(count, list);
                        }
                        list.Add(ng);
                    }
                }
            }
            _port = source.Port;
            _channel = source.Channel;
            _title = source.Title;
            timeline.Clear();
            source.Timeline.CopyTo(timeline);
            foreach (var (_, list) in groups)
            {
                foreach (var ng in CollectionsMarshal.AsSpan(list))
                {
                    AutoGroup(ng);
                }
            }
        }

        public Rational GetLastPosition() => _timeline.LastPosition;
    }
}
