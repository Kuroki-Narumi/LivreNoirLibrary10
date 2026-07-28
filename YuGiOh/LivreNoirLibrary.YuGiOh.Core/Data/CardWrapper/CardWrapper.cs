using LivreNoirLibrary.ObjectModel;
using System;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class CardWrapper(Card card) : ObservableObjectBase, ICard, INamedObject
    {
        public string? Name => ThisCard?.Name;
        public Card ThisCard { get; set => SetValue(ref field, value); } = card;
    }
}
