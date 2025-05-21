using System;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public static partial class ControlExtension
    {
        public static Point GetLeftTopPosition(this Control control, ListView listView, int index)
        {
            Rect bounds;
            if (listView.FindFirstDescendant<VirtualizingStackPanel>(out var stack))
            {
                var pos = stack.PointToScreen(new(0, 0));
                bounds = new(pos.X, pos.Y, stack.ActualWidth, stack.ActualHeight);
            }
            else
            {
                var pos = control.PointToScreen(new(0, 0));
                bounds = new(pos.X, pos.Y, control.ActualWidth, control.ActualHeight);
            }
            int i = 0;
            if (index is >= 0)
            {
                if (listView.FindFirstDescendant<ListViewItem>(d => i++ == index, out var item))
                {
                    var pos = item.PointToScreen(new(0, item.ActualHeight));
                    return new Point(Math.Clamp(pos.X, bounds.X, bounds.Right), Math.Clamp(pos.Y, bounds.Y, bounds.Bottom));
                }
            }
            return bounds.TopLeft;
        }

        public static Point GetLeftTopPosition(this Control control, ListView listView, Predicate<ListViewItem>? predicate)
        {
            Rect bounds;
            if (listView.FindFirstDescendant<ScrollContentPresenter>(out var content))
            {
                var pos = content.PointToScreen(new(0, 0));
                bounds = new(pos.X, pos.Y, content.ActualWidth, content.ActualHeight);
            }
            else
            {
                var pos = control.PointToScreen(new(0, 0));
                bounds = new(pos.X, pos.Y, control.ActualWidth, control.ActualHeight);
            }
            if (predicate is not null && listView.FindFirstDescendant(predicate, out var item))
            {
                var pos = item.PointToScreen(new(0, item.ActualHeight));
                return new Point(Math.Clamp(pos.X, bounds.X, bounds.Right), Math.Clamp(pos.Y, bounds.Y, bounds.Bottom));
            }
            return bounds.TopLeft;
        }

        public static Point GetLeftTopPosition(this Control control, TreeView treeView, Predicate<TreeViewItem>? predicate)
        {
            Rect bounds;
            Point pos;
            if (treeView.FindFirstDescendant<ScrollContentPresenter>(out var content))
            {
                pos = content.PointToScreen(new(0, 0));
                bounds = new(pos.X, pos.Y, content.ActualWidth, content.ActualHeight);
            }
            else
            {
                pos = control.PointToScreen(new(0, 0));
                bounds = new(pos.X, pos.Y, control.ActualWidth, control.ActualHeight);
            }
            pos = treeView.PointToScreen(new(0, 0));
            if (predicate is not null && treeView.FindFirstDescendant(predicate, out var item))
            {
                pos.Y = item.PointToScreen(new(0, item.ActualHeight)).Y;
            }
            return new Point(pos.X, Math.Clamp(pos.Y, bounds.Y, bounds.Bottom));
        }
    }
}
