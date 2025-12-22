using System;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly struct TimingInfo
    {
        public static TimingInfo Create(double tempo)
        {
            var bps = tempo / 240;
            return new(0, 0, 0, tempo, 0, 1, 1 / bps, bps);
        }

        public double Beat { get; }
        public double Time { get; }
        public double Position { get; }
        public double Tempo { get; }
        public double StopTime { get; }
        public double Scroll { get; }
        public double SecondsPerBeat { get; }
        public double BeatsPerSecond { get; }

        internal TimingInfo(double beat, double time, double position, double tempo, double stopTime, double scroll, double spb, double bps)
        {
            Beat = beat;
            Time = time;
            Position = position;
            Tempo = tempo;
            StopTime = stopTime;
            Scroll = scroll;
            SecondsPerBeat = spb;
            BeatsPerSecond = bps;
        }

        public (TimingInfo Before, TimingInfo After) SplitStop() 
            => (new(Beat, Time, Position, Tempo, StopTime, 0, double.PositiveInfinity, 0), new(Beat, Time + StopTime, Position, Tempo, 0, Scroll, SecondsPerBeat, BeatsPerSecond));
    }
}
