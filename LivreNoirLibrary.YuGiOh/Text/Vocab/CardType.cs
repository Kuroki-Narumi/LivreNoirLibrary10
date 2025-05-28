using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        public const string Monster = "モンスター";
        public const string Spell = "魔法";
        public const string Trap = "罠";

        public const string Token = "トークン";

        public const string Main = "メイン";
        public const string Fusion = "融合";
        public const string Ritual = "儀式";
        public const string Synchro = "シンクロ";
        public const string Xyz = "エクシーズ";
        public const string Pendulum = "ペンデュラム";
        public const string Link = "リンク";

        public const string Normal = "通常";
        public const string Effect = "効果";
        public const string Continuous = "永続";
        public const string Field = "フィールド";
        public const string Equip = "装備";
        public const string Quick = "速攻";
        public const string Counter = "カウンター";

        public const string Normal_Spell = $"{Normal}{Spell}";
        public const string Field_Spell = $"{Field}{Spell}";
        public const string Equip_Spell = $"{Equip}{Spell}";
        public const string Continuous_Spell = $"{Continuous}{Spell}";
        public const string Quick_Spell = $"{Quick}{Spell}";
        public const string Ritual_Spell = $"{Ritual}{Spell}";
        public const string Normal_Trap = $"{Normal}{Trap}";
        public const string Continuous_Trap = $"{Continuous}{Trap}";
        public const string Counter_Trap = $"{Counter}{Trap}";

        public static string GetLevelName(CardType type) => type switch { CardType.Link_Monster => Link, CardType.Xyz_Monster => Rank, _ => Level };

        public static string GetName(this CardType value, bool appendMonster = false)
        {
            if (_cType2name.TryGetValue(value, out var name))
            {
                if (appendMonster && value is >= CardType.Fusion_Monster and < CardType.Normal_Spell)
                {
                    name += Monster;
                }
                return name;
            }
            return value.ToString();
        }

        public static CardType GetCardType(this string? name) => GetEnumValue(name, _name2cType);
        public static bool TryGetCardType(this string name, out CardType type) => TryGetEnumValue(name, _name2cType, out type);

        private static readonly Dictionary<CardType, string> _cType2name = new()
        {
            { CardType.Main_Monster,    Monster },
            { CardType.Fusion_Monster,  Fusion },
            { CardType.Ritual_Monster,  Ritual },
            { CardType.Synchro_Monster, Synchro },
            { CardType.Xyz_Monster,     Xyz },
            { CardType.Link_Monster,    Link },
            { CardType.Token,           Token },

            { CardType.Normal_Spell, Normal_Spell },
            { CardType.Field_Spell,  Field_Spell },
            { CardType.Equip_Spell,  Equip_Spell },
            { CardType.Continuous_Spell, Continuous_Spell },
            { CardType.Quick_Spell,  Quick_Spell },
            { CardType.Ritual_Spell, Ritual_Spell },

            { CardType.Normal_Trap,     Normal_Trap },
            { CardType.Continuous_Trap, Continuous_Trap },
            { CardType.Counter_Trap,    Counter_Trap },
        };

        private static readonly Dictionary<string, CardType> _name2cType = CreateName2CType();

        private static Dictionary<string, CardType> CreateName2CType()
        {
            var dic = _cType2name.Invert();
            AppendEnglishNames(dic);
            dic.Add(nameof(Fusion), CardType.Fusion_Monster);
            dic.Add(nameof(Ritual), CardType.Ritual_Monster);
            dic.Add(nameof(Synchro), CardType.Synchro_Monster);
            dic.Add(nameof(Xyz), CardType.Xyz_Monster);
            dic.Add(nameof(Link), CardType.Link_Monster);

            void AddUnderscoreRemoved(CardType t)
            {
                dic.Add(t.ToString().Replace("_", ""), t);
            }

            AddUnderscoreRemoved(CardType.Normal_Spell);
            AddUnderscoreRemoved(CardType.Field_Spell);
            AddUnderscoreRemoved(CardType.Equip_Spell);
            AddUnderscoreRemoved(CardType.Continuous_Spell);
            AddUnderscoreRemoved(CardType.Quick_Spell);
            dic.Add("QuickPlaySpell", CardType.Quick_Spell);
            AddUnderscoreRemoved(CardType.Ritual_Spell);

            AddUnderscoreRemoved(CardType.Normal_Trap);
            AddUnderscoreRemoved(CardType.Continuous_Trap);
            AddUnderscoreRemoved(CardType.Counter_Trap);
            return dic;
        }
    }
}
