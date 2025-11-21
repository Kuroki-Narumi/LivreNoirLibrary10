using System;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class SpectrumView
    {
        private readonly struct FreqGrid(double freq, double th, double max)
        {
            public double Freq { get; } = freq;
            public double X { get; } = Math.Log2(freq) / max;
            public string Text { get; } = freq >= 1000 ? $"{freq / 1000:0.0}k" : $"{freq:0}";
            public double Threshold { get; } = th;
        }
    }
}
