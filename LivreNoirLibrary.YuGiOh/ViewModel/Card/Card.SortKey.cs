using System;
using System.Collections.Generic;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public partial class Card
    {
        private const int Padding = 0x10000;
        public const string Ruby_ZZZ = "ンンンンンンンン";
        public const string EnName_ZZZ = "ZZZZZZZZ";

        public string RubyForSort => string.IsNullOrEmpty(_ruby) ? Ruby_ZZZ : _ruby;
        public string EnNameForSort => string.IsNullOrEmpty(_enName) ? EnName_ZZZ : _enName;
        public string RubyForSortD => _ruby;
        public string EnNameForSortD => _enName;

        public int NameLength => _name.Length;
        public int RubyLength => string.IsNullOrEmpty(_ruby) ? Padding : _ruby.Length;
        public int RubyLengthD => string.IsNullOrEmpty(_ruby) ? -Padding : _ruby.Length;
        public int EnNameLength => string.IsNullOrEmpty(_enName) ? Padding : _enName.Length;
        public int EnNameLengthD => string.IsNullOrEmpty(_enName) ? -Padding : _enName.Length;
        public int TextLength => TextLengthConverter.GetLength(_text);
        public int PendulumTextLength => (IsMosnter() && IsPendulum) ? TextLengthConverter.GetLength(_pendulumText) : Padding;
        public int PendulumTextLengthD => (IsMosnter() && IsPendulum) ? TextLengthConverter.GetLength(_pendulumText) : -Padding;

        private readonly Dictionary<CardType, int> _type_index_list = new()
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
                var result = _type_index_list[_cardType];
                if (IsMosnter())
                {
                    if (_cardType is 0 && !_effect)
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

        public int TypeIdIndex => (TypeIndex + _level) * Padding + Id;
        public Attribute AttributeIndex => IsMosnter() ? _attribute : (Attribute)Padding;
        public Attribute AttributeIndexD => IsMosnter() ? _attribute : (Attribute)(-Padding);
        public MonsterType MonsterTypeIndex => IsMosnter() ? _monsterType : (MonsterType)Padding;
        public MonsterType MonsterTypeIndexD => IsMosnter() ? _monsterType : (MonsterType)(-Padding);
        public Ability AbilityIndex => IsMosnter() ? _ability : (Ability)Padding;
        public Ability AbilityIndexD => IsMosnter() ? _ability : (Ability)(-Padding);

        public int EffectIndex => IsMosnter() ? _effect ? 0 : 1 : 2;
        public int TunerIndex => IsMosnter() ? IsTuner ? 0 : 1 : 2;
        public int LevelIndex => IsMosnter() ? _level : Padding;
        public int LevelIndexD => IsMosnter() ? _level : -Padding;
        public int AtkIndex => IsMosnter() ? _atk : Padding;
        public int AtkIndexD => IsMosnter() ? _atk : -Padding;
        public int DefIndex => HasDef()? _def : Padding;
        public int DefIndexD => HasDef() ? _def : -Padding;
        public int ScaleIndex => (IsMosnter() && IsPendulum) ? _pendulumScale : Padding;
        public int ScaleIndexD => (IsMosnter() && IsPendulum) ? _pendulumScale : -Padding;

        public DateTime FirstDateOcg => _packInfo.GetFirstDateOcg(true);
        public DateTime FirstDateOcgD => _packInfo.GetFirstDateOcg(false);
        public DateTime LastDateOcg => _packInfo.GetLastDateOcg(true);
        public DateTime LastDateOcgD => _packInfo.GetLastDateOcg(false);
        public DateTime FirstDateTcg => _packInfo.GetFirstDateTcg(true);
        public DateTime FirstDateTcgD => _packInfo.GetFirstDateTcg(false);
        public DateTime LastDateTcg => _packInfo.GetLastDateTcg(true);
        public DateTime LastDateTcgD => _packInfo.GetLastDateTcg(false);
        public int PackCount => _packInfo.Count;
        public int PackCountOcg => _packInfo.OcgCount;
        public int PackCountTcg => _packInfo.TcgCount;
    }
}
