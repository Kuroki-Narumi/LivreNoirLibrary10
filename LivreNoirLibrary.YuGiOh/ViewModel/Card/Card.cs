using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    [JsonConverter(typeof(ViewModelCardJsonConverter))]
    public partial class Card() : ObservableObjectBase, IComparable<Card>, ICard
    {
        public static Card Dummy { get; } = new();

        [ObservableProperty]
        internal int _id;
        [ObservableProperty]
        internal string _name = "";
        [ObservableProperty]
        internal string _ruby = "";
        [ObservableProperty]
        internal string _enName = "";
        [ObservableProperty(Related = [nameof(Icon), nameof(LinkIcon), nameof(FrameBrush), nameof(CardTypeText), nameof(AbilityTextWithType), nameof(DefText), nameof(FullText)])]
        internal CardType _cardType;
        [ObservableProperty(Related = [nameof(FullText)])]
        internal string _text = "";
        [ObservableProperty(Related = [nameof(Usable)])]
        internal bool _unusable;

        [ObservableProperty(Related = [nameof(AttrIcon), nameof(AttributeText), nameof(AttrText), nameof(MonsterInfoText), nameof(FullText)])]
        internal Attribute _attribute;
        [ObservableProperty(Related = [nameof(MonsterTypeText), nameof(MonsterInfoText), nameof(FullText)])]
        internal MonsterType _monsterType;
        [ObservableProperty(Related = [nameof(EffectText), nameof(AbilityText), nameof(AbilityTextWithType), nameof(MonsterInfoText), nameof(FullText)])]
        internal bool _effect = false;
        [ObservableProperty(Related = [nameof(Icon), nameof(TunerIcon), nameof(FrameBrush), nameof(AbilityText), nameof(AbilityTextWithType), nameof(MonsterInfoText), nameof(FullText)])]
        internal Ability _ability;
        [ObservableProperty(Related = [nameof(LevelText), nameof(StatusText), nameof(FullText)])]
        internal int _level = -1;
        [ObservableProperty(Related = [nameof(AtkText), nameof(StatusText), nameof(FullText)])]
        internal int _atk = -1;
        [ObservableProperty(Related = [nameof(LinkDirection), nameof(DefText), nameof(LinkIcon), nameof(StatusText), nameof(FullText)])]
        internal int _def = -1;

        [ObservableProperty(Related = [nameof(FullText)])]
        internal int _pendulumScale = -1;
        [ObservableProperty(Related = [nameof(FullText)])]
        internal string _pendulumText = "";

        [ObservableProperty]
        internal PackInfoCollection _packInfo = [];

        public Card(Serializable.Card card) : this()
        {
            _id = card.Id;
            _name = card.Name ?? "";
            _ruby = card.Ruby ?? "";
            _enName = card.EnName ?? "";
            _cardType = card.CardType;
            _text = card.Text ?? "";
            _unusable = card.Unusable ?? false;
            if (card.MonsterInfo is Serializable.MonsterInfo minfo)
            {
                _attribute = minfo.Attribute;
                _monsterType = minfo.Type;
                _effect = minfo.Effect ?? false;
                _ability = minfo.Ability ?? 0;
                _level = minfo.Level;
                _atk = minfo.Atk;
                _def = minfo.Def;
            }
            if (card.PendulumInfo is Serializable.PendulumInfo pinfo)
            {
                _pendulumScale = pinfo.Scale;
                _pendulumText = pinfo.Text ?? "";
            }
            _packInfo = new(card.PackInfo);
        }

        public static string CoerceText(string value) => value.Replace(Environment.NewLine, "\n");
        public static string CoercePendulumText(string value) => CoerceText(value);
        public int CompareTo(Card? other) => other is not null ? _id.CompareTo(other._id) : 1;

        public Card Clone() => new()
        {
            _id = _id,
            _name = _name,
            _ruby = _ruby,
            _enName = _enName,
            _cardType = _cardType,
            _text = _text,
            _unusable = _unusable,
            _attribute = _attribute,
            _monsterType = _monsterType,
            _effect = _effect,
            _ability = _ability,
            _level = _level,
            _atk = _atk,
            _def = _def,
            _pendulumScale = _pendulumScale,
            _pendulumText = _pendulumText,
            _packInfo = _packInfo,
        };

        public void Update(Card source)
        {
            Id = source._id;
            Name = source._name;
            Ruby = source._ruby;
            EnName = source._enName;
            CardType = source._cardType;
            Text = source._text;
            Unusable = source._unusable;
            Attribute = source._attribute;
            MonsterType = source._monsterType;
            Effect = source._effect;
            Ability = source._ability;
            Level = source._level;
            Atk = source._atk;
            Def = source._def;
            PendulumScale = source._pendulumScale;
            PendulumText = source._pendulumText;
            PackInfo = source._packInfo;
        }

        private void OnTextChanged(string value)
        {
            _related_created = false;
            SendPropertyChanged(nameof(RelatedList));
            TextConvert.RemoveTextCache(this);
        }
        private void OnPendulumTextChanged(string value) => OnTextChanged(value);
    }
}
