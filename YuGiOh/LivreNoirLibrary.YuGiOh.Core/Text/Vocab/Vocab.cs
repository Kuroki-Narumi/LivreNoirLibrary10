using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        [GeneratedRegex(@"\s|-")]
        public static partial Regex Regex_Space { get; }
        public static string RemoveSpaces(string text) => Regex_Space.Replace(text, "");

        public static string GetLimitText(int limit)
        {
            return limit switch
            {
                LimitNumber.Unusable => Unusable,
                LimitNumber.Forbidden => Forbidden,
                LimitNumber.Limit1 => Limit1,
                LimitNumber.Limit2 => Limit2,
                LimitNumber.Specified => Specified,
                _ => Unlimited,
            };
        }

        private static void AppendEnglishNames<T>(Dictionary<string, T> dic)
            where T : struct, Enum
        {
            foreach (var value in Enum.GetValues<T>())
            {
                var name = value.ToString();
                dic.TryAdd(name, value);
                dic.TryAdd(name.ToUpper(), value);
            }
        }

        private static string GetEnumName<T>(T value, Dictionary<T, string> source)
            where T : struct, Enum
        {
            if (source.TryGetValue(value, out var name))
            {
                return name;
            }
            return value is 0 ? Unknown : value.ToString();
        }

        private static T GetEnumValue<T>(this string? name, Dictionary<string, T> source)
            where T : struct, Enum
        {
            if (!string.IsNullOrEmpty(name))
            {
                name = RemoveSpaces(name);
                if (source.TryGetValue(name, out var value))
                {
                    return value;
                }
            }
            return default;
        }

        private static bool TryGetEnumValue<T>(this string name, Dictionary<string, T> source, out T type)
            where T : struct, Enum
        {
            name = RemoveSpaces(name);
            return source.TryGetValue(name, out type);
        }
    }
}
