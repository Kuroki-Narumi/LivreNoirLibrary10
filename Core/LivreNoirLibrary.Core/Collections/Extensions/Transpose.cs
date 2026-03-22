using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        private static void TransposeImpl<T>(ReadOnlySpan<T> source, Span<T> destination, int columns)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(columns, 0);
            var rows = source.Length / columns;
            if (destination.Length < rows * columns)
            {
                throw new ArgumentException($"The length of the destination span must be at least {rows * columns}.", nameof(destination));
            }
            var index = 0;
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    destination[i + j * rows] = source[index];
                    index++;
                }
            }
        }

        /// <summary>
        /// Transposes the elements of a collection into a destination span according to the specified number of columns.
        /// </summary>
        /// <remarks>
        /// If the length of <paramref name="source"/> is not evenly divisible by the specified number of <paramref name="columns"/>, the remainder is ignored. <br/>
        /// The caller is responsible for ensuring that <paramref name="destination"/> is large enough to hold all transposed elements.
        /// </remarks>
        /// <typeparam name="T">The type of the elements contained in <paramref name="source"/> and <paramref name="destination"/>.</typeparam>
        /// <param name="source">The collection containing the elements to transpose.</param>
        /// <param name="destination">The span that receives the transposed elements. Must have sufficient capacity to hold all transposed values.</param>
        /// <param name="columns">The number of columns to use when transposing the source span. Must be greater than zero.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the number of columns is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="destination"/> is not large enough to hold all transposed elements.</exception>
        public static void Transpose<T>(this ReadOnlySpan<T> source, Span<T> destination, int columns) => TransposeImpl(source, destination, columns);

        /// <inheritdoc cref="Transpose{T}(ReadOnlySpan{T}, Span{T}, int)"/>
        public static void Transpose<T>(this ReadOnlyMemory<T> source, Span<T> destination, int columns) => TransposeImpl(source.Span, destination, columns);

        /// <inheritdoc cref="Transpose{T}(ReadOnlySpan{T}, Span{T}, int)"/>
        public static void Transpose<T>(this T[] source, Span<T> destination, int columns) => TransposeImpl(source, destination, columns);

        /// <inheritdoc cref="Transpose{T}(ReadOnlySpan{T}, Span{T}, int)"/>
        public static void Transpose<T>(this List<T> source, Span<T> destination, int columns) => TransposeImpl(source.AsSpan(), destination, columns);

        /// <inheritdoc cref="Transpose{T}(ReadOnlySpan{T}, Span{T}, int)"/>
        public static void Transpose<T>(this ObservableCollectionBase<T> source, Span<T> destination, int columns) => TransposeImpl(source.AsSpan(), destination, columns);
    }
}
