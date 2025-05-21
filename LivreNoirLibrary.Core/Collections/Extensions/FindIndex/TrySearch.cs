using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        private static int TrySearchCore_GetIndex(int index, int max, SearchMode mode)
        {
            if (index is >= 0)
            {
                switch (mode)
                {
                    case SearchMode.Equal or SearchMode.PreviousOrEqual or SearchMode.NextOrEqual:
                        return index;
                    case SearchMode.Previous:
                        return index - 1;
                    case SearchMode.Next:
                        index++;
                        return index < max ? index : -1;
                }
            }
            else
            {
                index = ~index;
                switch (mode)
                {
                    case SearchMode.Previous or SearchMode.PreviousOrEqual:
                        return index - 1;
                    case SearchMode.Next or SearchMode.NextOrEqual:
                        return index < max ? index : -1;
                }
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ReturnCore<T>(IList<T> list, int index, [MaybeNullWhen(false)] out T actualValue)
        {
            if (index is >= 0)
            {
                actualValue = list[index];
                return true;
            }
            else
            {
                actualValue = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ReturnCore<T>(List<T> list, int index, [MaybeNullWhen(false)] out T actualValue)
        {
            if (index is >= 0)
            {
                actualValue = list[index];
                return true;
            }
            else
            {
                actualValue = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ReturnCore<T>(T[] array, int index, [MaybeNullWhen(false)] out T actualValue)
        {
            if (index is >= 0)
            {
                actualValue = array[index];
                return true;
            }
            else
            {
                actualValue = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ReturnCore<T>(ReadOnlySpan<T> span, int index, [MaybeNullWhen(false)] out T actualValue)
        {
            if (index is >= 0)
            {
                actualValue = span[index];
                return true;
            }
            else
            {
                actualValue = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySearch<T>(this IList<T> list, T value, SearchMode mode, out int index, [MaybeNullWhen(false)] out T actualValue)
            where T : IComparable<T>
        {
            index = TrySearchCore_GetIndex(BinarySearch(list, value), list.Count, mode);
            return ReturnCore(list, index, out actualValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySearch<T>(this List<T> list, T value, SearchMode mode, out int index, [MaybeNullWhen(false)] out T actualValue)
            where T : IComparable<T>
        {
            index = TrySearchCore_GetIndex(list.BinarySearch(value), list.Count, mode);
            return ReturnCore(list, index, out actualValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySearch<T>(this T[] array, T value, SearchMode mode, out int index, [MaybeNullWhen(false)] out T actualValue)
            where T : IComparable<T>
        {
            index = TrySearchCore_GetIndex(Array.BinarySearch(array, value), array.Length, mode);
            return ReturnCore(array, index, out actualValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySearch<T>(this Span<T> span, T value, SearchMode mode, out int index, [MaybeNullWhen(false)] out T actualValue)
            where T : IComparable<T> => TrySearch((ReadOnlySpan<T>)span, value, mode, out index, out actualValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySearch<T>(this ReadOnlySpan<T> span, T value, SearchMode mode, out int index, [MaybeNullWhen(false)] out T actualValue)
            where T : IComparable<T>
        {
            index = TrySearchCore_GetIndex(MemoryExtensions.BinarySearch(span, value), span.Length, mode);
            return ReturnCore(span, index, out actualValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySearch<T1, T2, TComparer>(this IList<T1> list, T2 value, TComparer comparer, SearchMode mode, out int index, [MaybeNullWhen(false)] out T1 actualValue)
            where TComparer : IComparer<T1, T2>
        {
            index = TrySearchCore_GetIndex(BinarySearch(list, value, comparer), list.Count, mode);
            return ReturnCore(list, index, out actualValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySearch<T1, T2, TComparer>(this List<T1> list, T2 value, TComparer comparer, SearchMode mode, out int index, [MaybeNullWhen(false)] out T1 actualValue)
            where TComparer : IComparer<T1, T2>
        {
            index = TrySearchCore_GetIndex(BinarySearch(CollectionsMarshal.AsSpan(list), value, comparer), list.Count, mode);
            return ReturnCore(list, index, out actualValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySearch<T1, T2, TComparer>(this T1[] array, T2 value, TComparer comparer, SearchMode mode, out int index, [MaybeNullWhen(false)] out T1 actualValue)
            where TComparer : IComparer<T1, T2>
        {
            index = TrySearchCore_GetIndex(BinarySearch((ReadOnlySpan<T1>)array, value, comparer), array.Length, mode);
            return ReturnCore(array, index, out actualValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySearch<T1, T2, TComparer>(this Span<T1> span, T2 value, TComparer comparer, SearchMode mode, out int index, [MaybeNullWhen(false)] out T1 actualValue)
            where TComparer : IComparer<T1, T2>
            => TrySearch((ReadOnlySpan<T1>)span, value, comparer, mode, out index, out actualValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySearch<T1, T2, TComparer>(this ReadOnlySpan<T1> span, T2 value, TComparer comparer, SearchMode mode, out int index, [MaybeNullWhen(false)] out T1 actualValue)
            where TComparer : IComparer<T1, T2>
        {
            index = TrySearchCore_GetIndex(BinarySearch(span, value, comparer), span.Length, mode);
            return ReturnCore(span, index, out actualValue);
        }
    }
}
