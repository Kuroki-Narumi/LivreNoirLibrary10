using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media
{
    public abstract class XTimelineBase<TX, TValue, TInnerData, TOperator> : TimelineBase<TX, TValue, TOperator>, IXTimeline<TX, TValue>
        where TX : struct, IComparable<TX>
        where TOperator : IPositionOperator<TX>, new()
    {
        protected readonly List<TInnerData> _value_list = [];

        public abstract IEnumerator<(TX, TValue)> GetEnumerator();
        public IEnumerable<(TX, TValue)> Range(Range<TX> range) => RangeCore(GetPositionIndex(range));
        protected abstract IEnumerable<(TX, TValue)> RangeCore((int Start, int Length) range);

        public sealed override void Clear()
        {
            _pos_list.Clear();
            _value_list.Clear();
        }

        public sealed override bool RemoveAt(TX position)
        {
            if (TryGetIndex(position, out var index))
            {
                RemoveItem(index);
                return true;
            }
            return false;
        }

        protected void RemoveItem(int index)
        {
            _pos_list.RemoveAt(index);
            _value_list.RemoveAt(index);
            OnItemChanged(index);
        }

        protected virtual void OnItemChanged(int index) { }

        protected void RemoveItems(List<int> indexes)
        {
            if (indexes.Count is 0)
            {
                return;
            }
            var c = _pos_list.Count;
            var c2 = indexes.Count;
            int j = 0;
            var freeIndex = indexes[j];
            var current = freeIndex;
            OnItemChanged(freeIndex);
            while (current < c)
            {
                while (j < c2 && current < c)
                {
                    if (current == indexes[j])
                    {
                        current++;
                        j++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (current < c)
                {
                    _pos_list[freeIndex] = _pos_list[current];
                    _value_list[freeIndex] = _value_list[current];
                    freeIndex++;
                    current++;
                }
            }
            _pos_list.RemoveRange(freeIndex, c - freeIndex);
            _value_list.RemoveRange(freeIndex, c - freeIndex);
        }

        protected sealed override void RemoveRangeCore((int Start, int Length) range)
        {
            var l = range.Length;
            if (l is <= 0) { return; }
            var s = range.Start;
            _pos_list.RemoveRange(s, l);
            _value_list.RemoveRange(s, l);
            OnItemChanged(s);
        }

        protected void AddItem(TX position, TInnerData value)
        {
            if (TryGetIndex(position, out var index))
            {
                ReplaceItem(index, value);
                OnItemChanged(index);
            }
            else
            {
                InsertItem(~index, position, value);
            }
        }

        protected abstract void ReplaceItem(int index, TInnerData value);
        protected void InsertItem(int index, TX position, TInnerData value)
        {
            _pos_list.Insert(index, position);
            _value_list.Insert(index, value);
            OnItemChanged(index);
        }


        public sealed override void Move(TX from, TX to)
        {
            if (_operator.Equals(from, to))
            {
                return;
            }
            if (TryGetIndex(from, out var index))
            {
                var current = _value_list[index];
                RemoveItem(index);
                AddItem(to, current);
            }
        }

        protected sealed override void MoveCore(Func<TX, TX> converter, (int Start, int Length) range)
        {
            var l = range.Length;
            if (l is <= 0) { return; }
            List<(TX, TInnerData)> moveList = [];
            var s = range.Start;
            var e = s + l;
            for (int i = s; i < e; i++)
            {
                var pos = _pos_list[i];
                var val = _value_list[i];
                moveList.Add((converter(pos), val));
            }
            RemoveRangeCore(range);
            foreach (var (pos, val) in CollectionsMarshal.AsSpan(moveList))
            {
                AddItem(pos, val);
            }
        }

        protected sealed override void SlideCore(int start, TX amount, bool add)
        {
            var c = _pos_list.Count;
            for (int i = start; i < c; i++)
            {
                var pos = _pos_list[i];
                _pos_list[i] = add ? _operator.Add(pos, amount) : _operator.Subtract(pos, amount);
            }
        }

        public bool TryGet(TX position, [MaybeNullWhen(false)] out TInnerData value) => TryGet(position, SearchMode.Equal, out _, out value);

        public bool TryGet(TX position, SearchMode type, out TX actualPosition, [MaybeNullWhen(false)] out TInnerData value)
        {
            return TryGetCore(_pos_list.FindIndex(position, type), out actualPosition, out value);
        }

        protected bool TryGetCore(int index, out TX actualPosition, [MaybeNullWhen(false)] out TInnerData value)
        {
            if ((uint)index < (uint)_pos_list.Count)
            {
                actualPosition = _pos_list[index];
                value = _value_list[index];
                return true;
            }
            else
            {
                actualPosition = default;
                value = default;
                return false;
            }
        }
    }
}
