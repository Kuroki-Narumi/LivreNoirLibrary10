using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.Media.BM3;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Media.Integrated
{
    public static partial class MediaExtensions
    {

        [GeneratedRegex($"{PackUtils.ExportFormat_Filename}|{PackUtils.ExportFormat_Title}|{PackUtils.ExportFormat_Copyright}|{PackUtils.ExportFormat_TrackTitle}|{PackUtils.ExportFormat_TrackId}", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex Regex_Format { get; }

        public static string PackFormat(string format, string filename, IScore data, int trackId)
        {
            return Regex_Format.Replace(format, matched => matched.Value.ToLower() switch
            {
                PackUtils.ExportFormat_Filename => filename,
                PackUtils.ExportFormat_Title => data.Title ?? "",
                PackUtils.ExportFormat_Copyright => data.Copyright ?? "",
                PackUtils.ExportFormat_TrackTitle => data.GetTrackTitle(trackId),
                PackUtils.ExportFormat_TrackId => $"{trackId:D2}",
                _ => "",
            });
        }

        public static (MidiData Data, string Filename) CreateMidiData(
            this PackedTrack packed, 
            BM3Score source, 
            string baseFilename, 
            ScoreOptions scoreOptions,
            PackOptions packOptions)
        {
            var trackId = packed.TrackId;
            var filename = PackedTrack.GetMidiFilename(source, baseFilename, trackId, packOptions);
            var headroom = packOptions.Headroom;
            var sourceTempo = new TempoTimeline(source);

            MidiData data = new();
            var tempoTimeline = data.ConductorTrack.Timeline;
            var mainSrc = source.GetTrack(trackId);
            var mainDst = data.GetTrack(1);
            var timeline = mainDst.Timeline;
            var currentTid = 2;
            Dictionary<int, int> src2dst = [];

            Track GetNewTrack(int sourceIndex)
            {
                var target = data.GetTrack(currentTid);
                src2dst.Add(sourceIndex, currentTid);
                currentTid++;
                return target;
            }

            if (scoreOptions.SetupBarEnabled)
            {
                var endPos = source.GetTimeSignature(default).ToRational();
                headroom += (int)Math.Ceiling((double)endPos * 4);
                void ApplySetup(ITrack source, ITrack target)
                {
                    target.Port = source.Port;
                    target.Channel = source.Channel;
                    target.Title = $"{source.Title}(System)";
                    var tl = target.Timeline;
                    foreach (var (tPos, tObj) in source.Timeline.Range(RangeUtils.EndAt(endPos)))
                    {
                        if (!source.IsNormalNote(tObj))
                        {
                            tl.Add(tPos, tObj);
                        }
                    }
                }
                ApplySetup(mainSrc, mainDst);
                foreach (var (tid, ttrk) in source.EachTrack())
                {
                    if (tid != trackId && ttrk.Options.IsSystemTrack)
                    {
                        ITrack target;
                        if (tid is 0)
                        {
                            target = data.ConductorTrack;
                        }
                        else
                        {
                            target = GetNewTrack(tid);
                        }
                        ApplySetup(ttrk, target);
                    }
                }
            }
            else
            {
                mainDst.Port = mainSrc.Port;
                mainDst.Channel = mainSrc.Channel;
            }

            // initialize
            mainDst.Title = mainSrc.Title;
            data.SetTimeSignature(default, new(headroom, 4));
            tempoTimeline.SetTempo(default, sourceTempo.Get(default));
            // sidechain
            foreach (var sc in mainSrc.Options.SideChainSources)
            {
                if (source.TryGetTrack(sc, out var scSrc))
                {
                    Track scDst;
                    if (src2dst.TryGetValue(sc, out var tid))
                    {
                        scDst = data.GetTrack(tid);
                    }
                    else
                    {
                        scDst = GetNewTrack(sc);
                        scDst.Channel = scSrc.Channel;
                        scDst.Port = scSrc.Port;
                    }
                    scDst.Title = $"{scSrc.Title}(SideChain-{sc})";
                }
            }
            // contents
            PackedNoteExtendState state = new(packOptions, data, timeline, src2dst, headroom);
            foreach (var p in packed.PackedNotes)
            {
                p.Extend(packed, ref state);
            }
            // end of track
            var pos = state.Offset;
            timeline.Add(pos, new MetaText(MetaType.Marker, Media.Constants.IgnoreMarkerName));
            timeline.Add(pos, new Note() { Number = 0, Velocity = 1, Length = new(1, 64) });
            return (data, filename);
        }

        public static void ExtendConductor(this IScore score, IBmsData target, Rational length)
        {
            target.ClearBarLength();
            target.Timeline.RemoveAll((_, item) => item is IConductorNote);
            TempoTimeline conductor = new(score);
            target.Bpm = conductor.GetBpm(Rational.Zero);
            var enumer = conductor.GetEnumerator();
            var exists = enumer.MoveNext();
            foreach (var info in score.EachBar(length))
            {
                var head = info.Head;
                var len = info.Length;
                var number = info.Number;
                target.SetBarLength(number, len);
                var limit = head + len;
                while (exists)
                {
                    var (pos, tempo) = enumer.Current;
                    if (pos >= limit) { break; }
                    var bpm = TimeUtils.MicroSeconds2Bpm(tempo);
                    if (pos.IsZero())
                    {
                        target.Bpm = bpm;
                    }
                    else
                    {
                        BarPosition p = new(number, (pos - head) / len);
                        target.Timeline.Add(p, new ConductorNote(Channel.Bpm, (Rational)bpm));
                    }
                    exists = enumer.MoveNext();
                }
            }
        }

        public static void CreateBmsData(this PackedTrack packed, IBmsData target, string baseName, bool oneOrigin, ref int defId, ref int lane)
        {
            var defSource = packed.Defs;
            var defCount = defSource.Length;
            Dictionary<int, int> defMap = [];
            for (var i = 0; i < defCount; i++)
            {
                var name = defSource[i];
                if (!ExtRegs.Wav.IsMatch(name))
                {
                    name += $".{Exts.Wav}";
                }
                defId = target.FindFreeDefIndex(DefType.Wav, defId);
                target.SetDef(DefType.Wav, defId, $"{baseName}{name}");
                defMap.Add(i, defId);
            }

            var maxLane = packed.MaxLane;
            var alignToRight = packed.AlignToRight;
            foreach (var (pos, list) in packed.DefTimeline)
            {
                list.Sort();
                var bPos = oneOrigin ? target.GetBarPosition(pos + 1) : target.GetBarPosition(pos);
                var c = list.Count;
                for (var l = 0; l < c; l++)
                {
                    var id = defMap[list[l]];
                    var actualLane = lane + l;
                    if (alignToRight)
                    {
                        actualLane += maxLane - c;
                    }
                    SoundNote note = new(-actualLane, id);
                    target.Timeline.Add(bPos, note);
                }
            }
            lane += maxLane;
        }
    }
}
