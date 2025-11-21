using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        [GeneratedRegex(@"(?<title>.+)(?<chart>\[\w+\]|\(\w+\)|{\w+}|<\w+>)$")]
        private static partial Regex Regex_ChartName { get; }

        public static Bmson.BmsonData ConvertToBmson(
            this IBmsViewModel source,
            long resolution = 480, 
            string? modeHint = null, 
            IChannelToBmsonLaneConverter? converter = null, 
            Predicate<Note>? selector = null)
        {
            var root = source.Root;
            var current = source.CurrentData;
            Bmson.BmsonData data = new();
            var chartType = root.ChartType;
            var keyCount = root.GetKeyCount();
            converter ??= ChannelToBmsonLaneConverter.GetAuto(chartType, keyCount);
            selector ??= n => true;
            // header info
            var info = data.Info;
            var title = source.Title ?? "";
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
            info.SubTitle = source.SubTitle;
            info.Artist = source.Artist;
            var str = source.SubArtist;
            if (!string.IsNullOrEmpty(str))
            {
                info.SubArtists = [str];
            }
            info.Genre = source.Genre;
            info.ModeHint = modeHint ?? $"{chartType.ToString().ToLower()}-{keyCount}k";
            info.Level = source.PlayLevel;
            info.InitialBpm = source.Bpm;
            info.JudgeRank = source.ExRank;

            var total = source.Total;
            info.Total = total is <= 0 ? 100 : total / BmsUtils.CalcTotal(GetNoteCount(current));
            info.LnType = source.LnMode;
            info.BannerImage = source.Banner;
            info.EyecatchImage = source.StageFile;
            info.PreviewMusic = source.Preview;

            resolution = Math.Clamp(resolution, 48, 12_096_000);
            info.Resolution = resolution;
            //
            long Convert(double position) => (position * resolution * 4).RoundToLong();
            // bar
            List<Bmson.BarLine> barList = [];
            data.BarList = barList;
            // bga
            Bmson.BgaInfo bga = new();
            if (current.DefLists.TryGetList(DefType.Bmp, out var defList))
            {
                List<Bmson.BgaHeader> list = [];
                if (defList is DefList d)
                {
                    foreach (var (id, name) in d)
                    {
                        list.Add(new() { Id = id, FileName = name });
                    }
                }
                else
                {
                    foreach (var (id, name) in defList)
                    {
                        list.Add(new() { Id = id, FileName = name });
                    }
                }
            }
            // timeline
            SortedList<int, Bmson.SoundChannel> soundList = [];
            Dictionary<Channel, Bmson.Note> lastNotes = [];
            List<Bmson.Bpm> bpmList = [];
            List<Bmson.Stop> stopList = [];
            List<Bmson.RateEvent> scrollList = [];
            List<Bmson.RateEvent> speedList = [];
            List<Bmson.Note>? GetNoteList(int id)
            {
                if (source.GetDefValue(DefType.Wav, id) is { } name)
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
            foreach (var (pos, list) in current.Timeline.EnumerateList())
            {
                lastBar = pos.Bar;
                var time = Convert(source.GetAbsolutePosition(pos));
                var tempo = 0d;
                var scroll = 0d;
                var speed = 0d;
                var totalStop = 0d;
                foreach (var note in list.AsSpan())
                {
                    var ch = note.Channel;
                    switch (ch)
                    {
                        case Channel.Bpm:
                            tempo = note.Value;
                            break;
                        case Channel.Stop:
                            totalStop += note.Value;
                            break;
                        case Channel.Scroll:
                            scroll = note.Value;
                            break;
                        case Channel.Speed:
                            speed = note.Value;
                            break;
                        case Channel.Bga_Base:
                            bga.BaseList.Add(new() { Y = time, Id = (int)note.Value });
                            break;
                        case Channel.Bga_Layer1:
                            bga.LayerList.Add(new() { Y = time, Id = (int)note.Value });
                            break;
                        case Channel.Bga_Poor:
                            bga.PoorList.Add(new() { Y = time, Id = (int)note.Value });
                            break;
                        default:
                            if (selector(note))
                            {
                                if (note.Type is NoteType.LongEnd && lastNotes.Remove(ch, out var bn))
                                {
                                    bn.Length = time - bn.Y;
                                }
                                else if (note.IsPlayableSound() && converter.TryConvert(note.Channel, out var lane))
                                {
                                    bn = new()
                                    {
                                        X = lane,
                                        Y = time,
                                        Continue = false,
                                    };
                                    lastNotes[ch] = bn;
                                    GetNoteList((int)note.Value)?.Add(bn);
                                }
                            }
                            break;
                    }
                }
                if (tempo is not 0)
                {
                    bpmList.Add(new() { Y = time, Tempo = tempo });
                }
                if (totalStop is not 0)
                {
                    stopList.Add(new() { Y = time, Duration = Convert(totalStop * BmsConstants.StopUnit) });
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
