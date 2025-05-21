using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Text
{
    public static partial class StringExtensions
    {
        [GeneratedRegex(@"\r\n|\r|\n")]
        public static partial Regex EndOfLine { get; }

        public static string[] SplitLines(this string text) => EndOfLine.Split(text);

        public static string ReplaceEndOfLine(this string text) => ReplaceEndOfLine(text, Environment.NewLine);
        public static string ReplaceEndOfLine(this string text, string replacement) => EndOfLine.Replace(text, replacement);

        public static int CountLine(this string text, bool countEmptyLine = true)
        {
            if (string.IsNullOrEmpty(text))
            {
                return countEmptyLine ? 1 : 0;
            }
            var count = 1;
            var matches = EndOfLine.Matches(text);
            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (countEmptyLine || !string.IsNullOrEmpty(match.Value))
                {
                    count++;
                }
            }
            return count;
        }

        public static string? GetNullIfEmpty(this string? text) => string.IsNullOrEmpty(text) ? null : text;
        public static string? GetNullIfWhiteSpace(this string? text) => string.IsNullOrWhiteSpace(text) ? null : text;

        public static string Shared(this string text) => StringPool.Get(text);

        public static int CompareByNaturalOrder(this string? left, string? right)
        {
            if (!string.IsNullOrEmpty(left))
            {
                if (!string.IsNullOrEmpty(right))
                {
                    return StrCmpLogical(left, right);
                }
                else
                {
                    return 1;
                }
            }
            else if (!string.IsNullOrEmpty(right))
            {
                return -1;
            }
            return 0;
        }

        [LibraryImport("shlwapi", EntryPoint = $"{nameof(StrCmpLogical)}W", StringMarshalling = StringMarshalling.Utf16)]
        internal static partial int StrCmpLogical(string str1, string str2);
    }
}
