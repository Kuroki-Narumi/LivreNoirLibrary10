using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational : IParsable<Rational>, ISpanFormattable, ISpanParsable<Rational>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational Parse(string s, IFormatProvider? provider = null) => Parse(s.AsSpan(), NumberStyles.Number, provider);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) => Parse(s, NumberStyles.Number, provider);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational Parse(string s, NumberStyles style, IFormatProvider? provider) => Parse(s.AsSpan(), style, provider);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => Parse(s, DefaultConvertDenLimit, style, provider);
        /// <inheritdoc cref="Parse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?)">
        /// </inheritdoc>
        /// <param name="denLimit">Maximimu value of denominator.</param>
        /// <returns></returns>
        /// 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational Parse(ReadOnlySpan<char> s, ulong denLimit, NumberStyles style, IFormatProvider? provider)
        {
            if (!TryParse(s, denLimit, style, provider, out var value))
            {
                ThrowFormatException();
            }
            return value;
        }

        /// <inheritdoc cref="TryParse(string?, IFormatProvider?, out Rational)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(string? s, out Rational result) => TryParse(s.AsSpan(), NumberStyles.Number, null, out result);
        /// <inheritdoc cref="TryParse(ReadOnlySpan{char}, IFormatProvider?, out Rational)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ReadOnlySpan<char> s, out Rational result) => TryParse(s, NumberStyles.Number, null, out result);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(string? s, IFormatProvider? provider, out Rational result) => TryParse(s.AsSpan(), NumberStyles.Number, provider, out result);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Rational result) => TryParse(s, NumberStyles.Number, provider, out result);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out Rational result) => TryParse(s.AsSpan(), style, provider, out result);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Rational result) => TryParse(s, DefaultConvertDenLimit, style, provider, out result);

        /// <inheritdoc cref="TryParse(ReadOnlySpan{char}, ulong, NumberStyles, IFormatProvider?, out Rational)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(string? s, ulong denLimit, out Rational result) => TryParse(s.AsSpan(), denLimit, NumberStyles.Number, null, out result);

        /// <inheritdoc cref="TryParse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?, out Rational)"/>
        /// <param name="denLimit">Maximimu value of denominator.</param>
        public static bool TryParse(ReadOnlySpan<char> s, ulong denLimit, NumberStyles style, IFormatProvider? provider, out Rational result)
        {
            result = default;
            var dest = (stackalloc Range[3]);
            var count = s.Split(dest, '/', StringSplitOptions.TrimEntries);
            if (count is 0 or 3)
            {
                return false;
            }
            var longDen = 1L;
            var decNum = 0m;
            var decDen = 1m;
            var isLongOnly = false;
            var numSpan = s[dest[0]];
            if (!(isLongOnly = long.TryParse(numSpan, style, provider, out long longNum)))
            {
                if (!decimal.TryParse(numSpan, style, provider, out decNum))
                {
                    return false;
                }
            }
            if (count is 2)
            {
                var denSpan = s[dest[1]];
                if (isLongOnly && long.TryParse(denSpan, style, provider, out longDen))
                {
                }
                else if (!decimal.TryParse(denSpan, style, provider, out decDen) || decDen is 0)
                {
                    isLongOnly = false;
                    return false;
                }
            }
            if (isLongOnly)
            {
                if (longDen is 0)
                {
                    return false;
                }
                result = new(longNum, longDen);
                return true;
            }
            decNum /= decDen;
            if (decNum is < long.MinValue or > long.MaxValue)
            {
                return false;
            }
            result = ConvertBySBT(decNum, denLimit);
            return true;
        }

        public override string ToString() => _denominatorMinusOne is 0 ? $"{_numerator}" : $"{_numerator}/{Denominator}";

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var num = _numerator.ToString(format, formatProvider);
            return _denominatorMinusOne is 0 ? num : $"{num}/{Denominator.ToString(format, formatProvider)}";
        }

        public string ToMixedString()
        {
            var den = _denominatorMinusOne + 1;
            if (den is 1)
            {
                return $"{_numerator}";
            }
            var (quo, rem) = Math.DivRem(_numerator, den);
            return $"{quo}+{rem}/{den}";
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            if (_numerator.TryFormat(destination, out charsWritten, format, provider))
            {
                if (_denominatorMinusOne is 0)
                {
                    return true;
                }
                if (charsWritten < destination.Length - 1)
                {
                    destination[charsWritten] = '/';
                    charsWritten++;
                    var d2 = destination[charsWritten..];
                    var d = Denominator;
                    if (d.TryFormat(d2, out var c, format, provider))
                    {
                        for (int i = 0; i < c; i++)
                        {
                            destination[charsWritten + i] = d2[i];
                        }
                        charsWritten += c;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
