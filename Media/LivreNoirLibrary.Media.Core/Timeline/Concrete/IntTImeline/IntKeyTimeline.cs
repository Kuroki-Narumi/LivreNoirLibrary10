using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public partial class IntKeyTimeline<TKey, TValue> : XYSingleTimelineBase<TKey, int, TValue, Operator_int> where TKey : struct
    {
        protected (int Start, int Length) GetPositionIndex(List<int> list, Range<long> range) => list.IndexRange(range);
        public ReadOnlySpan<int> GetPositions(Range<long> range) => _pos_list.Range(range);
        public void RemoveRange(Range<long> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<int, int> converter, Range<long> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public ReadOnlySpan<int> GetPositions(TKey key, Range<long> range) => _value_list.TryGetValue(key, out var pv) ? pv.Positions.Range(range) : [];
        protected (int Start, int Length) GetPositionIndex(TKey key, Range<long> range) => _value_list.TryGetValue(key, out var pv) ? GetPositionIndex(pv.Positions, range) : (0, 0);
        public IEnumerable<(TKey, int, TValue)> Range(Range<long> range) => RangeCore(GetPositionIndex(_pos_list, range));
        public IEnumerable<(int, TValue)> Range(TKey key, Range<long> range) => RangeCore(key, GetPositionIndex(key, range));
        public void RemoveRange(TKey key, Range<long> range) => RemoveRangeCore(key, GetPositionIndex(key, range));
        public void Move(TKey key, Func<int, int> converter, Range<long> range) => MoveCore(key, converter, GetPositionIndex(key, range));

        public void CopyTo<T>(T destination, Range<long> srcRange) where T : IXYSingleTimeline<TKey, int, TValue>
            => CopyToCore(destination, _value_list.Keys, p => p.IndexRange(srcRange), 0);
        public void CopyTo<T>(T destination, Range<long> srcRange, int destOffset) where T : IXYSingleTimeline<TKey, int, TValue>
            => CopyToCore(destination, _value_list.Keys, p => p.IndexRange(srcRange), destOffset);
        public void CopyTo<T, TEnum>(T destination, TEnum keys, Range<long> srcRange) where T : IXYSingleTimeline<TKey, int, TValue> where TEnum : IEnumerable<TKey>
            => CopyToCore(destination, keys, p => p.IndexRange(srcRange), 0);
        public void CopyTo<T, TEnum>(T destination, TEnum keys, Range<long> srcRange, int destOffset) where T : IXYSingleTimeline<TKey, int, TValue> where TEnum : IEnumerable<TKey>
            => CopyToCore(destination, keys, p => p.IndexRange(srcRange), destOffset);
        public void CopyTo<T>(TKey key, T destination, Range<long> srcRange) where T : IXSingleTimeline<int, TValue>
            => CopyToCore(key, destination, GetPositionIndex(key, srcRange), 0);
        public void CopyTo<T>(TKey key, T destination, Range<long> srcRange, int destOffset) where T : IXSingleTimeline<int, TValue>
            => CopyToCore(key, destination, GetPositionIndex(key, srcRange), destOffset);

        protected (int Start, int Length) GetPositionIndex(List<int> list, Range<double> range) => list.IndexRange(range);
        public ReadOnlySpan<int> GetPositions(Range<double> range) => _pos_list.Range(range);
        public void RemoveRange(Range<double> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<int, int> converter, Range<double> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public ReadOnlySpan<int> GetPositions(TKey key, Range<double> range) => _value_list.TryGetValue(key, out var pv) ? pv.Positions.Range(range) : [];
        protected (int Start, int Length) GetPositionIndex(TKey key, Range<double> range) => _value_list.TryGetValue(key, out var pv) ? GetPositionIndex(pv.Positions, range) : (0, 0);
        public IEnumerable<(TKey, int, TValue)> Range(Range<double> range) => RangeCore(GetPositionIndex(_pos_list, range));
        public IEnumerable<(int, TValue)> Range(TKey key, Range<double> range) => RangeCore(key, GetPositionIndex(key, range));
        public void RemoveRange(TKey key, Range<double> range) => RemoveRangeCore(key, GetPositionIndex(key, range));
        public void Move(TKey key, Func<int, int> converter, Range<double> range) => MoveCore(key, converter, GetPositionIndex(key, range));

        public void CopyTo<T>(T destination, Range<double> srcRange) where T : IXYSingleTimeline<TKey, int, TValue>
            => CopyToCore(destination, _value_list.Keys, p => p.IndexRange(srcRange), 0);
        public void CopyTo<T>(T destination, Range<double> srcRange, int destOffset) where T : IXYSingleTimeline<TKey, int, TValue>
            => CopyToCore(destination, _value_list.Keys, p => p.IndexRange(srcRange), destOffset);
        public void CopyTo<T, TEnum>(T destination, TEnum keys, Range<double> srcRange) where T : IXYSingleTimeline<TKey, int, TValue> where TEnum : IEnumerable<TKey>
            => CopyToCore(destination, keys, p => p.IndexRange(srcRange), 0);
        public void CopyTo<T, TEnum>(T destination, TEnum keys, Range<double> srcRange, int destOffset) where T : IXYSingleTimeline<TKey, int, TValue> where TEnum : IEnumerable<TKey>
            => CopyToCore(destination, keys, p => p.IndexRange(srcRange), destOffset);
        public void CopyTo<T>(TKey key, T destination, Range<double> srcRange) where T : IXSingleTimeline<int, TValue>
            => CopyToCore(key, destination, GetPositionIndex(key, srcRange), 0);
        public void CopyTo<T>(TKey key, T destination, Range<double> srcRange, int destOffset) where T : IXSingleTimeline<int, TValue>
            => CopyToCore(key, destination, GetPositionIndex(key, srcRange), destOffset);

        protected (int Start, int Length) GetPositionIndex(List<int> list, Range<decimal> range) => list.IndexRange(range);
        public ReadOnlySpan<int> GetPositions(Range<decimal> range) => _pos_list.Range(range);
        public void RemoveRange(Range<decimal> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<int, int> converter, Range<decimal> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public ReadOnlySpan<int> GetPositions(TKey key, Range<decimal> range) => _value_list.TryGetValue(key, out var pv) ? pv.Positions.Range(range) : [];
        protected (int Start, int Length) GetPositionIndex(TKey key, Range<decimal> range) => _value_list.TryGetValue(key, out var pv) ? GetPositionIndex(pv.Positions, range) : (0, 0);
        public IEnumerable<(TKey, int, TValue)> Range(Range<decimal> range) => RangeCore(GetPositionIndex(_pos_list, range));
        public IEnumerable<(int, TValue)> Range(TKey key, Range<decimal> range) => RangeCore(key, GetPositionIndex(key, range));
        public void RemoveRange(TKey key, Range<decimal> range) => RemoveRangeCore(key, GetPositionIndex(key, range));
        public void Move(TKey key, Func<int, int> converter, Range<decimal> range) => MoveCore(key, converter, GetPositionIndex(key, range));

        public void CopyTo<T>(T destination, Range<decimal> srcRange) where T : IXYSingleTimeline<TKey, int, TValue>
            => CopyToCore(destination, _value_list.Keys, p => p.IndexRange(srcRange), 0);
        public void CopyTo<T>(T destination, Range<decimal> srcRange, int destOffset) where T : IXYSingleTimeline<TKey, int, TValue>
            => CopyToCore(destination, _value_list.Keys, p => p.IndexRange(srcRange), destOffset);
        public void CopyTo<T, TEnum>(T destination, TEnum keys, Range<decimal> srcRange) where T : IXYSingleTimeline<TKey, int, TValue> where TEnum : IEnumerable<TKey>
            => CopyToCore(destination, keys, p => p.IndexRange(srcRange), 0);
        public void CopyTo<T, TEnum>(T destination, TEnum keys, Range<decimal> srcRange, int destOffset) where T : IXYSingleTimeline<TKey, int, TValue> where TEnum : IEnumerable<TKey>
            => CopyToCore(destination, keys, p => p.IndexRange(srcRange), destOffset);
        public void CopyTo<T>(TKey key, T destination, Range<decimal> srcRange) where T : IXSingleTimeline<int, TValue>
            => CopyToCore(key, destination, GetPositionIndex(key, srcRange), 0);
        public void CopyTo<T>(TKey key, T destination, Range<decimal> srcRange, int destOffset) where T : IXSingleTimeline<int, TValue>
            => CopyToCore(key, destination, GetPositionIndex(key, srcRange), destOffset);

    }
}