using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh
{
    [JsonConverter(typeof(CardTypeJsonConverter))]
    public enum CardType
    {
        None = 0,
        Main_Monster,   // メインデッキのモンスター(儀式以外)
        Fusion_Monster, // 融合モンスター
        Ritual_Monster, // 儀式モンスター
        Synchro_Monster,// シンクロモンスター
        Xyz_Monster,    // エクシーズモンスター
        Link_Monster,   // リンクモンスター
        Token = 0x0f,   // モンスタートークン

        Normal_Spell = 0x10, // 通常魔法
        Field_Spell,  // フィールド魔法
        Equip_Spell,  // 装備魔法
        Continuous_Spell, // 永続魔法
        Quick_Spell,  // 速攻魔法
        Ritual_Spell, // 儀式魔法
        SpellMonster = 0x18, // 魔法モンスター

        Normal_Trap = 0x20, // 通常罠
        Continuous_Trap, // 永続罠
        Counter_Trap,    // カウンター罠
        TrapMonster = 0x28, // 罠モンスター
        Continuous_TrapMonster = 0x29, // 永続罠モンスター

        Type_Filter = 0xf0, // 種類判別用のフィルタ
    }
}
