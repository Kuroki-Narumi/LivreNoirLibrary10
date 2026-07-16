using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class CardPackCollection : ObservableSortedList<CardPackCollection.Key, CardPack>, IWriteJson, ICardPackProvider
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

        private bool _needRefresh = true;
        private readonly Dictionary<string, int> _name2index = [];

        private void InvalidateName2Index()
        {
            _needRefresh = true;
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            InvalidateName2Index();
        }

        protected override void AddItem(CardPack item, out bool replaced, out int index, out CardPack? oldItem)
        {
            base.AddItem(item, out replaced, out index, out oldItem);
            InvalidateName2Index();
        }

        protected override int RemoveItem(CardPack item)
        {
            InvalidateName2Index();
            return base.RemoveItem(item);
        }

        public void Load(List<Serializable.CardPack> source, ICardProvider provider)
        {
            ClearWithoutNotify();
            var c = source.Count;
            var list = _list;
            var keyList = _key_list;
            list.EnsureCapacity(c);
            keyList.EnsureCapacity(c);
            foreach (var item in source.AsSpan())
            {
                CardPack pack = new(item, provider);
                list.Add(pack);
                keyList.Add(GetKey(pack));
            }
            this.NotifyCollectionReset();
            InvalidateName2Index();
        }

        private Dictionary<string, int> EnsureName2Index()
        {
            var dic = _name2index;
            if (_needRefresh)
            {
                dic.Clear();
                var list = _list;
                var c = list.Count;
                for (var i = 0; i < c; i++)
                {
                    var id = list[i].ProductId;
                    dic[id] = i;
                }
            }
            return dic;
        }

        public bool Contains(string pid) => EnsureName2Index().ContainsKey(pid);

        public bool TryGet(string pid, [MaybeNullWhen(false)] out CardPack pack)
        {
            if (EnsureName2Index().TryGetValue(pid, out var index))
            {
                pack = _list[index];
                return true;
            }
            pack = default;
            return false;
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var item in _list.AsSpan())
            {
                JsonSerializer.Serialize(writer, item, options);
            }
            writer.WriteEndArray();
        }
    }
}
