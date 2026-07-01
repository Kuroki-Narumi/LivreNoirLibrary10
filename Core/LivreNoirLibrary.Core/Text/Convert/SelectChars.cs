using System;
using System.Threading;

namespace LivreNoirLibrary.Text
{
    public class SelectCharsStringConverter(Func<char, bool> selector) : IStringConverter
    {
        public static Func<char, bool> DefaultSelector { get; } = static c => true;

        public Func<char, bool> Selector { get; set; } = selector;

        public int GetMaxCharCount(ReadOnlySpan<char> span) => span.Length;

        public bool TryGetChar(ReadOnlySpan<char> span, ref int spanIndex, out char c)
        {
            var selector = Selector;
            for (; spanIndex < span.Length; spanIndex++)
            {
                c = span[spanIndex];
                if (selector(c))
                {
                    return true;
                }
            }
            c = default;
            return false;
        }
    }

    public static partial class TextConvert
    {
        private static readonly ThreadLocal<SelectCharsStringConverter> _sharedSCC = new(() => new(SelectCharsStringConverter.DefaultSelector));

        public static int SelectChars(Span<char> target, ReadOnlySpan<char> source, Func<char, bool> selector)
        {
            var converter = _sharedSCC.Value!;
            converter.Selector = selector;
            return converter.Convert(source, target);
        }

        public static string SelectChars(this ReadOnlySpan<char> text, Func<char, bool> selector)
        {
            var converter = _sharedSCC.Value!;
            converter.Selector = selector;
            return converter.Convert(text);
        }
    }
}
