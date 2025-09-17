using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public partial class DecimalKeyTimeline<TKey, TValue> : XYSingleTimelineBase<TKey, decimal, TValue, Operator_decimal> where TKey : struct
    {
        protected (int Start, int Length) GetPositionIndex(List<decimal> list, Range<double> range) => list.IndexRange(range);
        public ReadOnlySpan<decimal> GetPositions(Range<double> range) => _pos_list.Range(range);
        public void RemoveRange(Range<double> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<decimal, decimal> converter, Range<double> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public ReadOnlySpan<decimal> GetPositions(TKey key, Range<double> range) => _value_list.TryGetValue(key, out var pv) ? pv.Positions.Range(range) : [];
        protected (int Start, int Length) GetPositionIndex(TKey key, Range<double> range) => _value_list.TryGetValue(key, out var pv) ? GetPositionIndex(pv.Positions, range) : (0, 0);
        public IEnumerable<(TKey, decimal, TValue)> Range(Range<double> range) => RangeCore(GetPositionIndex(_pos_list, range));
        public IEnumerable<(decimal, TValue)> Range(TKey key, Range<double> range) => RangeCore(key, GetPositionIndex(key, range));
        public void RemoveRange(TKey key, Range<double> range) => RemoveRangeCore(key, GetPositionIndex(key, range));
        public void Move(TKey key, Func<decimal, decimal> converter, Range<double> range) => MoveCore(key, converter, GetPositionIndex(key, range));

        public void CopyTo<T>(T destination, Range<double> srcRange) where T : IXYSingleTimeline<TKey, decimal, TValue>
            => CopyToCore(destination, _value_list.Keys, p => p.IndexRange(srcRange), 0);
        public void CopyTo<T>(T destination, Range<double> srcRange, decimal destOffset) where T : IXYSingleTimeline<TKey, decimal, TValue>
            => CopyToCore(destination, _value_list.Keys, p => p.IndexRange(srcRange), destOffset);
        public void CopyTo<T, TEnum>(T destination, TEnum keys, Range<double> srcRange) where T : IXYSingleTimeline<TKey, decimal, TValue> where TEnum : IEnumerable<TKey>
            => CopyToCore(destination, keys, p => p.IndexRange(srcRange), 0);
        public void CopyTo<T, TEnum>(T destination, TEnum keys, Range<double> srcRange, decimal destOffset) where T : IXYSingleTimeline<TKey, decimal, TValue> where TEnum : IEnumerable<TKey>
            => CopyToCore(destination, keys, p => p.IndexRange(srcRange), destOffset);
        public void CopyTo<T>(TKey key, T destination, Range<double> srcRange) where T : IXSingleTimeline<decimal, TValue>
            => CopyToCore(key, destination, GetPositionIndex(key, srcRange), 0);
        public void CopyTo<T>(TKey key, T destination, Range<double> srcRange, decimal destOffset) where T : IXSingleTimeline<decimal, TValue>
            => CopyToCore(key, destination, GetPositionIndex(key, srcRange), destOffset);

    }
}