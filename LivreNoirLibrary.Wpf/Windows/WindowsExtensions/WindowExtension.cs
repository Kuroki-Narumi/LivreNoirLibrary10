using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace LivreNoirLibrary.Windows
{
    public static partial class WindowsExtensions
    {
        public static void SetDispatcher(this DispatcherObject obj, Action action, DispatcherPriority timing = DispatcherPriority.Loaded)
        {
            obj.Dispatcher.BeginInvoke(action, timing);
        }

        public static void ShowChildAsDialog(this Window owner, Window child)
        {
            void activateSub(object? sender, EventArgs e)
            {
                child.Activate();
            }
            owner.Activated += activateSub;
            owner.IsEnabled = false;
            child.Owner = owner;
            child.Closed += (s, e) =>
            {
                owner.Activated -= activateSub;
                owner.IsEnabled = true;
                owner.Activate();
            };
            child.Show();
        }

        public static Rect GetRect(this Window window)
        {
            return new(window.Left, window.Top, window.Width, window.Height);
        }

        public static bool IsLogicalAncestorOf(this DependencyObject ancestor, object? descendant)
        {
            if (descendant is DependencyObject obj)
            {
                var o = obj;
                while (o is not null)
                {
                    if (o is Visual or Visual3D)
                    {
                        o = VisualTreeHelper.GetParent(obj);
                    }
                    else
                    {
                        o = null;
                    }
                    o ??= LogicalTreeHelper.GetParent(obj);
                    obj = o;
                    if (obj == ancestor)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool FindAncestor<T>(this DependencyObject obj, [NotNullWhen(true)] out T? ancestor)
            where T : DependencyObject
        {
            var o = obj;
            while (o is not null)
            {
                if (o is Visual or Visual3D)
                {
                    o = VisualTreeHelper.GetParent(obj);
                }
                else
                {
                    o = null;
                }
                o ??= LogicalTreeHelper.GetParent(obj);
                obj = o;
                if (obj is T target)
                {
                    ancestor = target;
                    return true;
                }
            }
            ancestor = null;
            return false;
        }

        public static bool FindAncestor(this DependencyObject obj, Predicate<DependencyObject> predicate, [MaybeNullWhen(false)] out DependencyObject ancestor)
        {
            var o = obj;
            while (o is not null)
            {
                if (o is Visual or Visual3D)
                {
                    o = VisualTreeHelper.GetParent(obj);
                }
                else
                {
                    o = null;
                }
                o ??= LogicalTreeHelper.GetParent(obj);
                obj = o;
                if (predicate(obj))
                {
                    ancestor = obj;
                    return true;
                }
            }
            ancestor = null;
            return false;
        }

        public static bool FindFirstDescendant<T>(this DependencyObject obj, [NotNullWhen(true)]out T? descendant)
            where T : DependencyObject
        {
            Queue<DependencyObject> que = new([obj]);
            while (que.Count > 0)
            {
                var o = que.Dequeue();
                var count = VisualTreeHelper.GetChildrenCount(o);
                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(o, i);
                    if (child is T target)
                    {
                        descendant = target;
                        return true;
                    }
                    que.Enqueue(child);
                }
            }
            descendant = null;
            return false;
        }

        public static bool FindFirstDescendant(this DependencyObject obj, Predicate<DependencyObject> predicate, [MaybeNullWhen(false)]out DependencyObject descendant)
        {
            Queue<DependencyObject> que = new([obj]);
            while (que.Count > 0)
            {
                var o = que.Dequeue();
                var count = VisualTreeHelper.GetChildrenCount(o);
                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(o, i);
                    if (predicate(child))
                    {
                        descendant = child;
                        return true;
                    }
                    que.Enqueue(child);
                }
            }
            descendant = null;
            return false;
        }

        public static bool FindFirstDescendant<T>(this DependencyObject obj, Predicate<T> predicate, [NotNullWhen(true)]out T? descendant)
            where T : DependencyObject
        {
            Queue<DependencyObject> que = new([obj]);
            while (que.Count > 0)
            {
                var o = que.Dequeue();
                var count = VisualTreeHelper.GetChildrenCount(o);
                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(o, i);
                    if (child is T c && predicate(c))
                    {
                        descendant = c;
                        return true;
                    }
                    que.Enqueue(child);
                }
            }
            descendant = null;
            return false;
        }

        public static void WaitForUpdate()
        {
            DispatcherFrame frame = new();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, default(object).WFU_Continue, frame);
            Dispatcher.PushFrame(frame);
        }

        private static object? WFU_Continue(this object? o, object obj)
        {
            (obj as DispatcherFrame)!.Continue = false;
            return null;
        }

        public static void WaitForUpdate(this DispatcherObject obj) => WaitForUpdate();
    }
}
