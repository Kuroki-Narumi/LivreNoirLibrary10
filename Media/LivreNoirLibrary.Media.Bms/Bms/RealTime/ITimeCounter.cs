using System;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public interface ITimeCounter
    {
        public decimal Beat2Time(decimal beat);
        public decimal Time2Beat(decimal time);
    }

    public static class ITimeCounterExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal Beat2Time(this ITimeCounter counter, Rational beat) => counter.Beat2Time((decimal)beat);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Beat2Tick(this ITimeCounter counter, decimal beat) => TimeUtils.Seconds2Ticks(counter.Beat2Time(beat));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Beat2Tick(this ITimeCounter counter, Rational beat) => TimeUtils.Seconds2Ticks(counter.Beat2Time((decimal)beat));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan Beat2TimeSpan(this ITimeCounter counter, decimal beat) => TimeSpan.FromTicks(Beat2Tick(counter, beat));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan Beat2TimeSpan(this ITimeCounter counter, Rational beat) => TimeSpan.FromTicks(Beat2Tick(counter, beat));
        public static decimal Tick2Beat(this ITimeCounter counter, long ticks) => counter.Time2Beat(TimeUtils.Ticks2SecondsM(ticks));
    }
}
