using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public class NumbersCandidate : IClear
    {
        public int Number { get; set; }
        public List<NumbersKey> Keys { get; } = [];

        public void Clear()
        {
            Number = -1;
            Keys.Clear();
        }
    }
}
