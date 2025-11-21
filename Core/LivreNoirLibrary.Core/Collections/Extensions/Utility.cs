using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        /// <inheritdoc cref="CollectionsMarshal.AsSpan{T}(List{T}?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this List<T>? list) => CollectionsMarshal.AsSpan(list);

        /// <inheritdoc cref="CollectionsMarshal.AsSpan{T}(List{T}?)"/>
        /// <param name="start">The zero-based index at which to begin this slice.</param>
        /// <param name="length">The desired length for the slice (exclusive).</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this List<T> list, int start = 0, int length = -1)
            => list.AsSpan().Slice(start, length is < 0 ? list.Count - start : length);

        /// <inheritdoc cref="CollectionsMarshal.AsSpan{T}(List{T}?)"/>
        /// <param name="range">The range for the slice.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this List<T> list, Range range)
        {
            var (start, length) = range.GetOffsetAndLength(list.Count);
            return list.AsSpan().Slice(start, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Pop<T>(this IList<T> list)
        {
            var item = list[^1];
            list.RemoveAt(list.Count - 1);
            return item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? PopOrDefault<T>(this IList<T> list) => list.Count is > 0 ? Pop(list) : default;
    }
}
