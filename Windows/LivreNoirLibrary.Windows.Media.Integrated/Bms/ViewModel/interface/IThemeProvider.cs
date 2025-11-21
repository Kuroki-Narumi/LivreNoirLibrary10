using System;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public interface IThemeProvider
    {
        Theme Theme { get; set; }
        int ConductorIndex { get; set; }
        int MetaIndex { get; set; }
        int KeyIndex { get; set; }
        ScratchPosition ScratchPosition { get; set; }
        int BgmCount { get; set; }
        int LaneScale { get; set; }
    }
}
