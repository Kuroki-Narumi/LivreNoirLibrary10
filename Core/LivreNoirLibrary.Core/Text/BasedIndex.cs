using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Debug;

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
        public static long ParseToLong(this string? text, int radix) => string.IsNullOrEmpty(text) ? 0 : ParseToLong(text.AsSpan(), radix);
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

        public static void ToBased(this long value, Span<char> span, int radix, int digits = 2)
        {
            ThrowIfRadixOutOfRange(radix);
            if (digits is <= 0)
            {
                throw new ArgumentException($"digits must be > 0.", nameof(digits));
            }
            if (value is <= 0)
            {
                span[..digits].Fill('0');
            }
            else
            {
                var map = _i2s;
                for (var i = digits - 1; i >= 0; i--)
                {
                    (value, var r) = Math.DivRem(value, radix);
                    span[i] = map[(byte)r];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToBased(this short value, Span<char> target, int radix, int digits = 2) => ToBased((long)value, target, radix, digits);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToBased(this int value, Span<char> target, int radix, int digits = 2) => ToBased((long)value, target, radix, digits);

        private delegate string ToBasedDelegate<T>(T index, int radix, int minDgits, int maxDigits);

        private static string GetListTextCore<T>(IEnumerable<T> source, ToBasedDelegate<T> func, T start, int radix, int minDgits, int maxDigits)
            where T : INumber<T>
        {
            List<Segment> list = [];
            foreach (var index in source)
            {
                var text = func(index, radix, minDgits, maxDigits);
                if (index == ++start)
                {
                    list[^1].End = text;
                }
                else
                {
                    list.Add(new(text));
                    start = index;
                }
            }
            return string.Join(' ', list);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetListText(IEnumerable<long> source, int radix = DecimalRadix, int minDigits = 0, int maxDigits = 0)
            => GetListTextCore(source, ToBased, long.MinValue, radix, minDigits, maxDigits);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetListText(IEnumerable<int> source, int radix = DecimalRadix, int minDigits = 0, int maxDigits = 0)
            => GetListTextCore(source, ToBased, int.MinValue, radix, minDigits, maxDigits);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetListText(IEnumerable<short> source, int radix = DecimalRadix, int minDigits = 0, int maxDigits = 0)
            => GetListTextCore(source, ToBased, short.MinValue, radix, minDigits, maxDigits);

        private class Segment(string start)
        {
            public string Start { get; set; } = start;
            public string? End { get; set; }

            public override string ToString() => string.IsNullOrEmpty(End) ? Start : $"{Start}-{End}";
        }

        [GeneratedRegex(@"(?<a>[0-9A-Za-z]+)(?<b>[-.~～]+(?<c>[0-9A-Za-z]+))?")]
        private static partial Regex Regex_Index { get; }

        private delegate bool TryParseDelegate<T>(string? text, int radix, out T value);

        private static bool TryParseListTextCore<T>(string? text, ICollection<T> target, TryParseDelegate<T> func, int radix, T max)
            where T : INumber<T>
        {
            target.Clear();
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }
            var matches = Regex_Index.Matches(text);
            var c = matches.Count;
            for (var i = 0; i < c; i++)
            {
                var match = matches[i];
                if (func(match.Groups["a"].Value, radix, out var a) && a < max)
                {
                    target.Add(a);
                    if (match.Groups["b"].Success)
                    {
                        if (func(match.Groups["c"].Value, radix, out var b) && b > a && b < max)
                        {
                            for (var j = a + T.One; j <= b; j++)
                            {
                                target.Add(j);
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseListText(string? text, ICollection<long> target, int radix, long max = long.MaxValue)
            => TryParseListTextCore(text, target, TryParseToLong, radix, max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseListText(string? text, ICollection<int> target, int radix, int max = int.MaxValue)
            => TryParseListTextCore(text, target, TryParseToInt, radix, max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseListText(string? text, ICollection<short> target, int radix, short max = short.MaxValue)
            => TryParseListTextCore(text, target, TryParseToShort, radix, max);
    }
}
