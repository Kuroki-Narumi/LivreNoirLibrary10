using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace LivreNoirLibrary.Windows
{
    public static partial class DependencyObjectExtensions
    {
        extension(Window)
        {
            public static bool TryGetWindow(object? obj, [NotNullWhen(true)] out Window? window)
            {
                if (obj is DependencyObject d)
                {
                    window = Window.GetWindow(d);
                    return window is not null;
                }
                else
                {
                    window = null;
                    return false;
                }
            }
        }

        public static DispatcherOperation SetDispatcher(this DispatcherObject obj, Action action, DispatcherPriority timing = DispatcherPriority.Loaded)
        {
            return obj.Dispatcher.BeginInvoke(action, timing);
        }

        public static DependencyObject? GetParent(this DependencyObject? obj)
        {
            if (obj is Visual or Visual3D)
            {
                return VisualTreeHelper.GetParent(obj) ?? LogicalTreeHelper.GetParent(obj);
            }
            else if (obj is not null)
            {
                return LogicalTreeHelper.GetParent(obj);
            }
            else
            {
                return null;
            }
        }

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

        public static bool TryGetSelfOrAncestor<T>(this DependencyObject? obj, [NotNullWhen(true)] out T? element)
        {
            if (obj is T target)
            {
                element = target;
                return true;
            }
            return TryGetAncestor(obj, out element);
        }

        public static bool TryGetAncestor(this DependencyObject? obj, Predicate<DependencyObject> predicate, [NotNullWhen(true)] out DependencyObject? ancestor)
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
            ancestor = default;
            return false;
        }

        public static bool TryGetAncestor<T>(this DependencyObject? obj, Predicate<T> predicate, [NotNullWhen(true)] out T? ancestor)
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
            ancestor = default;
            return false;
        }

        public static IEnumerable<DependencyObject> EnumerateAncestors(this DependencyObject? obj)
        {
            while (obj is not null)
            {
                yield return obj;
                obj = GetParent(obj);
            }
        }

        public static IEnumerable<DependencyObject> EnumerateDescendantsByQueue(this DependencyObject? obj)
        {
            if (obj is null)
            {
                yield break;
            }
            using var o1 = ObjectPool.RentQueue<DependencyObject>(out var queue);
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

        public static IEnumerable<DependencyObject> EnumerateDescendantsByStack(this DependencyObject? obj)
        {
            if (obj is null)
            {
                yield break;
            }
            using var o1 = ObjectPool.RentStack<DependencyObject>(out var stack);
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

        public static bool TryGetFirstDescendant(this DependencyObject? obj, Predicate<DependencyObject> predicate, [NotNullWhen(true)] out DependencyObject? descendant)
        {
            if (obj is not null)
            {
                foreach (var child in EnumerateDescendantsByQueue(obj))
                {
                    if (child is not null && predicate(child))
                    {
                        descendant = child;
                        return true;
                    }
                }
            }
            descendant = null;
            return false;
        }

        public static bool TryGetFirstDescendant<T>(this DependencyObject? obj, [NotNullWhen(true)] out T? descendant)
        {
            if (obj is not null)
            {
                foreach (var child in EnumerateDescendantsByQueue(obj))
                {
                    if (child is T target)
                    {
                        descendant = target;
                        return true;
                    }
                }
            }
            descendant = default;
            return false;
        }

        public static bool TryGetFirstDescendant<T>(this DependencyObject? obj, Predicate<T> predicate, [NotNullWhen(true)] out T? descendant)
        {
            if (obj is not null)
            {
                foreach (var child in EnumerateDescendantsByQueue(obj))
                {
                    if (child is T target && predicate(target))
                    {
                        descendant = target;
                        return true;
                    }
                }
            }
            descendant = default;
            return false;
        }

        extension(Dispatcher)
        {
            public static void WaitForUpdate()
            {
                DispatcherFrame frame = new();
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, (null as object).WFU_Continue, frame);
                Dispatcher.PushFrame(frame);
            }
        }

        private static object? WFU_Continue(this object? o, object obj)
        {
            (obj as DispatcherFrame)!.Continue = false;
            return null;
        }
    }
}
