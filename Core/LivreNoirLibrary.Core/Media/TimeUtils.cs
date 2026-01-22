using System;

namespace LivreNoirLibrary.Media
{
    public static partial class TimeUtils
    {
        public const double SecondsPerTickD = 1d / TimeSpan.TicksPerSecond;
        public const double MillisecondsPerTickD = 1d / TimeSpan.TicksPerMillisecond;
        public const double MicrosecondsPerMinute = 60_000_000;

        public static double MicroSeconds2Bpm(int value) => Math.Round(MicrosecondsPerMinute / value, 3, MidpointRounding.ToEven);
        public static int Bpm2MicroSeconds(double value) => (int)Math.Round(MicrosecondsPerMinute / value);
        public static long Seconds2Ticks(double value) => (long)(value * TimeSpan.TicksPerSecond);
        public static long Seconds2Ticks(decimal value) => (long)(value * TimeSpan.TicksPerSecond);
        public static double Ticks2Seconds(long value) => value * SecondsPerTickD;
        public static double Ticks2Milliseconds(long value) => value * MillisecondsPerTickD;
        public static string Ticks2MsText(long value) => $"{Ticks2Milliseconds(value):F4}ms";

        public static string AutoFormat(this TimeSpan time) => time.Ticks switch
            {
                >= TimeSpan.TicksPerDay => time.ToString(@"d\d\ h\:mm\:ss"),
                >= TimeSpan.TicksPerHour => time.ToString(@"h\:mm\:ss\.f"),
                >= TimeSpan.TicksPerMinute => time.ToString(@"m\:ss\.ff"),
                _ => time.ToString(@"s\.ffff"),
            };

        public static string AutoFormat_Minutes(this TimeSpan time) => time.Ticks switch
            {
                >= TimeSpan.TicksPerDay => time.ToString(@"d\d\ h\:mm\:ss"),
                >= TimeSpan.TicksPerHour => time.ToString(@"h\:mm\:ss"),
                _ => time.ToString(@"m\:ss"),
            };
    }
}
