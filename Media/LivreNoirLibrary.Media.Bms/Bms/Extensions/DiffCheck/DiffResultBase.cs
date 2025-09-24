using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class DiffResultBase
    {
        public List<HeaderDiff> Headers { get; } = [];
        public Dictionary<DefType, Dictionary<short, DefDiff>> DefLists { get; } = [];
        public SortedDictionary<int, BarDefDiff> BarDefs { get; } = [];
        public SortedDictionary<int, NoteDiffList> Notes { get; } = [];
        public List<FlowDiff> Flows { get; } = [];

        public bool IsEmpty => (Headers.Count + DefLists.Count + BarDefs.Count + Notes.Count + Flows.Count) is 0;
    }
}
