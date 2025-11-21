using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public abstract class TimelineBase<TX, TValue, TOperator> : ITimeline<TX>
        where TX : struct
        where TOperator : IPositionOperator<TX>
    {
        protected readonly List<TX> _pos_list = [];

        public abstract int Count { get; }
        public bool IsEmpty => _pos_list.Count is 0;

        public TX FirstPosition => _pos_list.Count > 0 ? _pos_list[0] : TOperator.Zero;
        public TX LastPosition => _pos_list.Count > 0 ? _pos_list[^1] : TOperator.Zero;

        public ReadOnlySpan<TX> GetPositions() => _pos_list.AsSpan();
        public ReadOnlySpan<TX> GetPositions(Range<TX> range) => _pos_list.Range<TX, TX, TOperator>(range);

        public abstract void Clear();
        public abstract bool RemoveAt(TX position);
        public void RemoveRange(Range<TX> range) => RemoveRangeCore(GetPositionIndex(range));

        public void Move(Func<TX, TX> converter) => MoveCore(converter, GetPositionIndex());
        public void Move(Func<TX, TX> converter, Range<TX> range) => MoveCore(converter, GetPositionIndex(range));

        protected (int Start, int Length) GetPositionIndex() => GetPositionIndex(_pos_list);
        protected (int Start, int Length) GetPositionIndex(Range<TX> range) => GetPositionIndex(_pos_list, range);
        protected bool TryGetIndex(TX position, out int index) => TryGetIndex(_pos_list, position, out index);

        protected static (int Start, int Length) GetPositionIndex(List<TX> list) => (0, list.Count);
        protected static (int Start, int Length) GetPositionIndex(List<TX> list, Range<TX> range) => list.IndexRange<TX, TX, TOperator>(range);
        protected static bool TryGetIndex(List<TX> list, TX position, out int index)
        {
            index = list.BinarySearch(position);
            return (uint)index < (uint)list.Count;
        }

        protected abstract void RemoveRangeCore((int Start, int Length) range);
        protected abstract void MoveCore(Func<TX, TX> converter, (int Start, int Length) range);
    }
}
