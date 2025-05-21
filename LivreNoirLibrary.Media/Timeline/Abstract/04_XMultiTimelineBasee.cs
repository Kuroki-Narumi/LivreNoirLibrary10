using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media
{
    public abstract class XMultiTimelineBase<TX, TValue, TOperator> : XTimelineBase<TX, TValue, List<TValue>, TOperator>, IXMultiTimeline<TX, TValue>
        where TX : struct, IComparable<TX>
        where TOperator : IPositionOperator<TX>, new()
    {
        public sealed override int Count
        {
            get
            {
                var count = 0;
                foreach (var item in CollectionsMarshal.AsSpan(_value_list))
                {
                    count += item.Count;
                }
                return count;
            }
        }

        public sealed override IEnumerator<(TX, TValue)> GetEnumerator()
        {
            var c1 = _pos_list.Count;
            for (int i = 0; i < c1; i++)
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
            for (int i = s; i < e; i++)
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
            for (int i = start; i < end; i++)
            {
                var pos = _pos_list[i];
                var list = _value_list[i];
                yield return (pos, list);
            }
        }

        public IEnumerable<(TX, List<TValue>)> EachList() => EachListCore(0, _pos_list.Count);

        public IEnumerable<(TX, List<TValue>)> EachList(Range<TX> range)
        {
            var (s, l) = GetPositionIndex(range);
            return EachListCore(s, s + l);
        }

        protected sealed override void ReplaceItem(int index, List<TValue> value)
        {
            _value_list[index].AddRange(value);
        }

        public void Add(TX position, TValue value)
        {
            if (TryGetIndex(position, out var index))
            {
                _value_list[index].Add(value);
            }
            else
            {
                InsertItem(~index, position, [value]);
            }
        }

        public void AddToFront(TX position, TValue value)
        {
            if (TryGetIndex(position, out var index))
            {
                _value_list[index].Insert(0, value);
            }
            else
            {
                InsertItem(~index, position, [value]);
            }
        }

        public void Add<TEnumerable>(TX position, TEnumerable values)
            where TEnumerable : IEnumerable<TValue>
        {
            if (TryGetIndex(position, out var index))
            {
                _value_list[index].AddRange(values);
            }
            else
            {
                InsertItem(~index, position, [.. values]);
            }
        }

        public void Add<TEnumerable>(TEnumerable values)
            where TEnumerable : IEnumerable<(TX, TValue)>
        {
            foreach (var (position, value) in values)
            {
                Add(position, value);
            }
        }

        public bool Remove(TX position, TValue value)
        {
            if (TryGetIndex(position, out var index))
            {
                var list = _value_list[index];
                if (list.Remove(value))
                {
                    if (list.Count is 0)
                    {
                        RemoveItem(index);
                    }
                    return true;
                }
            }
            return false;
        }

        public int Remove<TEnumerable>(TX position, TEnumerable values)
            where TEnumerable : IEnumerable<TValue>
        {
            if (TryGetIndex(position, out var index))
            {
                int count = 0;
                var list = _value_list[index];
                foreach (var value in values)
                {
                    if (list.Remove(value))
                    {
                        count++;
                    }
                }
                if (list.Count is 0)
                {
                    RemoveItem(index);
                }
                return count;
            }
            return 0;
        }

        public int Remove<TEnumerable>(TEnumerable values)
            where TEnumerable : IEnumerable<(TX, TValue)>
        {
            int count = 0;
            if (ReferenceEquals(this, values))
            {
                count = Count;
                Clear();
            }
            else
            {
                foreach (var (position, value) in values)
                {
                    if (Remove(position, value))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public int RemoveIf(Predicate<TX, TValue> predicate) => RemoveIfCore(predicate, GetPositionIndex());
        public int RemoveIf(Predicate<TX, TValue> predicate, Range<TX> range) => RemoveIfCore(predicate, GetPositionIndex(range));

        protected int RemoveIfCore(Predicate<TX, TValue> predicate, (int Start, int Length) range)
        {
            var l = range.Length;
            if (l is <= 0) { return 0; }
            List<int> removeIndexes = [];
            var s = range.Start;
            var e = s + l;
            int count = 0;
            for (int i = s; i < e; i++)
            {
                var pos = _pos_list[i];
                var list = _value_list[i];
                count += list.RemoveAll(v => predicate(pos, v));
                if (list.Count is 0)
                {
                    removeIndexes.Add(i);
                }
            }
            RemoveItems(removeIndexes);
            return count;
        }

        public int RemoveIf(TX position, Predicate<TValue> predicate)
        {
            if (TryGetIndex(position, out var index))
            {
                var list = _value_list[index];
                var count = list.RemoveAll(predicate);
                if (list.Count is 0)
                {
                    RemoveItem(index);
                }
                return count;
            }
            return 0;
        }

        public void MoveIf(Predicate<TX, TValue> predicate, Func<TX, TX> converter) => MoveIfCore(predicate, converter, GetPositionIndex());
        public void MoveIf(Predicate<TX, TValue> predicate, Func<TX, TX> converter, Range<TX> range) => MoveIfCore(predicate, converter, GetPositionIndex(range));

        protected void MoveIfCore(Predicate<TX, TValue> predicate, Func<TX, TX> converter, (int Start, int Length) range)
        {
            var l = range.Length;
            if (l is <= 0) { return; }
            Dictionary<TX, List<TValue>> move = [];
            List<int> removeIndexes = [];
            var s = range.Start;
            var e = s + l;
            for (int i = s; i < e; i++)
            {
                var pos = _pos_list[i];
                var newPos = converter(pos);
                if (_operator.Equals(pos, newPos)) { continue; }
                if (!move.TryGetValue(newPos, out var mList))
                {
                    mList = [];
                    move.Add(newPos, mList);
                }
                var list = _value_list[i];
                list.RemoveAll(v =>
                {
                    if (predicate(pos, v))
                    {
                        mList.Add(v);
                        return true;
                    }
                    return false;
                });
                if (list.Count is 0)
                {
                    removeIndexes.Add(i);
                }
            }
            RemoveItems(removeIndexes);
            foreach (var (pos, list) in move)
            {
                Add(pos, list);
            }
        }

        public void MoveIf(TX from, TX to, Predicate<TValue> predicate)
        {
            if (_operator.Equals(from, to))
            {
                return;
            }
            if (TryGetIndex(from, out var index))
            {
                List<TValue> move = [];
                var list = _value_list[index];
                list.RemoveAll(v =>
                {
                    if (predicate(v))
                    {
                        move.Add(v);
                        return true;
                    }
                    return false;
                });
                Add(to, move);
            }
        }

        public bool Find(Predicate<TX, TValue> predicate, out TX position, [MaybeNullWhen(false)] out TValue value) => FindCore(predicate, GetPositionIndex(), out position, out value);
        public bool Find(Predicate<TX, TValue> predicate, Range<TX> range, out TX position, [MaybeNullWhen(false)] out TValue value) => FindCore(predicate, GetPositionIndex(range), out position, out value);

        protected bool FindCore(Predicate<TX, TValue> predicate, (int Start, int Length) range, out TX position, [MaybeNullWhen(false)] out TValue value)
        {
            var s = range.Start;
            var e = s + range.Length;
            for (int i = s; i < e; i++)
            {
                var pos = _pos_list[i];
                var list = _value_list[i];
                var c2 = list.Count;
                for (int j = 0; j < c2; j++)
                {
                    var v = list[j];
                    if (predicate(pos, v))
                    {
                        position = pos;
                        value = v;
                        return true;
                    }
                }
            }
            position = default;
            value = default;
            return false;
        }

        public bool Find(TX position, Predicate<TValue> predicate, [MaybeNullWhen(false)] out TValue value)
        {
            if (TryGetIndex(position, out var index))
            {
                var list = _value_list[index];
                foreach (var v in CollectionsMarshal.AsSpan(list))
                {
                    if (predicate(v))
                    {
                        value = v;
                        return true;
                    }
                }
            }
            value = default;
            return false;
        }

        public List<(TX, TValue)> FindAll(Predicate<TX, TValue> predicate) => FindAllCore(predicate, GetPositionIndex());
        public List<(TX, TValue)> FindAll(Predicate<TX, TValue> predicate, Range<TX> range) => FindAllCore(predicate, GetPositionIndex(range));

        protected List<(TX, TValue)> FindAllCore(Predicate<TX, TValue> predicate, (int Start, int Length) range)
        {
            List<(TX, TValue)> result = [];
            var l = range.Length;
            if (l is <= 0) { return result; }
            var s = range.Start;
            var e = s + l;
            for (int i = s; i < e; i++)
            {
                var pos = _pos_list[i];
                var list = _value_list[i];
                foreach (var v in CollectionsMarshal.AsSpan(list))
                {
                    if (predicate(pos, v))
                    {
                        result.Add((pos, v));
                    }
                }
            }
            return result;
        }

        public List<TValue> FindAll(TX position, Predicate<TValue> predicate)
        {
            if (TryGetIndex(position, out var index))
            {
                return _value_list[index].FindAll(predicate);
            }
            return [];
        }

        public void CopyTo<T>(T destination) where T : IXMultiTimeline<TX, TValue> => CopyToCore(destination, GetPositionIndex(), TOperator.Zero);
        public void CopyTo<T>(T destination, TX destOffset) where T : IXMultiTimeline<TX, TValue> => CopyToCore(destination, GetPositionIndex(), destOffset);
        public void CopyTo<T>(T destination, Range<TX> srcRange) where T : IXMultiTimeline<TX, TValue> => CopyToCore(destination, GetPositionIndex(srcRange), TOperator.Zero);
        public void CopyTo<T>(T destination, Range<TX> srcRange, TX destOffset) where T : IXMultiTimeline<TX, TValue> => CopyToCore(destination, GetPositionIndex(srcRange), destOffset);

        protected void CopyToCore<T>(T destination, (int Start, int Length) range, TX destOffset)
             where T : IXMultiTimeline<TX, TValue>
        {
            var s = range.Start;
            var e = s + range.Length;
            for (int i = s; i < e; i++)
            {
                destination.Add(_operator.Add(_pos_list[i], destOffset), _value_list[i]);
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
                var v = _value_list[i];
                writer.Write(v.Count);
                foreach (var item in CollectionsMarshal.AsSpan(v))
                {
                    valueWriter(writer, item);
                }
            }
        }

        public void ProcessLoad(BinaryReader reader, ValueReader<TValue> valueReader, string? chid = null)
        {
            reader.CheckChid(chid);
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var pos = _operator.Read(reader);
                var innerCount = reader.ReadInt32();
                List<TValue> values = new(innerCount);
                for (var j = 0; j < innerCount; j++)
                {
                    values.Add(valueReader(reader));
                }
                Add(pos, values);
            }
        }
    }
}
