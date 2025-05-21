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
    public partial class LongMultiTimeline<TValue> : XMultiTimelineBase<long, TValue, Operator_long>
    {
        protected (int Start, int Length) GetPositionIndex(List<long> list, Range<double> range) => list.IndexRange(range);
        public ReadOnlySpan<long> GetPositions(Range<double> range) => _pos_list.Range(range);
        public void RemoveRange(Range<double> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<long, long> converter, Range<double> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(long, TValue)> Range(Range<double> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public int RemoveIf(Predicate<long, TValue> predicate, Range<double> range) => RemoveIfCore(predicate, GetPositionIndex(_pos_list, range));
        public void MoveIf(Predicate<long, TValue> predicate, Func<long, long> converter, Range<double> range) => MoveIfCore(predicate, converter, GetPositionIndex(_pos_list, range));
        public bool Find(Predicate<long, TValue> predicate, Range<double> range, out long position, [MaybeNullWhen(false)] out TValue value) => FindCore(predicate, GetPositionIndex(_pos_list, range), out position, out value);
        public List<(long, TValue)> FindAll(Predicate<long, TValue> predicate, Range<double> range) => FindAllCore(predicate, GetPositionIndex(_pos_list, range));
        public void CopyTo<T>(T destination, Range<double> srcRange) where T : IXMultiTimeline<long, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<double> srcRange, long destOffset) where T : IXMultiTimeline<long, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

        protected (int Start, int Length) GetPositionIndex(List<long> list, Range<decimal> range) => list.IndexRange(range);
        public ReadOnlySpan<long> GetPositions(Range<decimal> range) => _pos_list.Range(range);
        public void RemoveRange(Range<decimal> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<long, long> converter, Range<decimal> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(long, TValue)> Range(Range<decimal> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public int RemoveIf(Predicate<long, TValue> predicate, Range<decimal> range) => RemoveIfCore(predicate, GetPositionIndex(_pos_list, range));
        public void MoveIf(Predicate<long, TValue> predicate, Func<long, long> converter, Range<decimal> range) => MoveIfCore(predicate, converter, GetPositionIndex(_pos_list, range));
        public bool Find(Predicate<long, TValue> predicate, Range<decimal> range, out long position, [MaybeNullWhen(false)] out TValue value) => FindCore(predicate, GetPositionIndex(_pos_list, range), out position, out value);
        public List<(long, TValue)> FindAll(Predicate<long, TValue> predicate, Range<decimal> range) => FindAllCore(predicate, GetPositionIndex(_pos_list, range));
        public void CopyTo<T>(T destination, Range<decimal> srcRange) where T : IXMultiTimeline<long, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<decimal> srcRange, long destOffset) where T : IXMultiTimeline<long, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

    }
}