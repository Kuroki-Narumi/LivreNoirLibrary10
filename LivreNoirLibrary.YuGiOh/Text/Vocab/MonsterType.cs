using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

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

        public static string GetName(this MonsterType value) => GetEnumName(value, _mType2name);
        public static string GetShortName(this MonsterType value) => GetEnumName(value, _mType2name_short);
        public static MonsterType GetMonsterType(this string? name) => GetEnumValue(name, _name2mType);
        public static bool TryGetMonsterType(this string name, out MonsterType type) => TryGetEnumValue(name, _name2mType, out type);

        private static readonly Dictionary<MonsterType, string> _mType2name = new()
        {
            { YuGiOh.MonsterType.Spellcaster, MT_Spellcaster },
            { YuGiOh.MonsterType.Dragon, MT_Dragon },
            { YuGiOh.MonsterType.Zombie, MT_Zombie },
            { YuGiOh.MonsterType.Warrior, MT_Warrior },
            { YuGiOh.MonsterType.BeastWarrior, MT_BeastWarrior },
            { YuGiOh.MonsterType.Beast, MT_Beast },
            { YuGiOh.MonsterType.WingedBeast, MT_WingedBeast },
            { YuGiOh.MonsterType.Machine, MT_Machine },
            { YuGiOh.MonsterType.Fiend, MT_Fiend },
            { YuGiOh.MonsterType.Fairy, MT_Fairy },
            { YuGiOh.MonsterType.Insect, MT_Insect },
            { YuGiOh.MonsterType.Dinosaur, MT_Dinosaur },
            { YuGiOh.MonsterType.Reptile, MT_Reptile },
            { YuGiOh.MonsterType.Fish, MT_Fish },
            { YuGiOh.MonsterType.SeaSerpent, MT_SeaSerpent },
            { YuGiOh.MonsterType.Aqua, MT_Aqua },
            { YuGiOh.MonsterType.Pyro, MT_Pyro },
            { YuGiOh.MonsterType.Thunder, MT_Thunder },
            { YuGiOh.MonsterType.Rock, MT_Rock },
            { YuGiOh.MonsterType.Plant, MT_Plant },
            { YuGiOh.MonsterType.Psychic, MT_Psychic },
            { YuGiOh.MonsterType.Wyrm, MT_Wyrm },
            { YuGiOh.MonsterType.Cyberse, MT_Cyberse },
            { YuGiOh.MonsterType.Illusion, MT_Illusion },
            { YuGiOh.MonsterType.DivineBeast, MT_DivineBeast},
            { YuGiOh.MonsterType.CreatorGod, MT_CreatorGod },
        };

        private static readonly Dictionary<MonsterType, string> _mType2name_short = new()
        {
            { YuGiOh.MonsterType.Spellcaster, Spellcaster },
            { YuGiOh.MonsterType.Dragon, Dragon },
            { YuGiOh.MonsterType.Zombie, Zombie },
            { YuGiOh.MonsterType.Warrior, Warrior },
            { YuGiOh.MonsterType.BeastWarrior, BeastWarrior },
            { YuGiOh.MonsterType.Beast, Beast },
            { YuGiOh.MonsterType.WingedBeast, WingedBeast },
            { YuGiOh.MonsterType.Machine, Machine },
            { YuGiOh.MonsterType.Fiend, Fiend },
            { YuGiOh.MonsterType.Fairy, Fairy },
            { YuGiOh.MonsterType.Insect, Insect },
            { YuGiOh.MonsterType.Dinosaur, Dinosaur },
            { YuGiOh.MonsterType.Reptile, Reptile },
            { YuGiOh.MonsterType.Fish, Fish },
            { YuGiOh.MonsterType.SeaSerpent, SeaSerpent },
            { YuGiOh.MonsterType.Aqua, Aqua },
            { YuGiOh.MonsterType.Pyro, Pyro },
            { YuGiOh.MonsterType.Thunder, Thunder },
            { YuGiOh.MonsterType.Rock, Rock },
            { YuGiOh.MonsterType.Plant, Plant },
            { YuGiOh.MonsterType.Psychic, Psychic },
            { YuGiOh.MonsterType.Wyrm, Wyrm },
            { YuGiOh.MonsterType.Cyberse, Cyberse },
            { YuGiOh.MonsterType.Illusion, Illusion },
            { YuGiOh.MonsterType.DivineBeast, DivineBeast},
            { YuGiOh.MonsterType.CreatorGod, CreatorGod },
        };

        private static readonly Dictionary<string, MonsterType> _name2mType = CreateName2MType();

        private static Dictionary<string, MonsterType> CreateName2MType()
        {
            var dic = _mType2name.Invert();
            foreach (var (type, name) in _mType2name_short)
            {
                dic.Add(name, type);
            }
            AppendEnglishNames(dic);
            dic.Add($"{Illusion}Type", YuGiOh.MonsterType.Illusion);
            return dic;
        }
    }
}
