using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsFormatter
    {
        private class SaveState
        {
            public readonly SortedDictionary<int, Bar> _bars = [];
            public long MaxDenominator { get; private set; }

            public SaveState(IBmsDataUnit data, int lnObj, Dictionary<DefType, OrderedDictionary<double, int>> conductorDefs, ref int radix)
            {
                foreach (var (_, list) in data.DefLists.EnumerateList())
                {
                    UpdateRadix(list.MaxIndex, ref radix);
                }
                var bars = _bars;
                Dictionary<Channel, (BarPosition, Note)> lastNote = [];
                foreach (var (number, length) in data.BarDefs)
                {
                    bars.GetOrAdd(number).Length = length;
                }
                foreach (var (pos, note) in data.Timeline)
                {
                    var (number, beat) = pos;
                    var bar = bars.GetOrAdd(number);
                    var bgmOffset = 0;
                    var ch = note.Channel;
                    var value = note.Value;
                    if (ch.IsConductor())
                    {
                        BmsUtils.TryGetDefType(ch, out var defType);
                        if (ch is not Channel.Bpm || value.NeedsBpmDef())
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
                    }
                    else
                    {
                        var intVal = (int)value;
                        if (ch.IsDefValue())
                        {
                            UpdateRadix(intVal, ref radix);
                        }
                        if (ch.IsBgm())
                        {
                            bar.AddBgm(ch, beat, intVal, ref bgmOffset);
                            continue;
                        }
                        else if (ch.IsKey())
                        {
                            void ApplyLastNote()
                            {
                                if (lastNote.TryGetValue(ch, out var prev))
                                {
                                    var (pp, pn) = prev;
                                    bars.GetOrAdd(pp.Bar).Add(ch, pp.Offset, (int)pn.Value);
                                    lastNote.Remove(ch);
                                }
                            }

                            var noteType = note.Type;
                            switch (noteType)
                            {
                                case NoteType.Normal:
                                    ApplyLastNote();
                                    if (lnObj is 0)
                                    {
                                        lastNote[ch] = (pos, note);
                                        continue;
                                    }
                                    break;
                                case NoteType.LongEnd:
                                    if (lnObj is 0)
                                    {
                                        ch = ch.ToLong();
                                        ApplyLastNote();
                                    }
                                    else
                                    {
                                        intVal = lnObj;
                                    }
                                    break;
                                default:
                                    ch = BmsUtils.Merge(ch, noteType);
                                    break;
                            }
                        }
                        bar.Add(ch, beat, intVal);
                    }
                }
                foreach (var (_, (pp, pn)) in lastNote)
                {
                    bars.GetOrAdd(pp.Bar).Add(pn.Channel, pp.Offset, (int)pn.Value);
                }
                var maxDen = 0L;
                foreach (var (_, bar) in bars)
                {
                    maxDen = Math.Max(maxDen, bar.GetMaxDenominator());
                }
                MaxDenominator = maxDen;
            }

            private static void UpdateRadix(int value, ref int radix)
            {
                if (radix is < BmsConstants.Base_Extended)
                {
                    if (value is >= BmsConstants.DefMax_Default)
                    {
                        radix = BmsConstants.Base_Extended;
                    }
                    else if (value is >= BmsConstants.DefMax_Legacy)
                    {
                        radix = BmsConstants.Base_Default;
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
    }
}