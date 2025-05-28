using System;
using System.Windows.Media;
using LivreNoirLibrary.YuGiOh.Controls;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public partial class Card
    {
        public bool Usable
        {
            get => !_unusable;
            set => Unusable = !value;
        }

        public LinkDirection LinkDirection
        {
            get => (LinkDirection)_def;
            set
            {
                Def = (int)value;
                Level = value.GetCount();
            }
        }

        public void ValidateLink()
        {
            Def = Math.Clamp(_def, 0, 255);
            Level = ((LinkDirection)_def).GetCount();
            Ability &= ~(Ability.Pendulum | Ability.Flip);
        }

        public void OnLimitChanged()
        {
            SendPropertyChanged(nameof(LimitText));
            SendPropertyChanged(nameof(LimitIcon));
        }

        public bool IsMosnter() => _cardType.IsMonster();
        public bool IsSpell() => _cardType.IsSpell();
        public bool IsTrap() => _cardType.IsTrap();
        public bool IsMainDeck() => _cardType.IsMainDeck();

        public bool IsMainMonster() => _cardType.IsMainMonster();
        public bool IsFusion() => _cardType.IsFusion();
        public bool IsRitual() => _cardType.IsRitual();
        public bool IsSynchro() => _cardType.IsSynchro();
        public bool IsXyz() => _cardType.IsXyz();
        public bool IsLink() => _cardType.IsLink();
        public bool IsToken() => _cardType.IsToken();
        public bool HasLevel() => _cardType.HasLevel();
        public bool HasDef() => _cardType.HasDef();
        public bool IsMainDeckMonster() => _cardType.IsMainDeckMonster();
        public bool IsExtraDeck() => _cardType.IsExtraDeck();

        public bool IsSpecialSummon { get => _ability.IsSpecualSummon(); set => SetAbility(Ability.SpecialSummon, value); }
        public bool IsPendulum { get => _ability.IsPendulum(); set => SetAbility(Ability.Pendulum, value); }
        public bool IsToon { get => _ability.IsToon(); set => SetAbility(Ability.Toon, value); }
        public bool IsGemini { get => _ability.IsGemini(); set => SetAbility(Ability.Gemini, value); }
        public bool IsUnion { get => _ability.IsUnion(); set => SetAbility(Ability.Union, value); }
        public bool IsSpirit { get => _ability.IsSpirit(); set => SetAbility(Ability.Spirit, value); }
        public bool IsTuner { get => _ability.IsTuner(); set => SetAbility(Ability.Tuner, value); }
        public bool IsFlip { get => _ability.IsFlip(); set => SetAbility(Ability.Flip, value); }

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

        public bool IsOcgReleased() => _packInfo.ContainsOcg();
        public bool IsTcgReleased() => _packInfo.ContainsTcg();
        public string GetNumber(string pid) => _packInfo.GetNumber(pid);

        public DrawingImage Icon => Icons.GetCardIcon(GetFrameType());
        public DrawingImage? LimitIcon => Icons.GetLimitIcon(Regulation.Instance.Get(this));
        public DrawingImage? AttrIcon => Icons.GetAttrIcon(_attribute);
        public DrawingImage? TunerIcon => _ability.IsTuner() ? Icons.TunerIcon : null;
        public DrawingImage LinkIcon => IsLink() ? Icons.GetLinkIcon(LinkDirection) : Icon;
        public Brush FrameBrush => Icons.GetFrameBrush(GetFrameType());

        public CardIconType GetFrameType() => Icons.GetIconType(_cardType, _effect, _ability);
    }
}
