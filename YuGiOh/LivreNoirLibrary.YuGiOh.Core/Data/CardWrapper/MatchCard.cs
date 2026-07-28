using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class MatchCard() : CardWrapper(null!), IClear
    {
        public int MatchCount { get; set => SetValue(ref field, value); }

        public void Clear()
        {
            ThisCard = null!;
            MatchCount = 0;
        }
    }
}
