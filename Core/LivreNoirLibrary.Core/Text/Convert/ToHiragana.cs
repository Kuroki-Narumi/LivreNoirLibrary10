using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LivreNoirLibrary.Text
{
    public readonly struct ToHiraganaStringConverter : IStringConverter
    {
        public int GetMaxCharCount(ReadOnlySpan<char> span) => span.Length;

        public bool TryGetChar(ReadOnlySpan<char> span, ref int spanIndex, out char c) => TextConvert.TryGetHiragana(span, ref spanIndex, out c);
    }

    public static partial class TextConvert
    {
        /// <summary>
        /// Returns a <see cref="string"/> in which all full-width Katakana characters in the given <see cref="string"/> have been converted to Hiragana.
        /// </summary>
        /// <remarks>
        /// This process does not convert half-width Katakana. If you want to convert it as well, use <see cref="ToHiragana(ReadOnlySpan{char})"/>.
        /// </remarks>
        /// <param name="text">Text to convert.</param>
        /// <returns>A <see cref="string"/> converted to Hiragana.</returns>
        public static string ToHiraganaSimple(this ReadOnlySpan<char> text) => string.Create(text.Length, text, Convert_Kana);

        internal const int Offset_Kana = 'ァ' - 'ぁ';

        private static void Convert_Kana(Span<char> span, ReadOnlySpan<char> src)
        {
            for (var i = 0; i < src.Length; i++)
            {
                var c = src[i];
                span[i] = (c is >= 'ァ' and <= 'ヶ') ? (char)(c - Offset_Kana) : c;
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> in which all full-width Katakana and half-width Katakana characters in the given <see cref="string"/> have been converted to Hiragana.
        /// </summary>
        /// <param name="text">Text to convert.</param>
        /// <returns>A <see cref="string"/> converted to Hiragana.</returns>
        public static string ToHiragana(this ReadOnlySpan<char> text) => new ToHiraganaStringConverter().Convert(text);

        public static bool TryGetHiragana(ReadOnlySpan<char> span, ref int spanIndex, out char c)
        {
            c = span[spanIndex];
            if (c is >= 'ァ' and <= 'ヶ')
            {
                c = (char)(c - Offset_Kana);
            }
            else if (c is >= 'ｦ' and <= 'ﾝ')
            {
                c = _katakana[c - 'ｦ'];
                var nextIndex = spanIndex + 1;
                if (nextIndex < span.Length)
                {
                    switch (span[nextIndex])
                    {
                        case 'ﾞ':
                            if (c is 'う')
                            {
                                c = 'ゔ';
                                spanIndex++;
                            }
                            else if (_dakutenable.Contains(c))
                            {
                                c = (char)(c + 1);
                                spanIndex++;
                            }
                            break;
                        case 'ﾟ':
                            if (_handakutenable.Contains(c))
                            {
                                c = (char)(c + 2);
                                spanIndex++;
                            }
                            break;
                    }
                }
            }
            return true;
        }

        internal static readonly char[] _katakana = BuildHalfKatakana();
        static char[] BuildHalfKatakana()
        {
            var length = 'ﾝ' + 1 - 'ｦ';
            var result = new char[length];
            var hiragana = "をぁぃぅぇぉゃゅょっーあいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわん";
            var hiraSpan = hiragana.AsSpan();
            for (var i = 0; i < length; i++)
            {
                result[i] = hiraSpan[i];
            }
            return result;
        }

        internal static readonly HashSet<char> _dakutenable = [.. "かきくけこさしすせそたちつてとはひふへほ"];
        internal static readonly HashSet<char> _handakutenable = [.. "はひふへほ"];
    }
}
