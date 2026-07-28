using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class SortedCheckableCardList() : SortedICardList<CheckableCard>(card => new CheckableCard(card))
    {
        public void ClearValues()
        {
            foreach (var item in _list.AsSpan())
            {
                item.IsChecked = false;
            }
        }
    }
}
