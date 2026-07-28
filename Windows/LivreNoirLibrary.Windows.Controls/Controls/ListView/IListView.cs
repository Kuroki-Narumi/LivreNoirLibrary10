using System;
using System.Collections;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IListView
    {
        object? DataContext { get; }
        object? Tag { get; }
        int SelectedIndex { get; set; }
        IList SelectedItems { get; }
        void ScrollIntoView(object item);
        void SetSelectedItems(IEnumerable list);
        bool Focus();
    }
}
