using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public readonly struct JudgeInfo(
        JudgeType type,
        ComboChange comboChange,
        bool isMiss,
        int player,
        double error,
        double scoreGain,
        double gageGain)
    {
        public readonly JudgeType Type = type;
        public readonly ComboChange ComboChange = comboChange;
        public readonly bool IsMiss = isMiss;
        public readonly int Player = player;
        public readonly double Error = error;
        public readonly double ScoreGain = scoreGain;
        public readonly double GaugeGain = gageGain;
    }
}
