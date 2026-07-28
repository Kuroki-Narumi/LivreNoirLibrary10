using LivreNoirLibrary.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class SearchBarBase : SearchBar, ISearchListBox
    {
        [DependencyProperty]
        private ListBox? _listBox;

        ListBox? ISearchListBox.SearchListBox => ListBox;

        private void OnListBoxChanged(ListBox? oldValue, ListBox? newValue)
        {
            if (oldValue is not null)
            {
                InsulateListBox(oldValue);
            }
            BindingOperations.ClearBinding(this, ItemsCountProperty);
            if (newValue is not null)
            {
                SetBinding(ItemsCountProperty, new Binding("Items.Count") { Source = newValue });
                BindListBox(newValue);
            }
        }

        protected virtual void InsulateListBox(ListBox listBox) { }
        protected virtual void BindListBox(ListBox listBox) { }
    }
}
