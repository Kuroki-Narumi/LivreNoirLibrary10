using System;
using System.Collections.Generic;
using System.Linq;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public class IdCardList : ObservableList<int>, ICardList
    {
        public IdCardList() { }
        public IdCardList(int capacity) : base(capacity) { }
        public IdCardList(IEnumerable<int> source) : base(source) { }

        public bool Contains(Card card) => Contains(card.Id);
        public void Add(Card card) => Add(card.Id);
        public bool Remove(Card card) => Remove(card.Id);

        public IdCardList Slice(int index, int count)
        {
            IdCardList result = [];
            CopyTo(result, index, count);
            return result;
        }

        public IdCardList Splice(int index, int count)
        {
            IdCardList result = [];
            MoveTo(result, index, count);
            return result;
        }

        public (IdCardList, IdCardList) Split(int count)
        {
            IdCardList left = [], right = [];
            CopyTo(left, 0, count);
            CopyTo(right, count, Count - count);
            return (left, right);
        }

        public IEnumerable<Card> EnumCards()
        {
            var list = _list;
            var c = list.Count;
            for (var i = 0; i < c; i++)
            {
                if (CardPool.Instance.TryGet(list[i], out var card))
                {
                    yield return card;
                }
            }
        }

        public void Load(IEnumerable<Card> source) => Load(source.Select(c => c._id));
        public List<int> ToIdList() => [.. _list];
    }
}
