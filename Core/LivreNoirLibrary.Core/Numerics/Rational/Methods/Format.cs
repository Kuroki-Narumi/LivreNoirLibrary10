using System;
using System.Diagnostics.CodeAnalysis;
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
        public static Rational Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider = null) => Parse(s, DoubleDenominatorLimit, style, provider);

        /// <inheritdoc cref="Parse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?)"/>
        /// <param name="denLimit">Maximum value of denominator.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational Parse(ReadOnlySpan<char> s, long denLimit, NumberStyles style, IFormatProvider? provider)
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
        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Rational result) => TryParse(s, DoubleDenominatorLimit, style, provider, out result);

        /// <inheritdoc cref="TryParse(ReadOnlySpan{char}, long, NumberStyles, IFormatProvider?, out Rational)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(string? s, long denLimit, out Rational result) => TryParse(s.AsSpan(), denLimit, NumberStyles.Number, null, out result);

        /// <inheritdoc cref="TryParse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?, out Rational)"/>
        /// <param name="denLimit">Maximum value of denominator.</param>
        public static bool TryParse(ReadOnlySpan<char> s, long denLimit, NumberStyles style, IFormatProvider? provider, out Rational result)
        {
            if (TryParseCore(s, denLimit, style, provider, out var num, out var den))
            {
                result = new(true, num, den);
                return true;
            }
            result = default;
            return false;
        }

        public static bool TryParseCore(ReadOnlySpan<char> s, long denLimit, NumberStyles style, IFormatProvider? provider, out long numerator, out long denominator)
        {
            numerator = denominator = default;
            // 分子と分母に分割するためのバッファ
            var dest = (stackalloc Range[3]);
            // 区切り文字 "/" で分割
            var count = s.Split(dest, '/', StringSplitOptions.TrimEntries);
            // 分割数1(分子のみ)または2(分子/分母)が正しい形式
            if (count is not (1 or 2))
            {
                return false;
            }
            // 分子
            if (!double.TryParse(s[dest[0]], style, provider, out var dNum))
            {
                return false;
            }
            // 分母が無い場合
            if (count is 1)
            {
                return TryRationalize(dNum, denLimit, out numerator, out denominator);
            }
            if (!double.TryParse(s[dest[1]], style, provider, out var dDen) || dDen is 0)
            {
                return false;
            }
            // 両方が整数の場合
            var intNum = Math.Truncate(dNum);
            var intDen = Math.Truncate(dDen);
            if (intNum == dNum && intDen == dDen)
            {
                numerator = (long)intNum;
                denominator = (long)intDen;
                var gcd = numerator.GCD(denominator);
                numerator /= gcd;
                denominator /= gcd;
                return true;
            }
            return TryRationalize(dNum / dDen, denLimit, out numerator, out denominator);
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
            return quo is 0 ? $"{rem}/{den}" : $"{quo}+{rem}/{den}";
        }

        public static bool TryFormatCore(long numerator, long denominator, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            // 分子を書き込む
            if (!numerator.TryFormat(destination, out charsWritten, format, provider))
            {
                return false;
            }
            // 分母が 1 の場合は終了
            if (denominator is 1)
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
            var ret = denominator.TryFormat(destination[charsWritten..], out var denCharsWritten, format, provider);
            charsWritten += denCharsWritten;
            return ret;
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
            => TryFormatCore(Numerator, Denominator, destination, out charsWritten, format, provider);
    }
}
