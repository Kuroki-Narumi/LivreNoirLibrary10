using System;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class ReductOptions : IndexesOptionsBase
    {
        public const float DefaultWaveForm = 0.9f;
        public const float DefaultRms = 0.99f;
        public const float DefaultCentrold = 0.99f;

        public float WaveForm { get; set => SetValue(ref field, value); } = DefaultWaveForm;
        public float Rms { get; set => SetValue(ref field, value); } = DefaultRms;
        public float Centroid { get; set => SetValue(ref field, value); } = DefaultCentrold;

        public void SetDefaultThresholds()
        {
            WaveForm = DefaultWaveForm;
            Rms = DefaultRms;
            Centroid = DefaultCentrold;
        }
    }
}
