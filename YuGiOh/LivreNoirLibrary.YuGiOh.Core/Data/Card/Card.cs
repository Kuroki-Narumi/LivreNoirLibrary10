using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class Card() : ObservableObjectBase, ICard, IId, INamedObject
    {
        public static Card Dummy(int id) => new() { Id = id, Name = $"Card#{id}" };

        public Card ThisCard => this;

        public int Id { get; set => SetValue(ref field, value, [nameof(ThisCard)]); }
        public string Name { get; set => SetValue(ref field, value); } = "";
        public string Ruby { get; set => SetValue(ref field, value); } = "";
        public string EnName { get; set => SetValue(ref field, value); } = "";
        public CardType CardType { get;  set => SetValue(ref field, value, 
            [nameof(CardTypeText), nameof(CTypeText), nameof(AbilityTextWithType), nameof(DefText), nameof(FullText), nameof(Icon), nameof(LinkIcon), nameof(FrameBrush)]); }
        public string Text { get; set => SetValue(ref field, value.ReplaceLineEndings("\n"), [nameof(FullText)]); } = "";
        public bool Unusable { get; set => SetValue(ref field, value, [nameof(ActualLimitCount), nameof(LimitIcon), nameof(LimitText)]); }

        public Attribute Attribute { get; set => SetValue(ref field, value, [nameof(AttributeText), nameof(AttrText), nameof(MonsterInfoText), nameof(FullText), nameof(AttributeIcon)]); }
        public MonsterType MonsterType { get; set => SetValue(ref field, value, [nameof(MonsterTypeText), nameof(MTypeText), nameof(MonsterInfoText), nameof(FullText)]); }
        public bool HasEffect { get; set => SetValue(ref field, value, 
            [nameof(EffectText), nameof(AbilityText), nameof(AbilityTextWithType), nameof(MonsterInfoText), nameof(FullText), nameof(Icon), nameof(LinkIcon), nameof(FrameBrush)]); }
        public Ability Ability { get; set => SetValue(ref field, value, 
            [nameof(AbilityText), nameof(AbilityTextWithType), nameof(MonsterInfoText), nameof(FullText), nameof(TunerIcon), nameof(FrameBrush)]); }
        public int Level { get; set => SetValue(ref field, value, [nameof(LevelText), nameof(StatusText), nameof(FullText)]); } = -1;
        public int Atk { get; set => SetValue(ref field, value, [nameof(AtkText), nameof(StatusText), nameof(FullText)]); } = -1;
        public int Def { get; set => SetValue(ref field, value, [nameof(DefText), nameof(StatusText), nameof(FullText), nameof(LinkIcon)]); } = -1;

        public int PendulumScale { get; set => SetValue(ref field, value, [nameof(FullText)]); } = -1;
        public string PendulumText { get; set => SetValue(ref field, value.ReplaceLineEndings("\n"), [nameof(FullText)]); } = "";

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

        public Card Clone() => new(PackInfo)
        {
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

        public void CopyFrom(ICard source)
        {
            var card = source.ThisCard;
            Id = card.Id;
            Name = card.Name;
            Ruby = card.Ruby;
            EnName = card.EnName;
            CardType = card.CardType;
            Text = card.Text;
            Unusable = card.Unusable;
            Attribute = card.Attribute;
            MonsterType = card.MonsterType;
            HasEffect = card.HasEffect;
            Ability = card.Ability;
            Level = card.Level;
            Atk = card.Atk;
            Def = card.Def;
            PendulumScale = card.PendulumScale;
            PendulumText = card.PendulumText;
            PackInfo.Load(card.PackInfo);
            this.NotifyPropertyChanged(nameof(PackInfo));
        }
    }
}
