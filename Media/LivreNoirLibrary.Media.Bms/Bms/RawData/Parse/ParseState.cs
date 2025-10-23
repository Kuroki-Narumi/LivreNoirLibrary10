using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsParser
    {
        public class ParseState(ParseState? parent, BaseData data)
        {
            public BaseData Data { get; } = data;
            public ParseState? Parent { get; } = parent;

            public Dictionary<int, int> BgmLaneCounts { get; } = [];
            public HashSet<int> LastLongNotes { get; } = [];
            public SortedDictionary<Channel, List<(int, string)>> UnProcessedLines { get; } = [];
            public List<string> Comments { get; } = [];

            public void AddUnProcessedLine(int number, Channel channel, string line)
            {
                UnProcessedLines.GetOrAdd(channel).Add((number, line));
            }

            public int UpdateBgmLane(int number)
            {
                if (!BgmLaneCounts.TryGetValue(number, out var count))
                {
                    count = 0;
                }
                BgmLaneCounts[number] = count + 1;
                return -count;
            }
        }
    }
}
