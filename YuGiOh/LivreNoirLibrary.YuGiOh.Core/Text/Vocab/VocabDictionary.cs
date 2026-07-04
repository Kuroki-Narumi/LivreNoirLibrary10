using System;
using System.Collections.Generic;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Text.Convert;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        private static string GetEnumName<T>(T value, Dictionary<T, string> source)
            where T : struct, Enum
        {
            if (source.TryGetValue(value, out var name))
            {
                return name;
            }
            return value is 0 ? Unknown : value.ToString();
        }

        private static readonly VocabKeyStringConverter _converter = new();
        private static ConvertingStringComparer<VocabKeyStringConverter>? _keyComparer;

        private static Dictionary<string, T>.AlternateLookup<ReadOnlySpan<char>> CreateInvertedDictionary<T>(Dictionary<T, string> source)
            where T : struct, Enum
        {
            var converter = _converter;
            _keyComparer ??= new(converter);
            var dic = new Dictionary<string, T>(_keyComparer);
            var alternateLookup = dic.GetAlternateLookup<ReadOnlySpan<char>>();
            foreach (var (value, name) in source)
            {
                alternateLookup[name] = value;
                alternateLookup[value.ToString()] = value;
            }
            return alternateLookup;
        }

        private static bool TryGetEnumValue<T>(this ReadOnlySpan<char> name, Dictionary<string, T>.AlternateLookup<ReadOnlySpan<char>> source, out T value)
            where T : struct, Enum
        {
            if (Enum.TryParse(name, true, out value))
            {
                return true;
            }
            return source.TryGetValue(name, out value);
        }

        private static T GetEnumValue<T>(this ReadOnlySpan<char> name, Dictionary<string, T>.AlternateLookup<ReadOnlySpan<char>> source)
            where T : struct, Enum
            => TryGetEnumValue(name, source, out var value) ? value : default;
    }
}
