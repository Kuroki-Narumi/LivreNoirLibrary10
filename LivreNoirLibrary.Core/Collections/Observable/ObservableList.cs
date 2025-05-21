using System;
using System.Collections;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public class ObservableList<T> : ObservableCollectionBase<T>, IList<T>, IList, IReadOnlyList<T>
    {
        public T this[int index]
        {
            get => _list[index];
            set
            {
                var current = _list[index];
                _list[index] = value;
                OnCollectionReplaced(value, current, index);
            }
        }

        public ObservableList() : base() { }
        public ObservableList(int capacy) : base(capacy) { }
        public ObservableList(IEnumerable<T> collection) : base([.. collection]) { }

        public void AddRange(ObservableList<T> source)
        {
            var index = _list.Count;
            _list.AddRange(source._list);
            OnUpdate();
            OnCollectionAdded(source._list, index);
        }

        public void Insert(int index, T item)
        {
            if ((uint)index <= (uint)_list.Count)
            {
                AddItem(index, item);
                OnCollectionAdded(item, index);
            }
        }

        public void InsertWithoutNotify(int index, T item)
        {
            if ((uint)index <= (uint)_list.Count)
            {
                AddItem(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            if (TryRemove(index, out var current))
            {
                OnCollectionRemoved(current, index);
            }
        }

        public void RemoveAtWithoutNotify(int index)
        {
            TryRemove(index, out _);
        }

        public void RemoveRange(int index, int count)
        {
            var c = _list.Count;
            if ((uint)index < (uint)c)
            {
                if (index + count >= c)
                {
                    count = c - index;
                }
                if (count is > 0)
                {
                    List<T> current = _list[index..(index + count)];
                    _list.RemoveRange(index, count);
                    OnUpdate();
                    OnCollectionRemoved(current, index);
                }
            }
        }

        public void RemoveRangeWithoutNotify(int index, int count)
        {
            var c = _list.Count;
            if ((uint)index < (uint)c)
            {
                if (index + count >= c)
                {
                    count = c - index;
                }
                if (count is > 0)
                {
                    _list.RemoveRange(index, count);
                    OnUpdate();
                }
            }
        }

        public void Move(int oldIndex, int newIndex)
        {
            var c = (uint)_list.Count;
            if ((uint)oldIndex < c && (uint)newIndex < c)
            {
                var item = _list[oldIndex];
                RemoveItem(oldIndex);
                AddItem(newIndex, item);
                OnCollectionMoved(item, newIndex, oldIndex);
            }
        }

        public void Swap(int index1, int index2)
        {
            var c = (uint)_list.Count;
            if ((uint)index1 < c && (uint)index2 < c)
            {
                var item1 = _list[index1];
                var item2 = _list[index2];
                ReplaceItem(index1, item2);
                ReplaceItem(index2, item1);
                if (index1 > index2)
                {
                    (index1, index2) = (index2, index1);
                }
                OnCollectionMoved([item1, item2], index1, index2);
            }
        }

        public bool CanMoveDown(int index) => index is >= 0 && index < _list.Count - 1;
        public bool CanMoveUp(int index) => index is > 0 && index < _list.Count;

        public void MoveDown(int index)
        {
            if (CanMoveDown(index))
            {
                Swap(index, index + 1);
            }
        }

        public void MoveUp(int index)
        {
            if (CanMoveUp(index))
            {
                Swap(index, index - 1);
            }
        }

        public void Reverse() => Reverse(0, _list.Count);
        public void Reverse(int index, int count)
        {
            _list.Reverse(index, count);
            NotifyCollectionReset();
        }

        public void Sort() => Sort(0, _list.Count, null);
        public void Sort(IComparer<T> comparer) => Sort(0, _list.Count, comparer);
        public void Sort(int index, int count) => Sort(index, count, null);
        public void Sort(int index, int count, IComparer<T>? comparer)
        {
            _list.Sort(index, count, comparer);
            NotifyCollectionReset();
        }

        public void Shuffle() => Shuffle(0, _list.Count, Random.Shared);
        public void Shuffle(Random random) => Shuffle(0, _list.Count, random);
        public void Shuffle(int index, int count) => Shuffle(index, count, Random.Shared);
        public void Shuffle(int index, int count, Random random)
        {
            var c = _list.Count;
            if (index + count > c)
            {
                count = c - index;
            }
            for (var n = count; n > 0; n--)
            {
                var k = random.Next(n) + index;
                (_list[n - 1], _list[k]) = (_list[k], _list[n - 1]);
            }
            NotifyCollectionReset();
        }

        bool IList.IsFixedSize => false;
        bool IList.IsReadOnly => false;
        object? IList.this[int index]
        {
            get => _list[index];
            set
            {
                if (value is T item)
                {
                    this[index] = item;
                }
            }
        }
        int IList.Add(object? value)
        {
            if (value is T item)
            {
                Add(item);
                return _list.Count - 1;
            }
            return -1;
        }
        void IList.Clear() => Clear();
        bool IList.Contains(object? value) => value is T item && Contains(item);
        int IList.IndexOf(object? value) => value is T item ? IndexOf(item) : -1;
        void IList.Insert(int index, object? value)
        {
            if (value is T item)
            {
                Insert(index, item);
            }
        }
        void IList.Remove(object? value)
        {
            if (value is T item)
            {
                Remove(item);
            }
        }
        void IList.RemoveAt(int index) => RemoveAt(index);
        void ICollection.CopyTo(Array array, int index) => (_list as IList).CopyTo(array, index);
    }
}
