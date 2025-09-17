using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh
{
    [JsonConverter(typeof(AbilityJsonConverter))]
    [Flags]
    public enum Ability
    {
        Normal = 0,   // 通常及び効果を持たないモンスター
        Effect = 1,   // 効果

        SpecialSummon = 0b_1_0000_0000, // 特殊召喚
        Pendulum = 0b_1000_0000, // ペンデュラム
        Toon     = 0b_0100_0000, // トゥーン
        Gemini   = 0b_0010_0000, // デュアル
        Union    = 0b_0001_0000, // ユニオン
        Spirit   = 0b_0000_1000, // スピリット
        Tuner    = 0b_0000_0100, // チューナー
        Flip     = 0b_0000_0010, // リバース
    }
}
