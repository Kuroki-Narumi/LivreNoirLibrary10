using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public readonly struct GaugeGain(bool isRelative, double value)
    {
        public readonly bool IsRelative = isRelative;
        public readonly double Value = value;

        public static GaugeGain Relative(double value) => new(true, value);
        public static GaugeGain Absolute(double value) => new(false, value);

        public static implicit operator GaugeGain(double value) => Absolute(value);

        public double GetActualValue(double baseValue) => IsRelative ? baseValue * Value : Value;
    }
}
