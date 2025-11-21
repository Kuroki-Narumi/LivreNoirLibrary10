using System;

namespace LivreNoirLibrary.Windows.Controls.Wave
{
    public interface ISpectrumProvider
    {
        LivreNoirLibrary.Media.Wave.Spectrum? SpectrumData { get; }
        long SamplePosition { get; }
        ReadOnlySpan<double> GetFrequencyPositions();
    }
}
