using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class SortedCardList() : SortedICardList<Card>(static card => card), ICardCollection
    {
        bool ICardCollection.ContainsId(int id) => ContainsKey(id);
    }
}
