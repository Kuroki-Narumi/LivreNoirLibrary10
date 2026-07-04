using LivreNoirLibrary.Collections;
using System;

namespace LivreNoirLibrary.Text
{
    public interface IStringConverter
    {
        /// <summary>
        /// Gets the maximum number of characters that could be produced from the given <paramref name="span"/>.
        /// </summary>
        /// <param name="span">A span of <see cref="char"/> to convert.</param>
        /// <returns>The maximum number of characters.</returns>
        int GetMaxCharCount(ReadOnlySpan<char> span);

        /// <summary>
        /// Tries to read the character and set to <paramref name="c"/> from the given <paramref name="span"/> at <paramref name="spanIndex"/>.
        /// </summary>
        /// <remarks>
        /// This method typically simply returns the value of <paramref name="span"/>[<paramref name="spanIndex"/>] without modifying <paramref name="spanIndex"/>.
        /// <paramref name="spanIndex"/> advances by that amount only when it is necessary to skip elements. <br/>
        /// The caller must ensure that <paramref name="spanIndex"/> falls within the <paramref name="span"/>'s range.
        /// </remarks>
        /// <param name="span">A span of <see cref="char"/> to read.</param>
        /// <param name="spanIndex">The index of the character to read.</param>
        /// <param name="c">The character that was read, if successful.</param>
        /// <returns><see langword="true"/> if a character was read; otherwise, <see langword="false"/>.</returns>
        bool TryGetChar(ReadOnlySpan<char> span, ref int spanIndex, out char c);
    }

    public ref struct StringConverterEnumerator<T>(ReadOnlySpan<char> span, T converter)
        where T : IStringConverter
    {
        private readonly ReadOnlySpan<char> _span = span;
        private readonly T _converter = converter;
        private int _index;
        private char _current;

        public readonly char Current => _current;

        public bool MoveNext()
        {
            if (_index >= _span.Length)
            {
                return false;
            }
            var ret = _converter.TryGetChar(_span, ref _index, out _current);
            _index++;
            return ret;
        }

        public readonly StringConverterEnumerator<T> GetEnumerator() => this;
    }

    public static partial class TextConvert
    {
        /// <summary>
        /// Create a <see cref="StringConverterEnumerator{T}"/> that enumerates characters from the <paramref name="source"/>, using the provided <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter">The <see cref="IStringConverter"/> used for conversion.</param>
        /// <param name="source">The text to enumerate characters.</param>
        /// <returns></returns>
        public static StringConverterEnumerator<T> EnumerateChars<T>(this T converter, ReadOnlySpan<char> source) where T : IStringConverter => new(source, converter);

        /// <summary>
        /// Gets the actual number of characters that produced from the given <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter">The <see cref="IStringConverter"/> used for conversion.</param>
        /// <param name="source">The text to convert.</param>
        /// <returns>The actual number of characters.</returns>
        public static int GetCharCount<T>(this T converter, ReadOnlySpan<char> source)
            where T : IStringConverter
        {
            var count = 0;
            foreach (var _ in EnumerateChars(converter, source))
            {
                count++;
            }
            return count;
        }

        public static int Convert<T>(this T converter, ReadOnlySpan<char> source, Span<char> target)
            where T : IStringConverter
        {
            var targetIndex = 0;
            foreach (var c in EnumerateChars(converter, source))
            {
                target[targetIndex] = c;
                targetIndex++;
            }
            return targetIndex;
        }

        public static string Convert<T>(this T converter, ReadOnlySpan<char> source)
            where T : IStringConverter
        {
            if (source.Length is 0)
            {
                return string.Empty;
            }
            var maxCharCount = converter.GetMaxCharCount(source);
            using var o = ArrayPool.Rent<char>(maxCharCount);
            var length = Convert(converter, source, o.Span);
            return new(o.AsSpan(length));
        }
    }
}
