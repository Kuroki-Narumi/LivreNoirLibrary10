using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public class CardPackCollection : DataCollectionBase<(DateTime, string), CardPack>
    {
        protected override (DateTime, string) GetKey(CardPack item) => (item.Date, item.ProductId);

        public void Load(List<Serializable.CardPack> source)
        {
            ClearWithoutNotify();
            var c = source.Count;
            _list.EnsureCapacity(c);
            _key_list.EnsureCapacity(c);
            foreach (var item in CollectionsMarshal.AsSpan(source))
            {
                CardPack pack = new(item);
                _list.Add(pack);
                _key_list.Add(GetKey(pack));
            }
            NotifyCollectionReset();
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

        public CardPack GetNew(string pid, DateTime date)
        {
            CardPack pack;
            if (CheckUpdate().TryGetValue(pid, out var index))
            {
                pack = _list[index];
                if (date != pack.Date)
                {
                    RemoveWithoutNotify(pack);
                    pack.Date = date;
                    AddWithoutNotify(pack);
                }
            }
            else
            {
                pack = new() { ProductId = pid, Date = date };
                AddWithoutNotify(pack);
            }
            return pack;
        }

        public void Register(DateTime date, string pname, string pid, string number, bool tcg, Card card)
        {
            if (tcg && !CardPack.IsTcgPack(pid))
            {
                pid = $"{pid}e";
            }
            var key = (date, pid);
            var index = IndexOfKey(key);
            CardPack pack;
            if (index is >= 0)
            {
                pack = _list[index];
            }
            else
            {
                pack = new()
                {
                    Date = date,
                    Name = pname,
                    ProductId = pid,
                };
                InsertItem(~index, key, pack);
            }
            card.PackInfo.Add(new() { ProductId = pid, Number = number });
            pack.Add(card);
        }

        public override void Refresh()
        {
            _name2idx.Clear();
            var c = _list.Count;
            for (int i = 0; i < c; i++)
            {
                var id = _list[i].ProductId;
                _name2idx.TryAdd(id, i);
            }
        }
    }
}
