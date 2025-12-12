using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public class ScoreDefinition
    {
        private readonly Dictionary<JudgeType, double> _gains = [];

        public double InitialValue { get; init; }

        public ScoreDefinition() { }

        public ScoreDefinition(params ReadOnlySpan<(JudgeType, double)> gains)
        {
            var g = _gains;
            foreach (var (type, value) in gains)
            {
                g[type] = value;
            }
        }

        public ScoreDefinition(double initialValue, params ReadOnlySpan<(JudgeType, double)> gains) : this(gains)
        {
            InitialValue = initialValue;
        }

        public double GetScoreGain(JudgeType type) => _gains.TryGetValue(type, out var value) ? value : 0;
    }
}
