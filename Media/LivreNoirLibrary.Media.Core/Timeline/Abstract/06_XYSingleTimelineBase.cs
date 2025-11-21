using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public abstract class XYSingleTimelineBase<TY, TX, TValue, TOperator> : XYTimelineBase<TY, TX, TValue, TValue, TOperator>, IXYSingleTimeline<TY, TX, TValue>
        where TY : struct
        where TX : struct
        where TOperator : IPositionOperator<TX>
    {
        public sealed override int Count
        {
            get
            {
                var count = 0;
                foreach (var set in _key_list.AsSpan())
                {
                    count += set.Count;
                }
                return count;
            }
        }

        public sealed override IEnumerator<(TY, TX, TValue)> GetEnumerator()
        {
            Dictionary<TY, int> indexes = [];
            foreach (var key in _value_list.Keys)
            {
                indexes.Add(key, 0);
            }
            var c = _pos_list.Count;
            for (int i = 0; i < c; i++)
            {
                var pos = _pos_list[i];
                foreach (var key in _key_list[i])
                {
                    var (p, v) = _value_list[key];
                    var index = indexes[key];
                    yield return (key, pos, v[index]);
                    indexes[key] = index + 1;
                }
            }
        }

        protected sealed override IEnumerable<(TY, TX, TValue)> RangeCore((int Start, int Length) range)
        {
            var s = range.Start;
            var e = s + range.Length;
            Dictionary<TY, int> indexes = [];
            for (int i = s; i < e; i++)
            {
                var pos = _pos_list[i];
                foreach (var key in _key_list[i])
                {
                    var (p, v) = _value_list[key];
                    if (!indexes.TryGetValue(key, out var index))
                    {
                        index = p.BinarySearch(pos);
                        indexes.Add(key, index);
                    }
                    yield return (key, pos, v[index]);
                    indexes[key] = index + 1;
                }
            }
        }

        protected sealed override IEnumerable<(TX, TValue)> RangeCore(TY key, (int Start, int Length) range)
        {
            var l = range.Length;
            if (l is <= 0) { yield break; }
            var (p, v) = _value_list[key];
            var s = range.Start;
            var e = s + l;
            for (int i = s; i < e; i++)
            {
                yield return (p[i], v[i]);
            }
        }

        protected sealed override void ReplaceItem(List<TValue> v, int index, TValue value)
        {
            v[index] = value;
        }

        public void Set(TY key, TX position, TValue value) => AddItem(key, position, value);

        public void CopyTo(IXYSingleTimeline<TY, TX, TValue> destination, TX destOffset)
            => CopyToCore(destination, _value_list.Keys, p => (0, p.Count), destOffset);

        public void CopyTo(IXYSingleTimeline<TY, TX, TValue> destination, Range<TX> srcRange, TX destOffset)
            => CopyToCore(destination, _value_list.Keys, p => p.IndexRange<TX, TX, TOperator>(srcRange), destOffset);

        public void CopyTo(IXYSingleTimeline<TY, TX, TValue> destination, IEnumerable<TY> keys, TX destOffset)
            => CopyToCore(destination, keys, p => (0, p.Count), destOffset);

        public void CopyTo(IXYSingleTimeline<TY, TX, TValue> destination, IEnumerable<TY> keys, Range<TX> srcRange, TX destOffset)
            => CopyToCore(destination, keys, p => p.IndexRange<TX, TX, TOperator>(srcRange), destOffset);

        protected void CopyToCore(IXYSingleTimeline<TY, TX, TValue> destination, IEnumerable<TY> keys, Func<List<TX>, (int, int)> rangeFunc, TX destOffset)
        {
            foreach (var key in keys)
            {
                if (_value_list.TryGetValue(key, out var pv))
                {
                    var (p, v) = pv;
                    var (s, l) = rangeFunc(p);
                    if (l is <= 0) { continue; }
                    var e = s + l;
                    for (int i = s; i < e; i++)
                    {
                        destination.Set(key, TOperator.Add(p[i], destOffset), v[i]);
                    }
                }
            }
        }

        public void CopyTo(TY key, IXSingleTimeline<TX, TValue> destination, TX destOffset)
            => CopyToCore(key, destination, GetPositionIndex(key), destOffset);

        public void CopyTo(TY key, IXSingleTimeline<TX, TValue> destination, Range<TX> srcRange, TX destOffset)
            => CopyToCore(key, destination, GetPositionIndex(key, srcRange), destOffset);

        protected void CopyToCore(TY key, IXSingleTimeline<TX, TValue> destination, (int Start, int Length) range, TX destOffset)
        {
            var l = range.Length;
            if (l is <= 0) { return; }
            var s = range.Start;
            var e = s + l;
            var (p, v) = _value_list[key];
            for (int i = s; i < e; i++)
            {
                destination.Set(TOperator.Add(p[i], destOffset), v[i]);
            }
        }

        public void ProcessDump(BinaryWriter writer, ValueWriter<TY> keyWriter, ValueWriter<TValue> valueWriter, string? chid = null)
        {
            writer.WriteChid(chid);
            var c = _value_list.Count;
            writer.Write(c);
            foreach (var (key, (p, v)) in _value_list)
            {
                keyWriter(writer, key);
                var innerCount = p.Count;
                writer.Write(innerCount);
                for (var i = 0; i < innerCount; i++)
                {
                    TOperator.Write(writer, p[i]);
                    valueWriter(writer, v[i]);
                }
            }
        }

        public void ProcessLoad(BinaryReader reader, ValueReader<TY> keyReader, ValueReader<TValue> valueReader, string? chid = null)
        {
            reader.CheckChid(chid);
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var key = keyReader(reader);
                var innerCount = reader.ReadInt32();
                for (var j = 0; j < innerCount; j++)
                {
                    var pos = TOperator.Read(reader);
                    var value = valueReader(reader);
                    Set(key, pos, value);
                }
            }
        }

        public void ProcessLoad(BinaryReader reader, ValueReader<TY> keyReader, ValueReader<TY, TX, TValue> valueReader, string? chid = null)
        {
            reader.CheckChid(chid);
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var key = keyReader(reader);
                var innerCount = reader.ReadInt32();
                for (var j = 0; j < innerCount; j++)
                {
                    var pos = TOperator.Read(reader);
                    var value = valueReader(reader, key, pos);
                    Set(key, pos, value);
                }
            }
        }
    }
}
