using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class SharedCardIdList : ObservableSortedList<int, SharedCardId>, ICardIdList
    {
        protected override int GetKey(SharedCardId item) => item.Id;

        public bool Contains(int id) => ContainsKey(id);
        public void Add(int id) => Add(SharedCardId.GetItem(id));
        public void AddWithoutNotify(int id) => AddWithoutNotify(SharedCardId.GetItem(id));

        public void AddRange(IEnumerable<int> ids) => AddRange(ids.Select(SharedCardId.GetItem));
        public void AddRange(params ReadOnlySpan<int> ids)
        {
            foreach (var id  in ids)
            {
                AddWithoutNotify(id);
            }
            if (ids.Length is > 0)
            {
                this.NotifyCollectionReset();
            }
        }

        public bool Remove(int id) => RemoveKey(id);
        public bool RemoveWithoutNotify(int id) => RemoveKeyWithoutNotify(id);

        public IEnumerable<int> IdEnumerable => _key_list.Select(i => i);

        public void Load(CardDataCollection source)
        {
            ClearWithoutNotify();
            var keys = _key_list;
            var values = _list;
            var span = source.Ids;
            keys.AddRange(span);
            foreach (var id in span)
            {
                values.Add(SharedCardId.GetItem(id));
            }
            this.NotifyCollectionReset();
        }

        public void Load(IEnumerable<int> source)
        {
            ClearWithoutNotify();
            AddRange(source.Select(SharedCardId.GetItem));
        }
    }
}
