using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class MultiDefOptions : IndexesOptionsBase
    {
        [ObservableProperty]
        private decimal _minimumInterval = 0.1m;
        [ObservableProperty]
        private double _threshold = -24;
        [ObservableProperty]
        private int _maxCount = 16;
        [ObservableProperty]
        private bool _insertDefIndex;

        private static int CoerceMaxCount(int value) => Math.Clamp(value, 1, 16);
    }
}
