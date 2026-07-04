using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class SortedCardList : ObservableSortedList<int, Card>, ICardList
    {
        protected override int GetKey(Card item) => item.Id;

        public SortedCardList() { }
        public SortedCardList(int capacity) : base(capacity) { }
        public SortedCardList(IEnumerable<Card> collection) : base(collection) { }

        public IEnumerable<Card> EnumerateCards() => _list;

        public void Load(IEnumerable<Card> source)
        {
            ClearWithoutNotify();
            AddRange(source);
        }
    }
}
