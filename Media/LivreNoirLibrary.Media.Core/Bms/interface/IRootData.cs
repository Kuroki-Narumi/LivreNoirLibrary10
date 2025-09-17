using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IRootData : IBmsData
    {
        public ChartType ChartType { get; }
    }
}
