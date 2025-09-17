using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class DefSortOptions : ObservableObjectBase
    {
        public int Headroom { get; set => SetValue(ref field, value); } = 1;
        public bool RemoveUnusedDef { get; set => SetValue(ref field, value); }
        public bool Sort { get; set => SetValue(ref field, value); } = true;
        public bool SortByName { get; set => SetValue(ref field, value); }
        public bool FixLnEnd { get; set => SetValue(ref field, value); } = true;
        public bool RemoveMultiDef { get; set => SetValue(ref field, value); }
    }
}
