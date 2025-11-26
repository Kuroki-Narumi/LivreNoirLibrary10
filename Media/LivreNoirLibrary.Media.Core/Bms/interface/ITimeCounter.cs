using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface ITimeCounter
    {
        double MinTempo { get; }
        double MaxTempo { get; }
        double MainTempo { get; }
        double MainTimeTempo { get; }

        double Beat2Time(double absolutePosition);
        double Time2Beat(double time);
        double GetHighSpeed(double time);
    }
}
