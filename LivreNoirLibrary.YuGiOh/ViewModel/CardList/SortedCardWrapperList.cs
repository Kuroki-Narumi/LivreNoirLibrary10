using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public abstract class SortedCardWrapperList<T> : ObservableSortedList<int, T>, ICardList
        where T : CardWrapperBase
    {
        protected override int GetKey(T item) => item.Card.Id;

        public SortedCardWrapperList() { }
        public SortedCardWrapperList(int capacity) : base(capacity) { }
        public SortedCardWrapperList(IEnumerable<T> colleciton) : base(colleciton) { }

        public abstract void Add(Card card);
        public abstract bool Remove(Card card);

        public bool Contains(Card card) => _list.Exists(item => item.Card == card);

        public IEnumerable<Card> EnumCards()
        {
            var c = _list.Count;
            for (var i = 0; i < c; i++)
            {
                yield return _list[i].Card;
            }
        }

        public abstract void Load(IEnumerable<Card> source);
    }
}
