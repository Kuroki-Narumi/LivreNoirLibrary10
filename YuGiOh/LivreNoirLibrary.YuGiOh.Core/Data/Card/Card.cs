using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh.Data
{
    [JsonConverter(typeof(ViewModelCardJsonConverter))]
    public partial class Card() : ObservableObjectBase, IComparable<Card>, ICard
    {
        public static Card Dummy { get; } = new();

        public Card ThisCard => this;

        public int Id { get; set => SetValue(ref field, value, nameof(ThisCard)); }
        public string Name { get; set => SetValue(ref field, value); } = "";
        public string Ruby { get; set => SetValue(ref field, value); } = "";
        public string EnName { get; set => SetValue(ref field, value); } = "";
        public CardType CardType { get;  set => SetValue(ref field, value, 
            [nameof(CardTypeText), nameof(AbilityTextWithType), nameof(DefText), nameof(FullText), nameof(Icon), nameof(LinkIcon), nameof(FrameBrush)]); }
        public string Text { get; set => SetValue(ref field, value.ReplaceLineEndings("\n"), [nameof(FullText)], OnTextChanged); } = "";
        public bool Unusable { get; set => SetValue(ref field, value, [nameof(LimitIcon)]); }

        public Attribute Attribute { get; set => SetValue(ref field, value, [nameof(AttributeText), nameof(AttrText), nameof(MonsterInfoText), nameof(FullText), nameof(AttributeIcon)]); }
        public MonsterType MonsterType { get; set => SetValue(ref field, value, [nameof(MonsterTypeText), nameof(MonsterInfoText), nameof(FullText)]); }
        public bool HasEffect { get; set => SetValue(ref field, value, 
            [nameof(EffectText), nameof(AbilityText), nameof(AbilityTextWithType), nameof(MonsterInfoText), nameof(FullText), nameof(Icon), nameof(LinkIcon), nameof(FrameBrush)]); }
        public Ability Ability { get; set => SetValue(ref field, value, 
            [nameof(AbilityText), nameof(AbilityTextWithType), nameof(MonsterInfoText), nameof(FullText), nameof(TunerIcon), nameof(FrameBrush)]); }
        public int Level { get; set => SetValue(ref field, value, [nameof(LevelText), nameof(StatusText), nameof(FullText)]); } = -1;
        public int Atk { get; set => SetValue(ref field, value, [nameof(AtkText), nameof(StatusText), nameof(FullText)]); } = -1;
        public int Def { get; set => SetValue(ref field, value, [nameof(DefText), nameof(StatusText), nameof(FullText), nameof(LinkIcon)]); } = -1;

        public int PendulumScale { get; set => SetValue(ref field, value, [nameof(FullText)]); } = -1;
        public string PendulumText { get; set => SetValue(ref field, value.ReplaceLineEndings("\n"), [nameof(FullText)], OnTextChanged); } = "";

        public PackInfoCollection PackInfo { get; } = [];

        public Card(Serializable.Card card) : this()
        {
            Id = card.Id;
            Name = card.Name ?? "";
            Ruby = card.Ruby ?? "";
            EnName = card.EnName ?? "";
            CardType = card.CardType;
            Text = card.Text ?? "";
            Unusable = card.Unusable ?? false;
            if (card.MonsterInfo is Serializable.MonsterInfo minfo)
            {
                Attribute = minfo.Attribute;
                MonsterType = minfo.Type;
                HasEffect = minfo.HasEffect ?? false;
                Ability = minfo.Ability ?? 0;
                Level = minfo.Level;
                Atk = minfo.Atk;
                Def = minfo.Def;
            }
            if (card.PendulumInfo is Serializable.PendulumInfo pinfo)
            {
                PendulumScale = pinfo.Scale;
                PendulumText = pinfo.Text ?? "";
            }
        }

        private Card(PackInfoCollection packInfos) : this()
        {
            PackInfo.Load(packInfos);
        }

        public int CompareTo(Card? other) => other is not null ? Id.CompareTo(other.Id) : 1;

        public Card Clone() => new(PackInfo)
        {
            Id = Id,
            Name = Name,
            Ruby = Ruby,
            EnName = EnName,
            CardType = CardType,
            Text = Text,
            Unusable = Unusable,
            Attribute = Attribute,
            MonsterType = MonsterType,
            HasEffect = HasEffect,
            Ability = Ability,
            Level = Level,
            Atk = Atk,
            Def = Def,
            PendulumScale = PendulumScale,
            PendulumText = PendulumText,
        };

        public void CopyFrom(Card source)
        {
            Id = source.Id;
            Name = source.Name;
            Ruby = source.Ruby;
            EnName = source.EnName;
            CardType = source.CardType;
            Text = source.Text;
            Unusable = source.Unusable;
            Attribute = source.Attribute;
            MonsterType = source.MonsterType;
            HasEffect = source.HasEffect;
            Ability = source.Ability;
            Level = source.Level;
            Atk = source.Atk;
            Def = source.Def;
            PendulumScale = source.PendulumScale;
            PendulumText = source.PendulumText;
            PackInfo.Load(source.PackInfo);
        }

        private void OnTextChanged()
        {
            ClearRelatedText();
            SendPropertyChanged(nameof(RelatedList));
        }

        public void NotifyLimitChanged()
        {
            SendPropertyChanged(nameof(LimitIcon));
            SendPropertyChanged(nameof(LimitText));
        }
    }
}
