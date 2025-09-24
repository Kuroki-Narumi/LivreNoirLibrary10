using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IBmsData : IBarPositionProvider
    {
        public IBmsData? Parent { get; }
        public IHeaderCollection Headers { get; }
        public IDefListCollection DefLists { get; }
        public ITimeline Timeline { get; }

        public IEnumerable<IBmsData> EachData();

        public Rational GetHead(int number);
        public IEnumerable<BarInfo> EnumerateBars(int first, int last);
        public void ClearBarLength();
        public void SetBarLength(int number, Rational value);
        public void InsertBar(int number, Rational value);
        public void DeleteBar(int number, int count);
    }
}
