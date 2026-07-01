using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class ParseState(FlowAddress address, IBmsDataUnit data, ParseState? parent)
    {
        public FlowAddress Address { get; } = address;
        public IBmsDataUnit Data { get; } = data;

        public ParseState? Parent { get; } = parent;
        public FlowType ParentFlowType => Parent?.CurrentFlow?.Type ?? FlowType.None;

        public FlowContainer? CurrentFlow { get; set; }
        public FlowAddress CurrentFlowAddress { get; set; } = address;

        public Dictionary<int, int> BgmLaneCounts { get; } = [];
        public HashSet<Channel> LastLongNotes { get; } = [];
        public Dictionary<DefType, Dictionary<short, double>> ConductorDefs { get; } = [];
        public SortedDictionary<Channel, List<(int, string)>> UnProcessedLines { get; } = [];
        public List<string> Comments { get; } = [];
    }
}
