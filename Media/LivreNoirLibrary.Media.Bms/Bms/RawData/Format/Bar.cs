using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsFormatter
    {
        private class Bar()
        {
            internal readonly List<Line> _bgm = [];
            internal readonly SortedDictionary<Channel, List<Line>> _channels = [];

            public double Length { get; internal set; } = 1;

            public void AddBgm(Channel channel, double position, int value, ref int offset, long resolutionLimit)
            {
                var pos = BmsUtils.Rationalize(position);
                var lane = channel - Channel.Bgm_Start + offset;
                var bgm = _bgm;
                while (bgm.Count <= lane)
                {
                    bgm.Add(new());
                }
                while (!bgm[lane].TryAdd(pos, value, resolutionLimit))
                {
                    offset++;
                    lane++;
                    bgm.Add(new());
                }
            }

            public void Add(Channel channel, double position, int value, long resolutionLimit)
            {
                var pos = BmsUtils.Rationalize(position);
                var list = _channels.GetOrAdd(channel);
                foreach (var item in _channels.GetOrAdd(channel).AsSpan())
                {
                    if (item.TryAdd(pos, value, resolutionLimit))
                    {
                        return;
                    }
                }
                list.Add(new(pos, value));
            }

            public long GetMaxResolution()
            {
                var value = 0L;
                foreach (var line in _bgm.AsSpan())
                {
                    value = Math.Max(value, line._resol);
                }
                foreach (var (_, lines) in _channels)
                {
                    foreach (var line in lines.AsSpan())
                    {
                        value = Math.Max(value, line._resol);
                    }
                }
                return value;
            }
        }
    }
}