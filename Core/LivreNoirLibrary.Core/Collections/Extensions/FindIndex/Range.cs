using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> Range<T>(this ReadOnlySpan<T> span, Range<T> range)
            where T : struct, IComparable<T>
        {
            var (s, e) = GetRangeCore(span, range);
            return span[s..e];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> Range<T>(this Span<T> span, Range<T> range)
            where T : struct, IComparable<T>
        {
            var (s, e) = GetRangeCore(span, range);
            return span[s..e];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> Range<T>(this T[] array, Range<T> range)
            where T : struct, IComparable<T>
        {
            var (s, e) = GetRangeCore(array, range);
            return array.AsSpan()[s..e];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> Range<T>(this List<T> list, Range<T> range)
            where T : struct, IComparable<T>
        {
            var (s, e) = GetRangeCore(list, range);
            return CollectionsMarshal.AsSpan(list)[s..e];
        }

        public static IEnumerable<T> Range<T>(this IList<T> list, Range<T> range)
            where T : struct, IComparable<T>
        {
            var (s, e) = GetRangeCore(list, range);
            for (int i = s; i < e; i++)
            {
                yield return list[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ReadOnlySpan<T1> RangeCore<T1, T2, TComparer>(ReadOnlySpan<T1> span, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2>
        {
            var (s, e) = GetRangeCore(span, range, comparer);
            return span[s..e];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T1> Range<T1, T2, TComparer>(this ReadOnlySpan<T1> span, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2> => RangeCore(span, range, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T1> Range<T1, T2, TComparer>(this Span<T1> span, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2>
            => RangeCore<T1, T2, TComparer>(span, range, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T1> Range<T1, T2, TComparer>(this T1[] array, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2>
            => RangeCore<T1, T2, TComparer>(array, range, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T1> Range<T1, T2, TComparer>(this List<T1> list, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2>
            => RangeCore((ReadOnlySpan<T1>)CollectionsMarshal.AsSpan(list), range, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T1> Range<T1, T2, TComparer>(this IList<T1> list, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2>
        {
            var (s, e) = GetRangeCore(list, range, comparer);
            for (int i = s; i < e; i++)
            {
                yield return list[i];
            }
        }
    }
}
