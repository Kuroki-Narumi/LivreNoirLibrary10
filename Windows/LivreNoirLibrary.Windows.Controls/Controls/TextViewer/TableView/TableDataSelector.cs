using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.Controls
{
    public abstract class TableDataSelector
    {
        public abstract int GetKey(object item);
        public abstract IEnumerable<TableHeaderInfo> EnumHeader();
        public abstract bool SkipEmpty { get; }
    }

    public class DefaultTableDataSelector : TableDataSelector
    {
        public override int GetKey(object item) => item.GetHashCode();
        public override IEnumerable<TableHeaderInfo> EnumHeader()
        {
            yield break;
        }
        public override bool SkipEmpty => false;
    }
}
