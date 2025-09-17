using System;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class CountedCard(Card card, int count) : CardWrapperBase(card)
    {
        public int Count { get; set => SetValue(ref field, value); } = count;

        public void Deconstruct(out Card card, out int count)
        {
            card = Card;
            count = Count;
        }
    }
}
