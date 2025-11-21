using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public class SelectionHistory
    {
        private readonly Dictionary<BarPosition, List<SelectionHistoryItem>> _items = [];

        public SelectionHistory(Selection selection)
        {
            var items = _items;
            foreach (var item in selection)
            {
                items.Add(item.BarPosition, SelectionHistoryItem.Create(item));
            }
        }

        public void Restore(Selection target, ITimeline timeline)
        {
            var buffer = ObjectPool.Rent<List<SelectionHistoryItem>>();
            try
            {
                target.Clear();
                foreach (var (pos, list) in _items)
                {
                    if (timeline.TryGetValue(pos, SearchMode.Equal, out _, out var targetList))
                    {
                        buffer.AddRange(list);
                        foreach (var note in targetList.AsSpan())
                        {
                            var index = buffer.FindIndex(item => item.Equals(note));
                            if (index is >= 0)
                            {
                                target.Add(list[index].Restore(pos));
                                buffer.RemoveAt(index);
                            }
                        }
                        buffer.Clear();
                    }
                }
            }
            finally
            {
                ObjectPool.Return(buffer);
            }
        }
    }
}
