using System;
using System.Collections;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static bool Swap<T>(this T[] array, int index1, int index2)
        {
            var len = (uint)array.Length;
            if ((uint)index1 < len && (uint)index2 < len)
            {
                (array[index1], array[index2]) = (array[index2], array[index1]);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Swap<T>(this Span<T> span, int index1, int index2)
        {
            var len = (uint)span.Length;
            if ((uint)index1 < len && (uint)index2 < len)
            {
                (span[index1], span[index2]) = (span[index2], span[index1]);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Swap(IList list, int index1, int index2)
        {
            var len = (uint)list.Count;
            if ((uint)index1 < len && (uint)index2 < len)
            {
                (list[index1], list[index2]) = (list[index2], list[index1]);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Swap<T>(this IList<T> list, int index1, int index2)
        {
            var len = (uint)list.Count;
            if ((uint)index1 < len && (uint)index2 < len)
            {
                (list[index1], list[index2]) = (list[index2], list[index1]);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Swap<T>(this T[] array, T item1, T item2) => Swap(array, Array.IndexOf(array, item1), Array.IndexOf(array, item2));
        public static bool Swap<T>(this Span<T> span, T item1, T item2) where T : IEquatable<T> => Swap(span, span.IndexOf(item1), span.IndexOf(item2));
        public static bool Swap(IList list, object item1, object item2) => Swap(list, list.IndexOf(item1), list.IndexOf(item2));
        public static bool Swap<T>(this IList<T> list, T item1, T item2) => Swap(list, list.IndexOf(item1), list.IndexOf(item2));
    }
}
