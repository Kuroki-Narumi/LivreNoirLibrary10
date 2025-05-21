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
    public partial class IntMultiTimeline<TValue> : XMultiTimelineBase<int, TValue, Operator_int>
    {
        protected (int Start, int Length) GetPositionIndex(List<int> list, Range<long> range) => list.IndexRange(range);
        public ReadOnlySpan<int> GetPositions(Range<long> range) => _pos_list.Range(range);
        public void RemoveRange(Range<long> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<int, int> converter, Range<long> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(int, TValue)> Range(Range<long> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public int RemoveIf(Predicate<int, TValue> predicate, Range<long> range) => RemoveIfCore(predicate, GetPositionIndex(_pos_list, range));
        public void MoveIf(Predicate<int, TValue> predicate, Func<int, int> converter, Range<long> range) => MoveIfCore(predicate, converter, GetPositionIndex(_pos_list, range));
        public bool Find(Predicate<int, TValue> predicate, Range<long> range, out int position, [MaybeNullWhen(false)] out TValue value) => FindCore(predicate, GetPositionIndex(_pos_list, range), out position, out value);
        public List<(int, TValue)> FindAll(Predicate<int, TValue> predicate, Range<long> range) => FindAllCore(predicate, GetPositionIndex(_pos_list, range));
        public void CopyTo<T>(T destination, Range<long> srcRange) where T : IXMultiTimeline<int, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<long> srcRange, int destOffset) where T : IXMultiTimeline<int, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

        protected (int Start, int Length) GetPositionIndex(List<int> list, Range<double> range) => list.IndexRange(range);
        public ReadOnlySpan<int> GetPositions(Range<double> range) => _pos_list.Range(range);
        public void RemoveRange(Range<double> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<int, int> converter, Range<double> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(int, TValue)> Range(Range<double> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public int RemoveIf(Predicate<int, TValue> predicate, Range<double> range) => RemoveIfCore(predicate, GetPositionIndex(_pos_list, range));
        public void MoveIf(Predicate<int, TValue> predicate, Func<int, int> converter, Range<double> range) => MoveIfCore(predicate, converter, GetPositionIndex(_pos_list, range));
        public bool Find(Predicate<int, TValue> predicate, Range<double> range, out int position, [MaybeNullWhen(false)] out TValue value) => FindCore(predicate, GetPositionIndex(_pos_list, range), out position, out value);
        public List<(int, TValue)> FindAll(Predicate<int, TValue> predicate, Range<double> range) => FindAllCore(predicate, GetPositionIndex(_pos_list, range));
        public void CopyTo<T>(T destination, Range<double> srcRange) where T : IXMultiTimeline<int, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<double> srcRange, int destOffset) where T : IXMultiTimeline<int, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

        protected (int Start, int Length) GetPositionIndex(List<int> list, Range<decimal> range) => list.IndexRange(range);
        public ReadOnlySpan<int> GetPositions(Range<decimal> range) => _pos_list.Range(range);
        public void RemoveRange(Range<decimal> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<int, int> converter, Range<decimal> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(int, TValue)> Range(Range<decimal> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public int RemoveIf(Predicate<int, TValue> predicate, Range<decimal> range) => RemoveIfCore(predicate, GetPositionIndex(_pos_list, range));
        public void MoveIf(Predicate<int, TValue> predicate, Func<int, int> converter, Range<decimal> range) => MoveIfCore(predicate, converter, GetPositionIndex(_pos_list, range));
        public bool Find(Predicate<int, TValue> predicate, Range<decimal> range, out int position, [MaybeNullWhen(false)] out TValue value) => FindCore(predicate, GetPositionIndex(_pos_list, range), out position, out value);
        public List<(int, TValue)> FindAll(Predicate<int, TValue> predicate, Range<decimal> range) => FindAllCore(predicate, GetPositionIndex(_pos_list, range));
        public void CopyTo<T>(T destination, Range<decimal> srcRange) where T : IXMultiTimeline<int, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<decimal> srcRange, int destOffset) where T : IXMultiTimeline<int, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

    }
}