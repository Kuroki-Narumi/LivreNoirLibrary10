using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int BinarySearch<T>(this IList<T> list, T item)
            where T : IComparable<T>
        {
            var min = 0;
            var max = list.Count - 1;
            while (max >= min)
            {
                var i = min + (max - min) / 2;
                switch (item.CompareTo(list[i]))
                {
                    case 0:
                        return i;
                    case < 0:
                        max = i - 1;
                        break;
                    default:
                        min = i + 1;
                        break;
                }
            }
            return ~min;
        }

        public static int BinarySearch<T1, T2, TComparer>(this IList<T1> list, T2 item, TComparer comparer)
            where TComparer : IComparer<T1, T2>
        {
            var min = 0;
            var max = list.Count - 1;
            while (max >= min)
            {
                var i = min + (max - min) / 2;
                var p = list[i];
                if (comparer.Equals(p, item))
                {
                    return i;
                }
                else if (comparer.LessThan(p, item))
                {
                    min = i + 1;
                }
                else
                {
                    max = i - 1;
                }
            }
            return ~min;
        }

        public static int BinarySearch<T1, T2, TComparer>(this Span<T1> span, T2 item, TComparer comparer)
            where TComparer : IComparer<T1, T2> => BinarySearch((ReadOnlySpan<T1>)span, item, comparer);

        public static int BinarySearch<T1, T2, TComparer>(this ReadOnlySpan<T1> span, T2 item, TComparer comparer)
            where TComparer : IComparer<T1, T2>
        {
            var min = 0;
            var max = span.Length - 1;
            while (max >= min)
            {
                var i = min + (max - min) / 2;
                var p = span[i];
                if (comparer.Equals(p, item))
                {
                    return i;
                }
                else if (comparer.LessThan(p, item))
                {
                    min = i + 1;
                }
                else
                {
                    max = i - 1;
                }
            }
            return ~min;
        }
    }
}