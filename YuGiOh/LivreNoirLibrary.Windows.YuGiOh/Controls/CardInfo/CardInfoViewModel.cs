using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.YuGiOh.Converters;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class CardInfoViewModel(bool isReadOnly) : ObservableObjectBase
    {
        public Card? Source
        {
            get;
            set
            {
                if (SetValue(ref field, value) && value is not null)
                {
                    CopyFrom(value);
                }
            }
        }
        public Card Card { get; } = new();
        public bool IsReadOnly { get; } = isReadOnly;

        public CardType CardType
        {
            get => Card.CardType; 
            set
            {
                if (value != Card.CardType)
                {
                    Card.CardType = value;
                    this.NotifyPropertyChanged(nameof(CardType));
                    if (value.IsMonster())
                    {
                        HasMonsterInfo = true;
                        LevelName = value switch
                        {
                            CardType.Link_Monster => Vocab.Current.CInfo.Link,
                            CardType.Xyz_Monster => Vocab.Current.CInfo.Rank,
                            _ => Vocab.Current.CInfo.Level,
                        };
                        IsLevelEnabled = value is not CardType.Link_Monster;
                        IsSpecialSummonEnabled = value is CardType.Main_Monster;
                        IsTunerEnabled = value is not (CardType.Xyz_Monster or CardType.Link_Monster);
                    }
                    else
                    {
                        HasMonsterInfo = false;
                    }
                    UpdatePendulumInfo();
                }
            }
        }

        public VocabData? LevelName { get; private set => SetValue(ref field, value); }

        public bool IsLevelEnabled
        {
            get;
            set
            {
                if (SetValue(ref field, value) && !IsReadOnly)
                {
                    if (value)
                    {
                        Card.Def = 0;
                    }
                    else
                    {
                        IsFlip = false;
                        LinkDirections = (LinkDirection)255;
                        LinkDirections = 0;
                    }
                }
            }
        }

        public bool IsSpecialSummonEnabled
        {
            get; 
            private set
            {
                if (SetValue(ref field, value) && !IsReadOnly && !value)
                {
                    IsSpecialSummon = false;
                }
            }
        }

        public bool IsTunerEnabled
        {
            get;
            private set
            {
                if (SetValue(ref field, value) && !IsReadOnly && !value)
                {
                    IsTuner = false;
                }
            }
        }

        public bool HasMonsterInfo { get; private set => SetValue(ref field, value); }
        public bool HasPendulumInfo { get; private set => SetValue(ref field, value); }

        public bool IsPendulum
        {
            get => Card.IsPendulum(); 
            set
            {
                SetFlag(Ability.Pendulum, value);
                UpdatePendulumInfo();
            }
        }

        private void UpdatePendulumInfo()
        {
            HasPendulumInfo = Card.CardType.IsMonster() && Card.IsPendulum();
        }

        public bool IsSpecialSummon { get => Card.IsSpecualSummon(); set => SetFlag(Ability.SpecialSummon, value); }
        public bool IsTuner { get => Card.IsTuner(); set => SetFlag(Ability.Tuner, value); }
        public bool IsFlip { get => Card.IsFlip(); set => SetFlag(Ability.Flip, value); }
        public bool IsSpirit { get => Card.IsSpirit(); set => SetFlag(Ability.Spirit, value); }
        public bool IsToon { get => Card.IsToon(); set => SetFlag(Ability.Toon, value); }
        public bool IsGemini { get => Card.IsGemini(); set => SetFlag(Ability.Gemini, value); }
        public bool IsUnion { get => Card.IsUnion(); set => SetFlag(Ability.Union, value); }

        private void SetFlag(Ability ability, bool value, [CallerMemberName]string propertyName = "")
        {
            if (value ^ ((Card.Ability & ability) is not 0))
            {
                if (value)
                {
                    Card.Ability |= ability;
                }
                else
                {
                    Card.Ability &= ~ability;
                }
                this.NotifyPropertyChanged(propertyName);
            }
        }

        public LinkDirection LinkDirections
        {
            get => Card.GetLinkDirections(); 
            set
            {
                if (Card.SetLinkDirections(value))
                {
                    this.NotifyPropertyChanged(nameof(LinkDirections));
                }
            }
        }

        public RelatedTextCollection RelatedTexts { get; } = new();

        public void CopyFrom(Card card)
        {
            CardType = card.CardType;
            Card.CopyFrom(card);
            Card.LimitCount = card.LimitCount;
            UpdatePendulumInfo();
            this.NotifyPropertyChanged(nameof(IsPendulum));
            this.NotifyPropertyChanged(nameof(IsSpecialSummon));
            this.NotifyPropertyChanged(nameof(IsTuner));
            this.NotifyPropertyChanged(nameof(IsFlip));
            this.NotifyPropertyChanged(nameof(IsSpirit));
            this.NotifyPropertyChanged(nameof(IsToon));
            this.NotifyPropertyChanged(nameof(IsGemini));
            this.NotifyPropertyChanged(nameof(IsUnion));
            this.NotifyPropertyChanged(nameof(LinkDirections));
            if (IsReadOnly)
            {
                RelatedTexts.Load(card);
            }
        }

        public void CopyTo(Card card)
        {
            var src = Card;
            if (!HasMonsterInfo)
            {
                src.Attribute = 0;
                src.MonsterType = 0;
                src.HasEffect = false;
                src.Ability = 0;
                src.Level = -1;
                src.Atk = -1;
                src.Def = -1;
            }
            if (!HasPendulumInfo)
            {
                src.PendulumScale = -1;
                src.PendulumText = "";
            }
            card.CopyFrom(src);
        }
    }
}
