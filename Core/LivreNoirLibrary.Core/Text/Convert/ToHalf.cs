using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Text
{
    public static partial class TextConvert
    {
        internal const int Offset_Ascii = '０' - '0';

        public static bool IsDigitCharacter(char c) => c is >= '０' and <= '９';
        public static bool IsAsciiCharacter(char c) => c is >= '！' and <= '～';

        public static string ToHalf(this ReadOnlySpan<char> text) => string.Create(text.Length, text, Convert_Ascii);
        public static string ToHalfDigits(this ReadOnlySpan<char> text) => string.Create(text.Length, text, Convert_Digits);

        private static void Convert_Ascii(Span<char> span, ReadOnlySpan<char> src) => ToHalfAscii(span, src, IsAsciiCharacter);
        private static void Convert_Digits(Span<char> span, ReadOnlySpan<char> src) => ToHalfAscii(span, src, IsDigitCharacter);

        public static void ToHalfAscii(Span<char> span, ReadOnlySpan<char> src, Func<char, bool> selector)
        {
            for (var i = 0; i < src.Length; i++)
            {
                var c = src[i];
                span[i] = selector(c) ? (char)(c - Offset_Ascii) : c;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToHalf(this char c) => ToHalf(c, IsAsciiCharacter);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToHalfDigit(this char c) => ToHalf(c, IsDigitCharacter);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToHalf(this char c, Func<char, bool> selector) => selector(c) ? (char) (c - Offset_Ascii) : c;

        public static int ToHalfRegex(this ReadOnlySpan<char> text, Span<char> span, bool toHiragana = false)
        {
            var rChars = _regexChars;
            var index = 0;
            var escaping = false;
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                // 直前がエスケープ文字でなく、現在の文字がメタ文字の全角版の場合
                if (!escaping && rChars.Contains(c))
                {
                    // エスケープ文字を追加
                    span[index] = '\\';
                    index++;
                }
                if (IsAsciiCharacter(c))
                {
                    c = (char)(c - Offset_Ascii);
                }
                else if (toHiragana)
                {
                    TryGetHiragana(text, ref i, out c);
                }
                // エスケープ中かどうかの判定
                escaping = c is '\\' && !escaping;

                span[index] = c;
                index++;
            }
            return index;
        }

        public static string ToHalfRegex(this ReadOnlySpan<char> text, bool toHiragana = false)
        {
            if (text.Length is 0)
            {
                return string.Empty;
            }
            // 文字列長は最大で2倍になる可能性がある
            using var o = ArrayPool.Rent<char>(text.Length * 2);

            var targetSpan = o.Span;
            var targetIndex = ToHalfRegex(text, targetSpan, toHiragana);
            return new(targetSpan[..targetIndex]);
        }

        private static readonly HashSet<char> _regexChars = [.. "！＄（）＊＋，．＜＝＞？＼＾｛｜｝"];
    }
}
