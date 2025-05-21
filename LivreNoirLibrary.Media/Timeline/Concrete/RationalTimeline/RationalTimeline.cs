using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public partial class RationalTimeline<TValue> : XSingleTimelineBase<Rational, TValue, Operator_Rational>
    {
        protected (int Start, int Length) GetPositionIndex(List<Rational> list, Range<int> range) => list.IndexRange(range);
        public ReadOnlySpan<Rational> GetPositions(Range<int> range) => _pos_list.Range(range);
        public void RemoveRange(Range<int> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<Rational, Rational> converter, Range<int> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(Rational, TValue)> Range(Range<int> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public void CopyTo<T>(T destination, Range<int> srcRange) where T : IXSingleTimeline<Rational, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<int> srcRange, Rational destOffset) where T : IXSingleTimeline<Rational, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

        protected (int Start, int Length) GetPositionIndex(List<Rational> list, Range<long> range) => list.IndexRange(range);
        public ReadOnlySpan<Rational> GetPositions(Range<long> range) => _pos_list.Range(range);
        public void RemoveRange(Range<long> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<Rational, Rational> converter, Range<long> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(Rational, TValue)> Range(Range<long> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public void CopyTo<T>(T destination, Range<long> srcRange) where T : IXSingleTimeline<Rational, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<long> srcRange, Rational destOffset) where T : IXSingleTimeline<Rational, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

        protected (int Start, int Length) GetPositionIndex(List<Rational> list, Range<float> range) => list.IndexRange(range);
        public ReadOnlySpan<Rational> GetPositions(Range<float> range) => _pos_list.Range(range);
        public void RemoveRange(Range<float> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<Rational, Rational> converter, Range<float> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(Rational, TValue)> Range(Range<float> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public void CopyTo<T>(T destination, Range<float> srcRange) where T : IXSingleTimeline<Rational, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<float> srcRange, Rational destOffset) where T : IXSingleTimeline<Rational, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

        protected (int Start, int Length) GetPositionIndex(List<Rational> list, Range<double> range) => list.IndexRange(range);
        public ReadOnlySpan<Rational> GetPositions(Range<double> range) => _pos_list.Range(range);
        public void RemoveRange(Range<double> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<Rational, Rational> converter, Range<double> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(Rational, TValue)> Range(Range<double> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public void CopyTo<T>(T destination, Range<double> srcRange) where T : IXSingleTimeline<Rational, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<double> srcRange, Rational destOffset) where T : IXSingleTimeline<Rational, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

        protected (int Start, int Length) GetPositionIndex(List<Rational> list, Range<decimal> range) => list.IndexRange(range);
        public ReadOnlySpan<Rational> GetPositions(Range<decimal> range) => _pos_list.Range(range);
        public void RemoveRange(Range<decimal> range) => RemoveRangeCore(GetPositionIndex(_pos_list, range));
        public void Move(Func<Rational, Rational> converter, Range<decimal> range) => MoveCore(converter, GetPositionIndex(_pos_list, range));

        public IEnumerable<(Rational, TValue)> Range(Range<decimal> range) => RangeCore(GetPositionIndex(_pos_list, range));

        public void CopyTo<T>(T destination, Range<decimal> srcRange) where T : IXSingleTimeline<Rational, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), 0);
        public void CopyTo<T>(T destination, Range<decimal> srcRange, Rational destOffset) where T : IXSingleTimeline<Rational, TValue>
            => CopyToCore(destination, GetPositionIndex(_pos_list, srcRange), destOffset);

    }
}