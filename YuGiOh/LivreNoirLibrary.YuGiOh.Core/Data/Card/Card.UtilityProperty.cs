using System;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class Card
    {
        public bool Usable
        {
            get => !Unusable;
            set => Unusable = !value;
        }

        public LinkDirection LinkDirection
        {
            get => IsLink() ? (LinkDirection)Def : 0;
            set
            {
                CardType = CardType.Link_Monster;
                ValidateLink((int)value);
            }
        }

        public void ValidateLink(int direction)
        {
            Def = Math.Clamp(direction, 0, 255);
            Level = ((LinkDirection)Def).GetCount();
            Ability &= ~(Ability.Pendulum | Ability.Flip);
        }

        public void OnLimitChanged()
        {
            SendPropertyChanged(nameof(LimitText));
        }

        public bool IsMosnter() => CardType.IsMonster();
        public bool IsSpell() => CardType.IsSpell();
        public bool IsTrap() => CardType.IsTrap();
        public bool IsMainDeck() => CardType.IsMainDeck();

        public bool IsMainMonster() => CardType.IsMainMonster();
        public bool IsFusion() => CardType.IsFusion();
        public bool IsRitual() => CardType.IsRitual();
        public bool IsSynchro() => CardType.IsSynchro();
        public bool IsXyz() => CardType.IsXyz();
        public bool IsLink() => CardType.IsLink();
        public bool IsToken() => CardType.IsToken();
        public bool HasLevel() => CardType.HasLevel();
        public bool HasDef() => CardType.HasDef();
        public bool IsMainDeckMonster() => CardType.IsMainDeckMonster();
        public bool IsExtraDeck() => CardType.IsExtraDeck();

        public bool IsSpecialSummon { get => Ability.IsSpecualSummon(); set => SetAbility(Ability.SpecialSummon, value); }
        public bool IsPendulum { get => Ability.IsPendulum(); set => SetAbility(Ability.Pendulum, value); }
        public bool IsToon { get => Ability.IsToon(); set => SetAbility(Ability.Toon, value); }
        public bool IsGemini { get => Ability.IsGemini(); set => SetAbility(Ability.Gemini, value); }
        public bool IsUnion { get => Ability.IsUnion(); set => SetAbility(Ability.Union, value); }
        public bool IsSpirit { get => Ability.IsSpirit(); set => SetAbility(Ability.Spirit, value); }
        public bool IsTuner { get => Ability.IsTuner(); set => SetAbility(Ability.Tuner, value); }
        public bool IsFlip { get => Ability.IsFlip(); set => SetAbility(Ability.Flip, value); }

        private void SetAbility(Ability abi, bool value)
        {
            if (value)
            {
                Ability |= abi;
            }
            else
            {
                Ability &= ~abi;
            }
        }

        public bool IsOcgReleased() => PackInfo.ContainsOcg;
        public bool IsTcgReleased() => PackInfo.ContainsTcg;
    }
}
