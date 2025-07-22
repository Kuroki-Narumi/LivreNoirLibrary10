using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class MultiDefOptions : IndexesOptionsBase
    {
        [ObservableProperty]
        private int _minimumInterval = 0;
        [ObservableProperty]
        private double _threshold = -24;
        [ObservableProperty]
        private int _maxCount = 16;
        [ObservableProperty]
        private bool _insertDefIndex = true;

        private static int CoerceMaxCount(int value) => Math.Clamp(value, 1, 16);
    }
}
