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

        extension<T>(T obj) where T : IBarPositionProvider, ITimeCounter
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double Position2Time(BarPosition position) => obj.Beat2Time(obj.GetAbsolutePosition(position));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public BarPosition Time2Position(double time) => obj.GetBarPosition(obj.Time2Beat(time));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public long Position2Tick(BarPosition position) => obj.Beat2Tick(obj.GetAbsolutePosition(position));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public BarPosition Tick2Position(long time) => obj.GetBarPosition(obj.Tick2Beat(time));
        }
    }
}
