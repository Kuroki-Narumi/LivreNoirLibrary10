using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public partial class Track
    {
        public bool Dechord(out Selection newSelection)
        {
            newSelection = [];
            List<(Rational, NoteGroup)> groups = [];
            var timeline = _timeline;
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

        public bool Dechord(Selection selection, out Selection newSelection)
        {
            newSelection = [];
            var timeline = _timeline;
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

        public bool Enchord(GroupingOptions options, out Selection newSelection)
        {
            Selection selection = [];
            foreach (var (pos, obj) in _timeline)
            {
                if (obj is Note or NoteGroup)
                {
                    selection.Add(pos, obj);
                }
            }
            return Enchord(selection, options, out newSelection);
        }

        public bool Enchord(Selection selection, GroupingOptions options, out Selection newSelection)
        {
            var flag = false;
            if (options.PreDechord && Dechord(selection, out newSelection))
            {
                flag = true;
                selection = newSelection;
            }
            switch (options.Type)
            {
                case EnchordType.All:
                    return Enchord_All(selection, out newSelection) || flag;
                case EnchordType.Glide:
                    return Enchord_Glide(selection, out newSelection) || flag;
                case EnchordType.Group:
                    return Enchord_Group(selection, options.GroupCount, options.Downward, out newSelection) || flag;
                case EnchordType.Tuple:
                    return Enchord_Tuple(selection, options.TupleCount, options.Downward, out newSelection) || flag;
                default:
                    newSelection = selection;
                    return flag;
            }
        }

        public bool Enchord_All(Selection selection, out Selection newSelection, bool markEachNote = true)
        {
            if (selection.Count is <= 1)
            {
                newSelection = selection;
                return false;
            }
            var timeline = _timeline;
            List<(Rational, Note)> list = [];
            var offset = selection.GetFirstBeat();
            void Add(Rational pos, Note note)
            {
                list.Add((pos - offset, note));
            }
            foreach (var (pos, obj) in selection.EachItem())
            {
                if (obj is Note note && IsNormalNote(note) && timeline.Remove(pos, note))
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
                newSelection = [new(offset, group)];
                return true;
            }
            newSelection = [];
            return false;
        }

        public bool Enchord_Glide(Selection selection, out Selection newSelection)
        {
            if (selection.Count is <= 1)
            {
                newSelection = selection;
                return false;
            }
            var timeline = _timeline;
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
                if (obj is Note note && IsNormalNote(note))
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

        public bool Enchord_Group(Selection selection, int n, bool downward, out Selection newSelection) => Enchord_General(selection, downward, list => list.EachGroup(n), out newSelection);
        public bool Enchord_Tuple(Selection selection, int n, bool downward, out Selection newSelection) => Enchord_General(selection, downward, list => list.EachSlice(n), out newSelection);

        private delegate IEnumerable<Note[]> ListEnum(List<Note> list);

        private bool Enchord_General(Selection selection, bool downward, ListEnum func, out Selection newSelection)
        {
            if (selection.Count <= 1)
            {
                newSelection = selection;
                return false;
            }
            newSelection = [];
            var flag = false;
            // make group
            Dictionary<Rational, List<Note>> groups = [];
            foreach (var (pos, obj) in selection.EachItem())
            {
                if (obj is Note note && IsNormalNote(note))
                {
                    groups.Add(pos, note);
                }
            }
            foreach (var (pos, group) in groups)
            {
                List<Note> list = [];
                foreach (var note in group)
                {
                    list.Add(note);
                }
                list.Sort((n1, n2) => n1.Number.CompareTo(n2.Number));
                if (downward)
                {
                    list.Reverse();
                }
                if (AddChord(pos, func(list), newSelection))
                {
                    flag = true;
                }
            }
            return flag;
        }

        private bool AddChord(Rational pos, IEnumerable<Note[]> noteEnum, Selection newSelection)
        {
            var flag = false;
            var timeline = _timeline;
            foreach (var notes in noteEnum)
            {
                if (notes.Length >= 2)
                {
                    List<(Rational, Note)> list = [];
                    foreach (var note in notes)
                    {
                        timeline.Remove(pos, note);
                        list.Add((Rational.Zero, note));
                    }
                    NoteGroup ng = new(list);
                    timeline.Add(pos, ng);
                    newSelection.Add(pos, ng);
                    flag = true;
                }
                else
                {
                    foreach (var note in notes)
                    {
                        newSelection.Add(pos, note);
                    }
                }
            }
            return flag;
        }

        public bool AutoGroup(Selection selection)
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
                    if (AutoGroup(group))
                    {
                        flag = true;
                    }
                }
            }
            return flag;
        }

        public bool AutoGroup(NoteGroup source)
        {
            if (source.Count is <= 1) { return false; }
            var firstNote = source.FirstNote;
            SortedSet<Rational> posList = [];
            var result = false;
            var timeline = _timeline;
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
