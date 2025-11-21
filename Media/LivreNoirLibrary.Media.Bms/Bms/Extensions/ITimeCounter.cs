using System;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Beat2Tick(this ITimeCounter counter, double absolutePosition) => TimeUtils.Seconds2Ticks(counter.Beat2Time(absolutePosition));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan Beat2TimeSpan(this ITimeCounter counter, double absolutePosition) => TimeSpan.FromTicks(Beat2Tick(counter, absolutePosition));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Tick2Beat(this ITimeCounter counter, long ticks) => counter.Time2Beat(TimeUtils.Ticks2Seconds(ticks));
    }
}
