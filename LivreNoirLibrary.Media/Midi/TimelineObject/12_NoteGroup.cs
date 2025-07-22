using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Midi.RawData;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed class NoteGroup : INote, ICloneable<NoteGroup>, IDumpable<NoteGroup>
    {
        private readonly (Rational Offset, Note Note)[] _members;
        private Rational _length;
        private int _min_nn;
        private int _max_nn;
        private Rational[] _borders = [];
        private readonly SortedSet<Rational> _markers = [];

        ObjectType IObject.ObjectType => ObjectType.NoteGroup;
        public string ObjectName => nameof(NoteGroup);
        public string ContentString => $"Members:{_members.Length} Len:{_length}";
        public int Count => _members.Length;
        public Note FirstNote => _members[0].Note;
        public Note LastNote => _members[^1].Note;
        public Rational Length => _length;
        public int MinNumber => _min_nn;
        public int MaxNumber => _max_nn;
        public ReadOnlySpan<Rational> Borders => _borders;

        public NoteGroup(List<(Rational, Note)> list)
        {
            _members = [.. list];
            Array.Sort(_members, (a, b) =>
            {
                var (o1, n1) = a;
                var (o2, n2) = b;
                if (o1 < o2)
                {
                    return -1;
                }
                else if (o1 > o2)
                {
                    return 1;
                }
                else
                {
                    return n1.CompareTo(n2);
                }
            });
            Refresh();
        }

        public void Dump(BinaryWriter writer)
        {
            writer.Write(_members.Length);
            foreach (var (offset, note) in _members)
            {
                writer.Write(offset);
                note.Dump(writer);
            }
            writer.Write(_markers.Count);
            foreach (var offset in _markers)
            {
                writer.Write(offset);
            }
        }

        public static NoteGroup Load(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            List<(Rational, Note)> list = [];
            for (int i = 0; i < count; i++)
            {
                var offset = reader.ReadRational();
                var note = Note.Load(reader);
                list.Add((offset, note));
            }
            NoteGroup g = new(list);
            count = reader.ReadInt32();
            var markers = g._markers;
            for (int i = 0; i < count; i++)
            {
                markers.Add(reader.ReadRational());
            }
            return g;
        }

        public NoteGroup Clone()
        {
            List<(Rational, Note)> members = [];
            foreach (var (offset, note) in _members)
            {
                members.Add((offset, note.Clone()));
            }
            NoteGroup g = new(members);
            g.SetMarkers(_markers);
            return g;
        }
        IObject IObject.Clone() => Clone();

        public void ExtendToEvent(RawTimeline timeline, int channel, long tick, Rational position, long ticksPerWholeNote)
        {
            foreach (var (offset, note) in _members)
            {
                var npos = position + offset;
                var ntick = IObject.GetTick(npos, ticksPerWholeNote);
                note.ExtendToEvent(timeline, channel, ntick, npos, ticksPerWholeNote);
            }
        }

        public void Dechord(Rational position, Timeline timeline, Selection? selection = null)
        {
            foreach (var (offset, note) in _members)
            {
                var pos = position + offset;
                timeline.Add(pos, note);
                selection?.Add(pos, note);
            }
        }

        public int MarkerCount => GetMarkersArray().Length;

        public IEnumerable<(Rational, Note)> EachNote()
        {
            foreach (var (offset, note) in _members)
            {
                yield return (offset, note);
            }
        }

        public IEnumerable<(Rational, Note)> EachNote(Rational position)
        {
            foreach (var (offset, note) in _members)
            {
                yield return (position + offset, note);
            }
        }

        public void QuantizeVelocity(int q)
        {
            foreach (var (_, note) in _members)
            {
                note.QuantizeVelocity(q);
            }
        }

        public void QuantizeLength(Rational q)
        {
            foreach (var (_, note) in _members)
            {
                note.QuantizeLength(q);
            }
            Refresh();
        }

        public bool ContentEquals(INote other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other switch
            {
                Note n => ContentEquals(n),
                NoteGroup g => ContentEquals(g),
                _ => false,
            };
        }

        public bool ContentEquals(Note other)
        {
            foreach (var (offset, note) in _members)
            {
                return offset.IsZero() && note.ContentEquals(other);
            }
            return false;
        }

        public bool ContentEquals(NoteGroup other)
        {
            var c = Count;
            if (c != other.Count)
            {
                return false;
            }
            for (var i = 0; i < c; i++)
            {
                var (p1, n1) = _members[i];
                var (p2, n2) = other._members[i];
                if (p1 != p2 || !n1.ContentEquals(n2))
                {
                    return false;
                }
            }
            return true;
        }

        public int CompareTo(NoteGroup other)
        {
            var max = Math.Min(_members.Length, other._members.Length);
            for (int i = 0; i < max; i++)
            {
                var left = _members[i];
                var right = other._members[i];
                var comp = left.Note.CompareTo(right.Note);
                if (comp is not 0)
                {
                    return comp;
                }
            }
            return _members.Length - other._members.Length;
        }

        public int CompareTo(IObject? other)
        {
            if (other is NoteGroup ng)
            {
                return CompareTo(ng);
            }
            return IObject.CompareBase(this, other);
        }

        public void Refresh()
        {
            var length = Rational.Zero;
            var min = 127;
            var max = 0;
            SortedSet<Rational> set = [];
            foreach (var (offset, note) in _members)
            {
                var nn = note.Number;
                var len = offset + note.Length;
                set.Add(offset);
                set.Add(len);
                if (len > length)
                {
                    length = len;
                }
                if (nn < min)
                {
                    min = nn;
                }
                if (nn > max)
                {
                    max = nn;
                }
            }
            _borders = [.. set];
            _length = length;
            _min_nn = min;
            _max_nn = max;
        }

        public SortKey GetSortKey(SortKeyType key1, SortKeyType key2, SortKeyType key3, int index)
        {
            foreach (var (_, note) in _members)
            {
                return note.GetSortKey(key1, key2, key3, index);
            }
            return new(0, 0, 0, index);
        }

        public string GetMarkerName(string format)
        {
            return SliceUtils.ReplaceAutoSuffix(format,
                    () => JoinName(n => KeyNames.Get(n.Number)),
                    () => JoinName(n => n.Number.ToString()),
                    GetFirstVelocity,
                    () => INote.GetLengthText(_length)
                ); ;
        }

        public string JoinName(Func<Note, string> func)
        {
            StringBuilder sb = new();
            int i = 0;
            foreach (var (offset, note) in _members)
            {
                if (offset.IsZero())
                {
                    if (i is not 0)
                    {
                        sb.Append('-');
                    }
                    sb.Append(func(note));
                }
                else
                {
                    break;
                }
                i++;
            }
            return sb.ToString();
        }

        private string GetFirstVelocity()
        {
            foreach (var (_, note) in _members)
            {
                return note.Velocity.ToString();
            }
            return "0";
        }

        public Rational NearestBorder(Rational position)
        {
            var index = _borders.FindNearestIndex(position);
            return (uint)index < (uint)_borders.Length ? _borders[index] : Rational.Zero;
        }

        public Rational NextBorder(Rational position)
        {
            var index = _borders.FindIndex(position, SearchMode.Previous);
            return (uint)index < (uint)_borders.Length ? _borders[index] : _borders[^1];
        }

        public Rational PreviousBorder(Rational position)
        {
            var index = _borders.FindIndex(position, SearchMode.Next);
            return (uint)index < (uint)_borders.Length ? _borders[index] : _borders[0];
        }

        public Rational NearestBorder(double position)
        {
            var index = _borders.FindNearestIndex(position);
            return (uint)index < (uint)_borders.Length ? _borders[index] : Rational.Zero;
        }

        public Rational NextBorder(double position)
        {
            var index = _borders.FindIndex(position, SearchMode.Previous);
            return (uint)index < (uint)_borders.Length ? _borders[index] : _borders[^1];
        }

        public Rational PreviousBorder(double position)
        {
            var index = _borders.FindIndex(position, SearchMode.Next);
            return (uint)index < (uint)_borders.Length ? _borders[index] : _borders[0];
        }

        public void ClearMarkers()
        {
            _markers.Clear();
        }

        public void SetMarkers(IEnumerable<Rational> source)
        {
            _markers.Clear();
            _markers.UnionWith(source);
        }

        public void ClearMarkers(Rational start, Rational end)
        {
            _markers.RemoveWhere(offset => offset >= start && offset < end);
        }

        public bool AddMarker(Rational offset) => offset.IsPositive() && _markers.Add(offset);
        public bool RemoveMarker(Rational offset) => _markers.Remove(offset);
        public bool SwitchMarker(Rational offset)
        {
            if (_markers.Remove(offset))
            {
                return false;
            }
            return _markers.Add(offset);
        }

        public void MarkEachNote()
        {
            ClearMarkers();
            foreach (var (offset, _) in _members)
            {
                _markers.Add(offset);
            }
        }

        public void MarkEachNote(Rational start, Rational end)
        {
            ClearMarkers(start, end);
            foreach (var (offset, _) in _members)
            {
                if (offset >= start && offset < end)
                {
                    _markers.Add(offset);
                }
            }
        }

        public void MarkInterval(Rational interval)
        {
            ClearMarkers();
            for (var offset = Rational.Zero; offset < _length; offset += interval)
            {
                _markers.Add(offset);
            }
        }

        public void MarkInterval(Rational interval, Rational start, Rational end)
        {
            ClearMarkers(start, end);
            for (var offset = start; offset < end; offset += interval)
            {
                _markers.Add(offset);
            }
        }

        public Rational[] GetMarkersArray()
        {
            var ary = _markers.ToArray();
            if (ary.Length is 0 || !ary[0].IsZero())
            {
                ary = [Rational.Zero, .. ary];
            }
            return ary;
        }

        public Rational[] GetMarkersArray(Rational startOffset)
        {
            List<Rational> list = [];
            foreach (var marker in _markers)
            {
                list.Add(marker + startOffset);
            }
            if (list.Count is 0 || list[0] != startOffset)
            {
                list.Insert(0, startOffset);
            }
            return [.. list];
        }

        Rational[] INote.GetMarkersArray(Rational offset) => offset.IsZero() ? GetMarkersArray() : GetMarkersArray(offset);

        public IEnumerable<(Rational Offset, int Index)> EachMarker(Rational startOffset = default)
        {
            var ary = GetMarkersArray(startOffset);
            for (int i = 0; i < ary.Length; i++)
            {
                yield return (ary[i], i + 1);
            }
        }

        public IEnumerable<(Rational Offset, Rational Length, int Index)> EachMarkerWithLength(Rational startOffset = default)
        {
            var ary = GetMarkersArray(startOffset);
            var c = ary.Length;
            if (c is 1)
            {
                yield return (ary[0], _length, 1);
            }
            else
            {
                var prev = ary[0];
                for (int i = 1; i < c; i++)
                {
                    var curr = ary[i];
                    yield return (prev, curr - prev, i);
                    prev = curr;
                }
                yield return (prev, _length - prev, c);
            }
        }

        public bool TryFindNearestMarker(Rational position, out Rational actualPosition)
        {
            var ary = GetMarkersArray();
            var index = ary.FindNearestIndex(position);
            if ((uint)index < (uint)ary.Length)
            {
                actualPosition = ary[index];
                return true;
            }
            else
            {
                actualPosition = default;
                return false;
            }
        }

        public bool TryFindMarker(Rational position, SearchMode type, out Rational actualPosition)
        {
            var ary = GetMarkersArray();
            var index = ary.FindIndex(position, type);
            if ((uint)index < (uint)ary.Length)
            {
                actualPosition = ary[index];
                return true;
            }
            else
            {
                actualPosition = default;
                return false;
            }
        }

        internal SortedSet<Rational> Markers => _markers;

        bool INote.MatchesNumber(SortedSet<int> set)
        {
            foreach (var (_, note) in _members)
            {
                if (note.MatchedNumber(set))
                {
                    return true;
                }
            }
            return false;
        }

        INote INote.GetEdited(Rational lenQ, Func<Rational, Rational>? lenFunc, int velQ, Func<double, double>? velFunc, Func<double, double>? nnFunc)
        {
            List<(Rational, Note)> list = [];
            foreach (var (pos, note) in _members)
            {
                list.Add((INote.GetEdit(pos, lenQ, lenFunc), note.GetEdited(lenQ, lenFunc, velQ, velFunc, nnFunc)));
            }
            NoteGroup g = new(list);
            var markers = g._markers;
            foreach (var marker in _markers)
            {
                markers.Add(INote.GetEdit(marker, lenQ, lenFunc));
            }
            return g;
        }
    }
}
