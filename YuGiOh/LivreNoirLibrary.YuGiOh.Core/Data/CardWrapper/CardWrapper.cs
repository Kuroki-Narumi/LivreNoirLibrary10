using LivreNoirLibrary.ObjectModel;
using System;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class CardWrapper(Card card) : ObservableObjectBase, ICard
    {
        public Card ThisCard { get; set => SetValue(ref field, value); } = card;
    }
}
