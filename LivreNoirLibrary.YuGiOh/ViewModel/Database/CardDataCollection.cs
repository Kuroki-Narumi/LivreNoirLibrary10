using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public class CardDataCollection : DataCollectionBase<int, Card>
    {
        protected override int GetKey(Card item) => item.Id;

        public void Load(List<Serializable.Card> source, CardPackCollection? packs)
        {
            ClearWithoutNotify();
            var c = source.Count;
            _list.EnsureCapacity(c);
            _key_list.EnsureCapacity(c);
            foreach (var item in CollectionsMarshal.AsSpan(source))
            {
                Card card = new(item);
                _list.Add(card);
                _key_list.Add(card.Id);
                if (packs is not null)
                {
                    foreach (var info in card._packInfo)
                    {
                        var pack = packs.Get(info._productId);
                        pack.AddWithoutNotify(new NumberedCard(card, info._number));
                    }
                }
            }
            NotifyCollectionReset();
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
            if (!string.IsNullOrEmpty(name) && CheckUpdate().TryGetValue(TextConvert.Wide2Half(name), out var index))
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
            if (!string.IsNullOrEmpty(name) && CheckUpdate().TryGetValue(TextConvert.Wide2Half(name), out var index))
            {
                card = _list[index];
                return true;
            }
            card = null;
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
                name = TextConvert.Wide2Half(name);
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
    }
}
