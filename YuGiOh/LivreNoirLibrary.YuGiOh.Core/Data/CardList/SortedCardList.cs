using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class SortedCardList : ObservableSortedList<int, Card>, IIdEnumerable
    {
        protected override int GetKey(Card item) => item.Id;

        public void AddRange(ReadOnlySpan<int> ids, ICardProvider provider)
        {
            var modified = false;
            foreach (var id in ids)
            {
                if (provider.TryGet(id, out var card))
                {
                    AddWithoutNotify(card);
                    modified = true;
                }
            }
            if (modified)
            {
                this.NotifyCollectionReset();
            }
        }

        public IEnumerable<int> EnumerateIds() => _key_list;
    }
}
