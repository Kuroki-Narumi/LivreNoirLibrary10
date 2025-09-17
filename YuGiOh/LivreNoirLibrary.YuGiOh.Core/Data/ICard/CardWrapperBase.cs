using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public abstract class CardWrapperBase(Card card) : ObservableObjectBase, ICard, ICardWrapper
    {
        public Card Card { get; } = card;

        public int Id => Card.Id;
        public string Name => Card.Name;
        public string Ruby => Card.Ruby;
        public string EnName => Card.EnName;
        public CardType CardType => Card.CardType;
        public string Text => Card.Text;
        public bool Unusable => Card.Unusable;

        public int TypeIndex => Card.TypeIndex;
        public int TypeIdIndex => Card.TypeIdIndex;
        public Attribute Attribute => Card.AttributeIndex;
        public Attribute AttributeD => Card.AttributeIndexD;
        public MonsterType MonsterType => Card.MonsterTypeIndex;
        public MonsterType MonsterTypeD => Card.MonsterTypeIndexD;
        public bool HasEffect => Card.HasEffect;
        public int EffectIndex => Card.EffectIndex;
        public Ability Ability => Card.AbilityIndex;
        public Ability AbilityD => Card.AbilityIndexD;
        public int TunerIndex => Card.TunerIndex;
        public int Level => Card.LevelIndex;
        public int LevelD => Card.LevelIndexD;
        public int Atk => Card.AtkIndex;
        public int AtkD => Card.AtkIndexD;
        public int Def => Card.DefIndex;
        public int DefD => Card.DefIndexD;
        public int PendulumScale => Card.ScaleIndex;
        public int PendulumScaleD => Card.ScaleIndexD;
        public string PendulumText => Card.PendulumText;
        public int NameLength => Card.NameLength;
        public int RubyLength => Card.RubyLength;
        public int RubyLengthD => Card.RubyLengthD;
        public int EnNameLength => Card.EnNameLength;
        public int EnNameLengthD => Card.EnNameLengthD;
        public int TextLength => Card.TextLength;
        public int PTextLength => Card.PendulumTextLength;
        public int PTextLengthD => Card.PendulumTextLengthD;

        public DateTime FirstDateOcg => Card.FirstDateOcg;
        public DateTime FirstDateOcgD => Card.FirstDateOcgD;
        public DateTime LastDateOcg => Card.LastDateOcg;
        public DateTime LastDateOcgD => Card.LastDateOcgD;
        public DateTime FirstDateTcg => Card.FirstDateTcg;
        public DateTime FirstDateTcgD => Card.FirstDateTcgD;
        public DateTime LastDateTcg => Card.LastDateTcg;
        public DateTime LastDateTcgD => Card.LastDateTcgD;

        public string LimitText => Card.LimitText;
        public string CardTypeText => Card.CardTypeText;
        public string AttributeText => Card.AttributeText;
        public string AttrText => Card.AttrText;
        public string MonsterTypeText => Card.MonsterTypeText;
        public string EffectText => Card.EffectText;
        public string AbilityText => Card.AbilityText;
        public string AbilityTextWithType => Card.AbilityTextWithType;
        public string LevelText => Card.LevelText;
        public string AtkText => Card.AtkText;
        public string DefText => Card.DefText;
        public string MonsterInfoText => Card.MonsterInfoText;
        public string StatusText => Card.StatusText;
        public string FullText => Card.FullText;
    }
}
