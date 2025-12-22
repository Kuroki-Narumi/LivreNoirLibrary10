using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public static class TimerIdOffsets
    {
        public const int Play = 1000;

        public const int JudgeBegin = 1010;
        public const int Judge = 1;
        public const int Early = 2;
        public const int Late = 3;
        public const int Miss = 4;

        public const int ButtonBegin = 1100;
        public const int Press = 1;
        public const int Release = 2;
        public const int Bomb = 3;
        public const int LongBomb = 4;
        public const int Mine = 5;
    }
}
