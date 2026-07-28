using LivreNoirLibrary.ObjectModel;
using System;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class Card
    {
        public string CardTypeText => Vocab.GetName(CardType, true);
        public string CTypeText => Vocab.GetName(CardType, false);
        public string AttributeText => CardType.IsMonster() ? Vocab.GetName(Attribute) : Vocab.None;
        public string AttrText => CardType.IsMonster() ? Vocab.GetShortName(Attribute) : Vocab.None;
        public string MonsterTypeText => CardType.IsMonster() ? Vocab.GetName(MonsterType) : Vocab.None;
        public string MTypeText => CardType.IsMonster() ? Vocab.GetShortName(MonsterType) : Vocab.None;
        public string EffectText => HasEffect ? "◯" : "";
        public string AbilityText => Vocab.GetName(Ability);
        public string AbilityTextWithType => GetAbilityTextWithType(true);
        public string LevelText => CardType.IsMonster() ? Vocab.GetStatusText(Level) : Vocab.None;
        public string AtkText => CardType.IsMonster() ? Vocab.GetStatusText(Atk) : Vocab.None;
        public string DefText => CardType.HasDef() ? Vocab.GetStatusText(Def) : Vocab.None;
        public string MonsterInfoText => CardType.IsMonster() ? GetMonsterInfoText() : "";
        public string StatusText => CardType.IsMonster() ? GetStatusText() : "";
        public string FullText => GetFullText();

        public string GetAbilityTextWithType(bool addNone)
        {
            using var o = ObjectPool.RentStringBuilder(out var sb);
            var list = Vocab.GetNames(Ability);
            sb.AppendJoin(Vocab.Ability_Separator, list);
            if (HasEffect)
            {
                if (list.Length is > 0)
                {
                    sb.Append(Vocab.Ability_Separator);
                }
                sb.Append(Vocab.Effect);
            }
            else if (CardType is CardType.Main_Monster)
            {
                if (list.Length is > 0)
                {
                    sb.Append(Vocab.Ability_Separator);
                }
                sb.Append(Vocab.Normal);
            }
            else if (addNone && list.Length is 0)
            {
                sb.Append(Vocab.None);
            }
            return sb.ToString();
        }

        public string GetMonsterInfoText()
        {
            using var o = ObjectPool.RentStringBuilder(out var sb);
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
            using var o = ObjectPool.RentStringBuilder(out var sb);
            sb.Append(Vocab.GetLevelName(CardType));
            sb.Append($" {LevelText}");
            sb.Append(Vocab.Ability_Separator);
            sb.Append("ATK ");
            sb.Append(AtkText);
            sb.Append(Vocab.Ability_Separator);
            if (this.IsLink())
            {
                sb.Append(Vocab.GetName(this.GetLinkDirections()));
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
            using var o = ObjectPool.RentStringBuilder(out var sb);
            if (this.IsMonster())
            {
                sb.AppendLine(GetMonsterInfoText());
                sb.AppendLine(GetStatusText());
            }
            else
            {
                sb.AppendLine(CardTypeText);
            }
            sb.Append(Text);
            if (this.IsMonster() && this.IsPendulum())
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
