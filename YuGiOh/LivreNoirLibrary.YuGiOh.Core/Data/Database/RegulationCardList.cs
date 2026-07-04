using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class RegulationCardList : ObservableSortedList<int, CardId>, ICardList
    {
        protected override int GetKey(CardId item) => item.Id;

        public bool Contains(Card card) => ContainsKey(card.Id);

        public void Add(int id) => Add(GetItem(id));
        public void Add(Card card) => Add(GetItem(card.Id));

        public void AddWithoutNotify(int id) => AddWithoutNotify(GetItem(id));

        public void AddRange(IEnumerable<int> ids) => AddRange(ids.Select(GetItem));
        public void AddRange(params ReadOnlySpan<int> ids)
        {
            foreach (var id  in ids)
            {
                AddWithoutNotify(id);
            }
            if (ids.Length is > 0)
            {
                NotifyCollectionReset();
            }
        }

        public bool Remove(Card card) => RemoveKey(card.Id);

        public bool RemoveWithoutNotify(int id) => RemoveWithoutNotify(GetItem(id));
        public bool RemoveWithoutNotify(Card card) => RemoveWithoutNotify(GetItem(card));

        public IEnumerable<Card> EnumerateCards() => this.Select(i => i.Card);

        public void Load(IEnumerable<Card> source)
        {
            ClearWithoutNotify();
            AddRange(source.Select(GetItem));
        }

        private static CardId GetItem(int id) => _cache.GetOrAdd(id, static id => new(id));
        private static CardId GetItem(ICard card) => _cache.GetOrAdd(card.Id, static id => new(id));

        private static readonly Dictionary<int, CardId> _cache = [];
    }
}
