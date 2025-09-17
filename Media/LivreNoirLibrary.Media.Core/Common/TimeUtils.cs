using System;

namespace LivreNoirLibrary.Media
{
    public static partial class TimeUtils
    {
        public static double MicroSeconds2Bpm(int value) => Math.Floor(60_000_000_000d / value) * 0.001;
        public static int Bpm2MicroSeconds(double value) => (int)Math.Floor(60_000_000d / value);
    }
}
