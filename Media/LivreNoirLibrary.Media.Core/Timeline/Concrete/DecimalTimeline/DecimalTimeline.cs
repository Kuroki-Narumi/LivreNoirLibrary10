using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
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

        public void CopyTo<T>(T destination, Range<double> srcRange) where T : IXSingleTimeline<decimal, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<double> srcRange, decimal destOffset) where T : IXSingleTimeline<decimal, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

    }
}