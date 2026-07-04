using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        public const string Monster = "モンスター";
        public const string Spell = "魔法";
        public const string Trap = "罠";

        public const string Main = "メイン";
        public const string Fusion = "融合";
        public const string Ritual = "儀式";
        public const string Synchro = "シンクロ";
        public const string Xyz = "エクシーズ";
        public const string Link = "リンク";
        public const string Token = "トークン";

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

        public const string SpellMonster = $"{Spell}{Monster}";
        public const string TrapMonster = $"{Trap}{Monster}";
        public const string ContinuousTrapMonster = $"{Continuous_Trap}{Monster}";

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

        private static readonly Dictionary<string, CardType>.AlternateLookup<ReadOnlySpan<char>> _name2cType = CreateName2CType();
        private static Dictionary<string, CardType>.AlternateLookup<ReadOnlySpan<char>> CreateName2CType()
        {
            var dic = CreateInvertedDictionary(_cType2name);

            // "FusionMonster"ではなく"Fusion"と書かれていた場合のために
            dic[nameof(Fusion)] = CardType.Fusion_Monster;
            dic[nameof(Ritual)] = CardType.Ritual_Monster;
            dic[nameof(Synchro)] = CardType.Synchro_Monster;
            dic[nameof(Xyz)] = CardType.Xyz_Monster;
            dic[nameof(Link)] = CardType.Link_Monster;

            // TCGでの表記
            dic["QuickPlaySpell"] = CardType.Quick_Spell;

            return dic;
        }

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

        public static CardType GetCardType(this ReadOnlySpan<char> name) => GetEnumValue(name, _name2cType);
        public static bool TryGetCardType(this ReadOnlySpan<char> name, out CardType type) => TryGetEnumValue(name, _name2cType, out type);

    }
}
