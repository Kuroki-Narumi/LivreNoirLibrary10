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
    public partial class DoubleMultiTimeline<TValue> : XMultiTimelineBase<double, TValue, Operator_double>
    {
        protected (int Start, int Length) GetPositionIndex(List<double> list, Range<decimal> range) => list.IndexRange(range);
        public ReadOnlySpan<double> GetPositions(Range<decimal> range) => _pos_list.Range(range);
        public void RemoveRange(Range<decimal> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<double, double> converter, Range<decimal> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(double, TValue)> Range(Range<decimal> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public int RemoveIf(Predicate<double, TValue> predicate, Range<decimal> range) => RemoveIfCore(predicate, GetPositionIndex(_pos_list, range));
        public void MoveIf(Predicate<double, TValue> predicate, Func<double, double> converter, Range<decimal> range) => MoveIfCore(predicate, converter, GetPositionIndex(_pos_list, range));
        public bool Find(Predicate<double, TValue> predicate, Range<decimal> range, out double position, [MaybeNullWhen(false)] out TValue value) => FindCore(predicate, GetPositionIndex(_pos_list, range), out position, out value);
        public List<(double, TValue)> FindAll(Predicate<double, TValue> predicate, Range<decimal> range) => FindAllCore(predicate, GetPositionIndex(_pos_list, range));
        public void CopyTo<T>(T destination, Range<decimal> srcRange) where T : IXMultiTimeline<double, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<decimal> srcRange, double destOffset) where T : IXMultiTimeline<double, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

    }
}