using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Collections
{
    public abstract class ObservableCollectionBase<T> : ObservableObjectBase, ICollection<T>, ICollection, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        protected int _version = 0;
        protected readonly List<T> _list;

        public ObservableCollectionBase()
        {
            _list = [];
        }

        public ObservableCollectionBase(int capacity)
        {
            _list = new(capacity);
        }

        public ObservableCollectionBase(List<T> list)
        {
            _list = list;
        }

        public int Count { get => _list.Count; }

        /// <inheritdoc cref="List{T}.IndexOf"/>
        public virtual int IndexOf(T item) => _list.IndexOf(item);

        public bool Contains(T item) => (uint)IndexOf(item) < (uint)_list.Count;

        /// <inheritdoc cref="List{T}.ToArray"/>
        public T[] ToArray() => [.. _list];

        /// <inheritdoc cref="CollectionsMarshal.AsSpan"/>
        public ReadOnlySpan<T> AsSpan() => CollectionsMarshal.AsSpan(_list);

        public void Clear()
        {
            ClearItems();
            OnCollectionReset();
        }

        /// <inheritdoc cref="ICollection{T}.Clear"/>
        public void ClearWithoutNotify() => ClearItems();

        public void Add(T item)
        {
            if (TryAdd(item, out var index, out var current))
            {
                OnCollectionReplaced(item, current, index);
            }
            else
            {
                OnCollectionAdded(item, index);
            }
        }

        /// <inheritdoc cref="ICollection{T}.Add"/>
        public void AddWithoutNotify(T item)
        {
            TryAdd(item, out _, out _);
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (TryRemove(index, out var current))
            {
                OnCollectionRemoved(current, index);
                return true;
            }
            return false;
        }

        /// <inheritdoc cref="ICollection{T}.Remove"/>
        public bool RemoveWithoutNotify(T item) => TryRemove(IndexOf(item), out _);

        /// <inheritdoc cref="List{T}.AddRange"/>
        public void AddRange(ObservableCollectionBase<T> items) => AddRange(items._list);

        /// <inheritdoc cref="List{T}.AddRange"/>
        public void AddRange(IEnumerable<T> items)
        {
            if (AddRangeCore(items))
            {
                NotifyCollectionReset();
            }
        }

        /// <inheritdoc cref="List{T}.AddRange"/>
        public void AddRange(params ReadOnlySpan<T> items)
        {
            if (AddRangeCore(items))
            {
                NotifyCollectionReset();
            }
        }

        public void AddRange<TK>(IDictionary<TK, T> items) where TK : notnull => AddRange(items.Values);

        public void Load(ObservableCollectionBase<T> items)
        {
            ClearWithoutNotify();
            AddRange(items);
        }

        public void Load(IEnumerable<T> items)
        {
            ClearWithoutNotify();
            AddRange(items);
        }

        public int RemoveRange(IEnumerable<T> items)
        {
            var c = RemoveRangeCore(items);
            if (c > 0)
            {
                NotifyCollectionReset();
            }
            return c;
        }

        public int RemoveRange(params ReadOnlySpan<T> items)
        {
            var c = RemoveRangeCore(items);
            if (c > 0)
            {
                NotifyCollectionReset();
            }
            return c;
        }

        public int RemoveRange<TK>(IDictionary<TK, T> items) where TK : notnull => RemoveRange(items.Values);

        /// <inheritdoc cref="List{T}.ConvertAll{TOutput}(Converter{T, TOutput})"/>
        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) => _list.ConvertAll(converter);
        /// <inheritdoc cref="List{T}.Exists(Predicate{T})"/>
        public bool Exists(Predicate<T> predicate) => _list.Exists(predicate);
        /// <inheritdoc cref="List{T}.Find(Predicate{T})"/>
        public T? Find(Predicate<T> predicate) => _list.Find(predicate);
        /// <inheritdoc cref="List{T}.FindAll(Predicate{T})"/>
        public List<T> FindAll(Predicate<T> predicate) => _list.FindAll(predicate);
        public T? FindNext(Predicate<T> predicate, int index)
        {
            var c = _list.Count;
            if (index is >= -1)
            {
                for (int i = index + 1; i < c; i++)
                {
                    var item = _list[i];
                    if (predicate(item))
                    {
                        return item;
                    }
                }
            }
            if (index < c)
            {
                for (int i = 0; i < index; i++)
                {
                    var item = _list[i];
                    if (predicate(item))
                    {
                        return item;
                    }
                }
            }
            return default;
        }
        /// <inheritdoc cref="List{T}.FindIndex(Predicate{T})"/>
        public int FindIndex(Predicate<T> predicate) => _list.FindIndex(predicate);
        /// <inheritdoc cref="List{T}.FindLast(Predicate{T})"/>
        public T? FindLast(Predicate<T> predicate) => _list.FindLast(predicate);
        /// <inheritdoc cref="List{T}.FindLastIndex(Predicate{T})"/>
        public int FindLastIndex(Predicate<T> predicate) => _list.FindLastIndex(predicate);

        public void NotifyCollectionReset()
        {
            OnCollectionReset();
            OnUpdate();
        }

        public List<T>.Enumerator GetEnumerator() => _list.GetEnumerator();
        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        protected virtual void ClearItems()
        {
            _list.Clear();
            OnUpdate();
        }

        protected virtual bool TryAdd(T item, out int index, [MaybeNullWhen(false)] out T current)
        {
            index = _list.Count;
            AddItem(index, item);
            current = default;
            return false;
        }

        protected bool AddOrReplace(T item, out int index, [MaybeNullWhen(false)] out T current)
        {
            index = IndexOf(item);
            if (index is >= 0)
            {
                current = _list[index];
                ReplaceItem(index, item);
                return true;
            }
            else
            {
                index = ~index;
                current = default;
                AddItem(index, item);
                return false;
            }
        }

        protected bool TryRemove(int index, [MaybeNullWhen(false)] out T current)
        {
            if ((uint)index < (uint)_list.Count)
            {
                current = _list[index];
                RemoveItem(index);
                return true;
            }
            current = default;
            return false;
        }

        protected virtual void AddItem(int index, T item)
        {
            _list.Insert(index, item);
            OnUpdate();
        }

        protected virtual void ReplaceItem(int index, T item)
        {
            _list[index] = item;
            OnUpdate();
        }

        protected virtual void RemoveItem(int index)
        {
            _list.RemoveAt(index);
            OnUpdate();
        }

        protected virtual bool AddRangeCore(IEnumerable<T> items)
        {
            var count = _list.Count;
            _list.AddRange(items);
            OnUpdate();
            return _list.Count != count;
        }

        protected virtual bool AddRangeCore(ReadOnlySpan<T> items)
        {
            _list.AddRange(items);
            OnUpdate();
            return items.Length is > 0;
        }
        
        protected virtual int RemoveRangeCore(IEnumerable<T> items)
        {
            var count = 0;
            void Remove(T item)
            {
                if (RemoveWithoutNotify(item))
                {
                    count++;
                }
            }
            switch (items)
            {
                case T[] ary:
                    foreach (var item in ary)
                    {
                        Remove(item);
                    }
                    break;
                case List<T> list:
                    foreach (var item in CollectionsMarshal.AsSpan(list))
                    {
                        Remove(item);
                    }
                    break;
                case IList<T> list:
                    var c = list.Count;
                    for (int i = 0; i < c; i++)
                    {
                        Remove(list[i]);
                    }
                    break;
                default:
                    foreach (var item in items)
                    {
                        Remove(item);
                    }
                    break;
            }
            return count;
        }
        
        protected virtual int RemoveRangeCore(ReadOnlySpan<T> items)
        {
            var count = 0;
            foreach (var item in items)
            {
                if (RemoveWithoutNotify(item))
                {
                    count++;
                }
            }
            return count;
        }

        protected void OnCountChanged()
        {
            SendPropertyChanged(nameof(Count));
        }

        protected void OnCollectionReset()
        {
            OnCountChanged();
            SendCollectionChanged(new(NotifyCollectionChangedAction.Reset));
        }

        protected void OnCollectionAdded(T item, int indexTo)
        {
            OnCountChanged();
            SendCollectionChanged(new(NotifyCollectionChangedAction.Add, item, indexTo));
        }

        protected void OnCollectionRemoved(T item, int indexFrom)
        {
            OnCountChanged();
            SendCollectionChanged(new(NotifyCollectionChangedAction.Remove, item, indexFrom));
        }

        protected void OnCollectionReplaced(T newItem, T? oldItem, int index)
        {
            SendCollectionChanged(new(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));
        }

        protected void OnCollectionMoved(T item, int indexTo, int indexFrom)
        {
            SendCollectionChanged(new(NotifyCollectionChangedAction.Move, item, indexTo, indexFrom));
        }

        protected void SendCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        protected virtual void OnUpdate()
        {
            _version++;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        bool ICollection<T>.IsReadOnly => false;
        void ICollection.CopyTo(Array array, int index) => (_list as ICollection).CopyTo(array, index);
        bool ICollection.IsSynchronized => true;
        object ICollection.SyncRoot { get; } = new();
    }
}
