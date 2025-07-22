using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LivreNoirLibrary.Media.BM3
{
    public class PackedTrack
    {
        public const int ChannelFilter_KeySwitch = 0x10000;
        public const int Channel_KeySwitch_Once = 0x7FFFFFFF;
        public const int ChannelFilter_ControlChange = 0x20000;
        public const int ChannelFilter_SysEx = 0x30000;

        private readonly List<IObject> _objects = [];
        private readonly Dictionary<string, int> _obj2id = [];
        private readonly Dictionary<int, int> _id2channel = [];

        private readonly List<PackedNote> _packed = [];

        private readonly List<string> _defList = [];
        private readonly RationalMultiTimeline<int> _defTimeline = [];

        public PackedTrack(BM3Score data, int trackId, PackOptions options, SysExPrefixCollection sysExPrefixes)
        {
            var track = data.GetTrack(trackId);
            // fields
            var objects = _objects;
            var obj2id = _obj2id;
            var id2channel = _id2channel;
            // options
            var rhythm = options.IsRhythmTrack;
            var rhythmLength = options.RhythmMaxLength;
            var msV = options.MsV;
            var ignoreTempo = options.IgnoreTempo;
            var portamento = options.Portamento;
            var selectCC = options.SelectCC;
            var targetCC = options.TargetCCs;
            var sc = track.Options.SideChainSources;
            var margin = options.AfterMargin;
            var markerFormat = options.SuffixWithDefault;
            var sort = options.Sort;
            var sk1 = options.SortKey1;
            var sk2 = options.SortKey2;
            var sk3 = options.SortKey3;
            TempoTimeline tempo = new(data);
            List<PackedNote> notes = [];
            RationalTimeline<int> lastNN = [];
            RationalKeyTimeline<int, int> controls = [];
            RationalKeyTimeline<int, int> ksTimeline = [];
            Dictionary<(Rational, int), bool> ksFlags = [];

            // local methods
            (INote Note, string HashString) GetQuantized(IObject obj, Rational pos)
            {
                string GetHashString(Note n, Rational pos)
                {
                    var end = pos + n.Length;
                    if (ignoreTempo)
                    {
                        return $"NN:{n.Number} Vel:{n.Velocity} Len:{n.Length}";
                    }
                    else
                    {
                        var lenMs = INote.GetQuantized((tempo.GetSeconds(end) - tempo.GetSeconds(pos)) * 1000, msV);
                        return $"NN:{n.Number} Vel:{n.Velocity} Len:{lenMs}";
                    }
                }
                if (obj is Note n)
                {
                    lastNN.Set(pos + n.Length, n.Number);
                    return (n, $"{{Note {GetHashString(n, pos)}}}");
                }
                else if (obj is NoteGroup ng)
                {
                    StringBuilder sb = new();
                    sb.Append("{NoteGroup Members:[");
                    foreach (var (innerPos, innerNote) in ng.EachNote(pos))
                    {
                        lastNN.Set(innerPos + innerNote.Length, innerNote.Number);
                        sb.Append($"{innerPos - pos}-");
                        sb.Append(GetHashString(innerNote, innerPos));
                        sb.Append(',');
                    }
                    sb.Append("]}");
                    return (ng, sb.ToString());
                }
                else
                {
                    return default;
                }
            }

            int GetObjectId_Key(IObject obj, string key)
            {
                if (!obj2id.TryGetValue(key, out var id))
                {
                    id = objects.Count;
                    objects.Add(obj);
                    obj2id.Add(key, id);
                }
                return id;
            }

            int AddControl(int channel, Rational pos, IObject obj)
            {
                var id = GetObjectId_Key(obj, obj.GetIdentifier());
                controls.Set(channel, pos, id);
                id2channel.TryAdd(id, channel);
                return id;
            }

            int AddKeySwitch(Rational pos, KeySwitch obj)
            {
                var id = GetObjectId_Key(obj, obj.GetIdentifier());
                var gid = obj.GroupId;
                ksTimeline.Set(gid, pos, id);
                if (obj.Mode is KeySwitchMode.Once)
                {
                    ksFlags[(pos, id)] = true;
                    id2channel.TryAdd(id, Channel_KeySwitch_Once);
                }
                else
                {
                    id2channel.TryAdd(id, gid | ChannelFilter_KeySwitch);
                }
                return id;
            }

            // path 1 : register
            foreach (var (pos, list) in track.Timeline.EachList())
            {
                foreach (var obj in list.Order())
                {
                    if (track.IsNormalNote(obj))
                    {
                        var (qn, hash) = GetQuantized(obj, pos);
                        var objId = GetObjectId_Key(qn, hash);
                        var length = qn.Length;
                        var endPos = pos + length + margin;
                        PackedNote packed = new()
                        {
                            NoteId = objId,
                            Position = pos,
                            Length = qn.Length,
                            EndPosition = endPos,
                            SortKey = qn.GetSortKey(sk1, sk2, sk3, objId),
                            Markers = qn.GetMarkersArray(),
                            BaseMarkerName = qn.GetMarkerName(markerFormat),
                        };
                        foreach (var (p, n) in qn.EachNote(pos))
                        {
                            lastNN.Set(p, n.Number);
                        }
                        tempo.CopyTo(packed.Tempo, RangeUtils.Get(pos, endPos, false), -pos);
                        foreach (var scId in sc)
                        {
                            if (trackId != scId && data.TryGetTrack(scId, out var source))
                            {
                                var scTimeline = packed.GetSideChain(scId);
                                foreach (var (tPos, tObj) in source.Timeline.Range(RangeUtils.EndAt(endPos)))
                                {
                                    if (source.IsNormalNote(tObj))
                                    {
                                        (qn, hash) = GetQuantized(tObj, tPos);
                                        objId = GetObjectId_Key(qn, hash);
                                        foreach (var (pPos, pNote) in qn.EachNote(tPos))
                                        {
                                            var pEndPos = pPos + pNote.Length;
                                            if (pPos < endPos && pEndPos > pos)
                                            {
                                                scTimeline.Add(pPos - pos, objId);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        notes.Add(packed);
                    }
                    else
                    {
                        if (track.CheckKeySwitchNote(obj, out var kso))
                        {
                            var gid = kso.GroupId;
                            var mode = kso.Mode;
                            var note = (obj as Note)!;
                            var nn = note.Number;
                            if (mode is KeySwitchMode.Hold)
                            {
                                AddKeySwitch(pos, new KeySwitch() { Number = nn, GroupId = gid, Mode = KeySwitchMode.HoldOn });
                                AddKeySwitch(pos + note.Length, new KeySwitch() { Number = nn, GroupId = gid, Mode = KeySwitchMode.HoldOff });
                            }
                            else
                            {
                                AddKeySwitch(pos, new KeySwitch() { Number = nn, GroupId = gid, Mode = mode });
                            }
                        }
                        else
                        {
                            var channel = 0;
                            if (obj is ControlChange cc)
                            {
                                var type = cc.Type;
                                if (!selectCC || targetCC.Contains(cc.Type))
                                {
                                    channel = (int)type | ChannelFilter_ControlChange;
                                }
                            }
                            else if (obj is SysEx se)
                            {
                                var type = sysExPrefixes.FindIndex(s => se.StartsWith(s.Prefix));
                                if (type is >= 0)
                                {
                                    channel = type | ChannelFilter_SysEx;
                                }
                            }
                            if (channel is not 0)
                            {
                                AddControl(channel, pos, obj);
                            }
                        }
                    }
                }
            }

            // path 2 : identify
            var packedList = _packed;
            MemoryStream ms = new(32768);
            Dictionary<byte[], int> packed2id = new(new ByteArrayEqualityComparer());
            Dictionary<int, List<Rational>> packedPosList = [];
            var ksKeys = ksTimeline.GetKeyList();
            foreach (var packed in CollectionsMarshal.AsSpan(notes))
            {
                var pos = packed.Position;
                var endPos = packed.EndPosition;
                // Portamento
                if (portamento)
                {
                    packed.PreviousNoteNumber = lastNN.Get(pos, SearchMode.Previous, -1);
                }
                // Controls
                var ctrl = packed.Controls;
                foreach (var channel in controls.GetKeyList())
                {
                    var id = controls.Get(channel, pos, -1);
                    if (id is >= 0)
                    {
                        ctrl.Add(default, id);
                    }
                }
                foreach (var (_, pPos, id) in controls.Range(RangeUtils.Get(pos, endPos)))
                {
                    var p = pPos - pos;
                    if (p.IsPositiveThanZero() && id is >= 0)
                    {
                        ctrl.Add(pos, id);
                    }
                }
                // keyswitch
                var obj = (objects[packed.NoteId] as INote)!;
                foreach (var (pPos, note) in obj.EachNote(default))
                {
                    foreach (var ksKey in ksKeys)
                    {
                        if (ksTimeline.TryGet(ksKey, pPos + pos, SearchMode.PreviousOrEqual, out var acPos, out var kId))
                        {
                            var flagKey = (acPos, kId);
                            if (ksFlags.TryGetValue(flagKey, out var flag))
                            {
                                if (flag)
                                {
                                    ksFlags[flagKey] = false;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            ctrl.Add(pPos, kId);
                        }
                    }
                }
                // Register
                var key = packed.GetKey(ms);
                if (!packed2id.TryGetValue(key, out var packIndex))
                {
                    packIndex = packedList.Count;
                    packedList.Add(packed);
                    packed2id[key] = packIndex;
                }
                packedPosList.Add(packIndex, pos);
            }

            // path 3 : sort
            var sortIndex = 0;
            IEnumerable<PackedNote> enumerable = sort ? packedList.OrderBy(p => p.SortKey) : packedList;
            foreach (var packed in enumerable)
            {
                packed.SortIndex = sortIndex;
                sortIndex += packed.Markers.Length;
            }
            var maxSortIndex = sortIndex;

            // path 4 : marker
            var indexSuffix = SliceUtils.ContainsIndexSuffix(markerFormat);
            SortedDictionary<int, string> markerInfos = [];
            RationalMultiTimeline<int> markerTimeline = [];
            for (var i = 0; i < packedList.Count; i++)
            {
                var packed = packedList[i];
                var markers = packed.Markers;
                var markerCount = packed.Markers.Length;
                var markerName = packed.BaseMarkerName;
                sortIndex = packed.SortIndex;
                var markerNames = packed.MarkerNames;
                var posList = packedPosList[i];
                for (var j = 0; j < markerCount; j++)
                {
                    var markerId = sortIndex + j;
                    string name;
                    if (indexSuffix)
                    {
                        name = SliceUtils.ReplaceIndexSuffix(markerName, markerId, maxSortIndex);
                    }
                    else if (markerCount is > 1)
                    {
                        name = $"{markerName}_{SliceUtils.ReplaceIndexSuffix(SliceUtils.Suffix_Index1, j, markerCount)}";
                    }
                    else
                    {
                        name = markerName;
                    }
                    markerNames.Add(name);

                    var markerPosition = markers[j];
                    foreach (var packedPosition in CollectionsMarshal.AsSpan(posList))
                    {
                        markerTimeline.Add(packedPosition + markerPosition, markerId);
                    }

                    markerInfos.Add(markerId, name);
                }
            }

            // path 5 : timeline by defIndex
            var defList = _defList;
            var defTimeline = _defTimeline;
            foreach (var (pos, list) in markerTimeline.EachList())
            {
                foreach (var markerId in CollectionsMarshal.AsSpan(list))
                {
                    var name = markerInfos[markerId];
                    var defId = defList.Count;
                    defList.Add(name);
                    defTimeline.Add(pos, defId);
                }
            }
        }

        public static string GetMidiFilename(BM3Score source, string baseFilename, int trackId, PackOptions options)
            => PackUtils.Format(options.ExportFilenameWithDefault, baseFilename, source, trackId);

        public (MidiData Data, string Filename) CreateMidiData(BM3Score source, string baseFilename, int trackId, PackOptions options)
        {
            var cutTail = options.CutTail;
            var tailMargin = options.TailMargin;
            var filename = GetMidiFilename(source, baseFilename, trackId, options);
            var headroom = options.Headroom;
            var interval = options.Interval;
            var sourceTempo = new TempoTimeline(source);

            MidiData data = new();
            var tempoTimeline = data.ConductorTrack.Timeline;
            var mainSrc = source.GetTrack(trackId);
            var mainDst = data.GetTrack(1);
            var timeline = mainDst.Timeline;
            var currentTid = 2;
            Dictionary<int, int> src2dst = [];

            Rational pos = default;

            Track GetNewTrack(int sourceIndex)
            {
                var target = data.GetTrack(currentTid);
                src2dst.Add(sourceIndex, currentTid);
                currentTid++;
                return target;
            }

            if (source.Options.SetupBar)
            {
                var endPos = source.GetTimeSignature(pos).ToRational();
                headroom += (int)Math.Ceiling((double)endPos * 4);
                void ApplySetup(BM3Track source, Track target)
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
                        Track target;
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
            var lastLen = headroom;
            data.SetTimeSignature(default, new(headroom, 4));
            var lastTempo = sourceTempo.Get(default);
            tempoTimeline.SetTempo(default, lastTempo);
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
            var objects = _objects;
            Dictionary<int, int> lastCtrl = [];
            pos = new(headroom, 4);
            Rational portaLength = new(1, 16);
            var lastNN = -1;
            foreach (var packed in CollectionsMarshal.AsSpan(_packed))
            {
                var len = (int)Math.Ceiling(((double)packed.EndPosition - packed.Position) * 4) + interval;
                // portamento
                var nn = packed.PreviousNoteNumber;
                if (nn is >= 0 && nn != lastNN)
                {
                    timeline.Add(pos, new MetaText(MetaType.Marker, Marker.IgnoreName));
                    timeline.Add(pos + portaLength, new Note() { Number = nn, Velocity = 1, Length = portaLength });
                    len += interval;
                }
                else
                {
                    nn = -1;
                }
                // signature
                if (lastLen != len)
                {
                    lastLen = len;
                    data.SetTimeSignature(pos, new(len, 4));
                }
                if (nn is >= 0)
                {
                    pos += new Rational(interval, 4);
                }
                // tempo
                foreach (var (pPos, tempo) in packed.Tempo)
                {
                    if (lastTempo != tempo)
                    {
                        lastTempo = tempo;
                        tempoTimeline.SetTempo(pos + pPos, tempo);
                    }
                }
                // controls
                foreach (var (pPos, index) in packed.Controls)
                {
                    var channel = _id2channel[index];
                    if (channel is Channel_KeySwitch_Once || !(lastCtrl.TryGetValue(channel, out var current) && current == index))
                    {
                        lastCtrl[channel] = index;
                        var obj = objects[index];
                        timeline.Add(pos + pPos, obj);
                    }
                }
                // sidechain
                foreach (var (tid, sc) in packed.SideChains)
                {
                    var dst = data.GetTrack(src2dst[tid]).Timeline;
                    foreach (var (pPos, index) in sc)
                    {
                        dst.Add(pos + pPos, objects[index]);
                    }
                }
                // note
                var note = (objects[packed.NoteId] as INote)!;
                timeline.Add(pos, note);
                if (note is Note n)
                {
                    lastNN = n.Number;
                }
                else if (note is NoteGroup ng)
                {
                    lastNN = ng.LastNote.Number;
                }
                // markers
                var markers = packed.Markers;
                var names = packed.MarkerNames;
                for (int i = 0; i < markers.Length; i++)
                {
                    var marker = markers[i];
                    var name = names[i];
                    timeline.Add(pos + marker, new MetaText(MetaType.Marker, name));
                }
                if (cutTail)
                {
                    timeline.Add(pos + packed.Length + tailMargin, new MetaText(MetaType.Marker, Marker.IgnoreName));
                }

                if (nn is >= 0)
                {
                    len -= interval;
                }
                pos += new Rational(len, 4);
            }
            // dummy
            timeline.Add(pos, new MetaText(MetaType.Marker, Marker.IgnoreName));
            timeline.Add(pos, new Note() { Number = 0, Velocity = 1, Length = new(1, 64) });
            return (data, filename);
        }

        public (int NextDefId, int NextLane) CreateBmsData(Bms.BmsData target, string baseName, int defId, int startLane, bool oneOrigin)
        {
            var defSource = _defList;
            var defCount = defSource.Count;
            var defTarget = target.DefLists;
            Dictionary<int, int> defMap = [];
            for (var i = 0; i < defCount; i++)
            {
                var name = defSource[i];
                if (!ExtRegs.Wav.IsMatch(name))
                {
                    name += $".{Exts.Wav}";
                }
                defId = defTarget.FindFreeIndex(Bms.DefType.Wav, defId);
                defTarget.Set(Bms.DefType.Wav, defId, $"{baseName}{name}");
                defMap.Add(i, defId);
            }

            var maxLane = 0;
            foreach (var (pos, list) in _defTimeline.EachList())
            {
                list.Sort();
                var bPos = oneOrigin ? target.GetPosition(pos + 1) : target.GetPosition(pos);
                var c = list.Count;
                if (c > maxLane)
                {
                    maxLane = c;
                }
                for (var lane = 0; lane < c; lane++)
                {
                    var id = defMap[list[lane]];
                    Bms.Note note = new(Bms.NoteType.Normal, -(startLane + lane), id);
                    target.Timeline.Add(bPos, note);
                }
            }

            return (defId, startLane + maxLane);
        }
    }
}
