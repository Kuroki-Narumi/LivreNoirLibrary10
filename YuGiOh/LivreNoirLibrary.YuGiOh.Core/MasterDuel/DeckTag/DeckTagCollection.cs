using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public class DeckTagCollection : ObservableList<DeckTag>
    {
        private readonly HashSet<string> _names = [];

        public bool Contains(string name) => _names.Contains(name);

        protected override void ClearItems()
        {
            base.ClearItems();
            _names.Clear();
        }

        protected override void AddItem(DeckTag item, out bool replaced, out int index, out DeckTag? oldItem)
        {
            base.AddItem(item, out replaced, out index, out oldItem);
            if (item.Name is { } name)
            {
                _names.Add(name);
            }
        }

        public void AddRange(DeckTag[] items)
        {
            base.AddRange(items);
            _names.UnionWith(items.Select(item => item.Name ?? ""));
        }

        protected override int RemoveItem(DeckTag item)
        {
            var ret = base.RemoveItem(item);
            if (ret >= 0 && item.Name is { } name)
            {
                _names.Remove(name);
            }
            return ret;
        }

        public DeckTag Rename(DeckTag item, string name, string? hint = null)
        {
            var list = _list;
            var index = list.IndexOf(item);
            if (index >= 0)
            {
                if (item.Name is { } current)
                {
                    _names.Remove(current);
                }
                var repIndex = list.FindIndex(item => item.Name == name);
                // リネーム先のタグが既に存在している場合
                if (repIndex >= 0)
                {
                    var newItem = list[repIndex];
                    if (!string.IsNullOrEmpty(hint))
                    {
                        newItem.SearchHint = hint;
                    }
                    RemoveAt(index);
                    return newItem;
                }
                else
                {
                    item.Name = name;
                    if (!string.IsNullOrEmpty(hint))
                    {
                        item.SearchHint = hint;
                    }
                    _names.Add(name);
                    this.NotifyCollectionReplaced(index, item, item);
                }
            }
            return item;
        }
    }
}
