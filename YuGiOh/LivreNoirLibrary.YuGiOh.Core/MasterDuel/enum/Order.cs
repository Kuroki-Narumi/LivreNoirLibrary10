using System;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    [Flags]
    public enum Order : byte
    {
        IsSecond = 1,
        IsCoinLose = 2,

        First = 0,
        CFirst = IsCoinLose,
        Second = IsSecond | IsCoinLose,
        CSecond = IsSecond,
    }
}
