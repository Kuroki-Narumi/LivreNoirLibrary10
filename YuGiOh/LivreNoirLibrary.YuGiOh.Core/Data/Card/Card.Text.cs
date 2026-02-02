using LivreNoirLibrary.ObjectModel;
using System;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class Card
    {
        public string LimitText => Vocab.GetLimitText(Regulation.Instance.Get(this));
        public string CardTypeText => Vocab.GetName(CardType, true);

        public string AttributeText => Vocab.GetName(Attribute);
        public string AttrText => Vocab.GetShortName(Attribute);
        public string MonsterTypeText => Vocab.GetName(MonsterType);
        public string EffectText => HasEffect ? "◯" : "";
        public string AbilityText => Vocab.GetName(Ability);
        public string AbilityTextWithType => GetAbilityTextWithType(true);
        public string LevelText => Level is < 0 ? Vocab.Unknown : Level.ToString();
        public string AtkText => Atk is < 0 ? Vocab.Unknown : Atk.ToString();
        public string DefText => Def is < 0 ? Vocab.Unknown : Def.ToString();
        public string MonsterInfoText => IsMosnter() ? GetMonsterInfoText() : "";
        public string StatusText => IsMosnter() ? GetStatusText() : "";
        public string FullText => GetFullText();

        public string GetAbilityTextWithType(bool addNone)
        {
            var list = Vocab.GetNames(Ability);
            if (HasEffect)
            {
                list.Add(Vocab.Effect);
            }
            else if (CardType is CardType.Main_Monster)
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
            using var o = ObjectPool.Rent<StringBuilder>();
            var sb = o.Value;
            sb.Append(AttributeText);
            sb.Append(Vocab.Ability_Separator);
            sb.Append(MonsterTypeText);
            if (CardType is not CardType.Main_Monster)
            {
                sb.Append(Vocab.Ability_Separator);
                sb.Append(Vocab.GetName(CardType));
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
            using var o = ObjectPool.Rent<StringBuilder>();
            var sb = o.Value;
            sb.Append(Vocab.GetLevelName(CardType));
            sb.Append($" {LevelText}");
            sb.Append(Vocab.Ability_Separator);
            sb.Append("ATK ");
            sb.Append(AtkText);
            sb.Append(Vocab.Ability_Separator);
            if (CardType is CardType.Link_Monster)
            {
                sb.Append(Vocab.GetName((LinkDirection)Def));
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
            using var o = ObjectPool.Rent<StringBuilder>();
            var sb = o.Value;
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
                if (!string.IsNullOrEmpty(PendulumText))
                {
                    sb.AppendLine($"---- P{Vocab.Effect} ({Vocab.Scale} {PendulumScale}) ----");
                    sb.Append(PendulumText);
                }
                else
                {
                    sb.Append($"---- {Vocab.Scale_Short} {PendulumScale} ----");
                }
            }
            return sb.ToString();
        }
    }
}
