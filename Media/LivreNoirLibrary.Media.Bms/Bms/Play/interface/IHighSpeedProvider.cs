using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public interface IHighSpeedProvider
    {
        double HighSpeed { get; }
        HsCorrectionMode HsCorrectionMode { get; }
        double HighSpeedReference => double.NaN;
        double HighSpeedCorrection { get; set; }
    }
}