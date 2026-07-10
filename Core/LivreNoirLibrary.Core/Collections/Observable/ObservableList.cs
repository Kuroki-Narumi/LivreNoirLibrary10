using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public class ObservableList<T> : ObservableCollectionBase<T>, IList<T>, IList, IReadOnlyList<T>
    {
        public T this[int index]
        {
            get => GetItemAt(index);
            set => Replace(index, value);
        }

        public ObservableList() : base(0) { }
        public ObservableList(int capacity) : base(capacity) { }
        public ObservableList(IEnumerable<T> collection) : base(collection) { }

        /// <inheritdoc cref="List{T}.RemoveAt(int)"/>
        public void RemoveAt(int index)
        {
            var current = _list[index];
            _list.RemoveAt(index);
            OnCollectionRemoved(current, index);
        }

        /// <inheritdoc cref="List{T}.RemoveAt(int)"/>
        public void RemoveAtWithoutNotify(int index) => _list.RemoveAt(index);

        /// <inheritdoc cref="List{T}.Insert"/>
        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            OnCollectionAdded(item, index);
        }

        /// <inheritdoc cref="List{T}.Insert"/>
        public void InsertWithoutNotify(int index, T item) => _list.Insert(index, item);

        public void Replace(int index, T item)
        {
            var current = _list[index];
            _list[index] = item;
            OnCollectionReplaced(item, current, index);
        }

        public void ReplaceWithoutNotify(int index, T item) => _list[index] = item;

        /// <inheritdoc cref="List{T}.AddRange"/>
        public void AddRange(IEnumerable<T> collection)
        {
            _list.AddRange(collection);
            OnCollectionReset();
        }

        /// <inheritdoc cref="System.Collections.Generic.CollectionExtensions.AddRange"/>
        public void AddRange(params ReadOnlySpan<T> source)
        {
            _list.AddRange(source);
            OnCollectionReset();
        }

        /// <inheritdoc cref="List{T}.InsertRange"/>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            _list.InsertRange(index, collection);
            OnCollectionReset();
        }

        /// <inheritdoc cref="System.Collections.Generic.CollectionExtensions.InsertRange"/>
        public void InsertRange(int index, params ReadOnlySpan<T> source)
        {
            _list.InsertRange(index, source);
            OnCollectionReset();
        }

        /// <inheritdoc cref="List{T}.RemoveRange"/>
        public void RemoveRange(int index, int count)
        {
            _list.RemoveRange(index, count);
            OnCollectionReset();
        }

        public int RemoveRange(IEnumerable<T> collection)
        {
            var count = 0;
            foreach (var item in collection)
            {
                if (RemoveItem(item) is >= 0)
                {
                    count++;
                }
            }
            if (count is > 0)
            {
                OnCollectionReset();
            }
            return count;
        }

        public int RemoveRange(params ReadOnlySpan<T> source)
        {
            var count = 0;
            foreach (var item in source)
            {
                if (RemoveItem(item) is >= 0)
                {
                    count++;
                }
            }
            if (count is > 0)
            {
                OnCollectionReset();
            }
            return count;
        }

        /// <inheritdoc cref="List{T}.RemoveAll"/>
        public int RemoveAll(Predicate<T> match)
        {
            var count = _list.RemoveAll(match);
            if (count is > 0)
            {
                OnCollectionReset();
            }
            return count;
        }

        public void Swap(int index1, int index2)
        {
            var item1 = _list[index1];
            var item2 = _list[index2];
            _list[index1] = item2;
            _list[index2] = item1;
            OnCollectionMoved(item1, index2, index1);
            OnCollectionMoved(item2, index1, index2);
        }

        public bool CanMoveDown(int index) => _list.CanMoveDown(index);
        public bool CanMoveUp(int index) => _list.CanMoveUp(index);

        public bool MoveDown(int index)
        {
            if (CanMoveDown(index))
            {
                Swap(index, index + 1);
                return true;
            }
            return false;
        }

        public bool MoveUp(int index)
        {
            if (CanMoveUp(index))
            {
                Swap(index, index - 1);
                return true;
            }
            return false;
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

        public void Shuffle() => Shuffle(Random.Shared);
        public void Shuffle(Random random)
        {
            random.Shuffle(_list.AsSpan());
            NotifyCollectionReset();
        }

        public void Shuffle(int index, int count) => Shuffle(index, count, Random.Shared);
        public void Shuffle(int index, int count, Random random)
        {
            random.Shuffle(_list.AsSpan(index, count));
            NotifyCollectionReset();
        }

        public void CopyTo(ICollection<T> target, int index, int count)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)Count);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)count, (uint)(Count - index));
            for (var i = 0; i < count; i++)
            {
                target.Add(_list[index + i]);
            }
        }

        public void CopyTo(ObservableCollectionBase<T> target, int index, int count)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)Count);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)count, (uint)(Count - index));
            for (var i = 0; i < count; i++)
            {
                target.AddWithoutNotify(_list[index + i]);
            }
            target.NotifyCollectionReset();
        }

        public void MoveTo(ICollection<T> target, int index, int count)
        {
            CopyTo(target, index, count);
            RemoveRange(index, count);
        }

        public void MoveTo(ObservableCollectionBase<T> target, int index, int count)
        {
            CopyTo(target, index, count);
            RemoveRange(index, count);
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
    }
}
