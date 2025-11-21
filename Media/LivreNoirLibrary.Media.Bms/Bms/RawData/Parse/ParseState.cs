using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsParser
    {
        public class ParseState(IBmsDataUnit data)
        {
            public IBmsDataUnit Data { get; } = data;

            public Dictionary<int, int> BgmLaneCounts { get; } = [];
            public HashSet<Channel> LastLongNotes { get; } = [];
            public SortedDictionary<Channel, List<(int, string)>> UnProcessedLines { get; } = [];
            public List<string> Comments { get; } = [];

            public void AddUnProcessedLine(int number, Channel channel, string line)
            {
                UnProcessedLines.GetOrAdd(channel).Add((number, line));
            }

            public Channel UpdateBgmLane(int number)
            {
                if (!BgmLaneCounts.TryGetValue(number, out var count))
                {
                    count = 0;
                }
                BgmLaneCounts[number] = count + 1;
                return Channel.Bgm_Start + (short)count;
            }
        }
    }
}
