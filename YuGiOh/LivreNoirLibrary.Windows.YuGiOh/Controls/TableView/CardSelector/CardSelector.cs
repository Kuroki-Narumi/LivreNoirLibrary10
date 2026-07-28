using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract class CardSelector : ITableDataSelector
    {
        public abstract IVocabData Name { get; }
        public abstract bool SkipEmpty { get; }
        public abstract int GetKey(Card card);
        public abstract IEnumerable<TableDataSelectorInfo> EnumerateInfo();

        int ITableDataSelector.GetKey(object item) => item is Card card ? GetKey(card) : 0;
    }
}
