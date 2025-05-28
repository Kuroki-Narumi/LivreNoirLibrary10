using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public class SortedCardList : ObservableSortedList<int, Card>, ICardList
    {
        protected override int GetKey(Card item) => item.Id;

        public SortedCardList() { }
        public SortedCardList(int capacity) : base(capacity) { }
        public SortedCardList(IEnumerable<Card> collection) : base(collection) { }

        public IEnumerable<Card> EnumCards() => _list;
    }
}
