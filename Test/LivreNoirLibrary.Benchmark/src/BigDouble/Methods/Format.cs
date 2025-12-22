using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct BigDouble
        : IFormattable, ISpanFormattable, IParsable<BigDouble>, ISpanParsable<BigDouble>
    {
        /// <inheritdoc cref="Parse(string, IFormatProvider?)"/>
        public static BigDouble Parse(string s) => Parse(s.AsSpan(), NumberStyles.Number, null);
        public static BigDouble Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), NumberStyles.Number, provider);
        public static BigDouble Parse(string s, NumberStyles style, IFormatProvider? provider) => Parse(s.AsSpan(), style, provider);
        /// <inheritdoc cref="Parse(ReadOnlySpan{char}, IFormatProvider?)"/>
        public static BigDouble Parse(ReadOnlySpan<char> s) => Parse(s, NumberStyles.Number, null);
        public static BigDouble Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, NumberStyles.Number, provider);
        public static BigDouble Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="TryParse(string, IFormatProvider?)"/>
        public static bool TryParse([NotNullWhen(true)] string? s, out BigDouble result)
            => TryParse(s.AsSpan(), NumberStyles.Number, null, out result);
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out BigDouble result)
            => TryParse(s.AsSpan(), NumberStyles.Number, provider, out result);
        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out BigDouble result)
            => TryParse(s.AsSpan(), style, provider, out result);
        /// <inheritdoc cref="Parse(ReadOnlySpan{char}, IFormatProvider?)"/>
        public static bool TryParse(ReadOnlySpan<char> s, out BigDouble result)
            => TryParse(s, NumberStyles.Number, null, out result);
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out BigDouble result)
            => TryParse(s, NumberStyles.Number, provider, out result);
        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out BigDouble result)
        {
            throw new NotImplementedException();
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            throw new NotImplementedException();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }
    }
}
