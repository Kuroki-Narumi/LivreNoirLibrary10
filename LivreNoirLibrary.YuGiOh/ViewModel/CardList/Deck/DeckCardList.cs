using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public class DeckCardList : ObservableSortedList<int, CountedCard>, ICardList
    {
        protected override int GetKey(CountedCard item) => item.TypeIdIndex;
        protected static int GetKey(Card item) => item.TypeIdIndex;

        private int _sum_count;
        private readonly Dictionary<int, CountedCard> _cache = [];

        public int SumCount => _sum_count;

        public int this[Card card]
        {
            get => TryGet(card, out var item) ? item.Count : 0;
            set => Set(card, value);
        }

        public new void NotifyCollectionReset()
        {
            UpdateCount();
            base.NotifyCollectionReset();
        }

        public new void Clear()
        {
            ClearWithoutNotify();
            NotifyCollectionReset();
        }

        public bool Contains(Card card) => _key_list.Contains(GetKey(card));
        public void Add(Card card) => Set(card, this[card] + 1);
        public bool Remove(Card card)
        {
            if (TryGet(card, out var item))
            {
                Set(card, item.Count - 1);
                return true;
            }
            return false;
        }

        public CountedCard? Set(Card card, int count)
        {
            CountedCard? item = null;
            var key = GetKey(card);
            var index = _key_list.BinarySearch(key);
            if (index is >= 0)
            {
                var current = _list[index];
                if (count <= 0)
                {
                    RemoveItem(index);
                    OnCollectionRemoved(current, index);
                }
                else
                {
                    current.Count = count;
                    item = current;
                    OnCollectionReplaced(current, current, index);
                }
            }
            else
            {
                index = ~index;
                if (!_cache.TryGetValue(key, out item))
                {
                    item = new(card, count);
                    _cache.Add(key, item);
                }
                else
                {
                    item.Count = count;
                }
                AddItem(index, item);
                OnCollectionAdded(item, index);
            }
            UpdateCount();
            return item;
        }

        private void UpdateCount()
        {
            _sum_count = 0;
            foreach (var item in CollectionsMarshal.AsSpan(_list))
            {
                _sum_count += item.Count;
            }
            SendPropertyChanged(nameof(SumCount));
        }

        public bool TryGet(Card card, [MaybeNullWhen(false)] out CountedCard item)
        {
            var key = GetKey(card);
            var index = _key_list.BinarySearch(key);
            if (index is >= 0)
            {
                item = _list[index];
                return true;
            }
            else
            {
                item = null;
                return false;
            }
        }

        public IEnumerable<Card> EnumCards()
        {
            var list = _list;
            var c = list.Count;
            for (var i = 0; i < c; i++)
            {
                var (card, count) = list[i];
                for (var j = 0; j < count; j++)
                {
                    yield return card;
                }
            }
        }

        public IEnumerable<(Card, int)> EnumCardsWithCount()
        {
            var list = _list;
            var c = list.Count;
            for (var i = 0; i < c; i++)
            {
                var (card, count) = list[i];
                yield return (card, count);
            }
        }

        public void Load(DeckCardList source)
        {
            ClearWithoutNotify();
            var count = source.Count;
            _key_list.EnsureCapacity(count);
            _list.EnsureCapacity(count);
            _sum_count = source._sum_count;
            for (var i = 0; i < count; i++)
            {
                var key = source._key_list[i];
                var value = source._list[i];
                _key_list.Add(key);
                _list.Add(new(value.Card, value.Count));
            }
            NotifyCollectionReset();
        }

        public void Load(IEnumerable<Card> source)
        {
            ClearWithoutNotify();
            foreach (var card in source)
            {
                AddWithoutNotify(card);
            }
            NotifyCollectionReset();
        }

        internal void AddWithoutNotify(Card card)
        {
            var key = GetKey(card);
            var index = _key_list.BinarySearch(key);
            if (index is >= 0)
            {
                _list[index].Count += 1;
            }
            else
            {
                index = ~index;
                if (!_cache.TryGetValue(key, out var item))
                {
                    item = new(card, 1);
                    _cache.Add(key, item);
                }
                else
                {
                    item.Count = 1;
                }
                AddItem(index, item);
            }
        }
    }
}
