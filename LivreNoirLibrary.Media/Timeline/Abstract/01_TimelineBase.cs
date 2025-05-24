using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media
{
    public delegate void ValueWriter<T>(BinaryWriter writer, T value);
    public delegate T ValueReader<T>(BinaryReader reader);
    public delegate TValue ValueReader<TX, TValue>(BinaryReader reader, TX position);
    public delegate TValue ValueReader<TY, TX, TValue>(BinaryReader reader, TY key, TX position);

    public abstract class TimelineBase<TX, TValue, TOperator> : ITimeline<TX, TValue>
        where TX : struct, IComparable<TX>
        where TOperator : IPositionOperator<TX>, new()
    {
        protected readonly List<TX> _pos_list = [];
        protected readonly TOperator _operator = new();

        public abstract int Count { get; }
        public bool IsEmpty() => _pos_list.Count is 0;

        public TX FirstPosition => _pos_list.Count > 0 ? _pos_list[0] : TOperator.Zero;
        public TX LastPosition => _pos_list.Count > 0 ? _pos_list[^1] : TOperator.Zero;

        public ReadOnlySpan<TX> GetPositions() => CollectionsMarshal.AsSpan(_pos_list);
        public ReadOnlySpan<TX> GetPositions(Range<TX> range) => _pos_list.Range(range);
        public Range<TX> GetRange(TX? start, TX? end, bool includesEnd = false) => Range<TX>.GetAuto(start, end, includesEnd);

        protected (int Start, int Length) GetPositionIndex() => GetPositionIndex(_pos_list);
        protected (int Start, int Length) GetPositionIndex(Range<TX> range) => GetPositionIndex(_pos_list, range);
        protected bool TryGetIndex(TX position, out int index) => TryGetIndex(_pos_list, position, out index);

        protected static (int Start, int Length) GetPositionIndex(List<TX> list) => (0, list.Count);
        protected static (int Start, int Length) GetPositionIndex(List<TX> list, Range<TX> range) => list.IndexRange(range);
        protected static bool TryGetIndex(List<TX> list, TX position, out int index)
        {
            index = list.BinarySearch(position);
            return (uint)index < (uint)list.Count;
        }
        protected static int FindNextOrEqual(List<TX> list, TX position) => list.FindIndex(position, SearchMode.NextOrEqual);
        protected static int FindPreviousOrEqual(List<TX> list, TX position) => list.FindIndex(position, SearchMode.PreviousOrEqual);

        public abstract void Clear();
        public abstract bool RemoveAt(TX position);
        public void RemoveRange(Range<TX> range) => RemoveRangeCore(GetPositionIndex(range));
        protected abstract void RemoveRangeCore((int Start, int Length) range);

        public abstract void Move(TX from, TX to);
        public void Move(Func<TX, TX> converter) => MoveCore(converter, GetPositionIndex());
        public void Move(Func<TX, TX> converter, Range<TX> range) => MoveCore(converter, GetPositionIndex(range));
        protected abstract void MoveCore(Func<TX, TX> converter, (int Start, int Length) range);

        public void InsertSpace(TX offset, TX length)
        {
            SlideCore(FindNextOrEqual(_pos_list, offset), length, true);
        }

        public void DeleteSpace(TX offset, TX length)
        {
            var end = _operator.Add(offset, length);
            RemoveRange(RangeUtils.Get(offset, end));
            SlideCore(FindNextOrEqual(_pos_list, end), length, false);
        }

        protected abstract void SlideCore(int start, TX amount, bool add);
    }
}
