using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public static class TimerIdOffsets
    {
        public const int Play = 1000;

        public const int GeneralJudge = 1010;
        public const int Miss = 0;
        public const int Perfect = 1;
        public const int Great = 2;
        public const int Good = 3;
        public const int Bad = 4;
        public const int Through = 5;
        public const int BlankShot = 6;

        public const int PlayerJudge = 1020;
        public const int Judge = 1;
        public const int Early = 2;
        public const int Late = 3;

        public const int Button = 1100;
        public const int Press = 1;
        public const int Release = 2;
        public const int Bomb = 3;
        public const int LongBomb = 4;
        public const int Mine = 5;
    }
}
