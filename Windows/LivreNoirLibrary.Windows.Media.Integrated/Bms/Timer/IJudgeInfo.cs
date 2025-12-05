using System;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly struct JudgeInfo(
        JudgeType type,
        ComboChange comboChange,
        double scoreGain,
        double gageGain)
    {
        public readonly JudgeType Type = type;
        public readonly ComboChange ComboChange = comboChange;
        public readonly double ScoreGain = scoreGain;
        public readonly double GageGain = gageGain;
    }
}
