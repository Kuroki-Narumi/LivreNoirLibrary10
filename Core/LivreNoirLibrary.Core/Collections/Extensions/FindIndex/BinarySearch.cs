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
                switch (list[i].CompareTo(item))
                {
                    case 0:
                        return i;
                    case < 0:
                        min = i + 1;
                        break;
                    default:
                        max = i - 1;
                        break;
                }
            }
            return ~min;
        }

        public static int BinarySearch<T1, T2, TComparer>(this IList<T1> list, T2 item)
            where TComparer : IComparer<T1, T2>
        {
            var min = 0;
            var max = list.Count - 1;
            while (max >= min)
            {
                var i = min + (max - min) / 2;
                var p = list[i];
                switch (TComparer.Compare(list[i], item))
                {
                    case 0:
                        return i;
                    case < 0:
                        min = i + 1;
                        break;
                    default:
                        max = i - 1;
                        break;
                }
            }
            return ~min;
        }

        public static int BinarySearch<T1, T2, TComparer>(this ReadOnlySpan<T1> span, T2 item)
            where TComparer : IComparer<T1, T2>
        {
            var min = 0;
            var max = span.Length - 1;
            while (max >= min)
            {
                var i = min + (max - min) / 2;
                switch (TComparer.Compare(span[i], item))
                {
                    case 0:
                        return i;
                    case < 0:
                        min = i + 1;
                        break;
                    default:
                        max = i - 1;
                        break;
                }
            }
            return ~min;
        }

        public static unsafe int BinarySearch<T>(T* pointer, int length, T item)
            where T : unmanaged, IComparable<T>
        {
            var min = 0;
            var max = length - 1;
            while (max >= min)
            {
                var i = min + (max - min) / 2;
                switch (pointer[i].CompareTo(item))
                {
                    case 0:
                        return i;
                    case < 0:
                        min = i + 1;
                        break;
                    default:
                        max = i - 1;
                        break;
                }
            }
            return ~min;
        }
    }
}