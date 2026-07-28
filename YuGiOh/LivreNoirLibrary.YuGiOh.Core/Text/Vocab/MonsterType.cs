using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        public const string MonsterType = "種族";
        public const string MType_Suffix = "族";

        public const string Spellcaster = "魔法使い";
        public const string Dragon = "ドラゴン";
        public const string Zombie = "アンデット";
        public const string Warrior = "戦士";
        public const string BeastWarrior = "獣戦士";
        public const string Beast = "獣";
        public const string WingedBeast = "鳥獣";
        public const string Machine = "機械";
        public const string Fiend = "悪魔";
        public const string Fairy = "天使";
        public const string Insect = "昆虫";
        public const string Dinosaur = "恐竜";
        public const string Reptile = "爬虫類";
        public const string Fish = "魚";
        public const string SeaSerpent = "海竜";
        public const string Aqua = "水";
        public const string Pyro = "炎";
        public const string Thunder = "雷";
        public const string Rock = "岩石";
        public const string Plant = "植物";
        public const string Psychic = "サイキック";
        public const string Wyrm = "幻竜";
        public const string Cyberse = "サイバース";
        public const string Illusion = "幻想魔";
        public const string DivineBeast = "幻神獣";
        public const string CreatorGod = "創造神";

        public const string MT_Spellcaster = $"{Spellcaster}{MType_Suffix}";
        public const string MT_Dragon = $"{Dragon}{MType_Suffix}";
        public const string MT_Zombie = $"{Zombie}{MType_Suffix}";
        public const string MT_Warrior = $"{Warrior}{MType_Suffix}";
        public const string MT_BeastWarrior = $"{BeastWarrior}{MType_Suffix}";
        public const string MT_Beast = $"{Beast}{MType_Suffix}";
        public const string MT_WingedBeast = $"{WingedBeast}{MType_Suffix}";
        public const string MT_Machine = $"{Machine}{MType_Suffix}";
        public const string MT_Fiend = $"{Fiend}{MType_Suffix}";
        public const string MT_Fairy = $"{Fairy}{MType_Suffix}";
        public const string MT_Insect = $"{Insect}{MType_Suffix}";
        public const string MT_Dinosaur = $"{Dinosaur}{MType_Suffix}";
        public const string MT_Reptile = $"{Reptile}{MType_Suffix}";
        public const string MT_Fish = $"{Fish}{MType_Suffix}";
        public const string MT_SeaSerpent = $"{SeaSerpent}{MType_Suffix}";
        public const string MT_Aqua = $"{Aqua}{MType_Suffix}";
        public const string MT_Pyro = $"{Pyro}{MType_Suffix}";
        public const string MT_Thunder = $"{Thunder}{MType_Suffix}";
        public const string MT_Rock = $"{Rock}{MType_Suffix}";
        public const string MT_Plant = $"{Plant}{MType_Suffix}";
        public const string MT_Psychic = $"{Psychic}{MType_Suffix}";
        public const string MT_Wyrm = $"{Wyrm}{MType_Suffix}";
        public const string MT_Cyberse = $"{Cyberse}{MType_Suffix}";
        public const string MT_Illusion = $"{Illusion}{MType_Suffix}";
        public const string MT_DivineBeast = $"{DivineBeast}{MType_Suffix}";
        public const string MT_CreatorGod = $"{CreatorGod}{MType_Suffix}";

        private static string[] MType2Name { get; } = [
            Unknown, MT_Spellcaster, MT_Dragon, MT_Zombie, MT_Warrior, MT_BeastWarrior, MT_Beast, MT_WingedBeast, MT_Machine, MT_Fiend,
            MT_Fairy, MT_Insect, MT_Dinosaur, MT_Reptile, MT_Fish, MT_SeaSerpent, MT_Aqua, MT_Pyro, MT_Thunder, MT_Rock,
            MT_Plant, MT_Psychic, MT_Wyrm, MT_Cyberse, MT_Illusion, MT_DivineBeast, MT_CreatorGod
        ];

        private static string[] MType2ShortName { get; } = [
            Unknown, Spellcaster, Dragon, Zombie, Warrior, BeastWarrior, Beast, WingedBeast, Machine, Fiend,
            Fairy, Insect, Dinosaur, Reptile, Fish, SeaSerpent, Aqua, Pyro, Thunder, Rock,
            Plant, Psychic, Wyrm, Cyberse, Illusion, DivineBeast, CreatorGod
        ];

        private static Dictionary<string, MonsterType>.AlternateLookup<ReadOnlySpan<char>> Name2MType { get; } = CreateName2MType();
        private static Dictionary<string, MonsterType>.AlternateLookup<ReadOnlySpan<char>> CreateName2MType()
        {
            var dic = CreateInvertedDictionary<MonsterType>();
            var ary1 = MType2Name;
            var ary2 = MType2ShortName;
            foreach (var value in EnumUtils.MonsterTypes)
            {
                var index = (int)value;
                dic[ary1[index]] = value;
                dic[ary2[index]] = value;
                dic[value.ToString()] = value;
            }
            return dic;
        }

        public static string GetName(this MonsterType value) => GetEnumName(value, (int)value, MType2Name);
        public static string GetShortName(this MonsterType value) => GetEnumName(value, (int)value, MType2ShortName);
        public static MonsterType GetMonsterType(ReadOnlySpan<char> name) => GetEnumValue(name, Name2MType);
        public static bool TryGetMonsterType(ReadOnlySpan<char> name, out MonsterType type) => TryGetEnumValue(name, Name2MType, out type);
    }
}
