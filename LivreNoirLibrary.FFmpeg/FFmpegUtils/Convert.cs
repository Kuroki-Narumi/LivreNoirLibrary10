using System;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public static unsafe partial class FFmpegUtils
    {
        public const string NA = "N/A";
        public static string? GetString(void* ptr) => Marshal.PtrToStringUTF8((nint)ptr);
        public static string GetName(void* ptr) => GetString(ptr) is string text ? Text.StringPool.Get(text) : NA;

        public static bool IsKeyFrame(AVPacket* packet) => (packet->flags & ffmpeg.AV_PKT_FLAG_KEY) is not 0;

        public static long ToTicks(this long timeStamp, AVRational timeBase) => 
            ffmpeg.av_rescale_rnd(timeStamp, TimeSpan.TicksPerSecond * timeBase.num, timeBase.den, AVRounding.AV_ROUND_NEAR_INF);
        public static long ToTicks(this Rational value) =>
            ffmpeg.av_rescale_rnd(value.Numerator, TimeSpan.TicksPerSecond, value.Denominator, AVRounding.AV_ROUND_NEAR_INF);
        public static TimeSpan ToTimeSpan(this Rational value, MidpointRounding mode = MidpointRounding.ToEven) 
            => TimeSpan.FromTicks((long)Math.Round((double)value.Numerator * TimeSpan.TicksPerSecond / value.Denominator, mode));

        public static long ToTimeStamp(this long ticks, AVRational timeBase) =>
            ffmpeg.av_rescale_rnd(ticks, timeBase.den, TimeSpan.TicksPerSecond * timeBase.num, AVRounding.AV_ROUND_NEAR_INF);
        public static long ToTimeStamp(this Rational value, AVRational timeBase) =>
            ffmpeg.av_rescale_rnd(value.Numerator, timeBase.den, value.Denominator * timeBase.num, AVRounding.AV_ROUND_NEAR_INF);

        public static Rational ToRational(this long ticks) => new(ticks, TimeSpan.TicksPerSecond);
        public static Rational ToRational(this long timeStamp, AVRational timeBase) => new(timeStamp * timeBase.num, timeBase.den);
        public static Rational ToRational(this TimeSpan timeSpan) => new(timeSpan.Ticks, TimeSpan.TicksPerSecond);

        public static Rational ToRational(this AVRational value) => new(value.num, value.den);
        public static AVRational ToAVRational(this Rational value) => new() { num = (int)value.Numerator, den = (int)value.Denominator };

        public static string ToS(this AVRational value) => $"{value.num}/{value.den}";
    }
}
