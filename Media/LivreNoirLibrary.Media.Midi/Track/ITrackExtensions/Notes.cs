using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public static partial class ITrackExtensions
    {
        public static bool ContainsNote<T>(this T track)
            where T : ITrack => track.Timeline.Any(item => track.IsNormalNote(item.Item2));

        public static bool IsNormalNote<T>(this T track, IObject obj)
            where T : ITrack => (obj is NoteGroup) || (obj is Note n && IsNormalNote(track, n));

        public static bool IsNormalNote<T>(this T track, Note note)
            where T : ITrack => track.KeySwitchOptions[note.Number].Mode is 0;

        public static bool CheckKeySwitchNote<T>(this T track, IObject obj, out KeySwitchOption ks)
            where T : ITrack
        {
            if (obj is Note note)
            {
                ks = track.KeySwitchOptions[note.Number];
                return ks.Mode is not 0;
            }
            ks = default;
            return false;
        }

        public static int GetNoteCount<T>(this T track)
            where T : ITrack
        {
            int c = 0;
            foreach (var (_, obj) in track.Timeline)
            {
                if (IsNormalNote(track, obj))
                {
                    c++;
                }
            }
            return c;
        }

        public static Rational GetFirstNotePosition<T>(this T track)
            where T : ITrack
        {
            foreach (var (pos, obj) in track.Timeline)
            {
                if (IsNormalNote(track,obj))
                {
                    return pos;
                }
            }
            return new(-1);
        }

        private static readonly Lock _border_lock = new();
        private static readonly SortedSet<Rational> _border_buffer = [];

        public static Rational[] CreateNoteBorders<T>(this T track)
            where T : ITrack
        {
            lock (_border_lock)
            {
                var set = _border_buffer;
                set.Clear();
                foreach (var (pos, obj) in track.Timeline)
                {
                    if (obj is INote note)
                    {
                        set.Add(pos);
                        set.Add(pos + note.Length);
                    }
                }
                return set.Count is 0 ? [Rational.Zero] : [.. set];
            }
        }

        private static Rational ReturnPosition(Rational[] ary, int index) => (uint)index < (uint)ary.Length ? ary[index] : Rational.MinusOne;

        public static Rational NearestNotePosition<T>(this T track, Rational position)
            where T : ITrack
        {
            var ary = CreateNoteBorders(track);
            return ReturnPosition(ary, ary.FindNearestIndex(position));
        }

        public static Rational GetNotePosition<T>(this T track, Rational position, SearchMode mode)
            where T : ITrack
        {
            var ary = CreateNoteBorders(track);
            return ReturnPosition(ary, ary.FindIndex(position, mode));
        }

        public static Rational NextNotePosition<T>(this T track, Rational position) where T : ITrack => GetNotePosition(track, position, SearchMode.Next);
        public static Rational PreviousNotePosition<T>(this T track, Rational position) where T : ITrack => GetNotePosition(track, position, SearchMode.Previous);

        public static Rational NearestNotePosition<T>(this T track, double position)
            where T : ITrack
        {
            var ary = CreateNoteBorders(track);
            return ReturnPosition(ary, ary.FindNearestIndex(position));
        }

        public static Rational GetNotePosition<T>(this T track, double position, SearchMode mode)
            where T : ITrack
        {
            var ary = CreateNoteBorders(track);
            return ReturnPosition(ary, ary.FindIndex(position, mode));
        }

        public static Rational NextNotePosition<T>(this T track, double position) where T : ITrack => GetNotePosition(track, position, SearchMode.Next);
        public static Rational PreviousNotePosition<T>(this T track, double position) where T : ITrack => GetNotePosition(track, position, SearchMode.Previous);

        public static IEnumerable<(Rational Position, int Index)> EachNotePosition<T>(this T track, Range<Rational> range = default, bool tail = false)
            where T : ITrack
        {
            var i = 0;
            foreach (var (pos, obj) in track.Timeline.Range(range))
            {
                if (obj is Note note && track.IsNormalNote(note))
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
