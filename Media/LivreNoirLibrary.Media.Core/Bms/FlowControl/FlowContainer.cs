using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class FlowContainer() : ObjectBase
    {
        public FlowType Type { get; set; }
        public int Max { get; set; }
        public bool IsFixed { get; set; }
        public List<FlowBranch> Branches { get; } = [];
        public FlowBranch? DefaultBranch { get; set; }
    }
}
