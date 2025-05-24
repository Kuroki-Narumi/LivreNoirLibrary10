using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static bool CanMoveDown<T>(this T[] array, int index) => (uint)index < (uint)(array.Length - 1);
        public static bool CanMoveUp<T>(this T[] array, int index) => (uint)(index - 1) < (uint)(array.Length - 1);

        public static bool CanMoveDown<T>(this Span<T> span, int index) => (uint)index < (uint)(span.Length - 1);
        public static bool CanMoveUp<T>(this Span<T> span, int index) => (uint)(index - 1) < (uint)(span.Length - 1);

        public static bool CanMoveDown(IList list, int index) => (uint)index < (uint)(list.Count - 1);
        public static bool CanMoveUp(IList list, int index) => (uint)(index - 1) < (uint)(list.Count - 1);

        public static bool CanMoveDown<T>(this IList<T> list, int index) => (uint)index < (uint)(list.Count - 1);
        public static bool CanMoveUp<T>(this IList<T> list, int index) => (uint)(index - 1) < (uint)(list.Count - 1);

        public static bool MoveDown<T>(this T[] array, int index) => CanMoveDown(array, index) && Swap(array, index, index + 1);
        public static bool MoveDown<T>(this T[] array, T item) => MoveDown(array, Array.IndexOf(array, item));
        public static bool MoveUp<T>(this T[] array, int index) => CanMoveUp(array, index) && Swap(array, index, index - 1);
        public static bool MoveUp<T>(this T[] array, T item) => MoveUp(array, Array.IndexOf(array, item));

        public static bool MoveDown<T>(this Span<T> span, int index) => CanMoveDown(span, index) && Swap(span, index, index + 1);
        public static bool MoveDown<T>(this Span<T> span, T item) where T : IEquatable<T> => MoveDown(span, span.IndexOf(item));
        public static bool MoveUp<T>(this Span<T> span, int index) => CanMoveUp(span, index) && Swap(span, index, index - 1);
        public static bool MoveUp<T>(this Span<T> span, T item) where T : IEquatable<T> => MoveUp(span, span.IndexOf(item));

        public static bool MoveDown(IList list, int index) => CanMoveDown(list, index) && Swap(list, index, index + 1);
        public static bool MoveDown(IList list, object item) => MoveDown(list, list.IndexOf(item));
        public static bool MoveUp(IList list, int index) => CanMoveUp(list, index) && Swap(list, index, index - 1);
        public static bool MoveUp(IList list, object item) => MoveUp(list, list.IndexOf(item));

        public static bool MoveDown<T>(this IList<T> list, int index) => CanMoveDown(list, index) && Swap(list, index, index + 1);
        public static bool MoveDown<T>(this IList<T> list, T item) => MoveDown(list, list.IndexOf(item));
        public static bool MoveUp<T>(this IList<T> list, int index) => CanMoveUp(list, index) && Swap(list, index, index - 1);
        public static bool MoveUp<T>(this IList<T> list, T item) => MoveUp(list, list.IndexOf(item));
    }
}
