using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using System;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class UnuniformedWaveImageData(int sourceId) : ObservableObjectBase
    {
        public int SourceId { get; set => SetValue(ref field, value); } = sourceId;
        public long SampleOffset { get; set => SetValue(ref field, value); }
        public Rational AbsolutePosition { get; set => SetValue(ref field, value); }
        public double Left { get; set => SetValue(ref field, value); }
        public double Width { get; set => SetValue(ref field, value); }
        public bool ShowLevelLine { get; set => SetValue(ref field, value); }
    }
}
