using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public enum JudgeType
    {
        None,
        Perfect = TimerIdOffsets.Perfect,
        Great = TimerIdOffsets.Great,
        Good = TimerIdOffsets.Good,
        Bad = TimerIdOffsets.Bad,
        Through = TimerIdOffsets.Through,
        BlankShot = TimerIdOffsets.BlankShot,
    }
}
