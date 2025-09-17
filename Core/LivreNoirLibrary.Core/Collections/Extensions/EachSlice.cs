using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    using SliceEnumer = IEnumerable<(int Index, int Count)>;

    public static partial class CollectionExtensions
    {
        public static SliceEnumer EachSlice(int n, int start, int count)
        {
            if (n is <= 0) { yield break; }
            var end = count / n * n;
            count -= end;
            for (var i = 0; i < end; i += n)
            {
                yield return (start + i, n);
            }
            if (count is > 0)
            {
                yield return (start + end, count);
            }
        }

        public static SliceEnumer EachCons(int n, int start, int count)
        {
            if (n is <= 0) { yield break; }
            if (count >= n)
            {
                count -= n;
                for (var i = 0; i <= count; i++)
                {
                    yield return (start + i, n);
                }
            }
            else
            {
                yield return (start, count);
            }
        }

        public static SliceEnumer EachGroup(int n, int start, int count) => n is > 0 ? EachSlice((count + n - 1) / n, start, count) : [];

        public static SliceEnumer EachSlice<T>(this T[] array, int n, int start = 0, int count = 0)
        {
            SimdOperations.AdjustArgs(array.Length, ref start, ref count);
            return EachSlice(n, start, count);
        }

        public static SliceEnumer EachCons<T>(this T[] array, int n, int start = 0, int count = 0)
        {
            SimdOperations.AdjustArgs(array.Length, ref start, ref count);
            return EachCons(n, start, count);
        }

        public static SliceEnumer EachGroup<T>(this T[] array, int n, int start = 0, int count = 0)
        {
            SimdOperations.AdjustArgs(array.Length, ref start, ref count);
            return EachGroup(n, start, count);
        }

        public static SliceEnumer EachSlice<T>(this ICollection<T> collection, int n, int start = 0, int count = 0)
        {
            SimdOperations.AdjustArgs(collection.Count, ref start, ref count);
            return EachSlice(n, start, count);
        }

        public static SliceEnumer EachCons<T>(this ICollection<T> collection, int n, int start = 0, int count = 0)
        {
            SimdOperations.AdjustArgs(collection.Count, ref start, ref count);
            return EachCons(n, start, count);
        }

        public static SliceEnumer EachGroup<T>(this ICollection<T> collection, int n, int start = 0, int count = 0)
        {
            SimdOperations.AdjustArgs(collection.Count, ref start, ref count);
            return EachGroup(n, start, count);
        }

        public static SliceEnumer EachSlice<T>(this Span<T> span, int n, int start = 0, int count = 0)
        {
            SimdOperations.AdjustArgs(span.Length, ref start, ref count);
            return EachSlice(n, start, count);
        }

        public static SliceEnumer EachCons<T>(this Span<T> span, int n, int start = 0, int count = 0)
        {
            SimdOperations.AdjustArgs(span.Length, ref start, ref count);
            return EachCons(n, start, count);
        }

        public static SliceEnumer EachGroup<T>(this Span<T> span, int n, int start = 0, int count = 0)
        {
            SimdOperations.AdjustArgs(span.Length, ref start, ref count);
            return EachGroup(n, start, count);
        }

        public static SliceEnumer EachSlice<T>(this ReadOnlySpan<T> span, int n, int start = 0, int count = 0)
        {
            SimdOperations.AdjustArgs(span.Length, ref start, ref count);
            return EachSlice(n, start, count);
        }

        public static SliceEnumer EachCons<T>(this ReadOnlySpan<T> span, int n, int start = 0, int count = 0)
        {
            SimdOperations.AdjustArgs(span.Length, ref start, ref count);
            return EachCons(n, start, count);
        }

        public static SliceEnumer EachGroup<T>(this ReadOnlySpan<T> span, int n, int start = 0, int count = 0)
        {
            SimdOperations.AdjustArgs(span.Length, ref start, ref count);
            return EachGroup(n, start, count);
        }
    }
}
