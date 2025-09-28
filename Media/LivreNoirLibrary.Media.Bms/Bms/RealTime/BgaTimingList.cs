using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public class BgaTimingList
    {
        private readonly Dictionary<Channel, LongTimeline<string>> _list = [];

        public void Load(IBmsData data, TimeCounter counter, string directory)
        {
            var list = _list;
            list.Clear();
            foreach (var (pos, notes) in data.Timeline.EachList())
            {
                var second = counter.Beat2Ticks(data.GetAbsolutePosition(pos));
                foreach (var note in CollectionsMarshal.AsSpan(notes))
                {
                    if (note is IMetaNote n && n.Channel.IsBga())
                    {
                        var timeline = list.GetOrAdd(n.Channel);
                        if (data.TryGetMediaPath(n.Value, directory, out var name, out var path))
                        {
                            timeline.Set(second, path);
                        }
                        else
                        {
                            timeline.Set(second, "");
                        }
                    }
                }
            }
        }

        public void Load(IBmsData data, string directory)
        {
            TimeCounter counter = new(data);
            Load(data, counter, directory);
        }

        public static BgaTimingList Create(IBmsData data, TimeCounter counter, string directory)
        {
            BgaTimingList list = new();
            list.Load(data, counter, directory);
            return list;
        }

        public static BgaTimingList Create(IBmsData data, string directory)
        {
            BgaTimingList list = new();
            list.Load(data, directory);
            return list;
        }

        public bool TryGetValue(Channel channel, long time, out long startTime, [MaybeNullWhen(false)]out string path)
        {
            startTime = default;
            path = default;
            return _list.TryGetValue(channel, out var timeline) && 
                timeline.TryGet(time, SearchMode.PreviousOrEqual, out startTime, out path);
        }
    }
}
