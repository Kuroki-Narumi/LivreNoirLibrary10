using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class ParseState(IBmsDataUnit data)
    {
        public IBmsDataUnit Data { get; } = data;

        public Dictionary<int, int> BgmLaneCounts { get; } = [];
        public HashSet<Channel> LastLongNotes { get; } = [];
        public SortedDictionary<Channel, List<(int, string)>> UnProcessedLines { get; } = [];
        public List<string> Comments { get; } = [];
    }
}
