using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IBmsDataUnit : INoteObject, IBarLengthProvider<double>
    {
        SortedDictionary<HeaderType, string> MainHeaders { get; }
        List<Header> SubHeaders { get; }
        IDefListCollection DefLists { get; }
        IBarLengthCollection BarDefs { get; }
        ITimeline Timeline { get; }
        List<IFlowContainer> Flows { get; }

        double IBarLengthProvider<double>.GetBarLength(int number) => BarDefs.TryGetValue(number, out var value) ? value : BmsConstants.DefaultBarLength;
    }
}
