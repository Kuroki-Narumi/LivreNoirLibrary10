using System;

namespace LivreNoirLibrary.Text
{
    public readonly record struct SearchSegment(string Text, SearchSegmentFlag Flag)
    {
        public SearchSegment(RefSearchSegment source) : this(source.Span.ToString(), source.Flag) { }

        public override string ToString() => $"{SearchUtils.GetSearchPrefix(Flag)}{Text}";
    }
}
