using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class CardPackCollection : DataCollectionBase<CardPackCollection.Key, CardPack>
    {
        public readonly record struct Key(DateTime Date, string ProductId) : IComparable<Key>
        {
            public int CompareTo(Key other)
            {
                var c = Date.CompareTo(other.Date);
                if (c is not 0)
                {
                    return c;
                }
                return ProductId.CompareTo(other.ProductId, StringComparison.Ordinal);
            }
        }

        protected override Key GetKey(CardPack item) => new(item.Date, item.ProductId);

        public void Load(List<Serializable.CardPack> source)
        {
            ClearWithoutNotify();
            var c = source.Count;
            var list = _list;
            var keyList = _key_list;
            list.EnsureCapacity(c);
            keyList.EnsureCapacity(c);
            foreach (var item in source.AsSpan())
            {
                CardPack pack = new(item);
                list.Add(pack);
                keyList.Add(GetKey(pack));
            }
            NotifyCollectionReset();
        }

        internal void AddInternal(Serializable.CardPack pack)
        {
            CardPack p = new(pack);
            _list.Add(p);
            _key_list.Add(GetKey(p));
        }

        public bool Contains(string pid) => CheckUpdate().ContainsKey(pid);

        public CardPack Get(string pid)
        {
            if (CheckUpdate().TryGetValue(pid, out var index))
            {
                return _list[index];
            }
            return [];
        }

        public bool Remove(string pid)
        {
            if (CheckUpdate().TryGetValue(pid, out var index))
            {
                return Remove(_list[index]);
            }
            return false;
        }

        public override void Refresh()
        {
            var dic = _name2idx;
            dic.Clear();
            var c = _list.Count;
            for (var i = 0; i < c; i++)
            {
                var id = _list[i].ProductId;
                dic[id] = i;
            }
        }
    }
}
