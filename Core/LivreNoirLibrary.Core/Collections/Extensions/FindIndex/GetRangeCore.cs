using System;
using System.Collections.Generic;
using System.Reflection;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        private static (int Start, int EndExclusive) GetRangeCore<T>(this IList<T> list, Range<T> range)
            where T : struct, IComparable<T>
        {
            var len = list.Count;
            int i1 = 0, i2 = len - 1;
            if (range.IsStartEnabled)
            {
                if (!TrySearch(list, range.Start, SearchMode.NextOrEqual, out i1, out _))
                {
                    i1 = int.MaxValue;
                }
            }
            if (range.IsEndEnabled)
            {
                if (!TrySearch(list, range.End, range.IncludesEnd ? SearchMode.PreviousOrEqual : SearchMode.Previous, out i2, out _))
                {
                    i2 = int.MinValue;
                }
            }
            if (i1 > i2)
            {
                return (0, 0);
            }
            return (i1, i2 + 1);
        }

        private static (int Start, int EndExclusive) GetRangeCore<T>(this List<T> list, Range<T> range)
            where T : struct, IComparable<T>
        {
            var len = list.Count;
            int i1 = 0, i2 = len - 1;
            if (range.IsStartEnabled)
            {
                if (!TrySearch(list, range.Start, SearchMode.NextOrEqual, out i1, out _))
                {
                    i1 = int.MaxValue;
                }
            }
            if (range.IsEndEnabled)
            {
                if (!TrySearch(list, range.End, range.IncludesEnd ? SearchMode.PreviousOrEqual : SearchMode.Previous, out i2, out _))
                {
                    i2 = int.MinValue;
                }
            }
            if (i1 > i2)
            {
                return (0, 0);
            }
            return (i1, i2 + 1);
        }

        private static (int Start, int EndExclusive) GetRangeCore<T>(this T[] array, Range<T> range)
            where T : struct, IComparable<T>
        {
            var len = array.Length;
            int i1 = 0, i2 = len - 1;
            if (range.IsStartEnabled)
            {
                if (!TrySearch(array, range.Start, SearchMode.NextOrEqual, out i1, out _))
                {
                    i1 = int.MaxValue;
                }
            }
            if (range.IsEndEnabled)
            {
                if (!TrySearch(array, range.End, range.IncludesEnd ? SearchMode.PreviousOrEqual : SearchMode.Previous, out i2, out _))
                {
                    i2 = int.MinValue;
                }
            }
            if (i1 > i2)
            {
                return (0, 0);
            }
            return (i1, i2 + 1);
        }

        private static (int Start, int EndExclusive) GetRangeCore<T>(this ReadOnlySpan<T> span, Range<T> range)
            where T : struct, IComparable<T>
        {
            var len = span.Length;
            int i1 = 0, i2 = len - 1;
            if (range.IsStartEnabled)
            {
                if (!TrySearch(span, range.Start, SearchMode.NextOrEqual, out i1, out _))
                {
                    i1 = int.MaxValue;
                }
            }
            if (range.IsEndEnabled)
            {
                if (!TrySearch(span, range.End, range.IncludesEnd ? SearchMode.PreviousOrEqual : SearchMode.Previous, out i2, out _))
                {
                    i2 = int.MinValue;
                }
            }
            if (i1 > i2)
            {
                return (0, 0);
            }
            return (i1, i2 + 1);
        }

        private static (int Start, int EndExclusive) GetRangeCore<T1, T2, TComparer>(this IList<T1> list, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2>
        {
            var len = list.Count;
            int i1 = 0, i2 = len - 1;
            if (range.IsStartEnabled)
            {
                if (!TrySearch(list, range.Start, comparer, SearchMode.NextOrEqual, out i1, out _))
                {
                    i1 = int.MaxValue;
                }
            }
            if (range.IsEndEnabled)
            {
                if (!TrySearch(list, range.End, comparer, range.IncludesEnd ? SearchMode.PreviousOrEqual : SearchMode.Previous, out i2, out _))
                {
                    i2 = int.MinValue;
                }
            }
            if (i1 > i2)
            {
                return (0, 0);
            }
            return (i1, i2 + 1);
        }

        private static (int Start, int EndExclusive) GetRangeCore<T1, T2, TComparer>(this ReadOnlySpan<T1> span, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2>
        {
            var len = span.Length;
            int i1 = 0, i2 = len - 1;
            if (range.IsStartEnabled)
            {
                if (!TrySearch(span, range.Start, comparer, SearchMode.NextOrEqual, out i1, out _))
                {
                    i1 = int.MaxValue;
                }
            }
            if (range.IsEndEnabled)
            {
                if (!TrySearch(span, range.End, comparer, range.IncludesEnd ? SearchMode.PreviousOrEqual : SearchMode.Previous, out i2, out _))
                {
                    i2 = int.MinValue;
                }
            }
            if (i1 > i2)
            {
                return (0, 0);
            }
            return (i1, i2 + 1);
        }
    }
}
