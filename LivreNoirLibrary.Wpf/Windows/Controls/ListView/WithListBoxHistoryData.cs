using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Windows.Contorls
{
    public class WithListBoxHistoryData<T>
    {
        private readonly T _data;
        private readonly List<(WeakReference<ListBox>, int)> _selected_indexes = [];
        private readonly List<(WeakReference<ListBox>, List<object>)> _selected_items = [];

        public T Data => _data;

        public WithListBoxHistoryData(T data, params ReadOnlySpan<ListBox> controls)
        {
            _data = data;
            foreach (var control in controls)
            {
                if (control.SelectionMode is SelectionMode.Single)
                {
                    _selected_indexes.Add((new(control), control.SelectedIndex));
                }
                else
                {
                    List<object> list = [.. control.SelectedItems];
                    _selected_items.Add((new(control), list));
                }
            }
        }

        public void ApplySelection()
        {
            foreach (var (@ref, index) in CollectionsMarshal.AsSpan(_selected_indexes))
            {
                if (@ref.TryGetTarget(out var control) && control.SelectionMode is SelectionMode.Single)
                {
                    control.SelectedIndex = index;
                }
            }
            foreach (var (@ref, list) in CollectionsMarshal.AsSpan(_selected_items))
            {
                if (@ref.TryGetTarget(out var control) && control.SelectionMode is not SelectionMode.Single)
                {
                    if (control is Controls.IListView lv)
                    {
                        lv.SetSelectedItems(list);
                    }
                    else
                    {
                        var items = control.SelectedItems;
                        items.Clear();
                        foreach (var item in CollectionsMarshal.AsSpan(list))
                        {
                            items.Add(item);
                        }
                    }
                }
            }
        }
    }
}
