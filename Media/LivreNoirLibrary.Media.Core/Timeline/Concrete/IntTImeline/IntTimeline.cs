using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public partial class IntTimeline<TValue> : XSingleTimelineBase<int, TValue, Operator_int>
    {
        protected (int Start, int Length) GetPositionIndex(List<int> list, Range<long> range) => list.IndexRange(range);
        public ReadOnlySpan<int> GetPositions(Range<long> range) => _pos_list.Range(range);
        public void RemoveRange(Range<long> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<int, int> converter, Range<long> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(int, TValue)> Range(Range<long> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public void CopyTo<T>(T destination, Range<long> srcRange) where T : IXSingleTimeline<int, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<long> srcRange, int destOffset) where T : IXSingleTimeline<int, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

        protected (int Start, int Length) GetPositionIndex(List<int> list, Range<double> range) => list.IndexRange(range);
        public ReadOnlySpan<int> GetPositions(Range<double> range) => _pos_list.Range(range);
        public void RemoveRange(Range<double> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<int, int> converter, Range<double> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(int, TValue)> Range(Range<double> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public void CopyTo<T>(T destination, Range<double> srcRange) where T : IXSingleTimeline<int, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<double> srcRange, int destOffset) where T : IXSingleTimeline<int, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

        protected (int Start, int Length) GetPositionIndex(List<int> list, Range<decimal> range) => list.IndexRange(range);
        public ReadOnlySpan<int> GetPositions(Range<decimal> range) => _pos_list.Range(range);
        public void RemoveRange(Range<decimal> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<int, int> converter, Range<decimal> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(int, TValue)> Range(Range<decimal> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public void CopyTo<T>(T destination, Range<decimal> srcRange) where T : IXSingleTimeline<int, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<decimal> srcRange, int destOffset) where T : IXSingleTimeline<int, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

    }
}