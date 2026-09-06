using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IFlowBranch : INoteObject
    {
        int DataIndex { get; set; }
    }
}
