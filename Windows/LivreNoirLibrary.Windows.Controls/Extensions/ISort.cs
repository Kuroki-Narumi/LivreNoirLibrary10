using System;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface ISort
    {
        void SortBy(ListBox control, string key);
    }

    public static partial class ControlExtensions
    {
        public static void OnClick_ColumnHeader(this ISort owner, object sender, RoutedEventArgs e)
        {
            var obj = sender as DependencyObject;
            if (obj.TryGetAncestor<ListBox>(out var control) &&
                obj.TryGetFirstDescendant<FrameworkElement>(f => f.Tag is string, out var f) &&
                f.Tag is string tag)
            {
                owner.SortBy(control, tag);
                e.Handled = true;
            }
        }
    }
}
