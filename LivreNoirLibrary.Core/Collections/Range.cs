using System;

namespace LivreNoirLibrary.Collections
{
    public readonly struct Range<T>
        where T : struct
    {
        public readonly T Start;
        public readonly T End;
        public readonly bool IsStartEnabled;
        public readonly bool IsEndEnabled;
        public readonly bool IncludesEnd;

        internal Range(T start, T end, bool isStartEnabled, bool isEndEnabled, bool includesEnd)
        {
            start.Equals(end);
            Start = start;
            End = end;
            IsStartEnabled = isStartEnabled;
            IsEndEnabled = isEndEnabled;
            IncludesEnd = includesEnd;
        }
        public Range(T start, T end, bool includesEnd = false) : this(start, end, true, true, includesEnd) { }

        public override string ToString() => $"[{(IsStartEnabled ? Start : "")}..{(IsEndEnabled ? End : "")}{(IncludesEnd ? ']' : ')')}";
        public static Range<T> StartAt(T start) => new(start, default, true, false, true);
        public static Range<T> EndAt(T end, bool includesEnd = false) => new(default, end, false, true, includesEnd);
        public static Range<T> All => new(default, default, false, false, true);

        public static implicit operator Range<T>((T start, T end) tuple) => new(tuple.start, tuple.end, true, true, false);
        public static implicit operator Range<T>((T start, T end, bool includesEnd) tuple) => new(tuple.start, tuple.end, true, true, tuple.includesEnd);

        public static Range<T> GetAuto(T? start, T? end, bool includesEnd)
        {
            bool se = false, ee = false;
            if (start is T s)
            {
                se = true;
            }
            else
            {
                s = default;
            }
            if (end is T e)
            {
                ee = true;
            }
            else
            {
                e = default;
            }
            return new(s, e, se, ee, includesEnd);
        }
    }

    public static class RangeUtils
    {
        public static Range<T> Get<T>(T start, T end, bool includesEnd = false) where T : struct => new(start, end, includesEnd);
        public static Range<T> StartAt<T>(T start) where T : struct => Range<T>.StartAt(start);
        public static Range<T> EndAt<T>(T end, bool includesEnd = false) where T : struct => Range<T>.EndAt(end, includesEnd);
        public static Range<T> All<T>() where T : struct => Range<T>.All;
        public static Range<T> GetAuto<T>(T? start, T? end, bool includesEnd = false) where T : struct => Range<T>.GetAuto(start, end, includesEnd);
    }
}
