using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BaseData
    {
        public void RemoveUnusedBgmLane()
        {
            SortedSet<int> used = [];
            foreach(var (_, note) in Timeline)
            {
                if (note.IsBgm())
                {
                    used.Add(-note.Lane);
                }
            }
            Dictionary<int, int> laneMap = [];
            var i = 0;
            foreach (var lane in used)
            {
                laneMap.Add(-lane, i);
                i--;
            }
            foreach (var (_, note) in Timeline)
            {
                if (note.IsBgm())
                {
                    note.Lane = laneMap[note.Lane];
                }
            }
        }

        public void SortToBgm(KeySortOptions? options = null, Selection? selection = null)
        {
            options ??= new() { GroupList = MakeGroup(DefLists[DefType.Wav], 2) };
            // 1st pass: 
            var groupList = options.GroupList;
            SortedList<int, NoteSortListItem> notesList = [];
            if (options.RemoveMeta)
            {
                Timeline.RemoveIf((_, n) => n.IsMetaKey());
            }
            var selector = options.Target.GetSelector(selection?.GetNoteHash() ?? [], false);
            foreach (var (pos, note) in Timeline)
            {
                if (selector(note) && groupList.TryGetValue(note.Id, out var groupId))
                {
                    if (!notesList.TryGetValue(groupId, out var list))
                    {
                        list = new();
                        notesList.Add(groupId, list);
                    }
                    list.Add(pos, note);
                }
            }
            // 2nd pass
            var lane = 1 - options.StartLane;
            foreach (var (groupId, list) in notesList)
            {
                list.Sort(ref lane);
            }
        }

        private class NoteSortListItem
        {
            public SortedList<BarPosition, List<Note>> Notes { get; } = [];
            public int MaxCount { get; private set; }

            public void Add(BarPosition position, Note note)
            {
                if (!Notes.TryGetValue(position, out var list))
                {
                    list = [];
                    Notes.Add(position, list);
                }
                list.Add(note);
                var count = list.Count;
                if (count > MaxCount)
                {
                    MaxCount = count;
                }
            }

            public void Sort(ref int lane)
            {
                foreach (var (_, list) in Notes)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Lane = lane - i;
                    }
                }
                lane -= MaxCount;
            }
        }

        public static SortedDictionary<int, int> MakeGroup(DefList defList, int minCount)
        {
            GroupNameTree tree = new();
            foreach (var (index, value) in defList)
            {
                var ary = Regex_Delimiter.Split(value);
                tree.AddChild(index, ary);
            }
            tree.RemoveChildren(minCount);
            var flatten = tree.Flatten();
            SortedDictionary<int, int> result = [];
            Dictionary<string, int> g2i = [];
            foreach (var (defIndex, groupName) in flatten)
            {
                if (!g2i.TryGetValue(groupName, out var groupIndex))
                {
                    groupIndex = g2i.Count;
                    g2i.Add(groupName, groupIndex);
                }
                result.Add(defIndex, groupIndex);
            }
            return result;
        }

        private class GroupNameTree(string key = "")
        {
            private readonly Dictionary<string, GroupNameTree> _children = [];
            private readonly string _key = key;
            private readonly List<int> _index_list = [];

            public void AddChild(int index, ReadOnlySpan<string> keys, int keyIndex = 0)
            {
                if (keyIndex < keys.Length)
                {
                    var key = keys[keyIndex];
                    if (!_children.TryGetValue(key, out var child))
                    {
                        child = new($"{_key}{key}");
                        _children.Add(key, child);
                    }
                    child.AddChild(index, keys, keyIndex + 1);
                }
                else
                {
                    _index_list.Add(index);
                }
            }

            public void RemoveChildren(int minCount)
            {
                var keys = _children.Keys.ToArray();
                foreach (var key in keys)
                {
                    var child = _children[key];
                    child.RemoveChildren(minCount);
                    if (child._children.Count is 0 && child._index_list.Count < minCount)
                    {
                        _index_list.AddRange(child._index_list);
                        _children.Remove(key);
                    }
                }
                _index_list.Sort();
            }

            public SortedDictionary<int, string> Flatten()
            {
                SortedDictionary<int, string> result = [];
                ProcessFlatten(result);
                return result;
            }

            private void ProcessFlatten(SortedDictionary<int, string> target)
            {
                var key = _key;
                foreach (var index in CollectionsMarshal.AsSpan(_index_list))
                {
                    target.Add(index, key);
                }
                foreach (var (_, child) in _children)
                {
                    child.ProcessFlatten(target);
                }
            }
        }

        [GeneratedRegex(@"(v\d+l\d+(?:-\d+)?o\d+[cdefgab]p?|[0-9]+|[^0-9-,._]+)")]
        private static partial Regex Regex_Delimiter { get; }
    }
}
