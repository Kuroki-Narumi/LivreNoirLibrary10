using System;

namespace LivreNoirLibrary.Media.Bms
{
    public enum JudgeType
    {
        None,
        Perfect = TimerIdOffsets.Perfect,
        Great = TimerIdOffsets.Great,
        Good = TimerIdOffsets.Good,
        Bad = TimerIdOffsets.Bad,
        Miss = TimerIdOffsets.Miss,
    }

    public enum ComboChange
    {
        Continue,
        Increase,
        Reset,
    }
}
