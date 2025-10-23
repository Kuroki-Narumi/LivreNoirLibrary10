using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsFormatter
    {
        private class SaveState
        {
            public readonly SortedDictionary<int, Bar> _bars = [];
            public long MaxDenominator { get; private set; }

            public SaveState(BaseData data, int lnObj, Dictionary<DefType, OrderedDictionary<decimal, int>> conductorDefs, ref int radix)
            {
                foreach (var (_, list) in data.DefLists)
                {
                    UpdateRadix(list.MaxIndex, ref radix);
                }
                var bars = _bars;
                Dictionary<short, (BarPosition, Channel, ISoundNote)> lastNote = [];
                foreach (var (number, length) in data.Bars)
                {
                    bars.GetOrAdd(number).Length = length;
                }
                foreach (var (pos, note) in data.Timeline)
                {
                    var (number, beat) = pos;
                    var bar = bars.GetOrAdd(number);
                    var bgmOffset = 0;
                    switch (note)
                    {
                        case IConductorNote c:
                            var ch = c.Channel;
                            var defType = BmsUtils.GetDefType(ch);
                            var value = c.Value;
                            if (ch is not Channel.Bpm || BmsUtils.NeedsBpmDef(value))
                            {
                                var dic = conductorDefs.GetOrAdd(defType);
                                if (!dic.TryGetValue(value, out var index))
                                {
                                    index = dic.Count + 1;
                                    dic.Add(value, index);
                                    UpdateRadix(index, ref radix);
                                }
                                bar.Add(ch, beat, index);
                            }
                            else
                            {
                                bar.Add(Channel.Bpm_Base, beat, (int)value);
                            }
                            break;
                        case IMetaNote m:
                            var vv = m.Value;
                            if (m.IsDef())
                            {
                                UpdateRadix(vv, ref radix);
                            }
                            bar.Add(m.Channel, beat, vv);
                            break;
                        case ISoundNote s:
                            var lane = (short)s.Lane;
                            var type = s.Type;
                            vv = s.Value;
                            ch = BmsUtils.GetChannel(type, lane);
                            UpdateRadix(vv, ref radix);
                            if (type is NoteType.LongEnd)
                            {
                                if (lnObj is 0)
                                {
                                    ch = ch.ToLong();
                                    if (lastNote.TryGetValue(lane, out var prev))
                                    {
                                        var (pp, _, pn) = prev;
                                        bars.GetOrAdd(pp.Bar).Add(ch, pp.Offset, pn.Value);
                                        lastNote.Remove(lane);
                                    }
                                    bar.Add(ch, beat, s.Value);
                                }
                                else
                                {
                                    bar.Add(ch, beat, lnObj);
                                }
                            }
                            else
                            {
                                if (lastNote.TryGetValue(lane, out var prev))
                                {
                                    var (pp, ch2, pn) = prev;
                                    bars.GetOrAdd(pp.Bar).Add(ch2, pp.Offset, pn.Value);
                                    lastNote.Remove(lane);
                                }
                                if (lane is <= 0)
                                {
                                    bar.AddBgm(-lane, beat, s.Value, ref bgmOffset);
                                }
                                else if (type is NoteType.Normal && lnObj is 0)
                                {
                                    lastNote[lane] = (pos, ch, s);
                                }
                                else
                                {
                                    bar.Add(ch, beat, s.Value);
                                }
                            }
                            break;
                    }
                }
                foreach (var (_, (pp, ch, pn)) in lastNote)
                {
                    bars.GetOrAdd(pp.Bar).Add(ch, pp.Offset, pn.Value);
                }
                var maxDen = 0L;
                foreach (var (_, bar) in bars)
                {
                    maxDen = Math.Max(maxDen, bar.GetMaxDenominator());
                }
                MaxDenominator = maxDen;
            }

            static void UpdateRadix(int value, ref int radix)
            {
                if (radix is < Constants.Base_Extended)
                {
                    if (value is >= Constants.DefMax_Default)
                    {
                        radix = Constants.Base_Extended;
                    }
                    else if (value is >= Constants.DefMax_Legacy)
                    {
                        radix = Constants.Base_Default;
                    }
                }
            }

            public void ReductDenominator(long limit)
            {
                if (MaxDenominator < limit)
                {
                    foreach (var (_, bar) in _bars)
                    {
                        bar.ReductDenominator(limit);
                    }
                    MaxDenominator = limit;
                }
            }
        }

        private class Bar()
        {
            internal readonly List<Line> _bgm = [];
            internal readonly SortedDictionary<Channel, List<Line>> _channels = [];

            public Rational Length { get; internal set; } = Rational.One;

            public void AddBgm(int lane, Rational position, int value, ref int offset)
            {
                var l = lane + offset;
                var bgm = _bgm;
                while (bgm.Count <= l)
                {
                    bgm.Add(new());
                }
                while (!bgm[l].TryAdd(position, value))
                {
                    offset++;
                    l++;
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
                foreach (var line in CollectionsMarshal.AsSpan(_bgm))
                {
                    value = Math.Max(value, line._den);
                }
                foreach (var (_, lines) in _channels)
                {
                    foreach (var line in CollectionsMarshal.AsSpan(lines))
                    {
                        value = Math.Max(value, line._den);
                    }
                }
                return value;
            }

            public void ReductDenominator(long limit)
            {
                foreach (var line in CollectionsMarshal.AsSpan(_bgm))
                {
                    line.ReductDenominator(limit);
                }
                foreach (var (_, lines) in _channels)
                {
                    foreach (var line in CollectionsMarshal.AsSpan(lines))
                    {
                        line.ReductDenominator(limit);
                    }
                }
            }
        }

        private class Line
        {
            private readonly SortedList<Rational, ushort> _list = [];
            internal long _den = 1;

            public Line() { }
            public Line(Rational pos, int value)
            {
                _list[pos] = (ushort)value;
                _den = pos.Denominator;
            }

            public bool TryAdd(Rational position, int value)
            {
                if (_list.ContainsKey(position))
                {
                    return false;
                }
                _den = NumberExtensions.LCM(_den, position.Denominator);
                _list.Add(position, (ushort)value);
                return true;
            }

            public void ReductDenominator(long limit)
            {
                var list = _list;
                var newList = ObjectPool.Rent<List<(Rational, ushort)>>();
                try
                {
                    foreach (var ((n, d), value) in list)
                    {
                        var newPos = new Rational(n * limit / d, limit);
                        newList.Add((newPos, value));
                    }
                    list.Clear();
                    foreach (var (pos, value) in CollectionsMarshal.AsSpan(newList))
                    {
                        list[pos] = value;
                    }
                    _den = limit;
                }
                finally
                {
                    ObjectPool.Return(newList);
                }
            }

            public void WriteText(BmsTextWriter writer, int radix)
            {
                var den = _den;
                var index = 0;
                foreach (var ((n, d), value) in _list)
                {
                    var num = n * den / d;
                    while (index < num)
                    {
                        writer.Write("00");
                        index++;
                    }
                    writer.Write(BmsUtils.ToBased(value, radix));
                    index++;
                }
                while (index < den)
                {
                    writer.Write("00");
                    index++;
                }
                writer.WriteLine();
            }
        }
    }
}