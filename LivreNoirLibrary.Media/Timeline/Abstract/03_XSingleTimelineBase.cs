using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media
{
    public abstract class XSingleTimelineBase<TX, TValue, TOperator> : XTimelineBase<TX, TValue, TValue, TOperator>, IXSingleTimeline<TX, TValue>
        where TX : struct, IComparable<TX>
        where TOperator : IPositionOperator<TX>, new()
    {
        public sealed override int Count => _value_list.Count;

        public sealed override IEnumerator<(TX, TValue)> GetEnumerator()
        {
            var c = _pos_list.Count;
            for (int i = 0; i < c; i++)
            {
                yield return (_pos_list[i], _value_list[i]);
            }
        }

        protected sealed override IEnumerable<(TX, TValue)> RangeCore((int Start, int Length) range)
        {
            var s = range.Start;
            var e = s + range.Length;
            for (int i = s; i < e; i++)
            {
                yield return (_pos_list[i], _value_list[i]);
            }
        }

        protected sealed override void ReplaceItem(int index, TValue value)
        {
            _value_list[index] = value;
        }

        public void Set(TX position, TValue value) => AddItem(position, value);
        public TValue Get(TX position, TValue ifnone) => Get(position, SearchMode.PreviousOrEqual, ifnone);

        public TValue Get(TX position, SearchMode type, TValue ifnone)
        {
            if (TryGet(position, type, out _, out var value))
            {
                return value;
            }
            return ifnone;
        }

        public void CopyTo<T>(T destination) where T : IXSingleTimeline<TX, TValue> => CopyToCore(destination, GetPositionIndex(), TOperator.Zero);
        public void CopyTo<T>(T destination, TX destOffset) where T : IXSingleTimeline<TX, TValue> => CopyToCore(destination, GetPositionIndex(), destOffset);
        public void CopyTo<T>(T destination, Range<TX> srcRange) where T : IXSingleTimeline<TX, TValue> => CopyToCore(destination, GetPositionIndex(srcRange), TOperator.Zero);
        public void CopyTo<T>(T destination, Range<TX> srcRange, TX destOffset) where T : IXSingleTimeline<TX, TValue> => CopyToCore(destination, GetPositionIndex(srcRange), destOffset);

        protected void CopyToCore<T>(T destination, (int Start, int Length) range, TX destOffset)
             where T : IXSingleTimeline<TX, TValue>
        {
            var s = range.Start;
            var e = s + range.Length;
            for (int i = s; i < e; i++)
            {
                destination.Set(_operator.Add(_pos_list[i], destOffset), _value_list[i]);
            }
        }

        public void ProcessDump(BinaryWriter writer, ValueWriter<TValue> valueWriter, string? chid = null)
        {
            writer.WriteChid(chid);
            var c = _pos_list.Count;
            writer.Write(c);
            for (int i = 0; i < c; i++)
            {
                _operator.Write(writer, _pos_list[i]);
                valueWriter(writer, _value_list[i]);
            }
        }

        public void ProcessLoad(BinaryReader reader, ValueReader<TValue> valueReader, string? chid = null)
        {
            reader.CheckChid(chid);
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var pos = _operator.Read(reader);
                var value = valueReader(reader);
                Set(pos, value);
            }
        }
    }
}
