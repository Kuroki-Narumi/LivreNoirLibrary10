using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public interface ITimeline : IXMultiTimeline<BarPosition, INote>
    {
        public void InsertBar(int number);
        public void DeleteBar(int number);
        public void CutBar(IBarPositionProvider provider, Dictionary<int, BarResizeInfo> newValues, bool overlap);
    }
}
