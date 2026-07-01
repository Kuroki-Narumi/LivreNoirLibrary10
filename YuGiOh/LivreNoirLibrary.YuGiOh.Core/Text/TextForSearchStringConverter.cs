using System;
using System.Collections.Generic;
using System.Text;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh
{
    delegate bool TryGetCharFunc(ReadOnlySpan<char> span, ref int spanIndex, out char c);

    public readonly struct TextForSearchStringConverter(bool ignoreCase, bool ignoreSymbols) : IStringConverter
    {
        private readonly TryGetCharFunc _func = TextConvert.GetSearchFunc(ignoreCase, ignoreSymbols);

        public int GetMaxCharCount(ReadOnlySpan<char> span) => span.Length;

        public bool TryGetChar(ReadOnlySpan<char> span, ref int spanIndex, out char c) => _func(span, ref spanIndex, out c);
    }

    public static partial class TextConvert
    {
        public static string ConvertForSearch(this ReadOnlySpan<char> text, bool ignoreCase, bool ignoreSymbols)
        {
            var converter = new TextForSearchStringConverter(ignoreCase, ignoreSymbols);
            return converter.Convert(text);
        }

        private static readonly HashSet<char> _symbols = [.. " \t\n\r\v\"#$%&'*+,-./:;<=>[\\]^_`{|}~　・“”‘’＝－―★☆×「」。、｢｣｡､『』【】《》"];

        internal static TryGetCharFunc GetSearchFunc(bool ignoreCase, bool ignoreSymbols)
        {
            if (ignoreCase)
            {
                if (ignoreSymbols)
                {
                    return TryGetChar_Case_Symbol;
                }
                else
                {
                    return TryGetChar_Case;
                }
            }
            else
            {
                if (ignoreSymbols)
                {
                    return TryGetChar_Symbol;
                }
                else
                {
                    return TryGetChar_None;
                }
            }
        }

        private static bool TryGetChar_None(ReadOnlySpan<char> span, ref int spanIndex, out char c)
        {
            c = span[spanIndex].ToHalf();
            return true;
        }

        private static bool TryGetChar_Symbol(ReadOnlySpan<char> span, ref int spanIndex, out char c)
        {
            var symbols = _symbols;
            for (; spanIndex < span.Length; spanIndex++)
            {
                c = span[spanIndex].ToHalf();
                if (!symbols.Contains(c))
                {
                    return true;
                }
            }
            c = default;
            return false;
        }

        private static bool TryGetChar_Case(ReadOnlySpan<char> span, ref int spanIndex, out char c)
        {
            _ = Text.TextConvert.TryGetHiragana(span, ref spanIndex, out c);
            c = char.ToLowerInvariant(c.ToHalf());
            return true;
        }

        private static bool TryGetChar_Case_Symbol(ReadOnlySpan<char> span, ref int spanIndex, out char c)
        {
            var symbols = _symbols;
            for (; spanIndex < span.Length; spanIndex++)
            {
                _ = Text.TextConvert.TryGetHiragana(span, ref spanIndex, out c);
                c = char.ToLowerInvariant(c.ToHalf());
                if (!symbols.Contains(c))
                {
                    return true;
                }
            }
            c = default;
            return false;
        }
    }
}
