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
                var innerPos = pos.Offset / bars.Get(pos.Bar);
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
