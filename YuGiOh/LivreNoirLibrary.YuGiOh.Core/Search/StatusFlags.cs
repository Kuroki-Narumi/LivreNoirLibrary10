using System;

namespace LivreNoirLibrary.YuGiOh.Search
{
    [Flags]
    public enum StatusFlags
    {
        Normal = 1,
        Effect = 2,
        AbilityPerf = 4,
        StatsAdvanced = 8,
        LinkMarkerPerf = 16,
    }
}
