using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public partial class DecimalTimeline<TValue> : XSingleTimelineBase<decimal, TValue, Operator_decimal>
    {
        protected (int Start, int Length) GetPositionIndex(List<decimal> list, Range<double> range) => list.IndexRange(range);
        public ReadOnlySpan<decimal> GetPositions(Range<double> range) => _pos_list.Range(range);
        public void RemoveRange(Range<double> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<decimal, decimal> converter, Range<double> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(decimal, TValue)> Range(Range<double> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public void CopyTo(IXSingleTimeline<decimal, TValue> destination, Range<double> srcRange)
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo(IXSingleTimeline<decimal, TValue> destination, Range<double> srcRange, decimal destOffset)
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

    }
}