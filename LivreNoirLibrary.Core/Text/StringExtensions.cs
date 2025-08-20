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

        /// <summary>
        /// Compares two <see cref="string"/>s and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="isNullMinimum">If <see cref="bool">true</see>, an empty <see cref="string"/> is always considered minimum. Otherwise, it is maximum.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.<br/>
        /// <b>Value</b> - Meaning<br/>
        /// <b>Less than zero</b> - <paramref name="x"/> is less than <paramref name="y"/>.<br/>
        /// <b>Zero</b> - <paramref name="x"/> equals <paramref name="y"/>.<br/>
        /// <b>Greater than zero</b> - <paramref name="x"/> is greater than <paramref name="y"/>.<br/>
        /// </returns>
        public static int CompareByNaturalOrder(this string? x, string? y, bool isNullMinimum = true)
        {
            if (string.IsNullOrEmpty(x))
            {
                if (string.IsNullOrEmpty(y))
                {
                    return 0;
                }
                else
                {
                    return isNullMinimum ? -1 : 1;
                }
            }
            else if (string.IsNullOrEmpty(y))
            {
                return isNullMinimum ? 1 : -1;
            }
            else
            {
                return StrCmpLogical(x, y);
            }
        }

        [LibraryImport("shlwapi", EntryPoint = $"{nameof(StrCmpLogical)}W", StringMarshalling = StringMarshalling.Utf16)]
        internal static partial int StrCmpLogical(string str1, string str2);

        public static string AutoFormat(this TimeSpan time) => time.Ticks switch
        {
            >= TimeSpan.TicksPerDay => time.ToString(@"d\d\ h\:mm\:ss"),
            >= TimeSpan.TicksPerHour => time.ToString(@"h\:mm\:ss\.f"),
            >= TimeSpan.TicksPerMinute => time.ToString(@"m\:ss\.ff"),
            _ => time.ToString(@"s\.ffff"),
        };
    }
}
