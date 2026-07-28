using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface ITableDataSelector
    {
        bool SkipEmpty { get; }
        int GetKey(object item);
        IEnumerable<TableDataSelectorInfo> EnumerateInfo();
    }

    public class TableDataSelector : ITableDataSelector
    {
        public static TableDataSelector Default { get; } = new();

        public bool SkipEmpty => false;
        public int GetKey(object item) => 1;
        public IEnumerable<TableDataSelectorInfo> EnumerateInfo()
        {
            yield return new(1, "");
        }
    }
}
