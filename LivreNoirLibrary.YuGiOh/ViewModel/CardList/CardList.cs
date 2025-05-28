using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public class CardList : ObservableList<Card>, ICardList
    {
        public CardList() { }
        public CardList(int capacity) : base(capacity) { }
        public CardList(IEnumerable<Card> source) : base(source) { }

        public CardList Slice(int index, int count)
        {
            CardList result = [];
            CopyTo(result, index, count);
            return result;
        }

        public CardList Splice(int index, int count)
        {
            CardList result = [];
            MoveTo(result, index, count);
            return result;
        }

        public (CardList, CardList) Split(int count)
        {
            CardList left = [], right = [];
            CopyTo(left, 0, count);
            CopyTo(right, count, Count - count);
            return (left, right);
        }

        public IEnumerable<Card> EnumCards() => _list;
    }
}
