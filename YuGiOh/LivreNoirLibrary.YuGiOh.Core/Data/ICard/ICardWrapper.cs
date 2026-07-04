using System;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICardWrapper : ICard
    {
        Card Card { get; }

        Card ICard.ThisCard => Card;

        int ICard.Id => Card.Id;
        string ICard.Name => Card.Name;
        string ICard.Ruby => Card.Ruby;
        string ICard.EnName => Card.EnName;
        CardType ICard.CardType => Card.CardType;
        string ICard.Text => Card.Text;
        bool ICard.Unusable => Card.Unusable;

        Attribute ICard.Attribute => Card.Attribute;
        MonsterType ICard.MonsterType => Card.MonsterType;
        bool ICard.HasEffect => Card.HasEffect;
        Ability ICard.Ability => Card.Ability;
        int ICard.Level => Card.Level;
        int ICard.Atk => Card.Atk;
        int ICard.Def => Card.Def;
        int ICard.PendulumScale => Card.PendulumScale;
        string ICard.PendulumText => Card.PendulumText;

        PackInfoCollection ICard.PackInfo => Card.PackInfo;
    }
}
