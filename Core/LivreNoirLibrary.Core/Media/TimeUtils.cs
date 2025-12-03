using System;

namespace LivreNoirLibrary.Media
{
    public static partial class TimeUtils
    {
        public const double SecondsPerTickD = 1d / TimeSpan.TicksPerSecond;
        public const decimal SecondsPerTickM = 1m / TimeSpan.TicksPerSecond;
        public const double MillisecondsPerTickD = 1d / TimeSpan.TicksPerMillisecond;
        public const decimal MillisecondsPerTickM = 1m / TimeSpan.TicksPerMillisecond;

        public static double MicroSeconds2Bpm(int value) => Math.Floor(60_000_000_000d / value) * 0.001;
        public static int Bpm2MicroSeconds(double value) => (int)Math.Floor(60_000_000d / value);
        public static long Seconds2Ticks(double value) => (long)(value * TimeSpan.TicksPerSecond);
        public static long Seconds2Ticks(decimal value) => (long)(value * TimeSpan.TicksPerSecond);
        public static double Ticks2Seconds(long value) => value * SecondsPerTickD;
        public static decimal Ticks2SecondsM(long value) => value * SecondsPerTickM;
        public static double Ticks2Milliseconds(long value) => value * MillisecondsPerTickD;
        public static decimal Ticks2MillisecondsM(long value) => value * MillisecondsPerTickM;
        public static string Ticks2MsText(long value) => $"{Ticks2Milliseconds(value):F4}ms";
    }
}
