using LivreNoirLibrary.Text;
using System;

namespace LivreNoirLibrary.YuGiOh
{
    public readonly struct VocabKeyStringConverter : IStringConverter
    {
        public int GetMaxCharCount(ReadOnlySpan<char> span) => span.Length;

        public bool TryGetChar(ReadOnlySpan<char> span, ref int spanIndex, out char c)
        {
            for (; spanIndex < span.Length; spanIndex++)
            {
                c = char.ToLowerInvariant(span[spanIndex]);
                if (!(c is '-' or '_' || char.IsWhiteSpace(c)))
                {
                    return true;
                }
            }
            c = default;
            return false;
        }
    }
}
