using LivreNoirLibrary.ObjectModel;
using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface ITimeCounter : IClear
    {
        double Beat2Time(double absolutePosition);
        double Time2Beat(double time);
        double GetHighSpeed(double time);
    }
}
