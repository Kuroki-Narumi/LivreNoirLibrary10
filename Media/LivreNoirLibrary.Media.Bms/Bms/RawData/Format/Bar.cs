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

            public void AddBgm(Channel channel, Rational position, int value, ref int offset)
            {
                var lane = channel - Channel.Bgm_Start + offset;
                var bgm = _bgm;
                while (bgm.Count <= lane)
                {
                    bgm.Add(new());
                }
                while (!bgm[lane].TryAdd(position, value))
                {
                    offset++;
                    lane++;
                    bgm.Add(new());
                }
            }

            public void Add(Channel channel, Rational position, int value)
            {
                var list = _channels.GetOrAdd(channel);
                if (!list.Any(l => l.TryAdd(position, value)))
                {
                    list.Add(new(position, value));
                }
            }

            public long GetMaxDenominator()
            {
                var value = 0L;
                foreach (var line in _bgm.AsSpan())
                {
                    value = Math.Max(value, line._den);
                }
                foreach (var (_, lines) in _channels)
                {
                    foreach (var line in lines.AsSpan())
                    {
                        value = Math.Max(value, line._den);
                    }
                }
                return value;
            }

            public void ReductDenominator(long limit)
            {
                foreach (var line in _bgm.AsSpan())
                {
                    line.ReductDenominator(limit);
                }
                foreach (var (_, lines) in _channels)
                {
                    foreach (var line in lines.AsSpan())
                    {
                        line.ReductDenominator(limit);
                    }
                }
            }
        }
    }
}