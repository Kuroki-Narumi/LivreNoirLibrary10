using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public class BmsHistoryData(byte[] data, Selection selection)
    {
        public byte[] MainData { get; } = data;
        public SelectionHistory Selection { get; } = new(selection);
        public Dictionary<FlowAddress, FlowHistoryItem> FlowData { get; } = [];
    }
}
