using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace LivreNoirLibrary.Windows
{
    public static partial class DependencyObjectExtensions
    {
        public static void SetDispatcher(this DispatcherObject obj, Action action, DispatcherPriority timing = DispatcherPriority.Loaded)
        {
            obj.Dispatcher.BeginInvoke(action, timing);
        }

        public static DependencyObject? GetParent(this DependencyObject obj) => (obj is Visual or Visual3D ? VisualTreeHelper.GetParent(obj) : null) ?? LogicalTreeHelper.GetParent(obj);

        public static bool IsAncestorOf(this DependencyObject? ancestor, object? descendant)
        {
            for (var obj = descendant as DependencyObject; obj is not null;)
            {
                var parent = GetParent(obj);
                if (parent == ancestor)
                {
                    return true;
                }
                obj = parent;
            }
            return false;
        }

        public static bool TryGetAncestor(this DependencyObject? obj, Predicate<DependencyObject> predicate, [MaybeNullWhen(false)] out DependencyObject ancestor)
        {
            while (obj is not null)
            {
                var parent = GetParent(obj);
                if (parent is not null && predicate(parent))
                {
                    ancestor = parent;
                    return true;
                }
                obj = parent;
            }
            ancestor = null;
            return false;
        }

        public static bool TryGetAncestor<T>(this DependencyObject? obj, [NotNullWhen(true)] out T? ancestor)
            where T : DependencyObject
        {
            while (obj is not null)
            {
                var parent = GetParent(obj);
                if (parent is T target)
                {
                    ancestor = target;
                    return true;
                }
                obj = parent;
            }
            ancestor = null;
            return false;
        }

        public static bool TryGetAncestor<T>(this DependencyObject? obj, Predicate<T> predicate, [MaybeNullWhen(false)] out T ancestor)
            where T : DependencyObject
        {
            while (obj is not null)
            {
                var parent = GetParent(obj);
                if (parent is T target && predicate(target))
                {
                    ancestor = target;
                    return true;
                }
                obj = parent;
            }
            ancestor = null;
            return false;
        }

        public static IEnumerable<DependencyObject> EnumerateDescendantsByQueue(this DependencyObject obj)
        {
            var queue = ObjectPool.Rent<Queue<DependencyObject>>();
            try
            {
                queue.Enqueue(obj);
                while (queue.TryDequeue(out var o))
                {
                    var count = VisualTreeHelper.GetChildrenCount(o);
                    for (var i = 0; i < count; i++)
                    {
                        var child = VisualTreeHelper.GetChild(o, i);
                        if (child is not null)
                        {
                            yield return child;
                            queue.Enqueue(child);
                        }
                    }
                }
            }
            finally
            {
                ObjectPool.Return(queue);
            }
        }

        public static IEnumerable<DependencyObject> EnumerateDescendantsByStack(this DependencyObject obj)
        {
            var stack = ObjectPool.Rent<Stack<DependencyObject>>();
            try
            {
                stack.Push(obj);
                while (stack.TryPop(out var o))
                {
                    var count = VisualTreeHelper.GetChildrenCount(o);
                    for (var i = 0; i < count; i++)
                    {
                        var child = VisualTreeHelper.GetChild(o, i);
                        if (child is not null)
                        {
                            yield return child;
                            stack.Push(child);
                        }
                    }
                }
            }
            finally
            {
                ObjectPool.Return(stack);
            }
        }

        public static bool TryGetFirstDescendant(this DependencyObject obj, Predicate<DependencyObject> predicate, [NotNullWhen(true)] out DependencyObject? descendant)
        {
            foreach (var child in EnumerateDescendantsByQueue(obj))
            {
                if (child is not null && predicate(child))
                {
                    descendant = child;
                    return true;
                }
            }
            descendant = null;
            return false;
        }

        public static bool TryGetFirstDescendant<T>(this DependencyObject obj, [NotNullWhen(true)] out T? descendant)
            where T : DependencyObject
        {
            foreach (var child in EnumerateDescendantsByQueue(obj))
            {
                if (child is T target)
                {
                    descendant = target;
                    return true;
                }
            }
            descendant = null;
            return false;
        }

        public static bool TryGetFirstDescendant<T>(this DependencyObject obj, Predicate<T> predicate, [NotNullWhen(true)] out T? descendant)
            where T : DependencyObject
        {
            foreach (var child in EnumerateDescendantsByQueue(obj))
            {
                if (child is T target && predicate(target))
                {
                    descendant = target;
                    return true;
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
