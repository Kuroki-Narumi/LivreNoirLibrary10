using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Text
{
    public static partial class StringExtensions
    {
        public static string GetTypeName<T>(this T obj) => GetFriendlyName(typeof(T));

        public static string GetFriendlyName(this Type type)
        {
            var typeName = type.Name;
            if (!type.IsGenericType)
            {
                return typeName;
            }
            var baseName = typeName[..typeName.IndexOf('`')];
            var args = type.GetGenericArguments().Select(GetFriendlyName);
            return $"{baseName}<{string.Join(", ", args)}>";
        }

        public static string[] SplitLines(this string? text, bool trim = false)
        {
            var buffer = ObjectPool.Rent<List<string>>();
            try
            {
                foreach (var span in text.AsSpan().EnumerateLines())
                {
                    buffer.Add(new(trim ? span.Trim() : span));
                }
                return [.. buffer];
            }
            finally
            {
                ObjectPool.Return(buffer);
            }
        }

        public static int CountLine(this string? text, bool countEmptyLine = true)
        {
            if (string.IsNullOrEmpty(text))
            {
                return countEmptyLine ? 1 : 0;
            }
            var count = 1;
            foreach (var span in text.AsSpan().EnumerateLines())
            {
                if (countEmptyLine || (span.Length is > 0 && !span.IsWhiteSpace()))
                {
                    count++;
                }
            }
            return count;
        }

        public static int LengthWithoutSpace(this string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }
            var count = 0;
            foreach (var rune in text.EnumerateRunes())
            {
                if (!Rune.IsWhiteSpace(rune))
                {
                    count++;
                }
            }
            return count;
        }

        public static string? GetNullIfEmpty(this string? text) => string.IsNullOrEmpty(text) ? null : text;
        public static string? GetNullIfWhiteSpace(this string? text) => string.IsNullOrWhiteSpace(text) ? null : text;

        public static string Shared(this string text) => StringPool.Get(text);

        public static StringComparer NaturalOrderComparer { get; } = StringComparer.Create(CultureInfo.InvariantCulture, CompareOptions.NumericOrdering);

        public static int CompareByNaturalOrder(this string? left, string? right) => NaturalOrderComparer.Compare(left, right);
        public static int CompareByNaturalOrder(this string? left, string? right, bool isNullMinimum)
        {
            if (string.IsNullOrEmpty(left))
            {
                if (string.IsNullOrEmpty(right))
                {
                    return 0;
                }
                else
                {
                    return isNullMinimum ? -1 : 1;
                }
            }
            else if (string.IsNullOrEmpty(right))
            {
                return isNullMinimum ? 1 : -1;
            }
            else
            {
                return NaturalOrderComparer.Compare(left, right);
            }
        }
    }
}
