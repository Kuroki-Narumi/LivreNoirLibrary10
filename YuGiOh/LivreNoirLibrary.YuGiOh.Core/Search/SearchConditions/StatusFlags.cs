using System;

namespace LivreNoirLibrary.YuGiOh.Search
{
    [Flags]
    public enum StatusFlags
    {
        None = 0,

        Normal = 1,
        Effect = 2,
        AbilityPerf = 4,
        LinkMarkerPerf = 8,
        StatusExpression = 16,

        Default = LinkMarkerPerf,
    }
}
