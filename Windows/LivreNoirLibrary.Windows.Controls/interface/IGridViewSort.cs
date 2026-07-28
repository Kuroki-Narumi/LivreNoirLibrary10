using System;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IGridViewSort
    {
        bool ClearSortIfEmptyTag => false;
        void SortBy(ListBox control, string key);
    }

    public static partial class ControlExtensions
    {
        public static void OnClick_ColumnHeader(this IGridViewSort owner, object sender, RoutedEventArgs e)
        {
            var header = (sender as GridViewColumnHeader) ?? (e.OriginalSource as GridViewColumnHeader);
            if (header.TryGetAncestor<ListBox>(out var control))
            {
                e.Handled = true;
                if (header.TryGetFirstDescendant<FrameworkElement>(f => f.Tag is string, out var f))
                {
                    owner.SortBy(control, (f.Tag as string)!);
                }
                else if (owner.ClearSortIfEmptyTag)
                {
                    control.Items.SortDescriptions.Clear();
                }
            }
        }
    }
}
