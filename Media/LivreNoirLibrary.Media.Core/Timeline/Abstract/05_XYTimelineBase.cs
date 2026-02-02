using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public abstract class XYTimelineBase<TY, TX, TValue, TInnerData, TOperator> : TimelineBase<TX, TValue, TOperator>, IXYTimeline<TY, TX, TValue>
        where TY : struct
        where TX : struct
        where TOperator : IPositionOperator<TX>
    {
        protected readonly List<SortedSet<TY>> _key_list = [];
        protected readonly SortedList<TY, (List<TX> Positions, List<TInnerData> Values)> _value_list = [];

        public ReadOnlySpan<TX> GetPositions(TY key) => _value_list.TryGetValue(key, out var pv) ? pv.Positions.AsSpan() : [];
        public ReadOnlySpan<TX> GetPositions(TY key, Range<TX> range) => _value_list.TryGetValue(key, out var pv) ? pv.Positions.Range<TX, TX, TOperator>(range) : [];
        public ReadOnlySpan<TY> GetKeyList() => _value_list.Keys.ToArray();
        protected (int Start, int Length) GetPositionIndex(TY key) => _value_list.TryGetValue(key, out var pv) ? (0, pv.Positions.Count) : (0, 0);
        protected (int Start, int Length) GetPositionIndex(TY key, Range<TX> range) => _value_list.TryGetValue(key, out var pv) ? pv.Positions.IndexRange<TX, TX, TOperator>(range) : (0, 0);

        public abstract IEnumerator<(TY, TX, TValue)> GetEnumerator();
        public IEnumerable<(TY, TX, TValue)> Range(Range<TX> range) => RangeCore(GetPositionIndex(range));
        protected abstract IEnumerable<(TY, TX, TValue)> RangeCore((int Start, int Length) range);

        public IEnumerable<(TX, TValue)> Range(TY key) => RangeCore(key, GetPositionIndex(key));
        public IEnumerable<(TX, TValue)> Range(TY key, Range<TX> range) => RangeCore(key, GetPositionIndex(key, range));
        protected abstract IEnumerable<(TX, TValue)> RangeCore(TY key, (int Start, int Length) range);

        public sealed override void Clear()
        {
            _pos_list.Clear();
            _value_list.Clear();
            _key_list.Clear();
        }

        protected static void RemoveAt<T1, T2>(List<T1> list1, List<T2> list2, int index)
        {
            list1.RemoveAt(index);
            list2.RemoveAt(index);
        }

        protected static void RemoveRange<T1, T2>(List<T1> list1, List<T2> list2, int start, int length)
        {
            list1.RemoveRange(start, length);
            list2.RemoveRange(start, length);
        }

        protected void RemoveKeyPos<T1, T2>(TY key, List<T1> list1, List<T2> list2, int index)
        {
            RemoveAt(list1, list2, index);
            if (list1.Count is 0)
            {
                _value_list.Remove(key);
            }
        }

        protected void RemoveKeyPos<T1, T2>(TY key, List<T1> list1, List<T2> list2, (int Start, int Length) range)
        {
            RemoveRange(list1, list2, range.Start, range.Length);
            if (list1.Count is 0)
            {
                _value_list.Remove(key);
            }
        }

        protected void RemovePosKey(TY key, int index)
        {
            var set = _key_list[index];
            set.Remove(key);
            if (set.Count is 0)
            {
                RemoveAt(_pos_list, _key_list, index);
            }
        }

        protected int FindPreviousOrEqual(List<TX> list, TX position) => list.TrySearch<TX, TX, TOperator>(position, SearchMode.PreviousOrEqual, out var index, out _) ? index : 0;
        protected int FindNextOrEqual(List<TX> list, TX position) => list.TrySearch<TX, TX, TOperator>(position, SearchMode.NextOrEqual, out var index, out _) ? index : 0;

        protected void RemoveRangeUnsafe(TY key, List<TX> p, int s, int e)
        {
            var index = FindPreviousOrEqual(_pos_list, p[s]);
            for (int i = s; i < e; i++)
            {
                var pos = p[i];
                while (TOperator.Compare(_pos_list[index], pos) is not 0) { index++; }
                RemovePosKey(key, index);
            }
        }

        public sealed override bool RemoveAt(TX position)
        {
            if (TryGetIndex(position, out var i1))
            {
                var keys = _key_list[i1];
                foreach (var key in keys)
                {
                    var (p, v) = _value_list[key];
                    RemoveKeyPos(key, p, v, p.BinarySearch(position));
                }
                RemoveAt(_pos_list, _key_list, i1);
                return true;
            }
            return false;
        }

        protected sealed override void RemoveRangeCore((int Start, int Length) range)
        {
            var l = range.Length;
            if (l is <= 0) { return; }
            var s = range.Start;
            var e = s + l;
            var posStart = _pos_list[s];
            var posEnd = _pos_list[e - 1];
            RemoveRange(_pos_list, _key_list, s, l);
            var keys = _value_list.Keys.ToArray();
            foreach (var key in keys)
            {
                var (p, v) = _value_list[key];
                RemoveKeyPos(key, p, v, p.IndexRange<TX, TX, TOperator>(RangeUtils.Get(posStart, posEnd, true)));
            }
        }

        public bool RemoveKey(TY key)
        {
            if (_value_list.TryGetValue(key, out var pv))
            {
                var p = pv.Positions;
                RemoveRangeUnsafe(key, p, 0, p.Count);
                _value_list.Remove(key);
                return true;
            }
            return false;
        }

        public bool RemoveAt(TY key, TX position)
        {
            if (_value_list.TryGetValue(key, out var pv))
            {
                var (p, v) = pv;
                if (TryGetIndex(p, position, out var index))
                {
                    RemovePosKey(key, _pos_list.BinarySearch(position));
                    RemoveKeyPos(key, p, v, index);
                    return true;
                }
            }
            return false;
        }

        public void RemoveRange(TY key, Range<TX> range) => RemoveRangeCore(key, GetPositionIndex(key, range));

        protected void RemoveRangeCore(TY key, (int Start, int Length) range)
        {
            var l = range.Length;
            if (l is <= 0) { return; }
            var s = range.Start;
            var e = s + l;
            var (p, v) = _value_list[key];
            RemoveRangeUnsafe(key, p, s, e);
            RemoveKeyPos(key, p, v, range);
        }

        protected void AddItem(TY key, TX position, TInnerData value)
        {
            if (!_value_list.TryGetValue(key, out var pv))
            {
                pv = ([], []);
                _value_list.Add(key, pv);
            }
            AddItem(key, pv.Positions, pv.Values, position, value);
        }

        protected void AddItem(TY key, List<TX> p, List<TInnerData> v, TX position, TInnerData value)
        {
            if (TryGetIndex(p, position, out var index))
            {
                ReplaceItem(v, index, value);
            }
            else
            {
                InsertItem(key, p, v, ~index, position, value);
            }
        }

        protected abstract void ReplaceItem(List<TInnerData> v, int index, TInnerData value);
        protected void InsertItem(TY key, List<TX> p, List<TInnerData> v, int index, TX position, TInnerData value)
        {
            InsertKeyPos(p, v, index, position, value);
            InsertPosKey(key, position);
        }

        protected void InsertKeyPos(List<TX> p, List<TInnerData> v, int index, TX position, TInnerData value)
        {
            p.Insert(index, position);
            v.Insert(index, value);
        }

        protected void InsertPosKey(TY key, TX position)
        {
            if (TryGetIndex(position, out var index))
            {
                _key_list[index].Add(key);
            }
            else
            {
                index = ~index;
                _pos_list.Insert(index, position);
                _key_list.Insert(index, [key]);
            }
        }

        protected void InsertPosKey(SortedSet<TY> keys, TX position)
        {
            if (TryGetIndex(position, out var index))
            {
                _key_list[index].UnionWith(keys);
            }
            else
            {
                index = ~index;
                _pos_list.Insert(index, position);
                _key_list.Insert(index, keys);
            }
        }

        protected sealed override void MoveCore(Func<TX, TX> converter, (int Start, int Length) range)
        {
            var l = range.Length;
            if (l is <= 0) { return; }
            Dictionary<TY, List<(TX, TInnerData)>> moveList = [];
            var s = range.Start;
            var e = s + l;
            for (int i = s; i < e; i++)
            {
                var pos = _pos_list[i];
                var newPos = converter(pos);
                var keys = _key_list[i];
                foreach (var key in keys)
                {
                    if (!moveList.TryGetValue(key, out var list))
                    {
                        list = [];
                        moveList.Add(key, list);
                    }
                    var (p, v) = _value_list[key];
                    var index = p.BinarySearch(pos);
                    list.Add((newPos, v[index]));
                    RemoveKeyPos(key, p, v, index);
                }
            }
            RemoveRange(_pos_list, _key_list, s, l);
            foreach (var (key, list) in moveList)
            {
                foreach (var (pos, val) in list.AsSpan())
                {
                    AddItem(key, pos, val);
                }
            }
        }

        public void Move(TY key, Func<TX, TX> converter) => MoveCore(key, converter, GetPositionIndex(key));
        public void Move(TY key, Func<TX, TX> converter, Range<TX> range) => MoveCore(key, converter, GetPositionIndex(key, range));

        protected void MoveCore(TY key, Func<TX, TX> converter, (int Start, int Length) range)
        {
            var l = range.Length;
            if (l is <= 0) { return; }
            List<(TX, TInnerData)> moveList = [];
            var (p, v) = _value_list[key];
            var s = range.Start;
            var e = s + l;
            for (int i = s; i < e; i++)
            {
                moveList.Add((converter(p[i]), v[i]));
            }
            RemoveRange(p, v, s, l);
            RemoveRangeUnsafe(key, p, s, e);
            foreach (var (pos, val) in moveList.AsSpan())
            {
                AddItem(key, p, v, pos, val);
            }
        }

        public bool TryGetValue(TY key, TX position, SearchMode mode, out TX actualPosition, [MaybeNullWhen(false)] out TInnerData value)
            => TryGetCore(key, p => p.FindIndex<TX, TX, TOperator>(position, mode), out actualPosition, out value);

        public bool TryGetNearest(TY key, TX position, out TX actualPosition, [MaybeNullWhen(false)] out TInnerData value)
            => TryGetCore(key, p => p.FindNearestIndex<TX, TX, TOperator>(position), out actualPosition, out value);

        protected bool TryGetCore(TY key, Func<List<TX>, int> func, out TX actualPosition, [MaybeNullWhen(false)] out TInnerData value)
        {
            if (_value_list.TryGetValue(key, out var pv))
            {
                var (p, v) = pv;
                var index = func(p);
                if ((uint)index < (uint)p.Count)
                {
                    actualPosition = p[index];
                    value = v[index];
                    return true;
                }
            }
            actualPosition = default;
            value = default;
            return false;
        }
    }
}
