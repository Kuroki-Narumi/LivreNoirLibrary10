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

        IEnumerable<Card> ICardEnumerable.CardEnumerable => this;
    }
}
