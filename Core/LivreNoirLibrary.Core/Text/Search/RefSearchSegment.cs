using System;

namespace LivreNoirLibrary.Text
{
    public readonly ref struct RefSearchSegment(ReadOnlySpan<char> span, SearchSegmentFlag flag)
    {
        public readonly ReadOnlySpan<char> Span = span;
        public readonly SearchSegmentFlag Flag = flag;

        public void Deconstruct(out ReadOnlySpan<char> span, out  SearchSegmentFlag flag)
        {
            span = Span;
            flag = Flag;
        }
    }
}
