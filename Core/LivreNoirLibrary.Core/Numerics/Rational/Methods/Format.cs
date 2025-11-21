using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational : ISpanParsable<Rational>, ISpanFormattable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational Parse(string s, IFormatProvider? provider = null) => Parse(s.AsSpan(), NumberStyles.Number, provider);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) => Parse(s, NumberStyles.Number, provider);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational Parse(string s, NumberStyles style, IFormatProvider? provider = null) => Parse(s.AsSpan(), style, provider);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider = null) => Parse(s, DefaultConvertDenLimit, style, provider);

        /// <inheritdoc cref="Parse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?)"/>
        /// <param name="denLimit">Maximum value of denominator.</param>
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
        /// <param name="denLimit">Maximum value of denominator.</param>
        public static bool TryParse(ReadOnlySpan<char> s, ulong denLimit, NumberStyles style, IFormatProvider? provider, out Rational result)
        {
            result = default;
            // 分子と分母に分割するためのバッファ
            var dest = (stackalloc Range[3]);
            // 区切り文字 "/" で分割
            var count = s.Split(dest, '/', StringSplitOptions.TrimEntries);
            // 分割数 1(分子のみ)または 2(分子/分母)のみを正しい書式とみなす
            if (count is not 1 or 2)
            {
                return false;
            }
            // 分子
            var numSpan = s[dest[0]];
            var doubleNum = 0d;
            var doubleDen = 1d;
            // 分子が整数
            var isLong = long.TryParse(numSpan, style, provider, out var longNum);
            if (isLong)
            {
                doubleNum = longNum;
            }
            // 分子が数値として解釈できない
            else if (!double.TryParse(numSpan, style, provider, out doubleNum))
            {
                return false;
            }
            // 分母
            if (count is 2)
            {
                var denSpan = s[dest[1]];
                // 分母も整数
                if (isLong && long.TryParse(denSpan, style, provider, out var longDen))
                {
                    if (longDen is 0)
                    {
                        return false;
                    }
                    result = new(longNum, longDen);
                    return true;
                }
                // 分母が数値として解釈できない、または 0
                if (!double.TryParse(denSpan, style, provider, out doubleDen) || doubleDen is 0)
                {
                    return false;
                }
                doubleNum /= doubleDen;
            }
            if (isLong)
            {
                result = new(longNum);
            }
            else if (!double.IsFinite(doubleNum) || doubleNum is < long.MinValue or > long.MaxValue)
            {
                return false;
            }
            else
            {
                result = ConvertBySBT(doubleNum, denLimit);
            }
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
            var den = Denominator;
            if (den is 1)
            {
                return $"{_numerator}";
            }
            var (quo, rem) = Math.DivRem(_numerator, den);
            return $"{quo}+{rem}/{den}";
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            // 分子を書き込む
            if (!_numerator.TryFormat(destination, out charsWritten, format, provider))
            {
                return false;
            }
            // 分母が 1 の場合は終了
            if (_denominatorMinusOne is 0)
            {
                return true;
            }
            // "/" 以降を書き込む余裕があるかのチェック
            if (destination.Length - charsWritten is < 2)
            {
                return false;
            }
            destination[charsWritten] = '/';
            charsWritten++;
            var ret = Denominator.TryFormat(destination[charsWritten..], out var denCharsWritten, format, provider);
            charsWritten += denCharsWritten;
            return ret;
        }
    }
}
