using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IFlowContainer : INoteObject
    {
        FlowType Type { get; set; }
        int Max { get; set; }
        bool IsFixed { get; set; }

        List<FlowBranch> Branches { get; }
        FlowBranch? DefaultBranch { get; set; }
    }
}
