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

        public int Id { get; set => SetValue(ref field, value); }
        public string Name { get; set => SetValue(ref field, value); } = "";
        public string Ruby { get; set => SetValue(ref field, value); } = "";
        public string EnName { get; set => SetValue(ref field, value); } = "";
        public CardType CardType
        { 
            get; 
            set => SetValue(ref field, value, [nameof(CardTypeText), nameof(AbilityTextWithType), nameof(DefText), nameof(FullText)]); 
        }
        public string Text { get; set => SetValue(ref field, value.ReplaceLineEndings("\n"), [nameof(FullText)]); } = "";
        public bool Unusable { get; set => SetValue(ref field, value, [nameof(Usable)]); }

        public Attribute Attribute { get; set => SetValue(ref field, value, [nameof(AttributeText), nameof(AttrText), nameof(MonsterInfoText), nameof(FullText)]); }
        public MonsterType MonsterType { get; set => SetValue(ref field, value, [nameof(MonsterTypeText), nameof(MonsterInfoText), nameof(FullText)]); }
        public bool HasEffect { get; set => SetValue(ref field, value, [nameof(EffectText), nameof(AbilityText), nameof(AbilityTextWithType), nameof(MonsterInfoText), nameof(FullText)]); }
        public Ability Ability { get; set => SetValue(ref field, value, [nameof(AbilityText), nameof(AbilityTextWithType), nameof(MonsterInfoText), nameof(FullText)]); }
        public int Level { get; set => SetValue(ref field, value, [nameof(LevelText), nameof(StatusText), nameof(FullText)]); } = -1;
        public int Atk { get; set => SetValue(ref field, value, [nameof(AtkText), nameof(StatusText), nameof(FullText)]); } = -1;
        public int Def { get; set => SetValue(ref field, value, [nameof(LinkDirection), nameof(DefText), nameof(StatusText), nameof(FullText)]); } = -1;

        public int PendulumScale { get; set => SetValue(ref field, value, [nameof(FullText)]); } = -1;
        public string PendulumText { get; set => SetValue(ref field, value.ReplaceLineEndings("\n"), [nameof(FullText)]); } = "";

        public PackInfoCollection PackInfo { get; set => SetValue(ref field, value); } = [];

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
            PackInfo = new(card.PackInfo);
        }

        public int CompareTo(Card? other) => other is not null ? Id.CompareTo(other.Id) : 1;

        public Card Clone() => new()
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
            PackInfo = PackInfo,
        };

        public void Update(Card source)
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
            PackInfo = source.PackInfo;
        }

        private void OnTextChanged()
        {
            _related_created = false;
            SendPropertyChanged(nameof(RelatedList));
            TextConvert.RemoveTextCache(this);
        }
        private void OnPendulumTextChanged() => OnTextChanged();
    }
}
