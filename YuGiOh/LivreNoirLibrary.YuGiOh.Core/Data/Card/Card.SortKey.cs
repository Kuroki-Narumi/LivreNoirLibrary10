using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Converters;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class Card
    {
        private const int Padding = 0x10000;
        public const string Ruby_ZZZ = "ンンンンンンンン";
        public const string EnName_ZZZ = "ZZZZZZZZ";

        public string RubyForSort => string.IsNullOrEmpty(Ruby) ? Ruby_ZZZ : Ruby;
        public string EnNameForSort => string.IsNullOrEmpty(EnName) ? EnName_ZZZ : EnName;
        public string RubyForSortD => Ruby;
        public string EnNameForSortD => EnName;

        public int NameLength => Name.Length;
        public int RubyLength => string.IsNullOrEmpty(Ruby) ? Padding : Ruby.Length;
        public int RubyLengthD => string.IsNullOrEmpty(Ruby) ? -Padding : Ruby.Length;
        public int EnNameLength => string.IsNullOrEmpty(EnName) ? Padding : EnName.Length;
        public int EnNameLengthD => string.IsNullOrEmpty(EnName) ? -Padding : EnName.Length;
        public int TextLength => Text.LengthWithoutSpace();
        public int PendulumTextLength => (IsMosnter() && IsPendulum) ? PendulumText.LengthWithoutSpace() : Padding;
        public int PendulumTextLengthD => (IsMosnter() && IsPendulum) ? PendulumText.LengthWithoutSpace() : -Padding;

        private readonly Dictionary<CardType, int> Type_index_list = new()
        {
            { CardType.Main_Monster, 32 },
            { CardType.Ritual_Monster, 64 },
            { CardType.Fusion_Monster, 96 },
            { CardType.Synchro_Monster, 128 },
            { CardType.Xyz_Monster, 160 },
            { CardType.Link_Monster, 192 },

            { CardType.Normal_Spell, 224 },
            { CardType.Equip_Spell, 225 },
            { CardType.Field_Spell, 226 },
            { CardType.Ritual_Spell, 227 },
            { CardType.Continuous_Spell, 228 },
            { CardType.Quick_Spell, 229 },

            { CardType.Normal_Trap, 256 },
            { CardType.Counter_Trap, 257 },
            { CardType.Continuous_Trap, 258 },
        };

        public int TypeIndex
        {
            get
            {
                var result = Type_index_list[CardType];
                if (IsMosnter())
                {
                    if (CardType is 0 && !HasEffect)
                    {
                        result = 0;
                    }
                    if (IsPendulum)
                    {
                        result += 16;
                    }
                }
                return result;
            }
        }

        public int TypeIdIndex => (TypeIndex + Level) * Padding + Id;
        public Attribute AttributeIndex => IsMosnter() ? Attribute : (Attribute)Padding;
        public Attribute AttributeIndexD => IsMosnter() ? Attribute : (Attribute)(-Padding);
        public MonsterType MonsterTypeIndex => IsMosnter() ? MonsterType : (MonsterType)Padding;
        public MonsterType MonsterTypeIndexD => IsMosnter() ? MonsterType : (MonsterType)(-Padding);
        public Ability AbilityIndex => IsMosnter() ? Ability : (Ability)Padding;
        public Ability AbilityIndexD => IsMosnter() ? Ability : (Ability)(-Padding);

        public int EffectIndex => IsMosnter() ? HasEffect ? 0 : 1 : 2;
        public int TunerIndex => IsMosnter() ? IsTuner ? 0 : 1 : 2;
        public int LevelIndex => IsMosnter() ? Level : Padding;
        public int LevelIndexD => IsMosnter() ? Level : -Padding;
        public int AtkIndex => IsMosnter() ? Atk : Padding;
        public int AtkIndexD => IsMosnter() ? Atk : -Padding;
        public int DefIndex => HasDef()? Def : Padding;
        public int DefIndexD => HasDef() ? Def : -Padding;
        public int ScaleIndex => (IsMosnter() && IsPendulum) ? PendulumScale : Padding;
        public int ScaleIndexD => (IsMosnter() && IsPendulum) ? PendulumScale : -Padding;

        public DateTime FirstDateOcg => PackInfo.GetFirstDateOcg(true);
        public DateTime FirstDateOcgD => PackInfo.GetFirstDateOcg(false);
        public DateTime LastDateOcg => PackInfo.GetLastDateOcg(true);
        public DateTime LastDateOcgD => PackInfo.GetLastDateOcg(false);
        public DateTime FirstDateTcg => PackInfo.GetFirstDateTcg(true);
        public DateTime FirstDateTcgD => PackInfo.GetFirstDateTcg(false);
        public DateTime LastDateTcg => PackInfo.GetLastDateTcg(true);
        public DateTime LastDateTcgD => PackInfo.GetLastDateTcg(false);
        public int PackCount => PackInfo.Count;
        public int PackCountOcg => PackInfo.OcgCount;
        public int PackCountTcg => PackInfo.TcgCount;
    }
}
