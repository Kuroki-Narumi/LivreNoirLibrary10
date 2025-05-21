using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media
{
    public partial class DecimalMultiTimeline<TValue> : XMultiTimelineBase<decimal, TValue, Operator_decimal>
    {
        protected (int Start, int Length) GetPositionIndex(List<decimal> list, Range<double> range) => list.IndexRange(range);
        public ReadOnlySpan<decimal> GetPositions(Range<double> range) => _pos_list.Range(range);
        public void RemoveRange(Range<double> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<decimal, decimal> converter, Range<double> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(decimal, TValue)> Range(Range<double> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public int RemoveIf(Predicate<decimal, TValue> predicate, Range<double> range) => RemoveIfCore(predicate, GetPositionIndex(_pos_list, range));
        public void MoveIf(Predicate<decimal, TValue> predicate, Func<decimal, decimal> converter, Range<double> range) => MoveIfCore(predicate, converter, GetPositionIndex(_pos_list, range));
        public bool Find(Predicate<decimal, TValue> predicate, Range<double> range, out decimal position, [MaybeNullWhen(false)] out TValue value) => FindCore(predicate, GetPositionIndex(_pos_list, range), out position, out value);
        public List<(decimal, TValue)> FindAll(Predicate<decimal, TValue> predicate, Range<double> range) => FindAllCore(predicate, GetPositionIndex(_pos_list, range));
        public void CopyTo<T>(T destination, Range<double> srcRange) where T : IXMultiTimeline<decimal, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<double> srcRange, decimal destOffset) where T : IXMultiTimeline<decimal, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

    }
}