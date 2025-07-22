using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class ReductOptions : IndexesOptionsBase
    {
        public const float DefaultWaveForm = 0.9f;
        public const float DefaultRms = 0.99f;
        public const float DefaultCentrold = 0.99f;

        [ObservableProperty]
        private float _waveForm = DefaultWaveForm;
        [ObservableProperty]
        private float _rms = DefaultRms;
        [ObservableProperty]
        private float _centroid = DefaultCentrold;

        public void SetDefaultThresholds()
        {
            WaveForm = DefaultWaveForm;
            Rms = DefaultRms;
            Centroid = DefaultCentrold;
        }
    }
}
