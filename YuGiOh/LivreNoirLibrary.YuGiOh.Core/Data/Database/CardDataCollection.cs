using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class CardDataCollection : DataCollectionBase<int, Card>, ICardEnumerable
    {
        protected override int GetKey(Card item) => item.Id;

        public void Load(List<Serializable.Card> source)
        {
            ClearWithoutNotify();
            var c = source.Count;
            var list = _list;
            var keys = _key_list;
            list.EnsureCapacity(c);
            keys.EnsureCapacity(c);
            foreach (var item in source.AsSpan())
            {
                Card card = new(item);
                list.Add(card);
                keys.Add(card.Id);
            }
            NotifyCollectionReset();
        }

        internal void AddInternal(Serializable.Card card)
        {
            Card c = new(card);
            _list.Add(c);
            _key_list.Add(c.Id);
        }

        public bool Contains(int id) => _key_list.Contains(id);

        public Card Get(int id)
        {
            var index = IndexOfKey(id);
            Card card;
            if (index is >= 0)
            {
                card = _list[index];
            }
            else
            {
                card = new() { Id = id, Name = $"<ID{id}>" };
                AddWithoutNotify(card);
            }
            return card;
        }

        public Card Get(string name)
        {
            if (!string.IsNullOrEmpty(name) && CheckUpdate().TryGetValue(name.ToHalf(), out var index))
            {
                return _list[index];
            }
            return new()
            {
                Id = name.GetHashCode(),
                Name = name,
            };
        }

        public bool TryGet(int id, [MaybeNullWhen(false)] out Card card)
        {
            var index = IndexOfKey(id);
            if (index is >= 0)
            {
                card = _list[index];
                return true;
            }
            else
            {
                card = default;
                return false;
            }
        }

        public bool TryGet(string name, [MaybeNullWhen(false)] out Card card)
        {
            if (!string.IsNullOrEmpty(name) && CheckUpdate().TryGetValue(name.ToHalf(), out var index))
            {
                card = _list[index];
                return true;
            }
            card = null;
            return false;
        }

        public bool Remove(int id)
        {
            if (TryGet(id, out var card))
            {
                return Remove(card);
            }
            return false;
        }

        public override void Refresh()
        {
            _name2idx.Clear();
            var c = _list.Count;
            for (int i = 0; i < c; i++)
            {
                var card = _list[i];
                RegisterName2Idx(card.Name, i);
                RegisterName2Idx(card.EnName, i);
            }
        }

        private void RegisterName2Idx(string name, int index)
        {
            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToHalf();
                if (_name2idx.TryGetValue(name, out var current))
                {
                    if (current > index)
                    {
                        _name2idx[name] = index;
                    }
                }
                else
                {
                    _name2idx.Add(name, index);
                }
            }
        }

        IEnumerable<Card> ICardEnumerable.EnumerateCards() => this;
    }
}
