using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Text
{
    public static partial class BasedNumber
    {
        public const int MinimumRadix = 2;
        public const int DecimalRadix = 10;
        public const int HexRadix = 16;
        public const int StandardRadix = 36;
        public const int MaximumRadix = 64;

        private static readonly Dictionary<char, byte> _s2i;
        private static readonly Dictionary<byte, char> _i2s;

        static BasedNumber()
        {
            var s2i = _s2i = [];
            var i2s = _i2s = [];

            byte i = 0;
            void Add(char c, byte v)
            {
                s2i.Add(c, v);
                i2s.Add(v, c);
            }

            for (var c = '0'; c <= '9'; c++, i++)
            {
                Add(c, i);
            }
            for (var c = 'A'; c <= 'Z'; c++, i++)
            {
                Add(c, i);
            }
            for (var c = 'a'; c <= 'z'; c++, i++)
            {
                Add(c, i);
            }
            Add('+', 62);
            Add('/', 63);
        }

        private static void ThrowIfRadixOutOfRange(int radix)
        {
            if (radix is < MinimumRadix or > MaximumRadix)
            {
                throw new ArgumentOutOfRangeException($"radix must be between {MinimumRadix} and {MaximumRadix} ({radix})");
            }
        }

        private static void ThrowFormatException(char c) => throw new FormatException($"invalid character appeared ({c})");

        public static long ParseToLong(this ReadOnlySpan<char> span, int radix)
        {
            if (span.Length is 0)
            {
                return 0;
            }
            ThrowIfRadixOutOfRange(radix);
            var result = 0L;
            var map = _s2i;
            foreach (var c in span)
            {
                result *= radix;
                if (map.TryGetValue(c, out var n))
                {
                    if (n is >= StandardRadix && radix is <= StandardRadix)
                    {
                        n -= 26;
                    }
                    if (n < radix)
                    {
                        result += n;
                        continue;
                    }
                }
                ThrowFormatException(c);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ParseToShort(this ReadOnlySpan<char> span, int radix) => (short)ParseToLong(span, radix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ParseToInt(this ReadOnlySpan<char> span, int radix) => (int)ParseToLong(span, radix);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ParseToLong(this string? text, int radix) => ParseToLong(text.AsSpan(), radix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ParseToShort(this string? text, int radix) => (short)ParseToLong(text, radix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ParseToInt(this string? text, int radix) => (int)ParseToLong(text, radix);

        public static bool TryParseToLong(this ReadOnlySpan<char> span, int radix, out long value)
        {
            value = default;
            if (span.Length is 0 || (radix is < MinimumRadix or > MaximumRadix))
            {
                return false;
            }
            var result = 0L;
            var map = _s2i;
            foreach (var c in span)
            {
                result *= radix;
                if (map.TryGetValue(c, out var n))
                {
                    if (n is >= StandardRadix && radix is <= StandardRadix)
                    {
                        n -= 26;
                    }
                    if (n < radix)
                    {
                        result += n;
                        continue;
                    }
                }
                return false;
            }
            value = result;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseToShort(this ReadOnlySpan<char> span, int radix, out short value)
        {
            if (TryParseToLong(span, radix, out var v))
            {
                value = (short)v;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseToInt(this ReadOnlySpan<char> span, int radix, out int value)
        {
            if (TryParseToLong(span, radix, out var v))
            {
                value = (int)v;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        private delegate bool TryParseStringDelegate<T>(ReadOnlySpan<char> index, int radix, out T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryParseStringCore<T>(this string? text, int radix, out T value, TryParseStringDelegate<T> del)
            where T : struct
        {
            if (string.IsNullOrEmpty(text))
            {
                value = default;
                return false;
            }
            else
            {
                return del(text.AsSpan(), radix, out value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseToLong(this string? text, int radix, out long value) => TryParseStringCore(text, radix, out value, TryParseToLong);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseToShort(this string? text, int radix, out short value) => TryParseStringCore(text, radix, out value, TryParseToShort);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseToInt(this string? text, int radix, out int value) => TryParseStringCore(text, radix, out value, TryParseToInt);

        public static string ToBased(this long value, int radix, int minDigits = 0, int maxDigits = 0)
        {
            ThrowIfRadixOutOfRange(radix);
            if (value is <= 0)
            {
                return (minDigits is <= 0 ? "0" : new string('0', minDigits)).Shared();
            }
            var len = Math.Max((int)long.Log2(value) + 1, minDigits);
            var buffer = (stackalloc char[len]);
            var i = len - 1;
            var map = _i2s;
            while (value >= radix)
            {
                (value, var r) = Math.DivRem(value, radix);
                buffer[i] = map[(byte)r];
                i--;
            }
            buffer[i] = map[(byte)value];
            var count = len - i;
            if (maxDigits is > 0 && count > maxDigits)
            {
                i = len - maxDigits;
            }
            else if (minDigits is > 0)
            {
                while (count < minDigits)
                {
                    count++;
                    i--;
                    buffer[i] = '0';
                }
            }
            return new string(buffer[i..]).Shared();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToBased(this short value, int radix, int minDigits = 0, int maxDigits = 0) => ToBased((long)value, radix, minDigits, maxDigits);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToBased(this int value, int radix, int minDigits = 0, int maxDigits = 0) => ToBased((long)value, radix, minDigits, maxDigits);

        public static void ToBased(this long value, Span<char> span, int radix)
        {
            ThrowIfRadixOutOfRange(radix);
            if (value is <= 0)
            {
                span.Fill('0');
            }
            else
            {
                var map = _i2s;
                for (var i = span.Length - 1; i >= 0; i--)
                {
                    (value, var r) = Math.DivRem(value, radix);
                    span[i] = map[(byte)r];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToBased(this short value, Span<char> target, int radix) => ToBased((long)value, target, radix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToBased(this int value, Span<char> target, int radix) => ToBased((long)value, target, radix);

        private delegate string ToBasedDelegate<T>(T index, int radix, int minDgits, int maxDigits);

        private readonly record struct Segment(string Start, string? End = null)
        {
            public override string ToString() => string.IsNullOrEmpty(End) ? Start : $"{Start}-{End}";
        }

        private static string GetListTextCore<T>(IEnumerable<T> source, ToBasedDelegate<T> func, T start, int radix, int minDgits, int maxDigits)
            where T : INumber<T>
        {
            using var obj = ObjectPool.Rent<List<Segment>>(out var list);
            foreach (var index in source)
            {
                var text = func(index, radix, minDgits, maxDigits);
                if (index == ++start)
                {
                    list[^1] = list[^1] with
                    {
                        End = text
                    };
                }
                else
                {
                    list.Add(new(text));
                    start = index;
                }
            }
            return string.Join(' ', list);
        }

        public static string GetListText(IEnumerable<long> source, int radix = DecimalRadix, int minDigits = 0, int maxDigits = 0)
            => GetListTextCore(source, ToBased, long.MinValue, radix, minDigits, maxDigits);

        public static string GetListText(IEnumerable<int> source, int radix = DecimalRadix, int minDigits = 0, int maxDigits = 0)
            => GetListTextCore(source, ToBased, int.MinValue, radix, minDigits, maxDigits);

        public static string GetListText(IEnumerable<short> source, int radix = DecimalRadix, int minDigits = 0, int maxDigits = 0)
            => GetListTextCore(source, ToBased, short.MinValue, radix, minDigits, maxDigits);

        public static string GetListText(this RangeSet<long> set, int radix = DecimalRadix, int minDigits = 0, int maxDigits = 0)
            => set.ToString(v => ToBased(v, radix, minDigits, maxDigits), "-", " ");

        public static string GetListText(this RangeSet<int> set, int radix = DecimalRadix, int minDigits = 0, int maxDigits = 0)
            => set.ToString(v => ToBased(v, radix, minDigits, maxDigits), "-", " ");

        public static string GetListText(this RangeSet<short> set, int radix = DecimalRadix, int minDigits = 0, int maxDigits = 0)
            => set.ToString(v => ToBased(v, radix, minDigits, maxDigits), "-", " ");

        const string IndexFormat = @"[0-9A-Za-z+/]+";
        const string RangeSeparator = @"(?:[-:~～]+|\.{2,})";

        [GeneratedRegex($@"(?:{IndexFormat}|{RangeSeparator})+")]
        private static partial Regex Regex_Number { get; }

        [GeneratedRegex(RangeSeparator)]
        private static partial Regex Regex_Range { get; }

        private delegate bool TryParseDelegate<T>(ReadOnlySpan<char> text, int radix, out T value);

        private static bool TryParseRangeSetCore<T>(string? text, RangeSet<T>? target, TryParseDelegate<T> func, int radix)
            where T : INumber<T>, IMinMaxValue<T>
        {
            target?.Clear();
            var span = text.AsSpan();
            var rangeSeparator = Regex_Range;
            // 範囲表現の候補となる部分文字列を走査
            foreach (var (index, length) in Regex_Number.EnumerateMatches(span))
            {
                // 想定されるマッチ: "1", "-", "1-", "-5", "1-5", "1-5-10", "-5-", "1-5-", "-5-10", "1---5"
                // 受領する表現: "1", "1-", "-5", "1-5", "1---5"
                var slice = span.Slice(index, length);
                T value, start = default!, end = default!;
                var valueCount = 0;
                foreach (var range in rangeSeparator.EnumerateSplits(slice))
                {
                    var expr = slice[range];
                    switch (++valueCount)
                    {
                        case 1:
                            if (expr.Length is 0) // 下限無し
                            {
                                start = T.MinValue;
                            }
                            else if (func(expr, radix, out value))
                            {
                                start = end = value;
                            }
                            else
                            {
                                return false;
                            }
                            break;
                        case 2:
                            if (expr.Length is 0) // 上限無し
                            {
                                end = T.MaxValue;
                            }
                            else if (func(expr, radix, out value))
                            {
                                end = value;
                            }
                            else
                            {
                                return false;
                            }
                            break;
                        default: // "1-5-10" のような表現: 不正
                            return false;
                    }
                }
                target?.AddRange(start, end);
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseRangeSet(string? text, RangeSet<long>? target, int radix)
            => TryParseRangeSetCore(text, target, TryParseToLong, radix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseRangeSet(string? text, RangeSet<int>? target, int radix)
            => TryParseRangeSetCore(text, target, TryParseToInt, radix);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseRangeSet(string? text, RangeSet<short>? target, int radix)
            => TryParseRangeSetCore(text, target, TryParseToShort, radix);
    }
}
