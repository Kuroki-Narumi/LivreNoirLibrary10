using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class SortedCardIdList : ObservableSortedList<int>, ICardIdList, IIdEnumerable
    {
        public IEnumerable<int> EnumerateIds()
        {
            throw new NotImplementedException();
        }

        public void Load(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }
    }
}
