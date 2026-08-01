using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public interface IDeckTag
    {
        public string? Name { get; }
        public string? SearchHint { get; }
    }

    public static class IDeckTagExtensions
    {
        public static bool IsMatch(this IDeckTag tag, ReadOnlySpan<char> text) 
            => tag.Name.Contains(text, StringComparison.OrdinalIgnoreCase) || tag.SearchHint.Contains(text, StringComparison.OrdinalIgnoreCase);

        public static int Compare(IDeckTag? left, IDeckTag? right) => left is null ? (right is null ? 0 : -1) : (right is null ? 1 : left.Name.CompareTo(right.Name, StringComparison.Ordinal));
    }
}
