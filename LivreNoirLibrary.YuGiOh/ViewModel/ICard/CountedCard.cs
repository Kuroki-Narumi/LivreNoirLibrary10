using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public partial class CountedCard(Card card, int count) : CardWrapperBase(card)
    {
        [ObservableProperty]
        private int _count = count;

        public void Deconstruct(out Card card, out int count)
        {
            card = Card;
            count = _count;
        }
    }
}
