using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace LivreNoirLibrary.Numerics
{
    public class RangeSet<T> : IEnumerable<RangeSet<T>.Range>
        where T : INumber<T>, IMinMaxValue<T>
    {
        private readonly List<Range> _ranges = [];

        public void Clear() => _ranges.Clear();

        public int ValidRangeCount => _ranges.Count;

        public bool Contains(T value)
        {
            var ranges = _ranges;
            var left = 0;
            var right = ranges.Count - 1;
            while (left <= right)
            {
                var mid = left + (right - left) / 2;
                var range = ranges[mid];
                switch (range.CompareTo(value))
                {
                    case 0:
                        return true;
                    case < 0:
                        right = mid - 1;
                        break;
                    case > 0:
                        left = mid + 1;
                        break;
                }
            }
            return false;
        }

        public void Add(T value) => AddImpl(value, value);
        public void AddRange(T start, T endInclusive) => AddImpl(start, endInclusive);
        public void AddUntil(T value) => AddImpl(T.MinValue, value);
        public void AddSince(T value) => AddImpl(value, T.MaxValue);

        private void AddImpl(T start, T endInclusive)
        {
            // 不正な範囲は修正
            if (start > endInclusive)
            {
                (start, endInclusive) = (endInclusive, start);
            }

            var newRange = new Range(start, endInclusive);
            var ranges = _ranges;
            var count = ranges.Count;

            // 初めて範囲を追加する場合
            if (count is 0)
            {
                ranges.Add(newRange);
                return;
            }

            // アクセスの高速化のためにSpan化
            ReadOnlySpan<Range> span = ranges.AsSpan();

            // マージ開始位置の探索
            var left = 0;
            var right = count - 1;
            // マージ可能な範囲の開始位置(見つからなかった場合は-1)
            var mergeStart = -1;
            // マージ可能範囲が見つからなかった場合に挿入する位置(初期値は末尾)
            var insertIndex = count;

            while (left <= right)
            {
                var mid = left + (right - left) / 2;
                var other = span[mid];
                switch (newRange.CheckCanMerge(other))
                {
                    case 0: // マージ可能な範囲が見つかった
                        // 範囲の更新
                        newRange = newRange.Merge(other);
                        mergeStart = mid;
                        right = mid - 1; // さらに左側にもマージ可能な範囲があるかもしれないので探索を続ける
                        break;
                    case < 0: // 新しい範囲は mid より左側
                        insertIndex = mid;
                        right = mid - 1;
                        break;
                    case > 0: // 新しい範囲は mid より右側
                        insertIndex = mid + 1;
                        left = mid + 1;
                        break;
                }
            }

            // マージ開始位置が見つかった場合
            if (mergeStart is >= 0)
            {
                // マージ終了位置の探索
                var mergeEnd = mergeStart;
                for (var i = mergeStart + 1; i < count; i++)
                {
                    var other = span[i];
                    if (newRange.CheckCanMerge(other) is 0)
                    {
                        // 範囲の更新
                        newRange = newRange.Merge(other);
                        mergeEnd = i;
                    }
                    else
                    {
                        break;
                    }
                }
                // 古い範囲を削除
                ranges.RemoveRange(mergeStart, mergeEnd - mergeStart + 1);
                // 挿入位置はマージ開始位置と同じ
                insertIndex = mergeStart;
            }

            // いずれにせよ新しい範囲を挿入
            ranges.Insert(insertIndex, newRange);
        }
        
        public void OverwriteFrom(RangeSet<T> other)
        {
            Clear();
            _ranges.AddRange(other._ranges);
        }

        public override string ToString() => ToString(ToStringFactory<T>.Instance);

        public string ToString(Func<T, string?> formatter, string rangeSeparator = "..", string setSeparator = ", ")
        {
            using var obj = ObjectPool.RentStringBuilder(out var sb);
            sb.Append('{');
            var first = true;
            foreach (var range in _ranges.AsSpan())
            {
                if (!first)
                {
                    sb.Append(setSeparator);
                }
                sb.Append(range.ToString(formatter, rangeSeparator));
                first = false;
            }
            sb.Append('}');
            return sb.ToString();
        }

        public List<Range>.Enumerator GetEnumerator() => _ranges.GetEnumerator();
        IEnumerator<Range> IEnumerable<Range>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public readonly struct Range(T start, T end)
        {
            public readonly T Start = start;
            public readonly T End = end;

            /// <summary>
            /// Compares the specified value to the range and indicates whether it is less than, within, or greater than the range.
            /// </summary>
            /// <param name="value">The value to compare to the range boundaries.</param>
            /// <returns>
            /// A signed integer that indicates the relative position of the value to the range: <br/>
            /// &lt;0 - the value is less than the start of the range. <br/>
            /// 0 - the value is within the range. <br/>
            /// &gt;0 - the value is greater than the end of the range.
            /// </returns>
            public int CompareTo(T value) => value < Start ? -1 : value <= End ? 0 : 1;

            /// <summary>
            /// Determines the relative position of the current range to another range and indicates whether they can be merged.
            /// </summary>
            /// <remarks>
            /// Use this method to check if two ranges are disjoint or if they can be
            /// combined into a single continuous range. The method does not modify either range.
            /// </remarks>
            /// <param name="other">The range to compare with the current range.</param>
            /// <returns>
            /// A value indicating the merge possibility: <br/>
            /// &lt;0 - the current range is entirely before the other range. <br/>
            /// 0 - the ranges overlap or are adjacent and can be merged. <br/>
            /// &gt;0 - the current range is entirely after the other range.
            /// </returns>
            public int CheckCanMerge(Range other)
            {
                var otherStart = other.Start;
                var otherEnd = other.End;
                if (End < otherStart && End < --otherStart)
                {
                    return -1;
                }
                if (Start <= otherEnd || Start <= ++otherEnd)
                {
                    return 0;
                }
                return 1;
            }

            public Range Merge(Range other) => new(T.Min(Start, other.Start), T.Max(End, other.End));

            public void Deconstruct(out T start, out T end)
            {
                start = Start;
                end = End;
            }

            public override string? ToString() => ToString(ToStringFactory<T>.Instance);

            public string? ToString(Func<T, string?> formatter, string separator = "..")
            {
                if (Start <= T.MinValue)
                {
                    if (End >= T.MaxValue)
                    {
                        return separator;
                    }
                    else
                    {
                        return $"{separator}{formatter(End)}";
                    }
                }
                if (End >= T.MaxValue)
                {
                    return $"{formatter(Start)}{separator}";
                }
                if (Start == End)
                {
                    return formatter(Start);
                }
                return $"{formatter(Start)}{separator}{formatter(End)}";
            }
        }
    }
}