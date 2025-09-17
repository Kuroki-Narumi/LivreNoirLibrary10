using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Buffers;
using System.Text;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.BM3
{
    public class PackedNote(int noteId,
                            Rational position,
                            Rational length,
                            Rational endPosition,
                            SortKey sortKey,
                            Rational[] markers,
                            string baseMarkerName)
    {
        internal readonly TempoTimeline _tempo = [];
        internal readonly RationalMultiTimeline<int> _controls = [];
        internal readonly Dictionary<int, RationalMultiTimeline<int>> _sideChains = [];
        internal readonly List<string> _markerNames = [];
        internal readonly int _noteId = noteId;
        internal readonly Rational _position = position;
        internal readonly Rational _length = length;
        internal readonly Rational _endPosition = endPosition;
        internal readonly SortKey _sortKey = sortKey;
        internal readonly Rational[] _markers = markers;
        internal readonly string _baseMarkerName = baseMarkerName;

        internal int _previousNoteNumber = -1;
        internal int _sortIndex;

        /*
         * } : 0
         * - : 1
         * , : 2
         * ; : 3
         * { : 8
         * Ctrl{ : 4,5
         * SdCh{ : 9,5
         * Mark{ : 14,5
         */
        private static readonly byte[] _bytes_cache = [.. "}-,;Ctrl{SdCh{Mark{"u8];

        public byte[] GetKey(MemoryStream? ms = null)
        {
            if (ms is not null)
            {
                ms.SetLength(0);
            }
            else
            {
                ms = new(32768);
            }
            var bytes = _bytes_cache;
            using BinaryWriter writer = new(ms, Encoding.UTF8, true);

            void WriteList(Rational pos, List<int> list)
            {
                writer.Write(pos);
                writer.Write(bytes, 1, 1);
                foreach (var value in CollectionsMarshal.AsSpan(list))
                {
                    writer.Write(value);
                    writer.Write(bytes, 2, 1);
                }
                writer.Write(bytes, 3, 1);
            }

            writer.Write(_noteId);
            writer.Write((byte)_previousNoteNumber);

            // Controls
            writer.Write(bytes, 4, 5);
            foreach (var (pos, list) in _controls.EachList())
            {
                WriteList(pos, list);
            }
            writer.Write(bytes, 0, 1);
            // SideChain
            writer.Write(bytes, 9, 5);
            foreach (var (tid, timeline) in _sideChains)
            {
                writer.Write(bytes, 8, 1);
                writer.Write(tid);
                writer.Write(bytes, 1, 1);
                foreach (var (pos, list) in timeline.EachList())
                {
                    WriteList(pos, list);
                }
                writer.Write(bytes, 0, 1);
            }
            writer.Write(bytes, 0, 1);
            // Marker
            writer.Write(bytes, 14, 5);
            foreach (var value in _markers)
            {
                writer.Write(value);
                writer.Write(bytes, 2, 1);
            }
            writer.Write(bytes, 0, 1);

            return ms.ToArray();
        }

        public void Extend(PackedTrack packedTrack, ref PackedNoteExtendState state)
        {
            var data = state.TargetData;
            var timeline = state.TargetTimeline;
            var tempoTimeline = data.ConductorTrack.Timeline;
            var offset = state.Offset;
            var interval = state.Interval;
            var lastNN = state.LastNoteNumber;
            var lastTempo = state.LastTempo;
            var src2dst = state.SideChainMap;
            var lastCtrl = state.LastControl;
            var len = (int)Math.Ceiling(((double)_endPosition - _position) * 4) + interval;
            var objects = packedTrack.Objects;
            var nn = _previousNoteNumber;
            if (nn is >= 0 && nn != lastNN)
            {
                timeline.Add(offset, new MetaText(MetaType.Marker, Constants.IgnoreMarkerName));
                var porta = state.PortamentoLength;
                timeline.Add(offset + porta, new Note() { Number = nn, Velocity = 1, Length = porta });
                len += interval;
            }
            else
            {
                nn = -1;
            }
            // signature
            if (state.LastBarLength != len)
            {
                state.LastBarLength = len;
                data.SetTimeSignature(offset, new(len, 4));
            }
            if (nn is >= 0)
            {
                offset += new Rational(interval, 4);
            }
            // tempo
            foreach (var (pPos, tempo) in _tempo)
            {
                if (lastTempo != tempo)
                {
                    lastTempo = tempo;
                    tempoTimeline.SetTempo(offset + pPos, tempo);
                }
            }
            // controls
            foreach (var (pPos, index) in _controls)
            {
                var channel = packedTrack.Id2Channel(index);
                if (channel is PackedTrack.Channel_KeySwitch_Once || !(lastCtrl.TryGetValue(channel, out var current) && current == index))
                {
                    lastCtrl[channel] = index;
                    var obj = objects[index];
                    timeline.Add(offset + pPos, obj);
                }
            }
            // sidechain
            foreach (var (tid, sc) in _sideChains)
            {
                var dst = data.GetTrack(src2dst[tid]).Timeline;
                foreach (var (pPos, index) in sc)
                {
                    dst.Add(offset + pPos, objects[index]);
                }
            }
            // note
            var note = (objects[_noteId] as INote)!;
            timeline.Add(offset, note);
            if (note is Note n)
            {
                lastNN = n.Number;
            }
            else if (note is NoteGroup ng)
            {
                lastNN = ng.LastNote.Number;
            }
            // markers
            var markers = _markers;
            var names = _markerNames;
            for (int i = 0; i < markers.Length; i++)
            {
                var marker = markers[i];
                var name = names[i];
                timeline.Add(offset + marker, new MetaText(MetaType.Marker, name));
            }
            if (state.CutTail)
            {
                timeline.Add(offset + _length + state.TailMargin, new MetaText(MetaType.Marker, Constants.IgnoreMarkerName));
            }
            if (nn is >= 0)
            {
                len -= interval;
            }
            state.Offset = offset + new Rational(len, 4);
            state.LastTempo = lastTempo;
            state.LastNoteNumber = lastNN;
        }
    }
}
