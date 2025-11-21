using System;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly record struct TimingInfo(double Beat, double Position, double Time, double Tempo, double Stop, double Scroll)
    {
        public double SecondsPerBeat { get; } = 240 / Tempo;
        public double BeatsPerSecond { get; } = Tempo / 240;
    }
}
