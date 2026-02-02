using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace LivreNoirLibrary.Media
{
    public abstract class XMultiTimelineBase<TX, TValue, TOperator> : XTimelineBase<TX, TValue, List<TValue>, TOperator>, IXMultiTimeline<TX, TValue>
        where TX : struct
        where TOperator : IPositionOperator<TX>
    {
        public sealed override int Count
        {
            get
            {
                var count = 0;
                foreach (var item in _value_list.AsSpan())
                {
                    count += item.Count;
                }
                return count;
            }
        }

        public bool TryGetList(TX position, [MaybeNullWhen(false)]out List<TValue> values)
        {
            if (TryGetIndex(position, out var index))
            {
                values = _value_list[index];
                return true;
            }
            values = null;
            return false;
        }

        public List<TValue> GetOrAddList(TX position)
        {
            if (TryGetIndex(position, out var index))
            {
                return _value_list[index];
            }
            else
            {
                List<TValue> list = [];
                InsertItem(~index, position, list);
                return list;
            }
        }

        public sealed override IEnumerator<(TX, TValue)> GetEnumerator()
        {
            var c1 = _pos_list.Count;
            for (var i = 0; i < c1; i++)
            {
                var pos = _pos_list[i];
                var list = _value_list[i];
                var c2 = list.Count;
                for (int j = 0; j < c2; j++)
                {
                    yield return (pos, list[j]);
                }
            }
        }

        protected sealed override IEnumerable<(TX, TValue)> RangeCore((int Start, int Length) range)
        {
            var s = range.Start;
            var e = s + range.Length;
            for (var i = s; i < e; i++)
            {
                var pos = _pos_list[i];
                var list = _value_list[i];
                var c2 = list.Count;
                for (int j = 0; j < c2; j++)
                {
                    yield return (pos, list[j]);
                }
            }
        }

        protected IEnumerable<(TX, List<TValue>)> EachListCore(int start, int end)
        {
            for (var i = start; i < end; i++)
            {
                var pos = _pos_list[i];
                var list = _value_list[i];
                yield return (pos, list);
            }
        }

        public IEnumerable<(TX, List<TValue>)> EnumerateList() => EachListCore(0, _pos_list.Count);

        public IEnumerable<(TX, List<TValue>)> EnumerateList(Range<TX> range)
        {
            var (s, l) = GetPositionIndex(range);
            return EachListCore(s, s + l);
        }

        protected IEnumerable<(TX, List<TValue>)> ReverseEachListCore(int start, int end)
        {
            for (var i = end - 1; i >= start; i--)
            {
                var pos = _pos_list[i];
                var list = _value_list[i];
                yield return (pos, list);
            }
        }

        public IEnumerable<(TX, List<TValue>)> ReverseEnumerateList() => ReverseEachListCore(0, _pos_list.Count);

        public IEnumerable<(TX, List<TValue>)> ReverseEnumerateList(Range<TX> range)
        {
            var (s, l) = GetPositionIndex(range);
            return EachListCore(s, s + l);
        }

        protected sealed override void ReplaceItem(int index, List<TValue> value)
        {
            _value_list[index].AddRange(value);
        }

        public void CopyTo(IXMultiTimeline<TX, TValue> destination, TX destOffset) => CopyToCore(destination, GetPositionIndex(), destOffset);
        public void CopyTo(IXMultiTimeline<TX, TValue> destination, Range<TX> srcRange, TX destOffset) => CopyToCore(destination, GetPositionIndex(srcRange), destOffset);

        protected void CopyToCore(IXMultiTimeline<TX, TValue> destination, (int Start, int Length) range, TX destOffset)
        {
            var s = range.Start;
            var e = s + range.Length;
            for (int i = s; i < e; i++)
            {
                destination.AddRange(TOperator.Add(_pos_list[i], destOffset), _value_list[i]);
            }
        }

        public void ProcessDump(BinaryWriter writer, ValueWriter<TValue> valueWriter, string? chid = null)
        {
            writer.WriteChid(chid);
            var c = _pos_list.Count;
            writer.Write(c);
            for (var i = 0; i < c; i++)
            {
                TOperator.Write(writer, _pos_list[i]);
                var v = _value_list[i];
                writer.Write(v.Count);
                foreach (var item in v.AsSpan())
                {
                    valueWriter(writer, item);
                }
            }
        }

        public void ProcessLoad(BinaryReader reader, ValueReader<TValue> valueReader, string? chid = null)
        {
            reader.CheckChid(chid);
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var pos = TOperator.Read(reader);
                var innerCount = reader.ReadInt32();
                List<TValue> values = new(innerCount);
                for (var j = 0; j < innerCount; j++)
                {
                    values.Add(valueReader(reader));
                }
                AddItem(pos, values);
            }
        }

        public void ProcessLoad(BinaryReader reader, ValueReader<TX, TValue> valueReader, string? chid = null)
        {
            reader.CheckChid(chid);
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var pos = TOperator.Read(reader);
                var innerCount = reader.ReadInt32();
                List<TValue> values = new(innerCount);
                for (var j = 0; j < innerCount; j++)
                {
                    values.Add(valueReader(reader, pos));
                }
                AddItem(pos, values);
            }
        }

        public MergedEnumerator EnumerateMerged(XMultiTimelineBase<TX, TValue, TOperator> source) => new(this, source);
        public static MergedEnumerator EnumerateMerged(XMultiTimelineBase<TX, TValue, TOperator> left, XMultiTimelineBase<TX, TValue, TOperator> right) => new(left, right);

        public struct MergedEnumerator(XMultiTimelineBase<TX, TValue, TOperator> left, XMultiTimelineBase<TX, TValue, TOperator> right)
        {
            private readonly List<TX> _leftPosList = left._pos_list;
            private readonly List<List<TValue>> _leftValueList = left._value_list;
            private readonly List<TX> _rightPosList = right._pos_list;
            private readonly List<List<TValue>> _rightValueList = right._value_list;
            private readonly int _leftCount = left._pos_list.Count;
            private readonly int _rightCount = right._pos_list.Count;
            private readonly List<TValue> _buffer = [];
            private int _leftIndex = 0;
            private int _rightIndex = 0;
            private (TX, List<TValue>) _current;

            public readonly (TX, List<TValue>) Current => _current;

            public bool MoveNext()
            {
                var leftIndex = _leftIndex;
                var rightIndex = _rightIndex;
                // 左側が終了済み
                if (leftIndex >= _leftCount)
                {
                    // 右側が終了済み
                    if (rightIndex >= _rightCount)
                    {
                        return false;
                    }
                    // 右側の位置を更新
                    _current = (_rightPosList[rightIndex], _rightValueList[rightIndex]);
                    _rightIndex = rightIndex + 1;
                    return true;
                }

                // 右側が終了済み
                if (rightIndex >= _rightCount)
                {
                    _current = (_leftPosList[leftIndex], _leftValueList[leftIndex]);
                    _leftIndex = leftIndex + 1;
                    return true;
                }
                var leftPos = _leftPosList[leftIndex];
                var rightPos = _rightPosList[rightIndex];
                // 左右の現在時刻を比べる
                switch (TOperator.Compare(leftPos, rightPos))
                {
                    case < 0: // 左が先行
                        _current = (leftPos, _leftValueList[leftIndex]);
                        _leftIndex = leftIndex + 1;
                        return true;
                    case > 0: // 右が先行
                        _current = (rightPos, _rightValueList[rightIndex]);
                        _rightIndex = rightIndex + 1;
                        return true;
                    default:
                        var buffer = _buffer;
                        buffer.Clear();
                        buffer.AddRange(_leftValueList[leftIndex]);
                        buffer.AddRange(_rightValueList[rightIndex]);
                        _current = (leftPos, buffer);
                        _leftIndex = leftIndex + 1;
                        _rightIndex = rightIndex + 1;
                        return true;
                }
            }

            public readonly MergedEnumerator GetEnumerator() => this;
        }
    }
}
