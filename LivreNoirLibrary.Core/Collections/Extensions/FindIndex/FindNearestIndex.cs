using System;
using System.Collections.Generic;
using System.Numerics;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static int FindNearestIndex<T>(this Span<T> span, T value)
            where T : INumber<T> => FindNearest(span, value, out var index, out _) ? index : -1;

        public static int FindNearestIndex<T>(this ReadOnlySpan<T> span, T value)
            where T : INumber<T> => FindNearest(span, value, out var index, out _) ? index : -1;

        public static int FindNearestIndex<T>(this T[] array, T value)
            where T : INumber<T> => FindNearest(array, value, out var index, out _) ? index : -1;

        public static int FindNearestIndex<T>(this List<T> list, T value)
            where T : INumber<T> => FindNearest(list, value, out var index, out _) ? index : -1;

        public static int FindNearestIndex<T>(this IList<T> list, T value)
            where T : INumber<T> => FindNearest(list, value, out var index, out _) ? index : -1;

        public static int FindNearestIndex<T1, T2, TComparer>(this ReadOnlySpan<T1> span, T2 value, TComparer comparer)
            where TComparer : IComparer<T1, T2> => FindNearest(span, value, comparer, out var index, out _) ? index : -1;

        public static int FindNearestIndex<T1, T2, TComparer>(this Span<T1> span, T2 value, TComparer comparer)
            where TComparer : IComparer<T1, T2> => FindNearest(span, value, comparer, out var index, out _) ? index : -1;

        public static int FindNearestIndex<T1, T2, TComparer>(this T1[] array, T2 value, TComparer comparer)
            where TComparer : IComparer<T1, T2> => FindNearest(array, value, comparer, out var index, out _) ? index : -1;

        public static int FindNearestIndex<T1, T2, TComparer>(this List<T1> list, T2 value, TComparer comparer)
            where TComparer : IComparer<T1, T2> => FindNearest(list, value, comparer, out var index, out _) ? index : -1;

        public static int FindNearestIndex<T1, T2, TComparer>(this IList<T1> list, T2 value, TComparer comparer)
            where TComparer : IComparer<T1, T2> => FindNearest(list, value, comparer, out var index, out _) ? index : -1;
    }
}
