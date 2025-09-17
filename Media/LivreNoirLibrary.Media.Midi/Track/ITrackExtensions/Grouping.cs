using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public static partial class ITrackExtensions
    {
        public static bool Dechord<T>(this T track, out ISelection newSelection)
            where T : ITrack
        {
            newSelection = new Selection();
            List<(Rational, NoteGroup)> groups = [];
            var timeline = track.Timeline;
            foreach (var (pos, obj) in timeline)
            {
                if (obj is NoteGroup ng)
                {
                    groups.Add((pos, ng));
                }
            }
            if (groups.Count is > 0)
            {
                foreach (var (pos, ng) in CollectionsMarshal.AsSpan(groups))
                {
                    timeline.Remove(pos, ng);
                    ng.Dechord(pos, timeline, newSelection);
                }
            }
            return false;
        }

        public static bool Dechord<T>(this T track, ISelection selection, out ISelection newSelection)
            where T : ITrack
        {
            newSelection = new Selection();
            var timeline = track.Timeline;
            var flag = false;
            foreach (var (pos, obj) in selection.EachItem())
            {
                if (obj is NoteGroup ng && timeline.Remove(pos, obj))
                {
                    ng.Dechord(pos, timeline, newSelection);
                    flag = true;
                }
                else
                {
                    newSelection.Add(pos, obj);
                }
            }
            return flag;
        }

        public static bool Enchord<T>(this T track, GroupingOptions options, [MaybeNullWhen(false)] out ISelection newSelection)
            where T : ITrack
        {
            Selection selection = [];
            foreach (var (pos, obj) in track.Timeline)
            {
                if (obj is Note or NoteGroup)
                {
                    selection.Add(pos, obj);
                }
            }
            return Enchord(track, selection, options, out newSelection);
        }

        public static bool Enchord<T>(this T track, ISelection selection, GroupingOptions options, [MaybeNullWhen(false)] out ISelection newSelection)
            where T : ITrack
        {
            var flag = false;
            if (options.PreDechord && Dechord(track, selection, out newSelection))
            {
                flag = true;
                selection = newSelection;
            }
            switch (options.Type)
            {
                case EnchordType.All:
                    return EnchordAll(track, selection, out newSelection) || flag;
                case EnchordType.Glide:
                    return Enchord_Glide(track, selection, out newSelection) || flag;
                case EnchordType.Group:
                    return Enchord_Group(track, selection, options.GroupCount, options.Downward, out newSelection) || flag;
                case EnchordType.Tuple:
                    return Enchord_Tuple(track, selection, options.TupleCount, options.Downward, out newSelection) || flag;
                default:
                    newSelection = selection;
                    return flag;
            }
        }

        public static bool EnchordAll<T>(this T track, ISelection selection, [MaybeNullWhen(false)] out ISelection newSelection, bool markEachNote = true)
            where T : ITrack
        {
            if (selection.Count is <= 1)
            {
                newSelection = selection;
                return false;
            }
            var timeline = track.Timeline;
            List<(Rational, Note)> list = [];
            var offset = selection.GetFirstBeat();
            void Add(Rational pos, Note note)
            {
                list.Add((pos - offset, note));
            }
            foreach (var (pos, obj) in selection.EachItem())
            {
                if (obj is Note note && track.IsNormalNote(note) && timeline.Remove(pos, note))
                {
                    Add(pos, note);
                }
            }
            if (list.Count is > 0)
            {
                NoteGroup group = new(list);
                if (markEachNote)
                {
                    group.MarkEachNote();
                }
                timeline.Add(offset, group);
                newSelection = new Selection() { new(offset, group) };
                return true;
            }
            newSelection = default;
            return false;
        }

        public static bool Enchord_Glide<T>(this T track, ISelection selection, [MaybeNullWhen(false)] out ISelection newSelection)
            where T : ITrack
        {
            if (selection.Count is <= 1)
            {
                newSelection = selection;
                return false;
            }
            var timeline = track.Timeline;
            var flag = false;
            var first = Rational.Zero;
            var last = Rational.Zero;
            Note? firstNote = null;
            Selection newSel = [];
            List<(Rational, Note)> list = [];
            void Add()
            {
                if (firstNote is not null)
                {
                    if (list.Count is > 1)
                    {
                        timeline.Remove(first, firstNote);
                        NoteGroup group = new(list);
                        timeline.Add(first, group);
                        newSel.Add(first, group);
                        flag = true;
                    }
                    else
                    {
                        newSel.Add(first, firstNote);
                    }
                    firstNote = null;
                }
            }
            foreach (var (pos, obj) in selection.EachItem())
            {
                if (obj is Note note && track.IsNormalNote(note))
                {
                    if (pos < last)
                    {
                        timeline.Remove(pos, obj);
                        list.Add((pos - first, note));
                        var ll = pos + note.Length;
                        if (ll > last)
                        {
                            last = ll;
                        }
                    }
                    else
                    {
                        Add();
                        list.Clear();
                        list.Add((Rational.Zero, note));
                        firstNote = note;
                        first = pos;
                        last = pos + note.Length;
                    }
                }
            }
            Add();
            newSelection = newSel;
            return flag;
        }

        public static bool Enchord_Group<T>(this T track, ISelection selection, int n, bool downward, [MaybeNullWhen(false)] out ISelection newSelection)
            where T : ITrack
            => Enchord_General(track, selection, downward, list => list.EachGroup(n), out newSelection);
        public static bool Enchord_Tuple<T>(this T track, ISelection selection, int n, bool downward, [MaybeNullWhen(false)] out ISelection newSelection)
            where T : ITrack
            => Enchord_General(track, selection, downward, list => list.EachSlice(n), out newSelection);

        private delegate IEnumerable<(int Index, int Count)> ListEnum(List<Note> list);

        private static bool Enchord_General<T>(T track, ISelection selection, bool downward, ListEnum func, [MaybeNullWhen(false)] out ISelection newSelection)
            where T : ITrack
        {
            if (selection.Count <= 1)
            {
                newSelection = selection;
                return false;
            }
            newSelection = new Selection();
            var flag = false;
            // make group
            Dictionary<Rational, List<Note>> groups = [];
            foreach (var (pos, obj) in selection.EachItem())
            {
                if (obj is Note note && track.IsNormalNote(note))
                {
                    groups.Add(pos, note);
                }
            }
            List<Note> list = [];
            foreach (var (pos, group) in groups)
            {
                list.Clear();
                foreach (var note in group)
                {
                    list.Add(note);
                }
                if (downward)
                {
                    list.Sort((n1, n2) => n2.Number.CompareTo(n1.Number));
                }
                else
                {
                    list.Sort((n1, n2) => n1.Number.CompareTo(n2.Number));
                }
                if (AddChord(track, list, pos, func, newSelection))
                {
                    flag = true;
                }
            }
            return flag;
        }

        private static bool AddChord<T>(T track, List<Note> list, Rational pos, ListEnum func, ISelection newSelection)
            where T : ITrack
        {
            var span = CollectionsMarshal.AsSpan(list);
            var flag = false;
            var timeline = track.Timeline;
            foreach (var (index, count) in func(list))
            {
                if (count is >= 2)
                {
                    List<(Rational, Note)> newList = [];
                    foreach (var note in span.Slice(index, count))
                    {
                        timeline.Remove(pos, note);
                        newList.Add((Rational.Zero, note));
                    }
                    NoteGroup ng = new(newList);
                    timeline.Add(pos, ng);
                    newSelection.Add(pos, ng);
                    flag = true;
                }
                else
                {
                    newSelection.Add(pos, span[index]);
                }
            }
            return flag;
        }

        public static bool AutoGroup<T>(this T track, ISelection selection)
            where T : ITrack
        {
            SortedDictionary<int, HashSet<NoteGroup>> groups = [];
            foreach (var (_, obj) in selection.EachItem())
            {
                if (obj is NoteGroup ng)
                {
                    var count = -ng.Count;
                    if (!groups.TryGetValue(count, out var list))
                    {
                        list = [];
                        groups.Add(count, list);
                    }
                    list.Add(ng);
                }
            }
            var flag = false;
            foreach (var (_, set) in groups)
            {
                foreach (var group in set)
                {
                    if (AutoGroup(track, group))
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }

        public static bool AutoGroup<T>(this T track, NoteGroup source)
            where T : ITrack
        {
            if (source.Count is <= 1) { return false; }
            var firstNote = source.FirstNote;
            SortedSet<Rational> posList = [];
            var result = false;
            var timeline = track.Timeline;
            foreach (var (pos, obj) in timeline)
            {
                if (obj is Note note && note.Equals(firstNote))
                {
                    posList.Add(pos);
                }
            }

            List<(Rational, Note)> list = [];
            foreach (var pos in posList)
            {
                var success = true;
                foreach (var (npos, note) in source.EachNote(pos))
                {
                    if (timeline.Find(npos, obj => obj is Note n && n.Equals(note), out var obj))
                    {
                        list.Add((npos, (obj as Note)!));
                    }
                    else
                    {
                        success = false;
                        break;
                    }
                }
                if (success)
                {
                    foreach (var (npos, note) in CollectionsMarshal.AsSpan(list))
                    {
                        timeline.Remove(npos, note);
                    }
                    timeline.Add(pos, source);
                    result = true;
                }
                list.Clear();
            }
            return result;
        }
    }
}
