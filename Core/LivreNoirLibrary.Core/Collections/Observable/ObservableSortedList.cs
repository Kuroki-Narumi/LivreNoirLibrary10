using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Collections
{
    public abstract class ObservableSortedList<TKey, TValue> : ObservableCollectionBase<TValue>, ICollection<TValue>
        where TKey : IComparable<TKey>
    {
        protected readonly IComparer<TKey>? _comparer = null;
        protected readonly List<TKey> _key_list = [];

        public TValue this[int index] => GetItemAt(index);

        public ObservableSortedList(IComparer<TKey>? comparer = null) : base(0)
        {
            _comparer = comparer;
        }

        public ObservableSortedList(int capacity, IComparer<TKey>? comparer = null) : base(capacity)
        {
            _comparer = comparer;
        }

        public ObservableSortedList(IEnumerable<TValue> collection, IComparer<TKey>? comparer = null) : base(0)
        {
            _comparer = comparer;
            foreach (var item in collection)
            {
                AddWithoutNotify(item);
            }
        }

        protected abstract TKey GetKey(TValue item);

        public sealed override int IndexOf(TValue item) => IndexOfKey(GetKey(item));

        public (TKey Key, int Index) GetKeyAndIndex(TValue item)
        {
            var key = GetKey(item);
            var index = IndexOfKey(key);
            return (key, index);
        }

        public bool ContainsKey(TKey key) => (uint)IndexOfKey(key) < (uint)_key_list.Count;

        public int IndexOfKey(TKey key) => _key_list.BinarySearch(key, _comparer);

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue item)
        {
            var index = IndexOfKey(key);
            if (index is >= 0)
            {
                item = _list[index];
                return true;
            }
            item = default;
            return false;
        }

        public void Add(TValue item, out int index)
        {
            AddItem(item, out var replaced, out index, out var oldItem);
            if (replaced)
            {
                OnCollectionReplaced(item, oldItem, index);
            }
            else
            {
                OnCollectionAdded(item, index);
            }
        }

        public bool RemoveKey(TKey key)
        {
            if (TryRemoveKey(key, out var index, out var current))
            {
                OnCollectionRemoved(current, index);
                return true;
            }
            return false;
        }

        public bool RemoveKeyWithoutNotify(TKey key) => TryRemoveKey(key, out _, out _);

        /// <inheritdoc cref="List{T}.AddRange"/>
        public void AddRange(IEnumerable<TValue> collection)
        {
            var modified = false;
            foreach (var item in collection)
            {
                AddItem(item, out _, out _, out _);
                modified = true;
            }
            if (modified)
            {
                OnCollectionReset();
            }
        }

        /// <inheritdoc cref="System.Collections.Generic.CollectionExtensions.AddRange"/>
        public void AddRange(params ReadOnlySpan<TValue> source)
        {
            var modified = false;
            foreach (var item in source)
            {
                AddItem(item, out _, out _, out _);
                modified = true;
            }
            if (modified)
            {
                OnCollectionReset();
            }
        }

        public int RemoveRange(IEnumerable<TValue> collection)
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

        public int RemoveRange(params ReadOnlySpan<TValue> source)
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

        public int RemoveKeys(IEnumerable<TKey> collection)
        {
            var count = 0;
            foreach (var item in collection)
            {
                if (TryRemoveKey(item, out _, out _))
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

        public int RemoveKeys(params ReadOnlySpan<TKey> source)
        {
            var count = 0;
            foreach (var item in source)
            {
                if (TryRemoveKey(item, out _, out _))
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

        public int RemoveAll(Func<TKey, TValue, bool> match)
        {
            var count = 0;
            for (var i = 0; i < _list.Count;)
            {
                if (match(_key_list[i], _list[i]))
                {
                    RemoveItem(i);
                    count++;
                }
                else
                {
                    i++;
                }
            }
            if (count is > 0)
            {
                OnCollectionReset();
            }
            return count;
        }

        protected override void ClearItems()
        {
            _key_list.Clear();
            base.ClearItems();
        }

        protected override void AddItem(TValue item, out bool replaced, out int index, out TValue? oldItem)
        {
            (var key, index) = GetKeyAndIndex(item);
            if (index is >= 0)
            {
                replaced = true;
                oldItem = _list[index];
                _list[index] = item;
            }
            else
            {
                index = ~index;
                replaced = false;
                oldItem = default;
                InsertItem(index, key, item);
            }
        }

        protected override int RemoveItem(TValue item)
        {
            var (_, index) = GetKeyAndIndex(item);
            if (index is >= 0 && Equals(_list[index], item))
            {
                RemoveItem(index);
                return index;
            }
            return -1;
        }

        protected void InsertItem(int index, TKey key, TValue item)
        {
            _list.Insert(index, item);
            _key_list.Insert(index, key);
        }

        protected void RemoveItem(int index)
        {
            _list.RemoveAt(index);
            _key_list.RemoveAt(index);
        }

        protected bool TryRemoveKey(TKey key, out int index, [MaybeNullWhen(false)] out TValue current)
        {
            index = IndexOfKey(key);
            if (index is >= 0)
            {
                current = _list[index];
                RemoveItem(index);
                return true;
            }
            current = default;
            return false;
        }

        public ReadOnlySpan<TKey> EnumerateKeys() => _key_list.AsSpan();
    }
}
