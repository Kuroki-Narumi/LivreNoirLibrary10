using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class CardList : ObservableList<Card>, ICardList
    {
        public bool ContainsId(int id)
        {
            foreach (var item in _list.AsSpan())
            {
                if (item.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public void Load(IEnumerable<int> ids, ICardProvider? provider)
        {
            Clear();
            if (provider is not null)
            {
                foreach (var id in ids)
                {
                    if (provider.TryGet(id, out var card))
                    {
                        AddWithoutNotify(card);
                    }
                }
                this.NotifyCollectionReset();
            }
        }

        IEnumerable<Card> ICardEnumerable.CardEnumerable => this;
    }
}
