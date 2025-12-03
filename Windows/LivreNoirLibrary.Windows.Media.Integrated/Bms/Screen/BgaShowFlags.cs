using System;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    [Flags]
    public enum BgaShowFlags
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
