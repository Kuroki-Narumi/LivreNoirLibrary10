using System;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public partial class Card
    {
        public string LimitText => Vocab.GetLimitText(Regulation.Instance.Get(this));
        public string CardTypeText => Vocab.GetName(_cardType, true);

        public string AttributeText => Vocab.GetName(_attribute);
        public string AttrText => Vocab.GetShortName(_attribute);
        public string MonsterTypeText => Vocab.GetName(_monsterType);
        public string EffectText => _effect ? "◯" : "";
        public string AbilityText => Vocab.GetName(_ability);
        public string AbilityTextWithType => GetAbilityTextWithType(true);
        public string LevelText => _level is < 0 ? Vocab.Unknown : _level.ToString();
        public string AtkText => _atk is < 0 ? Vocab.Unknown : _atk.ToString();
        public string DefText => _def is < 0 ? Vocab.Unknown : _def.ToString();
        public string MonsterInfoText => IsMosnter() ? GetMonsterInfoText() : "";
        public string StatusText => IsMosnter() ? GetStatusText() : "";
        public string FullText => GetFullText();

        public string GetAbilityTextWithType(bool addNone)
        {
            var list = Vocab.GetNames(_ability);
            if (_effect)
            {
                list.Add(Vocab.Effect);
            }
            else if (_cardType is CardType.Main_Monster)
            {
                list.Add(Vocab.Normal);
            }
            else if (addNone && list.Count <= 0)
            {
                list.Add(Vocab.None);
            }
            return string.Join(Vocab.Ability_Separator, list);
        }

        public string GetMonsterInfoText()
        {
            StringBuilder sb = new();
            sb.Append(AttributeText);
            sb.Append(Vocab.Ability_Separator);
            sb.Append(MonsterTypeText);
            if (_cardType is not CardType.Main_Monster)
            {
                sb.Append(Vocab.Ability_Separator);
                sb.Append(Vocab.GetName(_cardType));
            }
            var abi = GetAbilityTextWithType(false);
            if (!string.IsNullOrEmpty(abi))
            {
                sb.Append(Vocab.Ability_Separator);
                sb.Append(abi);
            }
            return sb.ToString();
        }

        public string GetStatusText()
        {
            StringBuilder sb = new();
            sb.Append(Vocab.GetLevelName(_cardType));
            sb.Append($" {LevelText}");
            sb.Append(Vocab.Ability_Separator);
            sb.Append("ATK ");
            sb.Append(AtkText);
            sb.Append(Vocab.Ability_Separator);
            if (_cardType is CardType.Link_Monster)
            {
                sb.Append(Vocab.GetName((LinkDirection)_def));
            }
            else
            {
                sb.Append("DEF ");
                sb.Append(DefText);
            }
            return sb.ToString();
        }

        public string GetFullText()
        {
            StringBuilder sb = new();
            if (IsMosnter())
            {
                sb.AppendLine(GetMonsterInfoText());
                sb.AppendLine(GetStatusText());
            }
            else
            {
                sb.AppendLine(CardTypeText);
            }
            sb.Append(Text);
            if (IsMosnter() && IsPendulum)
            {
                sb.AppendLine();
                if (!string.IsNullOrEmpty(_pendulumText))
                {
                    sb.AppendLine($"---- P{Vocab.Effect} ({Vocab.Scale} {_pendulumScale}) ----");
                    sb.Append(_pendulumText);
                }
                else
                {
                    sb.Append($"---- {Vocab.Scale_Short} {_pendulumScale} ----");
                }
            }
            return sb.ToString();
        }
    }
}
