using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    [Flags]
    public enum BgaVisibility
    {
        None = 0,
        Base = 1,
        Layer1 = 2,
        Layer2 = 4,
        Miss = 8,
        HideOnMiss = 16,

        NoPoor = Base | Layer1 | Layer2,
        Default = Base | Layer1 | Layer2 | Miss | HideOnMiss,
    }
}
