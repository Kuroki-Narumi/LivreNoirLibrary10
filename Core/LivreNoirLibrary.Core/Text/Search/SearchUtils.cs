using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Text
{
    public static class SearchUtils
    {
        public static char RequirePrefix { get; set; } = '+';
        public static char RejectPrefix { get; set; } = '-';

        public static string GetSearchPrefix(SearchSegmentFlag flag) => flag switch
        {
            SearchSegmentFlag.Required => $"{RequirePrefix}",
            SearchSegmentFlag.Rejected => $"{RejectPrefix}",
            _ => "",
        };

        public static SearchSegmentEnumerator EnumerateSearchSegments(this ReadOnlySpan<char> text) => new(text, RequirePrefix, RejectPrefix);

        public static List<SearchSegment> ToList(this SearchSegmentEnumerator enumer, List<SearchSegment>? list = null)
        {
            list ??= [];
            foreach (var segment in enumer)
            {
                list.Add(new(segment));
            }
            return list;
        }

        public static List<SearchSegment> ToSearchSegmentList(this ReadOnlySpan<char> text, List<SearchSegment>? list = null) => EnumerateSearchSegments(text).ToList(list);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ProcessMatch(ReadOnlySpan<char> segment, SearchSegmentFlag flag, ReadOnlySpan<char> text, StringComparison comparison, ref int anyCount, ref int matchCount)
        {
            if (text.Length is 0)
            {
                return true;
            }
            switch (flag)
            {
                case SearchSegmentFlag.Required:
                    if (!text.Contains(segment, comparison))
                    {
                        return true;
                    }
                    break;
                case SearchSegmentFlag.Rejected:
                    if (text.Contains(segment, comparison))
                    {
                        return true;
                    }
                    break;
                default:
                    anyCount++;
                    if (text.Contains(segment, comparison))
                    {
                        matchCount++;
                    }
                    break;
            }
            return false;
        }

        public static bool IsMatch(this ReadOnlySpan<char> searchText, ReadOnlySpan<char> text, StringComparison comparison = default)
        {
            var anyCount = 0;
            var matchCount = 0;
            foreach (var (segment, flag) in EnumerateSearchSegments(searchText))
            {
                if (ProcessMatch(segment, flag, text, comparison, ref anyCount, ref matchCount))
                {
                    return false;
                }
            }
            return anyCount is 0 || matchCount is > 0;
        }

        public static bool IsMatch(this ReadOnlySpan<SearchSegment> segments, ReadOnlySpan<char> text, StringComparison comparison = default)
        {
            var anyCount = 0;
            var matchCount = 0;
            foreach (var (segment, flag) in segments)
            {
                if (ProcessMatch(segment, flag, text, comparison, ref anyCount, ref matchCount))
                {
                    return false;
                }
            }
            return anyCount is 0 || matchCount is > 0;
        }

        public static bool IsMatch(this List<SearchSegment> segments, ReadOnlySpan<char> text, StringComparison comparison = default) => IsMatch(segments.AsSpan(), text, comparison);

        public static bool IsMatch(this IEnumerable<SearchSegment> segments, ReadOnlySpan<char> text, StringComparison comparison = default)
        {
            var anyCount = 0;
            var matchCount = 0;
            foreach (var (segment, flag) in segments)
            {
                if (ProcessMatch(segment, flag, text, comparison, ref anyCount, ref matchCount))
                {
                    return false;
                }
            }
            return anyCount is 0 || matchCount is > 0;
        }
    }
}
