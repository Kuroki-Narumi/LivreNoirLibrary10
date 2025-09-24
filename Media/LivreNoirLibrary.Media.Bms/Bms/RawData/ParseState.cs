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

            public HashSet<int> LastLongNotes { get; } = [];
            public SortedDictionary<Channel, List<(int, string)>> UnProcessedLines { get; } = [];
            public List<string> Comments { get; } = [];

            public void AddUnProcessedLine(int number, Channel channel, string line)
            {
                UnProcessedLines.GetOrAdd(channel).Add((number, line));
            }
        }
    }
}
