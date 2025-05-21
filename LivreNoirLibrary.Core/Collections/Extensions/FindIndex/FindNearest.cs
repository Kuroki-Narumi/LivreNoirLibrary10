using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static bool FindNearest<T>(this IList<T> list, T value, out int index, [MaybeNullWhen(false)] out T actualValue)
            where T : INumber<T>
        {
            index = BinarySearch(list, value);
            if (index is >= 0)
            {
                actualValue = list[index];
                return true;
            }
            else
            {
                var upper = ~index;
                var lower = upper - 1;
                if (lower is >= 0)
                {
                    if (upper < list.Count)
                    {
                        var v1 = list[lower];
                        var v2 = list[upper];
                        if (value - v1 < v2 - value)
                        {
                            actualValue = v1;
                            index = lower;
                            return true;
                        }
                        else
                        {
                            actualValue = v2;
                            index = upper;
                            return true;
                        }
                    }
                    else
                    {
                        actualValue = list[lower];
                        index = lower;
                        return true;
                    }
                }
                else
                {
                    if (upper < list.Count)
                    {
                        actualValue = list[upper];
                        index = upper;
                        return true;
                    }
                    else
                    {
                        actualValue = default;
                        return false;
                    }
                }
            }
        }

        private static bool FindNearestCore<T>(ReadOnlySpan<T> span, T value, ref int invertedIndex, [MaybeNullWhen(false)]out T actualValue)
            where T : INumber<T>
        {
            var upper = ~invertedIndex;
            var lower = upper - 1;
            if (lower is >= 0)
            {
                if (upper < span.Length)
                {
                    var v1 = span[lower];
                    var v2 = span[upper];
                    if (value - v1 < v2 - value)
                    {
                        actualValue = v1;
                        invertedIndex = lower;
                        return true;
                    }
                    else
                    {
                        actualValue = v2;
                        invertedIndex = upper;
                        return true;
                    }
                }
                else
                {
                    actualValue = span[lower];
                    invertedIndex = lower;
                    return true;
                }
            }
            else
            {
                if (upper < span.Length)
                {
                    actualValue = span[upper];
                    invertedIndex = upper;
                    return true;
                }
                else
                {
                    actualValue = default;
                    return false;
                }
            }
        }

        public static bool FindNearest<T>(this List<T> list, T value, out int index, [MaybeNullWhen(false)] out T actualValue)
            where T : INumber<T>
        {
            index = list.BinarySearch(value);
            if (index is >= 0)
            {
                actualValue = list[index];
                return true;
            }
            else
            {
                return FindNearestCore(CollectionsMarshal.AsSpan(list), value, ref index, out actualValue);
            }
        }

        public static bool FindNearest<T>(this T[] ary, T value, out int index, [MaybeNullWhen(false)] out T actualValue)
            where T : INumber<T>
        {
            index = Array.BinarySearch(ary, value);
            if (index is >= 0)
            {
                actualValue = ary[index];
                return true;
            }
            else
            {
                return FindNearestCore(ary, value, ref index, out actualValue);
            }
        }

        public static bool FindNearest<T>(this Span<T> span, T value, out int index, [MaybeNullWhen(false)] out T actualValue)
            where T : INumber<T>
            => FindNearest((ReadOnlySpan<T>)span, value, out index, out actualValue);

        public static bool FindNearest<T>(this ReadOnlySpan<T> span, T value, out int index, [MaybeNullWhen(false)] out T actualValue)
            where T : INumber<T>
        {
            index = MemoryExtensions.BinarySearch(span, value);
            if (index is >= 0)
            {
                actualValue = span[index];
                return true;
            }
            else
            {
                return FindNearestCore(span, value, ref index, out actualValue);
            }
        }

        public static bool FindNearest<T1, T2, TComparer>(this IList<T1> list, T2 value, TComparer comparer, out int index, [MaybeNullWhen(false)] out T1 actualValue)
            where TComparer : IComparer<T1, T2>
        {
            index = BinarySearch(list, value, comparer);
            if (index is >= 0)
            {
                actualValue = list[index];
                return true;
            }
            else
            {
                var upper = ~index;
                var lower = upper - 1;
                if (lower is >= 0)
                {
                    if (upper < list.Count)
                    {
                        var v1 = list[lower];
                        var v2 = list[upper];
                        if (comparer.IsCloser(v1, v2, value))
                        {
                            actualValue = v1;
                            index = lower;
                            return true;
                        }
                        else
                        {
                            actualValue = v2;
                            index = upper;
                            return true;
                        }
                    }
                    else
                    {
                        actualValue = list[lower];
                        index = lower;
                        return true;
                    }
                }
                else
                {
                    if (upper < list.Count)
                    {
                        actualValue = list[upper];
                        index = upper;
                        return true;
                    }
                    else
                    {
                        actualValue = default;
                        return false;
                    }
                }
            }
        }

        public static bool FindNearest<T1, T2, TComparer>(this List<T1> list, T2 value, TComparer comparer, out int index, [MaybeNullWhen(false)] out T1 actualValue)
            where TComparer : IComparer<T1, T2> => FindNearestCore(CollectionsMarshal.AsSpan(list), value, comparer, out index, out actualValue);

        public static bool FindNearest<T1, T2, TComparer>(this T1[] ary, T2 value, TComparer comparer, out int index, [MaybeNullWhen(false)] out T1 actualValue)
            where TComparer : IComparer<T1, T2> => FindNearestCore(ary, value, comparer, out index, out actualValue);

        public static bool FindNearest<T1, T2, TComparer>(this Span<T1> span, T2 value, TComparer comparer, out int index, [MaybeNullWhen(false)] out T1 actualValue)
            where TComparer : IComparer<T1, T2> => FindNearestCore(span, value, comparer, out index, out actualValue);

        public static bool FindNearest<T1, T2, TComparer>(this ReadOnlySpan<T1> span, T2 value, TComparer comparer, out int index, [MaybeNullWhen(false)] out T1 actualValue)
            where TComparer : IComparer<T1, T2> => FindNearestCore(span, value, comparer, out index, out actualValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool FindNearestCore<T1, T2, TComparer>(this ReadOnlySpan<T1> span, T2 value, TComparer comparer, out int index, [MaybeNullWhen(false)] out T1 actualValue)
            where TComparer : IComparer<T1, T2>
        {
            index = BinarySearch(span, value, comparer);
            if (index is >= 0)
            {
                actualValue = span[index];
                return true;
            }
            else
            {
                var upper = ~index;
                var lower = upper - 1;
                if (lower is >= 0)
                {
                    if (upper < span.Length)
                    {
                        var v1 = span[lower];
                        var v2 = span[upper];
                        if (comparer.IsCloser(v1, v2, value))
                        {
                            actualValue = v1;
                            index = lower;
                            return true;
                        }
                        else
                        {
                            actualValue = v2;
                            index = upper;
                            return true;
                        }
                    }
                    else
                    {
                        actualValue = span[lower];
                        index = lower;
                        return true;
                    }
                }
                else
                {
                    if (upper < span.Length)
                    {
                        actualValue = span[upper];
                        index = upper;
                        return true;
                    }
                    else
                    {
                        actualValue = default;
                        return false;
                    }
                }
            }
        }
    }
}
