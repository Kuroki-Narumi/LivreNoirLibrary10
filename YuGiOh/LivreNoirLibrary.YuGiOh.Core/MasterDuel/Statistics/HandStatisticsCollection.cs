using LivreNoirLibrary.Debug;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public class HandStatisticsCollection : StatisticsCollectionBase<int, HandStatisticsItem>
    {
        public void Append(Card card, DuelLog log)
        {
            var item = GetOrAdd(card.Id);
            item.Card = card;
            item.Append(log);
        }
    }
}
