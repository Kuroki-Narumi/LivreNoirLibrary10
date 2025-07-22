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
    public class PackedNote
    {
        public TempoTimeline Tempo { get; } = [];
        public RationalMultiTimeline<int> Controls { get; } = [];
        public Dictionary<int, RationalMultiTimeline<int>> SideChains { get; } = [];

        public int NoteId { get; set; }
        public int PreviousNoteNumber { get; set; } = -1;
        public Rational Position { get; set; }
        public Rational Length { get; set; }
        public Rational EndPosition { get; set; }
        public Rational[] Markers { get; set; } = [];
        public string BaseMarkerName { get; set; } = "";
        public SortKey SortKey { get; set; }

        public int SortIndex { get; set; }
        public List<string> MarkerNames { get; } = [];

        public RationalMultiTimeline<int> GetSideChain(int index)
        {
            if (!SideChains.TryGetValue(index, out var sc))
            {
                sc = [];
                SideChains.Add(index, sc);
            }
            return sc;
        }

        /*
         * } : 0
         * - : 1
         * , : 2
         * ; : 3
         * { : 8
         * Ctrl : 4,5
         * SdCh : 9,5
         * Mark : 14,5
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

            writer.Write(NoteId);
            writer.Write((byte)PreviousNoteNumber);

            // Controls
            writer.Write(bytes, 4, 5);
            foreach (var (pos, list) in Controls.EachList())
            {
                WriteList(pos, list);
            }
            writer.Write(bytes, 0, 1);
            // SideChain
            writer.Write(bytes, 9, 5);
            foreach (var (tid, timeline) in SideChains)
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
            foreach (var value in Markers)
            {
                writer.Write(value);
                writer.Write(bytes, 2, 1);
            }
            writer.Write(bytes, 0, 1);

            return ms.ToArray();
        }
    }
}
