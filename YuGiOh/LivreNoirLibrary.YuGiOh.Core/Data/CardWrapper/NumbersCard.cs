using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class NumbersCard() : CardWrapper(null!), IClear
    {
        public int Number { get; set => SetValue(ref field, value); } = -1;

        public void Clear()
        {
            ThisCard = null!;
            Number = -1;
        }
    }
}
