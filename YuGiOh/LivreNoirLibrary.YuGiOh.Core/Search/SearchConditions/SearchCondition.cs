using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public static class SearchCondition
    {
        public static bool NotMatch<T>(HashSet<T> set, T value) => set.Count is > 0 && !set.Contains(value);

        public static bool NotMatch(bool isReleased, LocaleState state)
            => state is LocaleState.Released && !isReleased || state is LocaleState.Unreleased && isReleased;

        public static bool NotMatch(NumberRange range, int value) => range.IsEnabled && range.IsOutOfRange(value);
        public static bool NotMatch(DateRange range, DateTime value) => range.IsEnabled && range.IsOutOfRange(value);

        public static bool IsMatch(bool checkFlag, ReadOnlySpan<char> text, Span<char> buffer, Regex regex, TextForSearchStringConverter converter)
        {
            if (checkFlag)
            {
                var length = converter.Convert(text, buffer);
                return regex.IsMatch(buffer[..length]);
            }
            return false;
        }

        public static bool IsMatch(bool checkFlag, ReadOnlySpan<char> text, Span<char> buffer, ReadOnlySpan<SearchSegment> segments, TextForSearchStringConverter converter)
        {
            if (checkFlag)
            {
                var length = converter.Convert(text, buffer);
                return segments.IsMatch(buffer[..length], StringComparison.Ordinal);
            }
            return false;
        }
    }
}
