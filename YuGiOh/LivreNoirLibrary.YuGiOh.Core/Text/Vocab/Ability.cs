using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        public const string Ability = "能力";

        public const string Toon = "トゥーン";
        public const string Gemini = "デュアル";
        public const string Union = "ユニオン";
        public const string Spirit = "スピリット";
        public const string Tuner = "チューナー";
        public const string Flip = "リバース";
        public const string Pendulum = "ペンデュラム";
        public const string SpecialSummon = "特殊召喚";

        public const string Ability_Separator = " / ";

        private static readonly Dictionary<Ability, string> _abi2name = new()
        {
            { YuGiOh.Ability.SpecialSummon, SpecialSummon },
            { YuGiOh.Ability.Pendulum, Pendulum },
            { YuGiOh.Ability.Toon, Toon },
            { YuGiOh.Ability.Gemini, Gemini },
            { YuGiOh.Ability.Union, Union },
            { YuGiOh.Ability.Spirit, Spirit },
            { YuGiOh.Ability.Flip, Flip },
            { YuGiOh.Ability.Tuner, Tuner },
            { YuGiOh.Ability.Normal, Normal },
            { YuGiOh.Ability.Effect, Effect },
        };

        private static readonly Dictionary<Ability, List<string>> _abi2names = [];

        private static readonly Dictionary<string, Ability>.AlternateLookup<ReadOnlySpan<char>> _name2abi = CreateInvertedDictionary(_abi2name);

        public static ReadOnlySpan<string> GetNames(this Ability value)
        {
            if (!_abi2names.TryGetValue(value, out var list))
            {
                list = [];
                foreach (var (abi, name) in _abi2name)
                {
                    if ((value & abi) is not 0)
                    {
                        list.Add(name);
                    }
                }
                _abi2names[value] = list;
            }
            return list.AsSpan();
        }

        public static string GetName(this Ability value) => string.Join(Ability_Separator, GetNames(value));
        public static string GetSingleName(this Ability value) => GetEnumName(value, _abi2name);

        public static Ability GetAbility(this ReadOnlySpan<char> text)
        {
            var result = YuGiOh.Ability.Normal;
            foreach (var range in text.Split(Separators))
            {
                var name = text[range].Trim();
                if (TryGetEnumValue(name, _name2abi, out var val))
                {
                    result |= val;
                }
            }
            return result;
        }

        public static Ability GetAbility(this IEnumerable<string> names)
        {
            var value = YuGiOh.Ability.Normal;
            foreach (var name in names)
            {
                if (TryGetEnumValue(name, _name2abi, out var val))
                {
                    value |= val;
                }
            }
            return value;
        }
    }
}
