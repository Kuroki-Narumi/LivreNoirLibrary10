using System;

namespace LivreNoirLibrary.YuGiOh
{
    [Flags]
    public enum LocaleType
    {
        None = 0,
        Ocg = 1,
        Tcg = 2,
        Both = Ocg | Tcg,
    }
}
