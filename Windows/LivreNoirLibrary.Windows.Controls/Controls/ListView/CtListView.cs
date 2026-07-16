using System;
using System.Collections;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public class CtListView : ListView, IListView
    {
        void IListView.SetSelectedItems(IEnumerable list) => SetSelectedItems(list);
    }
}
