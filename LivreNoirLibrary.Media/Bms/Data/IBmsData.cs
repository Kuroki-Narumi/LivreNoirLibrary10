using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IBmsData : IBarPositionProvider
    {
        public IRootData Root { get; }
        public HeaderCollection Headers { get; }
        public DefListCollection DefLists { get; }
        public BarLengthCollection Bars { get; }
        public NoteTimeline Timeline { get; }
    }

    public interface IRootData : IBmsData
    {
        public ChartType ChartType { get; }
        public BarLengthCache BarLengthCache { get; }
        public ReadOnlySpan<FlowContainer> Flows { get; }
        public ReadOnlySpan<FlowData> FlowData { get; }
    }
}
