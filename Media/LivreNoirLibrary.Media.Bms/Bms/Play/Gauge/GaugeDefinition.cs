using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public class GaugeDefinition
    {
        private readonly Dictionary<JudgeType, GaugeGain> _gains = [];

        public double InitialValue { get; init; }
        public double MinimumValue { get; init; }
        public double MaximumValue { get; init; }

        public double PassingValue { get; init; }
        public double LowValue { get; init; }

        public bool Endurance { get; init; }

        public GaugeDefinition() { }
        public GaugeDefinition(params ReadOnlySpan<(JudgeType, GaugeGain)> gains)
        {
            var g = _gains;
            foreach (var (type, value) in gains)
            {
                g[type] = value;
            }
        }

        public double GetGaugeGain(JudgeType type, double gainBase)
        {
            if (_gains.TryGetValue(type, out var gain))
            {
                return gain.GetActualValue(gainBase);
            }
            return 0;
        }
    }
}
