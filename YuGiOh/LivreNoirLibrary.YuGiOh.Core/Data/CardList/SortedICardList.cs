using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class SortedICardList<T>(Func<Card, T> factory) : ObservableSortedList<int, T>, ICardEnumerable, IIdEnumerable
        where T : ICard
    {
        private readonly Func<Card, T> _factory = factory;

        protected sealed override int GetKey(T item) => item.ThisCard.Id;

        public bool Contains(Card card) => ContainsKey(card.Id);

        public void Add(Card card)
        {
            if (!Contains(card))
            {
                Add(_factory(card));
            }
        }

        public void AddWithoutNotify(Card card)
        {
            if (!Contains(card))
            {
                AddWithoutNotify(_factory(card));
            }
        }

        public bool Remove(Card card) => RemoveKey(card.Id);
        public bool RemoveWithoutNotify(Card card) => RemoveKeyWithoutNotify(card.Id);

        public void AddRange(ReadOnlySpan<Card> cards)
        {
            var factory = _factory;
            var modified = false;
            foreach (var card in cards)
            {
                if (!Contains(card))
                {
                    AddWithoutNotify(factory(card));
                    modified = true;
                }
            }
            if (modified)
            {
                this.NotifyCollectionReset();
            }
        }

        public void AddRange(IEnumerable<Card> cards)
        {
            var factory = _factory;
            var modified = false;
            foreach (var card in cards)
            {
                if (!Contains(card))
                {
                    AddWithoutNotify(factory(card));
                    modified = true;
                }
            }
            if (modified)
            {
                this.NotifyCollectionReset();
            }
        }

        public void AddRange(ReadOnlySpan<int> ids, ICardProvider? provider)
        {
            var factory = _factory;
            var modified = false;
            foreach (var id in ids)
            {
                if (!ContainsKey(id) && provider.TryGet(id, out var card))
                {
                    AddWithoutNotify(factory(card));
                    modified = true;
                }
            }
            if (modified)
            {
                this.NotifyCollectionReset();
            }
        }

        public void AddRange(IEnumerable<int> ids, ICardProvider? provider)
        {
            var factory = _factory;
            var modified = false;
            foreach (var id in ids)
            {
                if (!ContainsKey(id) && provider.TryGet(id, out var card))
                {
                    AddWithoutNotify(factory(card));
                    modified = true;
                }
            }
            if (modified)
            {
                this.NotifyCollectionReset();
            }
        }

        public void AddRange(ReadOnlySpan<string> names, ICardProvider? provider)
        {
            if (provider is null)
            {
                return;
            }
            var factory = _factory;
            var modified = false;
            foreach (var name in names)
            {
                if (provider.TryGetByName(name, out var card) && !Contains(card))
                {
                    AddWithoutNotify(factory(card));
                    modified = true;
                }
            }
            if (modified)
            {
                this.NotifyCollectionReset();
            }
        }

        public void AddRange(IEnumerable<string> names, ICardProvider? provider)
        {
            if (provider is null)
            {
                return;
            }
            var factory = _factory;
            var modified = false;
            foreach (var name in names)
            {
                if (provider.TryGetByName(name, out var card) && !Contains(card))
                {
                    AddWithoutNotify(factory(card));
                    modified = true;
                }
            }
            if (modified)
            {
                this.NotifyCollectionReset();
            }
        }

        public int RemoveAll(Predicate<Card> predicate) => RemoveAll((k, v) => predicate(v.ThisCard));

        public IEnumerable<Card> CardEnumerable => typeof(T) == typeof(Card) ? (this as IEnumerable<Card>)! : this.Select(c => c.ThisCard);
    }
}
