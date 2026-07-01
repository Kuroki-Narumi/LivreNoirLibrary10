using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsDataUnit : INoteObject, IBmsDataUnit
    {
        public string? Note { get; set; }
        public SortedDictionary<HeaderType, string> MainHeaders { get; } = [];
        public List<Header> SubHeaders { get; } = [];
        public IDefListCollection DefLists { get; } = new DefListCollection();
        public IBarLengthCollection BarDefs { get; } = new BarLengthCollection();
        public ITimeline Timeline { get; } = new Timeline();
        public List<FlowContainer> Flows { get; } = [];
    }
}
