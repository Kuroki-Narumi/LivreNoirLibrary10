using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public partial class LongTimeline<TValue> : XSingleTimelineBase<long, TValue, Operator_long>
    {
        protected (int Start, int Length) GetPositionIndex(List<long> list, Range<double> range) => list.IndexRange(range);
        public ReadOnlySpan<long> GetPositions(Range<double> range) => _pos_list.Range(range);
        public void RemoveRange(Range<double> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<long, long> converter, Range<double> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(long, TValue)> Range(Range<double> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public void CopyTo<T>(T destination, Range<double> srcRange) where T : IXSingleTimeline<long, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<double> srcRange, long destOffset) where T : IXSingleTimeline<long, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

        protected (int Start, int Length) GetPositionIndex(List<long> list, Range<decimal> range) => list.IndexRange(range);
        public ReadOnlySpan<long> GetPositions(Range<decimal> range) => _pos_list.Range(range);
        public void RemoveRange(Range<decimal> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<long, long> converter, Range<decimal> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(long, TValue)> Range(Range<decimal> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public void CopyTo<T>(T destination, Range<decimal> srcRange) where T : IXSingleTimeline<long, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<decimal> srcRange, long destOffset) where T : IXSingleTimeline<long, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

    }
}