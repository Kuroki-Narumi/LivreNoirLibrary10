using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public enum TimerId
    {
        None,
        Scene_Start,
        Scene_Fadeout,

        Play_LoadingStart,
        Play_LoadingFinished,
        Play_MusicStart,
        Play_FullCombo,
        Play_Miss,

    }
}
