using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        /// <summary>
        /// Searches for an element that matches the given condition, and return the zero-based index of the <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <param name="span">The sorted <see cref="ReadOnlySpan{T}"/> to search.</param>
        /// <param name="value">The element search for.</param>
        /// <param name="type">Specifies how to search for the element.</param>
        /// <returns>The zero-based index of the element that matches the given condition, if found; otherwise, -1.</returns>
        public static int FindIndex<T>(this ReadOnlySpan<T> span, T value, SearchMode type)
            where T : IComparable<T>
            => TrySearch(span, value, type, out var index, out _) ? index : -1;

        /// <summary>
        /// Searches for an element that matches the given condition, and return the zero-based index of the <see cref="Span{T}"/>.
        /// </summary>
        /// <param name="span">The sorted <see cref="Span{T}"/> to search.</param>
        /// <param name="value">The element search for.</param>
        /// <param name="type">Specifies how to search for the element.</param>
        /// <returns>The zero-based index of the element that matches the given condition, if found; otherwise, -1.</returns>
        public static int FindIndex<T>(this Span<T> span, T value, SearchMode type)
            where T : IComparable<T>
            => TrySearch(span, value, type, out var index, out _) ? index : -1;

        /// <summary>
        /// Searches for an element that matches the given condition, and return the zero-based index of the <see cref="Array"/>.
        /// </summary>
        /// <param name="array">The sorted <see cref="Array"/> to search.</param>
        /// <param name="value">The element search for.</param>
        /// <param name="type">Specifies how to search for the element.</param>
        /// <returns>The zero-based index of the element that matches the given condition, if found; otherwise, -1.</returns>
        public static int FindIndex<T>(this T[] array, T value, SearchMode type)
            where T : IComparable<T>
            => TrySearch(array, value, type, out var index, out _) ? index : -1;

        /// <summary>
        /// Searches for an element that matches the given condition, and return the zero-based index of the <see cref="List{T}"/>.
        /// </summary>
        /// <param name="list">The sorted <see cref="List{T}"/> to search.</param>
        /// <param name="value">The element search for.</param>
        /// <param name="type">Specifies how to search for the element.</param>
        /// <returns>The zero-based index of the element that matches the given condition, if found; otherwise, -1.</returns>
        public static int FindIndex<T>(this List<T> list, T value, SearchMode type)
            where T : IComparable<T>
            => TrySearch(list, value, type, out var index, out _) ? index : -1;

        /// <summary>
        /// Searches for an element that matches the given condition, and return the zero-based index of the <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="list">The sorted <see cref="IList{T}"/> to search.</param>
        /// <param name="value">The element search for.</param>
        /// <param name="type">Specifies how to search for the element.</param>
        /// <returns>The zero-based index of the element that matches the given condition, if found; otherwise, -1.</returns>
        public static int FindIndex<T>(this IList<T> list, T value, SearchMode type)
            where T : IComparable<T>
            => TrySearch(list, value, type, out var index, out _) ? index : -1;

        /// <summary>
        /// Searches for an element that matches the given condition, and return the zero-based index of the <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <param name="span">The sorted <see cref="ReadOnlySpan{T}"/> to search.</param>
        /// <param name="value">The element search for.</param>
        /// <param name="comparer">The TComparer to use when comparing.</param>
        /// <param name="type">Specifies how to search for the element.</param>
        /// <returns>The zero-based index of the element that matches the given condition, if found; otherwise, -1.</returns>
        public static int FindIndex<T1, T2, TComparer>(this ReadOnlySpan<T1> span, T2 value, TComparer comparer, SearchMode type)
            where TComparer : IComparer<T1, T2>
            => TrySearch(span, value, comparer, type, out var index, out _) ? index : -1;

        /// <summary>
        /// Searches for an element that matches the given condition, and return the zero-based index of the <see cref="Span{T}"/>.
        /// </summary>
        /// <param name="span">The sorted <see cref="Span{T}"/> to search.</param>
        /// <param name="value">The element search for.</param>
        /// <param name="comparer">The TComparer to use when comparing.</param>
        /// <param name="type">Specifies how to search for the element.</param>
        /// <returns>The zero-based index of the element that matches the given condition, if found; otherwise, -1.</returns>
        public static int FindIndex<T1, T2, TComparer>(this Span<T1> span, T2 value, TComparer comparer, SearchMode type)
            where TComparer : IComparer<T1, T2>
            => TrySearch(span, value, comparer, type, out var index, out _) ? index : -1;

        /// <summary>
        /// Searches for an element that matches the given condition, and return the zero-based index of the <see cref="Array"/>.
        /// </summary>
        /// <param name="array">The sorted <see cref="Array"/> to search.</param>
        /// <param name="value">The element search for.</param>
        /// <param name="comparer">The TComparer to use when comparing.</param>
        /// <param name="type">Specifies how to search for the element.</param>
        /// <returns>The zero-based index of the element that matches the given condition, if found; otherwise, -1.</returns>
        public static int FindIndex<T1, T2, TComparer>(this T1[] array, T2 value, TComparer comparer, SearchMode type)
            where TComparer : IComparer<T1, T2>
            => TrySearch(array, value, comparer, type, out var index, out _) ? index : -1;

        /// <summary>
        /// Searches for an element that matches the given condition, and return the zero-based index of the <see cref="List{T}"/>.
        /// </summary>
        /// <param name="list">The sorted <see cref="List{T}"/> to search.</param>
        /// <param name="value">The element search for.</param>
        /// <param name="comparer">The TComparer to use when comparing.</param>
        /// <param name="type">Specifies how to search for the element.</param>
        /// <returns>The zero-based index of the element that matches the given condition, if found; otherwise, -1.</returns>
        public static int FindIndex<T1, T2, TComparer>(this List<T1> list, T2 value, TComparer comparer, SearchMode type)
            where TComparer : IComparer<T1, T2>
            => TrySearch(list, value, comparer, type, out var index, out _) ? index : -1;

        /// <summary>
        /// Searches for an element that matches the given condition, and return the zero-based index of the <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="list">The sorted <see cref="IList{T}"/> to search.</param>
        /// <param name="value">The element search for.</param>
        /// <param name="comparer">The TComparer to use when comparing.</param>
        /// <param name="type">Specifies how to search for the element.</param>
        /// <returns>The zero-based index of the element that matches the given condition, if found; otherwise, -1.</returns>
        public static int FindIndex<T1, T2, TComparer>(this IList<T1> list, T2 value, TComparer comparer, SearchMode type)
            where TComparer : IComparer<T1, T2>
            => TrySearch(list, value, comparer, type, out var index, out _) ? index : -1;
    }
}
