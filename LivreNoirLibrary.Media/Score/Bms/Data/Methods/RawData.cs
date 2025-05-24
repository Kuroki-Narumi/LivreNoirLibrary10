using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Media.Bms.RawData;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BaseData
    {
        internal void SetRawData(int bar, Channel channel, string data)
        {
            RawData.Set(bar, channel, data, Base);
        }

        private delegate void AddProc(BarPosition p, int id);

        private void AddRational(DefType defType, int id, Action<Rational> addProc)
        {
            if (DefLists.GetInherited(defType, id) is string text && Rational.TryParse(text, out var value))
            {
                addProc(value);
            }
        }

        internal void ExtractRawData(BaseData? parent = null)
        {
            ProcessInherit(parent, ExtractRawDataCore);
        }

        protected virtual void ExtractRawDataCore()
        {
            var raw = RawData;
            var lastBar = RawData.LastBar;

            int barNumber = 0;
            var defaultBarLength = (Rational)Constants.DefaultBarLength;
            var barLength = defaultBarLength;

            HashSet<int> lastNote_ln = []; // LN-channel type
            var lnobj = LnObj;

            void ProcessAdd(ChannelData data, AddProc addProc)
            {
                var resolution = data.Length;
                var basePos = barLength / resolution;
                for (int k = 0; k < resolution; k++)
                {
                    if (data[k] is not 0)
                    {
                        addProc(new(barNumber, basePos * k), data[k]);
                    }
                }
            }

            for (; barNumber <= lastBar; barNumber++)
            {
                if (!raw.TryGetValue(barNumber, out var barData))
                {
                    continue;
                }
                // barLength
                if (barData.Length is not 1)
                {
                    try
                    {
                        barLength = Rational.ConvertBySBT(barData.Length * Constants.DefaultBarLength, Constants.BarLengthDenominatorLimit);
                        if (barLength.IsNegativeOrZero())
                        {
                            ExConsole.Write($"#CAUTION# bar length is too small ({barNumber.GetBarText()}: {barData.Length})");
                            barLength = Rational.Epsilon;
                        }
                    }
                    catch (OverflowException)
                    {
                        ExConsole.Write($"#CAUTION# bar length is too large ({barNumber.GetBarText()}: {barData.Length})");
                        barLength = Rational.MaxValue;
                    }
                    Bars.Set(barNumber, barLength);
                }
                else
                {
                    barLength = defaultBarLength;
                }
                // bgm
                var list = barData.Bgms;
                var count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    ProcessAdd(list[i], (p, id) => AddNote(p, NoteType.Normal, -i, id));
                }
                // channel data
                list = barData.Channels;
                count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var data = list[i];
                    var ch = data.Channel;
                    AddProc addProc;
                    switch (ch)
                    {
                        case Channel.Bpm_Base:
                            addProc = (p, id) => AddTempo(p, id);
                            break;
                        case Channel.Bpm:
                            addProc = (p, id) => AddRational(DefType.Bpm, id, v => AddTempo(p, v));
                            break;
                        case Channel.Stop:
                            addProc = (p, id) => AddRational(DefType.Stop, id, v => AddStop(p, BmsUtils.GetStopLength(v)));
                            break;
                        case Channel.Scroll:
                            Root.UseExtendedConductor = true;
                            addProc = (p, id) => AddRational(DefType.Scroll, id, v => AddScroll(p, v));
                            break;
                        case Channel.Speed:
                            Root.UseExtendedConductor = true;
                            addProc = (p, id) => AddRational(DefType.Speed, id, v => AddSpeed(p, v));
                            break;
                        default:
                            var type = ch.GetNoteType();
                            var lane = ch.GetLane();
                            if (ch.IsLong())
                            {
                                addProc = (p, v) =>
                                {
                                    if (lastNote_ln.Remove(lane))
                                    {
                                        AddNote(p, NoteType.LongEnd, lane, v);
                                    }
                                    else
                                    {
                                        AddNote(p, NoteType.Normal, lane, v);
                                        lastNote_ln.Add(lane);
                                    }
                                };
                            }
                            else if (ch.IsVisible())
                            {
                                addProc = (p, v) =>
                                {
                                    if (v == lnobj)
                                    {
                                        AddNote(p, NoteType.LongEnd, lane, 0);
                                    }
                                    else
                                    {
                                        AddNote(p, NoteType.Normal, lane, v);
                                    }
                                };
                            }
                            else
                            {
                                if (ch.IsExtendedBga())
                                {
                                    Root.UseExtendedBga = true;
                                }
                                addProc = (p, v) => AddNote(p, type, lane, v);
                            }
                            break;
                    }
                    ProcessAdd(data, addProc);
                }
            }
            RawData.Clear();
            DefLists.Remove(DefType.Bpm);
            DefLists.Remove(DefType.Stop);
            DefLists.Remove(DefType.Scroll);
            DefLists.Remove(DefType.Speed);
        }

        protected void CreateRawData()
        {
            var raw = RawData;
            raw.Clear();

            foreach (var (number, length) in Bars)
            {
                raw.GetBar(number).Length = (decimal)length / Constants.DefaultBarLength;
            }

            var bars = Bars;
            Dictionary<int, (Bar Bar, Rational Beat, Note Note)> lastNotes = [];
            var lnobj = LnObj;
            foreach (var (pos, note) in Timeline)
            {
                var bar = raw.GetBar(pos.Bar);
                var innerPos = pos.Beat / bars.Get(pos.Bar);
                var exist = lastNotes.TryGetValue(note.Lane, out var last);
                var lane = note.Lane;
                if (note.IsTempo())
                {
                    var value = note.Decimal;
                    if (value is > 0 and <= 255 && (value == Math.Truncate(value)))
                    {
                        CreateRawData_Set(bar, Channel.Bpm_Base, innerPos, (int)value);
                    }
                    else
                    {
                        var index = DefLists.FindIndex(DefType.Bpm, value.ToString(), true);
                        CreateRawData_Set(bar, Channel.Bpm, innerPos, index);
                    }
                }
                else if (note.IsStop())
                {
                    var index = DefLists.FindIndex(DefType.Stop, BmsUtils.ConvertBackStopLength(note.Value).ToString(), true);
                    CreateRawData_Set(bar, Channel.Stop, innerPos, index);
                }
                else if (note.IsScroll())
                {
                    var index = DefLists.FindIndex(DefType.Scroll, note.Decimal.ToString(), true);
                    CreateRawData_Set(bar, Channel.Scroll, innerPos, index);
                }
                else if (note.IsSpeed())
                {
                    var index = DefLists.FindIndex(DefType.Speed, note.Decimal.ToString(), true);
                    CreateRawData_Set(bar, Channel.Speed, innerPos, index);
                }
                else
                {
                    var id = note.Id;
                    if (note.IsBgm())
                    {
                        CreateRawData_SetBGM(bar, -lane, innerPos, id);
                    }
                    else if (note.IsVisibleKey())
                    {
                        var c = Channel.P1_Visible + note.Lane;
                        if (exist)
                        {
                            CreateRawData_SetList(last.Bar, c, last.Beat, last.Note.Id);
                        }
                        lastNotes[lane] = (bar, innerPos, note);
                    }
                    else if (note.IsLongEnd())
                    {
                        Channel c;
                        if (lnobj is > 0)
                        {
                            c = Channel.P1_Visible + note.Lane;
                            id = lnobj;
                        }
                        else
                        {
                            c = Channel.P1_Long + note.Lane;
                        }
                        if (exist)
                        {
                            CreateRawData_SetList(last.Bar, c, last.Beat, last.Note.Id);
                            if (lnobj is 0 && id is 0)
                            {
                                id = last.Note.Id;
                            }
                        }
                        CreateRawData_SetList(bar, c, innerPos, id);
                        lastNotes.Remove(note.Lane);
                    }
                    else
                    {
                        var c = note.Type.GetChannel(lane);
                        CreateRawData_SetList(bar, c, innerPos, id);
                    }
                }
            }
            foreach (var (lane, last) in lastNotes)
            {
                CreateRawData_SetList(last.Bar, Channel.P1_Visible + lane, last.Beat, last.Note.Id);
            }
        }

        private static void CreateRawData_Set(Bar bar, Channel channel, in Rational pos, int value)
        {
            bar.GetChannel(channel).SetAt(pos, (ushort)value);
        }

        private static void CreateRawData_SetBGM(Bar bar, int lane, in Rational pos, int value)
        {
            bar.GetBgm(lane).SetAt(pos, (ushort)value);
        }

        private static void CreateRawData_SetList(Bar bar, Channel channel, in Rational pos, int value)
        {
            var (num, den) = pos;
            if (num >= den)
            {
                return;
            }
            foreach (var line in CollectionsMarshal.AsSpan(bar.Channels))
            {
                if (line.Channel == channel && line.GetAt(pos) is 0)
                {
                    line.SetAt(pos, (ushort)value);
                    return;
                }
            }
            bar.Channels.Add(ChannelData.Create(channel, pos, value));
        }
    }
}
