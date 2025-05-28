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
        public const string SpecialSummon = "特殊召喚";

        public const string Ability_Separator = " / ";

        public static string GetName(this Ability value)
        {
            return string.Join(Ability_Separator, GetNames(value));
        }

        public static string GetName(IEnumerable<Ability> list)
        {
            return string.Join(Ability_Separator, GetNames(list));
        }

        public static List<string> GetNames(this Ability value)
        {
            List<string> names = [];
            foreach (var (abi, name) in _abi2name)
            {
                if ((value & abi) is not 0)
                {
                    names.Add(name);
                }
            }
            return names;
        }

        public static List<string> GetNames(IEnumerable<Ability> list)
        {
            List<string> names = [];
            foreach (var abi in list)
            {
                if (_abi2name.TryGetValue(abi, out var name))
                {
                    names.Add(name);
                }
            }
            return names;
        }

        public static Ability GetAbility(this string? name) => GetEnumValue(name, _name2abi);
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

        private static readonly Dictionary<string, Ability> _name2abi = CreateName2Abi();
        private static Dictionary<string, Ability> CreateName2Abi()
        {
            var dic = _abi2name.Invert();
            AppendEnglishNames(dic);
            return dic;
        }
    }
}
