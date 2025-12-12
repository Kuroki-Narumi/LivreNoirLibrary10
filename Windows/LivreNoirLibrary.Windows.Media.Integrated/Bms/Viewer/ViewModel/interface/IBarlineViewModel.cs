using LivreNoirLibrary.Media.Bms;
using System;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public interface IBarlineViewModel
    {
        int SmallGrid { get; set; }
        int LargeGrid { get; set; }
        void LoadData(IBmsDataUnit? source);
    }
}
