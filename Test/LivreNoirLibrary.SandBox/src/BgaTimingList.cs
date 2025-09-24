using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.SandBox
{
    public class BgaTimingList
    {
        private readonly Dictionary<Channel, DecimalTimeline<string>> _list;

        public BgaTimingList(IBmsData data, TimeCounter counter, string directory)
        {
            var list = _list = new()
            {
                [Channel.Bga_Base] = [],
                [Channel.Bga_Layer1] = [],
                [Channel.Bga_Layer2] = [],
                [Channel.Bga_Poor] = [],
            };
            foreach (var (pos, notes) in data.Timeline.EachList())
            {
                var second = counter.Beat2Second(data.GetAbsolutePosition(pos));
                foreach (var note in CollectionsMarshal.AsSpan(notes))
                {
                    if (note is IMetaNote n && 
                        list.TryGetValue(n.Channel, out var timeline) &&
                        data.TryGetMediaPath(n.Value, directory, out _, out var path))
                    {
                        timeline.Set(second, path);
                    }
                }
            }
        }

        public bool TryGetValue(Channel channel, decimal time, out decimal actualTime, [MaybeNullWhen(false)]out string path)
        {
            actualTime = default;
            path = default;
            return _list.TryGetValue(channel, out var timeline) && 
                timeline.TryGet(time, Collections.SearchMode.PreviousOrEqual, out actualTime, out path);
        }
    }
}
