using System;

namespace LivreNoirLibrary.Media
{
    public static partial class MediaUtils
    {
        public static double MicroSeconds2Bpm(int value) => Math.Floor(60_000_000_000d / value) / 1000d;
        public static decimal MicroSeconds2BpmM(int value) => Math.Floor(60_000_000_000m / value) / 1000m;
        public static int Bpm2MicroSeconds(double value) => (int)Math.Floor(60_000_000d / value);
        public static int Bpm2MicroSeconds(decimal value) => (int)Math.Floor(60_000_000m / value);
    }
}
