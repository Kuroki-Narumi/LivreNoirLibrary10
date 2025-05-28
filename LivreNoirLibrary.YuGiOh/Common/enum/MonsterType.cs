using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh
{
    [JsonConverter(typeof(MonsterTypeJsonConverter))]
    public enum MonsterType
    {
        None,
        Spellcaster,  // 魔法使い族
        Dragon,       // ドラゴン族
        Zombie,       // アンデット族
        Warrior,      // 戦士族
        BeastWarrior, // 獣戦士族
        Beast,        // 獣族
        WingedBeast,  // 鳥獣族
        Machine,      // 機械族
        Fiend,        // 悪魔族
        Fairy,        // 天使族
        Insect,       // 昆虫族
        Dinosaur,     // 恐竜族
        Reptile,      // 爬虫類族
        Fish,         // 魚族
        SeaSerpent,   // 海竜族
        Aqua,         // 水族
        Pyro,         // 炎族
        Thunder,      // 雷族
        Rock,         // 岩石族
        Plant,        // 植物族
        Psychic,      // サイキック族
        Wyrm,         // 幻竜族
        Cyberse,      // サイバース族
        Illusion,     // 幻想魔族
        DivineBeast,  // 幻神獣族
        CreatorGod,   // 創造神族
    }
}
