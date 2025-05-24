using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BaseData
    {
        [GeneratedRegex(@"(?<title>.+)(?<chart>\[\w+\]|\(\w+\)|{\w+}|<\w+>)$")]
        private static partial Regex Regex_ChartName { get; }

        public Bmson.BmsonData ConvertToBmson(long resolution = 0, ILaneConverter? converter = null, Predicate<Note>? noteSelector = null)
        {
            Bmson.BmsonData data = new();
            var keyType = Root.GetKeyType();
            converter ??= ILaneConverter.GetAuto(keyType);
            noteSelector ??= n => true;
            // header info
            var info = data.Info;
            var title = Title ?? "";
            var match = Regex_ChartName.Match(title);
            if (match.Success)
            {
                info.Title = match.Groups["title"].Value.Trim();
                info.ChartName = match.Groups["chart"].Value[1..^1];
            }
            else
            {
                info.Title = title;
            }
            info.SubTitle = SubTitle;
            info.Artist = Artist;
            var str = SubArtist;
            if (!string.IsNullOrEmpty(str))
            {
                info.SubArtists = [str];
            }
            info.Genre = Genre;
            info.ModeHint = keyType;
            info.Level = PlayLevel;
            info.InitialBpm = Bpm;
            info.JudgeRank = ExRank;
            info.Total = CalcTotal() * 100 / BmsUtils.CalcTotal(GetNotesCount());
            info.LnType = LnMode;
            info.BannerImage = Headers.Get(HeaderType.Banner);
            info.EyecatchImage = Headers.Get(HeaderType.StageFile);
            info.PreviewMusic = Headers.Get(HeaderType.Preview);
            if (resolution is <= 0)
            {
                resolution = CalcResolution();
                if (resolution % 4 is 0)
                {
                    resolution /= 4;
                }
            }
            info.Resolution = resolution;
            //
            long Convert(Rational value) => ((double)value * resolution * 4).RoundToLong();

            // bar
            List<Bmson.BarLine> barList = [];
            data.BarList = barList;
            // bga
            var bga_base = BmsUtils.GetLane(Channel.Bga_Base);
            var bga_layer = BmsUtils.GetLane(Channel.Bga_Layer1);
            var bga_poor = BmsUtils.GetLane(Channel.Bga_Poor);
            Bmson.BgaInfo bga = new();
            if (DefLists.TryGetValue(DefType.Bmp, out var defList))
            {
                List<Bmson.BgaHeader> list = [];
                foreach (var (id, name) in defList)
                {
                    list.Add(new() { Id = id, FileName = name });
                }
            }
            // timeline
            SortedList<int, Bmson.SoundChannel> soundList = [];
            Dictionary<int, Bmson.Note> lastNotes = [];
            List<Bmson.Bpm> bpmList = [];
            List<Bmson.Stop> stopList = [];
            List<Bmson.RateEvent> scrollList = [];
            List<Bmson.RateEvent> speedList = [];
            List<Bmson.Note>? GetNoteList(int id)
            {
                var name = DefLists.Get(DefType.Wav, id);
                if (!string.IsNullOrEmpty(name))
                {
                    if (!soundList.TryGetValue(id, out var list))
                    {
                        list = new() { FileName = name };
                        soundList.Add(id, list);
                    }
                    return list.NoteList;
                }
                return null;
            }
            var lastBar = 0;
            foreach (var (pos, list) in Timeline.EachList())
            {
                lastBar = pos.Bar;
                var time = Convert(GetBeat(pos));
                decimal tempo = 0;
                decimal scroll = 0;
                decimal speed = 0;
                var totalStop = Rational.Zero;
                foreach (var note in CollectionsMarshal.AsSpan(list))
                {
                    if (note.IsTempo())
                    {
                        tempo = note.Decimal;
                    }
                    else if (note.IsStop())
                    {
                        totalStop += note.Value;
                    }
                    else if (note.IsScroll())
                    {
                        scroll = note.Decimal;
                    }
                    else if (note.IsSpeed())
                    {
                        speed = note.Decimal;
                    }
                    else if (noteSelector(note))
                    {
                        var lane = note.Lane;
                        if (note.IsPlayableSound())
                        {
                            Bmson.Note bnote = new()
                            {
                                X = converter.Convert(note.Lane),
                                Y = time,
                                Continue = false,
                            };
                            lastNotes[lane] = bnote;
                            GetNoteList(note.Id)?.Add(bnote);
                        }
                        else if (note.IsLongEnd())
                        {
                            if (lastNotes.TryGetValue(lane, out var bnote))
                            {
                                bnote.Length = time - bnote.Y;
                                lastNotes.Remove(lane);
                            }
                        }
                        else
                        {
                            if (lane == bga_base)
                            {
                                bga.BaseList.Add(new() { Y = time, Id = note.Id });
                            }
                            else if (lane == bga_layer)
                            {
                                bga.LayerList.Add(new() { Y = time, Id = note.Id });
                            }
                            else if (lane == bga_poor)
                            {
                                bga.PoorList.Add(new() { Y = time, Id = note.Id });
                            }
                        }
                    }
                }
                if (tempo is not 0)
                {
                    bpmList.Add(new() { Y = time, Tempo = tempo });
                }
                if (!totalStop.IsZero())
                {
                    stopList.Add(new() { Y = time, Duration = Convert(totalStop) });
                }
                if (scroll is not 0)
                {
                    scrollList.Add(new() { Y = time, Rate = scroll });
                }
                if (speed is not 0)
                {
                    speedList.Add(new() { Y = time, Rate = speed });
                }
            }
            if (bpmList.Count is > 0)
            {
                data.BpmList = bpmList;
            }
            if (stopList.Count is > 0)
            {
                data.StopList = stopList;
            }
            if (scrollList.Count is > 0)
            {
                data.ScrollList = scrollList;
            }
            if (speedList.Count is > 0)
            {
                data.SpeedList = speedList;
            }
            if (soundList.Count is > 0)
            {
                data.SoundList = [.. soundList.Values];
            }
            if (bga.Headers.Count is > 0 || bga.BaseList.Count is > 0 || bga.LayerList.Count is > 0 || bga.PoorList.Count is > 0)
            {
                data.BgaInfo = bga;
            }
            return data;
        }
    }
}
