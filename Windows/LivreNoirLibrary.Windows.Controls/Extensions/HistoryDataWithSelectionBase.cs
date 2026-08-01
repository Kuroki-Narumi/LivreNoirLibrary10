using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public class HistoryDataWithSelectionBase
    {
        private readonly List<object>[] _selectedItems;

        public bool IsSelectionStored { get; private set; }

        public HistoryDataWithSelectionBase(int listCount)
        {
            var ary = new List<object>[listCount];
            for (var i = 0; i < listCount; i++)
            {
                ary[i] = [];
            }
            _selectedItems = ary;
        }

        public void StoreSelection(ReadOnlySpan<ListBox> listViews)
        {
            var selected = _selectedItems.AsSpan();
            var max = Math.Min(selected.Length, listViews.Length);
            for (var i = 0; i < max; i++)
            {
                var target = selected[i];
                var source = listViews[i].SelectedItems;
                target.Clear();
                target.EnsureCapacity(source.Count);
                foreach (var item in source)
                {
                    target.Add(item);
                }
            }
            IsSelectionStored = true;
        }

        public void RestoreSelection(ReadOnlySpan<ListBox> listViews)
        {
            var selected = _selectedItems.AsSpan();
            var max = Math.Min(selected.Length, listViews.Length);
            for (var i = 0; i < max; i++)
            {
                listViews[i].SetSelectedItems(selected[i]);
            }
        }
    }
}
