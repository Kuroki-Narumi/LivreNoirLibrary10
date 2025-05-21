using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public abstract class ObservableCollectionBase<T> : ICollection<T>, ICollection, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool AllowsNotifyMultiple { get; set; } = false;

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

        public void Overwrite(IEnumerable<T> items)
        {
            ClearWithoutNotify();
            AddRange(items);
        }

        /// <inheritdoc cref="List{T}.AddRange"/>
        public void AddRange(IEnumerable<T> items)
        {
            if (AddRangeCore(items, out var index, out var list))
            {
                OnCollectionAdded(list, index);
            }
        }

        public void AddRange<TK>(IDictionary<TK, T> items) where TK : notnull => AddRange(items.Values);

        public int RemoveRange(IEnumerable<T> items)
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

        protected virtual void OnItemRemoved(T item) { }
        protected virtual void OnItemAdded(T item) { }

        protected virtual void ClearItems()
        {
            foreach (var item in CollectionsMarshal.AsSpan(_list))
            {
                OnItemRemoved(item);
            }
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
            OnItemAdded(item);
            OnUpdate();
        }

        protected virtual void ReplaceItem(int index, T item)
        {
            OnItemRemoved(_list[index]);
            _list[index] = item;
            OnItemAdded(item);
            OnUpdate();
        }

        protected virtual void RemoveItem(int index)
        {
            OnItemRemoved(_list[index]);
            _list.RemoveAt(index);
            OnUpdate();
        }

        protected virtual bool AddRangeCore(IEnumerable<T> items, out int index, [MaybeNullWhen(false)] out List<T> addedItems)
        {
            index = _list.Count;
            _list.AddRange(items);
            if (!items.TryGetNonEnumeratedCount(out var c))
            {
                c = 1;
            }
            addedItems = new(c);
            foreach (var item in items)
            {
                addedItems.Add(item);
                OnItemAdded(item);
            }
            OnUpdate();
            return true;
        }
        
        protected virtual int RemoveRangeCore(IEnumerable<T> items)
        {
            int count = 0;
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

        protected void OnCollectionAdded(List<T> items, int startingIndex)
        {
            OnCountChanged();
            if (AllowsNotifyMultiple)
            {
                SendCollectionChanged(new(NotifyCollectionChangedAction.Add, items, startingIndex));
            }
            else
            {
                OnCollectionReset();
            }
        }

        protected void OnCollectionRemoved(T item, int indexFrom)
        {
            OnCountChanged();
            SendCollectionChanged(new(NotifyCollectionChangedAction.Remove, item, indexFrom));
        }

        protected void OnCollectionRemoved(List<T> items, int startingIndex)
        {
            if (AllowsNotifyMultiple)
            {
                OnCountChanged();
                SendCollectionChanged(new(NotifyCollectionChangedAction.Remove, items, startingIndex));
            }
            else
            {
                OnCollectionReset();
            }
        }

        protected void OnCollectionReplaced(T newItem, T? oldItem, int index)
        {
            SendCollectionChanged(new(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));
        }

        protected void OnCollectionReplaced(List<T> added, List<T> removed, int startingIndex)
        {
            if (AllowsNotifyMultiple)
            {
                OnCountChanged();
                SendCollectionChanged(new(NotifyCollectionChangedAction.Replace, added, removed, startingIndex));
            }
            else
            {
                OnCollectionReset();
            }
        }

        protected void OnCollectionMoved(T item, int indexTo, int indexFrom)
        {
            SendCollectionChanged(new(NotifyCollectionChangedAction.Move, item, indexTo, indexFrom));
        }

        protected void OnCollectionMoved(List<T> items, int indexTo, int indexFrom)
        {
            if (AllowsNotifyMultiple)
            {
                OnCountChanged();
                SendCollectionChanged(new(NotifyCollectionChangedAction.Move, items, indexTo, indexFrom));
            }
            else
            {
                OnCollectionReset();
            }
        }

        protected void SendCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        protected void SendPropertyChanged([CallerMemberName]string propName = "")
        {
            PropertyChanged?.Invoke(this, new(propName));
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
