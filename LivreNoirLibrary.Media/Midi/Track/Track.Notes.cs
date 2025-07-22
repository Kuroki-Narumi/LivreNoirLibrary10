using System;
using System.Collections.Generic;
using System.Linq;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public partial class Track
    {
        public bool ContainsNote() => Timeline.Any(item => IsNormalNote(item.Item2));

        public bool IsNormalNote(IObject obj) => (obj is NoteGroup) || (obj is Note n && IsNormalNote(n));

        public bool IsNormalNote(Note note) => _keySwitch[note.Number].Mode is 0;

        public bool CheckKeySwitchNote(IObject obj, out KeySwitchOption ks)
        {
            if (obj is Note note)
            {
                ks = _keySwitch[note.Number];
                return ks.Mode is not 0;
            }
            ks = default;
            return false;
        }

        public int GetNoteCount()
        {
            int c = 0;
            foreach (var (_, obj) in _timeline)
            {
                if (IsNormalNote(obj))
                {
                    c++;
                }
            }
            return c;
        }

        public Rational GetFirstNotePosition()
        {
            foreach (var (pos, obj) in _timeline)
            {
                if (IsNormalNote(obj))
                {
                    return pos;
                }
            }
            return new(-1);
        }

        public Rational[] GetNoteBorders()
        {
            SortedSet<Rational> result = [];
            foreach (var (pos, obj) in _timeline)
            {
                if (obj is INote note)
                {
                    result.Add(pos);
                    result.Add(pos + note.Length);
                }
            }
            return result.Count is 0 ? [Rational.Zero] : [.. result];
        }

        public Rational NearestNotePosition(Rational position)
        {
            var ary = GetNoteBorders();
            var index = ary.FindNearestIndex(position);
            if ((uint)index < (uint)ary.Length)
            {
                return ary[index];
            }
            return -1;
        }

        public Rational GetNotePosition(Rational position, SearchMode type)
        {
            var ary = GetNoteBorders();
            var index = ary.FindIndex(position, type);
            if ((uint)index < (uint)ary.Length)
            {
                return ary[index];
            }
            return -1;
        }

        public Rational NextNotePosition(Rational position) => GetNotePosition(position, SearchMode.Next);
        public Rational PreviousNotePosition(Rational position) => GetNotePosition(position, SearchMode.Previous);

        public Rational NearestNotePosition(double position)
        {
            var ary = GetNoteBorders();
            var index = ary.FindNearestIndex(position);
            if ((uint)index < (uint)ary.Length)
            {
                return ary[index];
            }
            return -1;
        }

        public Rational GetNotePosition(double position, SearchMode type)
        {
            var ary = GetNoteBorders();
            var index = ary.FindIndex(position, type);
            if ((uint)index < (uint)ary.Length)
            {
                return ary[index];
            }
            return -1;
        }

        public Rational NextNotePosition(double position) => GetNotePosition(position, SearchMode.Next);
        public Rational PreviousNotePosition(double position) => GetNotePosition(position, SearchMode.Previous);

        public IEnumerable<(Rational Position, int Index)> EachNotePosition(Range<Rational> range = default, bool tail = false)
        {
            int i = 0;
            foreach (var (pos, obj) in _timeline.Range(range))
            {
                if (obj is Note note && IsNormalNote(note))
                {
                    yield return (pos, ++i);
                    if (tail)
                    {
                        yield return (pos + note.Length, -1);
                    }
                }
                else if (obj is NoteGroup g)
                {
                    foreach (var p in g.EachMarker(pos))
                    {
                        yield return (pos, ++i);
                    }
                    if (tail)
                    {
                        yield return (pos + g.Length, -1);
                    }
                }
            }
        }
    }
}
