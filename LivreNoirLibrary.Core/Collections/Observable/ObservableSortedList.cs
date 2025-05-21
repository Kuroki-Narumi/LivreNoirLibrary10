using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public abstract class ObservableSortedList<TKey, TValue> : ObservableCollectionBase<TValue>
        where TKey : IComparable<TKey>
    {
        protected readonly IComparer<TKey>? _comparer = null;
        protected readonly List<TKey> _key_list = [];

        public ObservableSortedList(IComparer<TKey>? comparer = null) : base()
        {
            _comparer = comparer;
        }

        public ObservableSortedList(int capacity, IComparer<TKey>? comparer = null) : base(capacity)
        {
            _comparer = comparer;
        }

        public ObservableSortedList(IEnumerable<TValue> collection, IComparer<TKey>? comparer = null) : base()
        {
            _comparer = comparer;
            foreach (var item in collection.OrderBy(GetKey))
            {
                _list.Add(item);
                _key_list.Add(GetKey(item));
            }
        }

        public sealed override int IndexOf(TValue item) => IndexOfKey(GetKey(item));

        public bool ContainsKey(TKey key) => (uint)IndexOfKey(key) < (uint)_key_list.Count;
        public int IndexOfKey(TKey key) => _key_list.BinarySearch(key, _comparer);

        public TValue this[int index]
        {
            get
            {
                if ((uint)index < (uint)_list.Count)
                {
                    return _list[index];
                }
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

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

        protected abstract TKey GetKey(TValue item);

        public void AddRange<TK, TCollection>(ObservableSortedList<TK, TValue> items)
            where TK : IComparable<TK>
        {
            AddRange(items._list);
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

        protected sealed override bool TryAdd(TValue item, out int index, [MaybeNullWhen(false)] out TValue current) => AddOrReplace(item, out index, out current);

        protected override bool AddRangeCore(IEnumerable<TValue> items, out int index, [MaybeNullWhen(false)] out List<TValue> addedItems)
        {
            void Add(TValue item, ref int index)
            {
                if (!AddOrReplace(item, out var j, out _) && j < index)
                {
                    index = j;
                }
            }
            index = _list.Count;
            switch (items)
            {
                case TValue[] ary:
                    foreach (var item in ary)
                    {
                        Add(item, ref index);
                    }
                    break;
                case List<TValue> list:
                    foreach (var item in CollectionsMarshal.AsSpan(list))
                    {
                        Add(item, ref index);
                    }
                    break;
                case IList<TValue> list:
                    var c = list.Count;
                    for (int i = 0; i < c; i++)
                    {
                        Add(list[i], ref index);
                    }
                    break;
                default:
                    foreach (var item in items)
                    {
                        Add(item, ref index);
                    }
                    break;
            }
            addedItems = [.. items];
            return true;
        }

        public bool RemoveKeyWithoutNotify(TKey key) => TryRemoveKey(key, out _, out _);

        protected bool TryRemoveKey(TKey key, out int index, [MaybeNullWhen(false)] out TValue current)
        {
            index = _key_list.BinarySearch(key);
            if (index is >= 0)
            {
                current = _list[index];
                RemoveItem(index);
                return true;
            }
            current = default;
            return false;
        }

        protected sealed override void ClearItems()
        {
            _key_list.Clear();
            base.ClearItems();
        }

        protected override void AddItem(int index, TValue item)
        {
            _key_list.Insert(index, GetKey(item));
            base.AddItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            _key_list.RemoveAt(index);
            base.RemoveItem(index);
        }
    }
}
