using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    using IDict = IDictionary<Card, int>;
    using KVPair = KeyValuePair<Card, int>;

    public class Regulation : IDict
    {
        public static Regulation Instance { get; } = [];

        private readonly SortedDictionary<Card, int> _list = [];
        private readonly Dictionary<int, SortedCardList> _list_map;

        public SortedCardList Forbidden { get; set; } = [];
        public SortedCardList Limit1 { get; set; } = [];
        public SortedCardList Limit2 { get; set; } = [];
        public SortedCardList Specified { get; set; } = [];

        public ICollection<Card> Keys => _list.Keys;
        public ICollection<int> Values => _list.Values;
        public int Count => _list.Count;
        bool ICollection<KVPair>.IsReadOnly => false;

        public int this[Card key]
        {
            get => Get(key);
            set => Set(key, value);
        }

        public Regulation()
        {
            _list_map = new()
            {
                { LimitNumber.Forbidden, Forbidden },
                { LimitNumber.Limit1, Limit1 },
                { LimitNumber.Limit2, Limit2 },
                { LimitNumber.Specified, Specified },
            };
        }

        public Regulation(Regulation source) : this() { Load(source); }
        public Regulation(Serializable.Regulation source) : this() { Load(source); }

        public void Clear()
        {
            var cards = _list.Keys.ToArray();
            _list.Clear();
            Forbidden.Clear();
            Limit1.Clear();
            Limit2.Clear();
            Specified.Clear();
            foreach (var card in cards)
            {
                card.OnLimitChanged();
            }
        }

        public void Clear(int value)
        {
            if (_list_map.TryGetValue(value, out var list))
            {
                foreach (var card in list)
                {
                    _list.Remove(card);
                    card.OnLimitChanged();
                }
                list.Clear();
            }
        }

        public bool LoadFile(string path)
        {
            if (Json.TryOpen<Serializable.Regulation>(path, out var data))
            {
                Load(data);
                return true;
            }
            return false;
        }

        public void Load(Regulation source)
        {
            Clear();
            Set(source);
        }

        public void Load(Serializable.Regulation source)
        {
            Clear();
            void SetInternal(List<string>? list, int num)
            {
                if (list is not null)
                {
                    Set(list.Select(CardPool.Instance.Get), num);
                }
            }
            SetInternal(source.Forbidden, LimitNumber.Forbidden);
            SetInternal(source.Limit1, LimitNumber.Limit1);
            SetInternal(source.Limit2, LimitNumber.Limit2);
            SetInternal(source.Specified, LimitNumber.Specified);
        }

        public void Load(IDict source)
        {
            Clear();
            Set(source);
        }

        public void Load(IDictionary<string, int> source)
        {
            Clear();
            Set(source);
        }

        public int Get(Card card)
        {
            if (card.Unusable)
            {
                return LimitNumber.Unusable;
            }
            else if (_list.TryGetValue(card, out var value))
            {
                return value;
            }
            else
            {
                return LimitNumber.Unlimited;
            }
        }

        public int GetActual(Card card) => Math.Clamp(Get(card), LimitNumber.Forbidden, LimitNumber.Unlimited);

        public bool Set(Card card, int value)
        {
            SortedCardList? list;
            if (_list.TryGetValue(card, out var current))
            {
                if (current == value)
                {
                    return false;
                }
                _list.Remove(card);
                if (_list_map.TryGetValue(current, out list))
                {
                    list.Remove(card);
                }
            }
            if (_list_map.TryGetValue(value, out list))
            {
                _list.Add(card, value);
                list.Add(card);
            }
            card.OnLimitChanged();
            return true;
        }

        public void Set(string name, int value)
        {
            Set(CardPool.Instance.Get(name), value);
        }

        public void Set(IEnumerable<Card> cards, int value)
        {
            SortedCardList? list;
            foreach (var card in cards)
            {
                if (_list.TryGetValue(card, out var current))
                {
                    if (current == value)
                    {
                        continue;
                    }
                    _list[card] = value;
                    if (_list_map.TryGetValue(current, out list))
                    {
                        list.Remove(card);
                    }
                }
                else
                {
                    _list.Add(card, value);
                }
                card.OnLimitChanged();
            }
            if (_list_map.TryGetValue(value, out list))
            {
                list.AddRange(cards);
            }
        }

        public void Set(IDict items)
        {
            foreach (var group in items.GroupBy(kv => kv.Value))
            {
                Set(group.Select(kv => kv.Key), group.Key);
            }
        }

        public void Set(IDictionary<string, int> items)
        {
            foreach (var group in items.GroupBy(kv => kv.Value))
            {
                Set(group.Select(kv => CardPool.Instance.Get(kv.Key)), group.Key);
            }
        }

        public void Remove(List<Card> cards)
        {
            foreach (var card in CollectionsMarshal.AsSpan(cards))
            {
                Forbidden.RemoveWithoutNotify(card);
                Limit1.RemoveWithoutNotify(card);
                Limit2.RemoveWithoutNotify(card);
                Specified.RemoveWithoutNotify(card);
                _list.Remove(card);
                card.OnLimitChanged();
            }
            Forbidden.NotifyCollectionReset();
            Limit1.NotifyCollectionReset();
            Limit2.NotifyCollectionReset();
            Specified.NotifyCollectionReset();
        }

        public void SetForbidden(List<Card> cards) => Set(cards, LimitNumber.Forbidden);
        public void SetLimit1(List<Card> cards) => Set(cards, LimitNumber.Limit1);
        public void SetLimit2(List<Card> cards) => Set(cards, LimitNumber.Limit2);
        public void SetSpecified(List<Card> cards) => Set(cards, LimitNumber.Specified);

        public SortedDictionary<Card, int>.Enumerator GetEnumerator() => _list.GetEnumerator();

        #region interface methods
        bool IDict.ContainsKey(Card key) => _list.ContainsKey(key);
        bool IDict.TryGetValue(Card key, out int value) => _list.TryGetValue(key, out value);
        void IDict.Add(Card key, int value) => Set(key, value);
        bool IDict.Remove(Card key) => Set(key, LimitNumber.Unlimited);

        bool ICollection<KVPair>.Contains(KVPair item) => Get(item.Key) == item.Value;
        void ICollection<KVPair>.Add(KVPair item) => Set(item.Key, item.Value);
        bool ICollection<KVPair>.Remove(KVPair item) => Set(item.Key, LimitNumber.Unlimited);
        void ICollection<KVPair>.CopyTo(KVPair[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        IEnumerator<KVPair> IEnumerable<KVPair>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}
