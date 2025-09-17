using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Start, int Length) IndexRange<T>(this ReadOnlySpan<T> span, Range<T> range)
            where T : struct, IComparable<T>
        {
            var (s, e) = GetRangeCore(span, range);
            return (s, e - s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Start, int Length) IndexRange<T>(this Span<T> span, Range<T> range)
            where T : struct, IComparable<T>
        {
            var (s, e) = GetRangeCore(span, range);
            return (s, e - s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Start, int Length) IndexRange<T>(this T[] array, Range<T> range)
            where T : struct, IComparable<T>
        {
            var (s, e) = GetRangeCore(array, range);
            return (s, e - s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Start, int Length) IndexRange<T>(this List<T> list, Range<T> range)
            where T : struct, IComparable<T>
        {
            var (s, e) = GetRangeCore(list, range);
            return (s, e - s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Start, int Length) IndexRange<T>(this IList<T> list, Range<T> range)
            where T : struct, IComparable<T>
        {
            var (s, e) = GetRangeCore(list, range);
            return (s, e - s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (int Start, int Length) IndexRangeCore<T1, T2, TComparer>(ReadOnlySpan<T1> span, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2>
        {
            var (s, e) = GetRangeCore(span, range, comparer);
            return (s, e - s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Start, int Length) IndexRange<T1, T2, TComparer>(this ReadOnlySpan<T1> span, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2>
            => IndexRangeCore(span, range, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Start, int Length) IndexRange<T1, T2, TComparer>(this Span<T1> span, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2>
            => IndexRangeCore<T1, T2, TComparer>(span, range, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Start, int Length) IndexRange<T1, T2, TComparer>(this T1[] array, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2>
            => IndexRangeCore<T1, T2, TComparer>(array, range, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Start, int Length) IndexRange<T1, T2, TComparer>(this List<T1> list, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2>
            => IndexRangeCore((ReadOnlySpan<T1>)CollectionsMarshal.AsSpan(list), range, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Start, int Length) IndexRange<T1, T2, TComparer>(this IList<T1> list, Range<T2> range, TComparer comparer)
            where T2 : struct
            where TComparer : IComparer<T1, T2>
        {
            var (s, e) = GetRangeCore(list, range, comparer);
            return (s, e - s);
        }
    }
}
