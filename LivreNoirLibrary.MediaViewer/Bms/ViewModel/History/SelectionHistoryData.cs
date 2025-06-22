using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    internal class SelectionHistoryData : Dictionary<BarPosition, List<StructNote>>
    {
        public SelectionHistoryData(Selection selection)
        {
            foreach (var (p, _, v) in selection.EachItem())
            {
                this.Add(p, v.ToStruct());
            }
        }

        private SelectionHistoryData(SelectionHistoryData source)
        {
            foreach (var (p, list) in source)
            {
                Add(p, [.. list]);
            }
        }

        public SelectionHistoryData Clone() => new(this);
    }
}
