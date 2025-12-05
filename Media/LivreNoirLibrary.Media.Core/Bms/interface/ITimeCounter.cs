using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface ITimeCounter
    {
        double MinTempo { get; }
        double MaxTempo { get; }
        double AverageTempo => (MinTempo + MaxTempo) / 2;
        double MainTempo { get; }
        double MainTimeTempo { get; }
        double LastTime { get; }

        double Beat2Time(double absolutePosition);
        double Time2Beat(double time);
        double GetHighSpeed(double time);
    }
}
