using System;
using System.Collections.Generic;
using System.Text.Json;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bmxon
{
    public class FlowBranch : ObjectBase
    {
        public int DataId { get; set; }
        public List<FlowInfo> Flows { get; } = [];
    }
}
