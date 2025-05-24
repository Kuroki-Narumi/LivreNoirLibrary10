using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms.RawData
{
    public class Sequence : SortedList<int, Bar>
    {
        public int LastBar => _lastBar;
        private int _lastBar = 0;

        public new void Clear()
        {
            base.Clear();
            _lastBar = 0;
        }

        public decimal GetLength(int barNumber)
        {
            return TryGetValue(barNumber, out var bar) ? bar.Length : 1;
        }

        public Bar GetBar(int barNumber)
        {
            if (!TryGetValue(barNumber, out var bar))
            {
                bar = new();
                if (barNumber > _lastBar) { _lastBar = barNumber; }
                Add(barNumber, bar);
            }
            return bar;
        }

        public void SetLength(int barNumber, decimal length)
        {
            GetBar(barNumber).Length = length;
        }

        public void Set(int barNumber, Channel channel, string list, int radix, int replace = -1)
        {
            GetBar(barNumber).Set(channel, list, radix, replace);
        }

        public void Set(int barNumber, ChannelData data, int replace = -1)
        {
            GetBar(barNumber).Set(data, replace);
        }

        public int GetBgmSize()
        {
            int result = 0;
            foreach (var (_, bar) in this)
            {
                if (bar.Bgms.Count > result)
                {
                    result = bar.Bgms.Count;
                }
            }
            return result;
        }

        public int CountNotes(ushort lnobj = 0)
        {
            int count = 0;
            var long_flags = new bool[72];
            var long_offset = Channel.P1_Long;
            foreach (var (_, bar) in this)
            {
                foreach (var c in bar.Channels)
                {
                    if (c is null) { continue; }
                    var channel = c.Channel;
                    var ary = c.Data;
                    if (BmsUtils.IsVisible(channel))
                    {
                        foreach (var v in ary)
                        {
                            if (v != 0 && v != lnobj)
                            {
                                count++;
                            }
                        }
                    }
                    else if (BmsUtils.IsLong(channel))
                    {
                        var index = channel - long_offset;
                        foreach (var v in ary)
                        {
                            if (v != 0)
                            {
                                if (long_flags[index])
                                {
                                    long_flags[index] = false;
                                }
                                else
                                {
                                    count++;
                                    long_flags[index] = true;
                                }
                            }
                        }
                    }
                }
            }
            return count;
        }

        internal void Dump(BmsTextWriter writer, int radix, bool dumpEmpty)
        {
            foreach (var (barNumber, bar) in this)
            {
                if (barNumber is > Constants.MaxBarNumber)
                {
                    break;
                }
                if (!bar.IsEmpty())
                {
                    bar.Dump(writer, barNumber, radix);
                    if (dumpEmpty)
                    {
                        writer.DumpEmpty();
                    }
                }
            }
        }

        public void Merge(Sequence other)
        {
            var bgmOffset = GetBgmSize();
            foreach (var (barNumber, srcBar) in other)
            {
                GetBar(barNumber).Merge(srcBar, bgmOffset);
            }
        }

        public void MergeBar(int barNumber, Bar bar, int bgmOffset = 0)
        {
            if (TryGetValue(barNumber, out var current))
            {
                current.Merge(bar, bgmOffset);
            }
            else
            {
                Add(barNumber, bar);
            }
        }
    }
}
