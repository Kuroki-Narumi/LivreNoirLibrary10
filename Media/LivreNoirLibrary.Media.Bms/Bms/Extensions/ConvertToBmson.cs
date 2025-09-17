using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class IBmsDataExtensions
    {
        [GeneratedRegex(@"(?<title>.+)(?<chart>\[\w+\]|\(\w+\)|{\w+}|<\w+>)$")]
        private static partial Regex Regex_ChartName { get; }

        public static Bmson.BmsonData ConvertToBmson(this IRootData source, long resolution = 0, string? modeHint = null, ILaneConverter? converter = null, Predicate<ISoundNote>? selector = null)
        {
            Bmson.BmsonData data = new();
            var chartType = source.ChartType;
            var keyCount = source.GetKeyCount();
            converter ??= LaneConverter.GetAuto(chartType, keyCount);
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
            info.Level = int.TryParse(source.PlayLevel, out var level) ? level : 0;
            info.InitialBpm = source.Bpm;
            info.JudgeRank = source.ExRank;

            var total = source.Total;
            info.Total = total is <= 0 ? 100 : total / BmsUtils.CalcTotal(GetNoteCount(source));
            info.LnType = source.LnMode;
            info.BannerImage = source.Banner;
            info.EyecatchImage = source.StageFile;
            info.PreviewMusic = source.Preview;

            if (resolution is <= 0)
            {
                resolution = (long)UInt128.Min((UInt128)GetMaxBarResolution(source), (UInt128)1_209_600_000);
                if (resolution % 4 is 0)
                {
                    resolution /= 4;
                }
            }
            info.Resolution = resolution;
            //
            long Convert(Rational position) => ((decimal)position * resolution * 4).RoundToLong();
            // bar
            List<Bmson.BarLine> barList = [];
            data.BarList = barList;
            // bga
            var bga_base = BmsUtils.GetLane(Channel.Bga_Base);
            var bga_layer = BmsUtils.GetLane(Channel.Bga_Layer1);
            var bga_poor = BmsUtils.GetLane(Channel.Bga_Poor);
            Bmson.BgaInfo bga = new();
            if (source.DefLists.TryGetList(DefType.Bmp, out var defList))
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
            Dictionary<int, Bmson.Note> lastNotes = [];
            List<Bmson.Bpm> bpmList = [];
            List<Bmson.Stop> stopList = [];
            List<Bmson.RateEvent> scrollList = [];
            List<Bmson.RateEvent> speedList = [];
            List<Bmson.Note>? GetNoteList(int id)
            {
                if (source.TryGetDef(DefType.Wav, id, out var name))
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
            foreach (var (pos, list) in source.Timeline.EachList())
            {
                lastBar = pos.Bar;
                var time = Convert(source.GetAbsolutePosition(pos));
                var tempo = 0d;
                var scroll = 0d;
                var speed = 0d;
                var totalStop = Rational.Zero;
                foreach (var note in CollectionsMarshal.AsSpan(list))
                {
                    if (note is ConductorNote conductor)
                    {
                        switch (conductor.Channel)
                        {
                            case Channel.Bpm:
                                tempo = conductor.DoubleValue;
                                break;
                            case Channel.Stop:
                                totalStop += conductor.Value;
                                break;
                            case Channel.Scroll:
                                scroll = conductor.DoubleValue;
                                break;
                            case Channel.Speed:
                                speed = conductor.DoubleValue;
                                break;
                        }
                    }
                    else if (note is MetaNote meta)
                    {
                        switch (meta.Channel)
                        {
                            case Channel.Bga_Base:
                                bga.BaseList.Add(new() { Y = time, Id = meta.Value });
                                break;
                            case Channel.Bga_Layer1:
                                bga.LayerList.Add(new() { Y = time, Id = meta.Value });
                                break;
                            case Channel.Bga_Poor:
                                bga.PoorList.Add(new() { Y = time, Id = meta.Value });
                                break;

                        }
                    }
                    else if (note is ISoundNote sound && selector(sound))
                    {
                        var lane = sound.Lane;
                        if (sound.Type is NoteType.Normal)
                        {
                            Bmson.Note bnote = new()
                            {
                                X = converter.Convert(sound.Lane),
                                Y = time,
                                Continue = false,
                            };
                            lastNotes[lane] = bnote;
                            GetNoteList(sound.Value)?.Add(bnote);
                        }
                        else if (sound.Type is NoteType.LongEnd)
                        {
                            if (lastNotes.TryGetValue(lane, out var bnote))
                            {
                                bnote.Length = time - bnote.Y;
                                lastNotes.Remove(lane);
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
