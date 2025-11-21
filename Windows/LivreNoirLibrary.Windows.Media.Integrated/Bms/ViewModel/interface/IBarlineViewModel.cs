using LivreNoirLibrary.Media.Bms;
using System;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public interface IBarlineViewModel
    {
        int SmallGrid { get; set; }
        int LargeGrid { get; set; }
        void LoadData(IBmsDataUnit? source);
    }
}
