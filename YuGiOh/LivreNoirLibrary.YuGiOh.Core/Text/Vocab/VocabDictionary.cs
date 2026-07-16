using System;
using System.Collections.Generic;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Text.Convert;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        private static string GetEnumName<T>(T value, int index, ReadOnlySpan<string> names)
            where T : struct, Enum
        {
            if ((uint)index < (uint)names.Length)
            {
                return names[index];
            }
            return value is 0 ? Unknown : value.ToString();
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

        private static readonly VocabKeyStringConverter _converter = new();
        private static ConvertingStringComparer<VocabKeyStringConverter>? _keyComparer;

        private static Dictionary<string, T>.AlternateLookup<ReadOnlySpan<char>> CreateInvertedDictionary<T>()
            where T : struct, Enum
        {
            _keyComparer ??= new(_converter);
            var dic = new Dictionary<string, T>(_keyComparer);
            return dic.GetAlternateLookup<ReadOnlySpan<char>>();
        }

        private static Dictionary<string, T>.AlternateLookup<ReadOnlySpan<char>> CreateInvertedDictionary<T>(Dictionary<T, string> source)
            where T : struct, Enum
        {
            var dic = CreateInvertedDictionary<T>();
            foreach (var (value, name) in source)
            {
                dic[name] = value;
                dic[value.ToString()] = value;
            }
            return dic;
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
