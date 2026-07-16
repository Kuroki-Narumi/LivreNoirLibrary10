using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly Dictionary<int, int> _id2channel = [];
        private readonly List<PackedNote> _packed = [];

        private readonly List<string> _defs = [];
        private readonly RationalMultiTimeline<int> _defTimeline = [];

        public int TrackId { get; }
        public ReadOnlySpan<IObject> Objects => _objects.AsSpan();
        public ReadOnlySpan<PackedNote> PackedNotes => _packed.AsSpan();
        public int Id2Channel(int id) => _id2channel[id];
        public int DefCount => _defs.Count;
        public ReadOnlySpan<string> Defs => _defs.AsSpan();
        public IEnumerable<(Rational, List<int>)> DefTimeline => _defTimeline.EnumerateList();
        public int MaxLane { get; }
        public bool AlignToRight { get; }

        public PackedTrack(IScore data, int trackId, PackOptions options, SysExPrefixCollection sysExPrefixes)
        {
            TrackId = trackId;
            AlignToRight = options.AlignToRight;
            var track = data.GetTrack(trackId);
            // fields
            var objects = _objects;
            var id2channel = _id2channel;
            // options
            var rhythm = options.IsRhythmTrack;
            var rhythmLength = options.RhythmLength;
            var lenQ = options.LengthQuantize;
            var velQ = options.VelQuantize;
            var needQuantize = lenQ.IsPositiveThanZero() || velQ is > 0;
            var msV = options.MsV;
            var ignoreTempo = options.IgnoreTempo;
            var portamento = options.Portamento;
            var selectCC = options.SelectCC;
            var targetCC = options.TargetCCs;
            var margin = options.AfterMargin;
            var markerFormat = options.SuffixWithDefault;
            var sort = options.Sort;
            var sk1 = options.SortKey1;
            var sk2 = options.SortKey2;
            var sk3 = options.SortKey3;
            var sc = track.SideChainSources;
            using var o = ObjectPool.RentStringBuilder(out var sb);
            TempoTimeline tempo = new(data);
            List<PackedNote> notes = [];
            Dictionary<string, int> obj2id = [];
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
                    if (needQuantize)
                    {
                        n = n.Clone();
                        n.QuantizeLength(lenQ);
                        n.QuantizeVelocity(velQ);
                    }
                    lastNN.Set(pos + n.Length, n.Number);
                    return (n, $"{{Note {GetHashString(n, pos)}}}");
                }
                else if (obj is NoteGroup ng)
                {
                    if (needQuantize)
                    {
                        ng = ng.Clone();
                        ng.QuantizeLength(lenQ);
                        ng.QuantizeVelocity(velQ);
                    }
                    sb.Length = 0;
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
            foreach (var (pos, list) in track.Timeline.EnumerateList())
            {
                foreach (var obj in list.Order())
                {
                    if (track.IsNormalNote(obj))
                    {
                        var (qn, hash) = GetQuantized(obj, pos);
                        var objId = GetObjectId_Key(qn, hash);
                        var length = qn.Length;
                        var endPos = pos + length + margin;
                        PackedNote packed = new(objId,
                                                pos,
                                                qn.Length,
                                                endPos,
                                                qn.GetSortKey(sk1, sk2, sk3, objId),
                                                qn.GetMarkersArray(),
                                                qn.GetMarkerName(markerFormat));
                        var sideChains = packed._sideChains;
                        foreach (var (p, n) in qn.EachNote(pos))
                        {
                            lastNN.Set(p, n.Number);
                        }
                        tempo.CopyTo(packed._tempo, RangeUtils.Get(pos, endPos, false), -pos);
                        foreach (var scId in sc)
                        {
                            if (trackId != scId && data.TryGetTrack(scId, out var source))
                            {
                                var scTimeline = sideChains.GetOrAdd(scId);
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
                                var type = sysExPrefixes.FindIndex(s => se.StartsWith(s.Bytes));
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
            Dictionary<byte[], int> packed2id = new(ByteArrayEqualityComparer.Default);
            Dictionary<int, List<Rational>> packedPosList = [];
            var ksKeys = ksTimeline.GetKeyList();
            foreach (var packed in notes.AsSpan())
            {
                var pos = packed._position;
                var endPos = packed._endPosition;
                // Portamento
                if (portamento)
                {
                    packed._previousNoteNumber = lastNN.Get(pos, SearchMode.Previous, -1);
                }
                // Controls
                var ctrl = packed._controls;
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
                        ctrl.Add(p, id);
                    }
                }
                // keyswitch
                var obj = (objects[packed._noteId] as INote)!;
                foreach (var (pPos, note) in obj.EachNote(default))
                {
                    foreach (var ksKey in ksKeys)
                    {
                        if (ksTimeline.TryGetValue(ksKey, pPos + pos, SearchMode.PreviousOrEqual, out var acPos, out var kId))
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
            IEnumerable<PackedNote> enumerable = sort ? packedList.OrderBy(p => p._sortKey) : packedList;
            foreach (var packed in enumerable)
            {
                packed._sortIndex = sortIndex;
                sortIndex += packed._markers.Length;
            }
            var maxSortIndex = sortIndex;

            // path 4 : marker
            var indexSuffix = SliceUtils.ContainsIndexSuffix(markerFormat);
            SortedDictionary<int, string> markerInfos = [];
            RationalMultiTimeline<int> markerTimeline = [];
            for (var i = 0; i < packedList.Count; i++)
            {
                var packed = packedList[i];
                var markers = packed._markers;
                var markerCount = markers.Length;
                var markerName = packed._baseMarkerName;
                sortIndex = packed._sortIndex;
                var markerNames = packed._markerNames;
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
                    foreach (var packedPosition in posList.AsSpan())
                    {
                        markerTimeline.Add(packedPosition + markerPosition, markerId);
                    }

                    markerInfos.Add(markerId, name);
                }
            }

            // path 5 : timeline by defIndex
            var maxLane = 0;
            var defList = _defs;
            var defTimeline = _defTimeline;
            foreach (var (pos, list) in markerTimeline.EnumerateList())
            {
                foreach (var markerId in list.AsSpan())
                {
                    var name = markerInfos[markerId];
                    var defId = defList.Count;
                    defList.Add(name);
                    defTimeline.Add(pos, defId);
                }
                maxLane = Math.Max(maxLane, list.Count);
            }

            MaxLane = maxLane;
        }

        public static string GetMidiFilename(IScore source, string baseFilename, int trackId, PackOptions options)
            => MidiPackUtils.Format(options.ExportFilenameWithDefault, baseFilename, source, trackId);

    }
}