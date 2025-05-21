using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.ObjectModel
{
    public class RecentlyList : ObservableList<RecentlyItem>
    {
        public int MaxCount { get; set; } = 10;

        public RecentlyList() { }

        public RecentlyList(int maxCount)
        {
            MaxCount = maxCount;
        }

        public void CheckExistence()
        {
            RemoveIf(item => !File.Exists(item.Path));
        }

        public void Add(string? path)
        {
            if (!string.IsNullOrEmpty(path) && (File.Exists(path) || Directory.Exists(path)))
            {
                if (Count >= MaxCount)
                {
                    RemoveAt(Count - 1);
                }
                RemoveIf(item => item.Path == path);
                Insert(0, new(path));
            }
        }

        public void Remove(string path)
        {
            RemoveIf(item => item.Path == path);
        }

        public void RemoveIf(Predicate<RecentlyItem> predicate)
        {
            int i = 0;
            while (i < Count)
            {
                if (predicate(this[i]))
                {
                    RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        public void Load(List<string> list)
        {
            ClearWithoutNotify();
            for (var i = 0; i < list.Count; i++)
            {
                Add(list[i]);
            }
        }

        public void Save(List<string> list)
        {
            list.Clear();
            for (var i = Count - 1; i >= 0; i--)
            {
                list.Add(this[i].Path);
            }
        }

        public void Refresh()
        {
            RemoveIf(item => !File.Exists(item.Path));
        }
    }
}
