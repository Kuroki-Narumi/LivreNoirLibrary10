using System;
using System.Collections;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public class CtListView : ListView, IListView
    {
        public new void SetSelectedItems(IEnumerable list)
        {
            base.SetSelectedItems(list);
        }
    }
}
