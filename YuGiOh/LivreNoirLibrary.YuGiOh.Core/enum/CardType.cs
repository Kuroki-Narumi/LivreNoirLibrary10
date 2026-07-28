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
        Token,          // モンスタートークン

        Normal_Spell = 0x10, // 通常魔法
        Field_Spell,  // フィールド魔法
        Equip_Spell,  // 装備魔法
        Continuous_Spell, // 永続魔法
        Quick_Spell,  // 速攻魔法
        Ritual_Spell, // 儀式魔法

        Normal_Trap = 0x20, // 通常罠
        Continuous_Trap, // 永続罠
        Counter_Trap,    // カウンター罠

        MonsterLike = 0x40, // モンスター化したカード
        Normal_Spell_Monster = MonsterLike | Normal_Spell,
        Field_Spell_Monster = MonsterLike | Field_Spell,
        Equip_Spell_Monster = MonsterLike | Equip_Spell,
        Continuous_Spell_Monster = MonsterLike | Continuous_Spell,
        Quick_Spell_Monster = MonsterLike | Quick_Spell,
        Ritual_Spell_Monster = MonsterLike | Ritual_Spell,

        Normal_Trap_Monster = MonsterLike | Normal_Trap,
        Continuous_Trap_Monster = MonsterLike | Continuous_Trap,
        Counter_Trap_Monster = MonsterLike | Counter_Trap,

        Type_Filter = 0x30, // 種類判別用のフィルタ
        MonsterLike_Filter = 0x3f // モンスター化したカードから元のカードタイプに戻すフィルタ
    }
}
