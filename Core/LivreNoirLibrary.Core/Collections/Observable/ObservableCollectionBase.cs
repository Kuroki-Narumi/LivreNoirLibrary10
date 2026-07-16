using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Collections
{
    public abstract class ObservableCollectionBase<T> : ObservableObjectBase, ICollection<T>, ICollection, IObservableCollection
    {
        internal protected readonly List<T> _list;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public int Count => _list.Count;

        public ObservableCollectionBase(int capacity) => _list = new(capacity);
        public ObservableCollectionBase(IEnumerable<T> collection) => _list = [.. collection];

        public T GetItemAt(int index) => _list[index];
        public virtual int IndexOf(T item) => _list.IndexOf(item);
        public bool Contains(T item) => _list.Count is > 0 && IndexOf(item) is >= 0;

        /// <inheritdoc cref="CollectionsMarshal.AsSpan"/>
        public ReadOnlySpan<T> AsSpan() => _list.AsSpan();
        /// <inheritdoc cref="CollectionsMarshal.AsSpan"/>
        public ReadOnlySpan<T> AsSpan(int index) => _list.AsSpan()[index..];
        /// <inheritdoc cref="CollectionsMarshal.AsSpan"/>
        public ReadOnlySpan<T> AsSpan(int index, int count) => _list.AsSpan(index, count);

        public void Clear()
        {
            ClearItems();
            this.NotifyCollectionReset();
        }

        /// <inheritdoc cref="Clear"/>
        public void ClearWithoutNotify() => ClearItems();

        public void Add(T item)
        {
            AddItem(item, out var replaced, out var index, out var oldItem);
            if (replaced)
            {
                this.NotifyCollectionReplaced(index, oldItem, item);
            }
            else
            {
                this.NotifyCollectionAdded(index, item);
            }
        }

        /// <inheritdoc cref="Add"/>
        public void AddWithoutNotify(T item) => AddItem(item, out _, out _, out _);

        public bool Remove(T item)
        {
            var index = RemoveItem(item);
            if (index is >= 0)
            {
                this.NotifyCollectionRemoved(index, item);
                return true;
            }
            return false;
        }

        /// <inheritdoc cref="Remove"/>
        public bool RemoveWithoutNotify(T item) => RemoveItem(item) is >= 0;

        /// <inheritdoc cref="List{T}.ConvertAll{TOutput}(Converter{T, TOutput})"/>
        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) => _list.ConvertAll(converter);
        /// <inheritdoc cref="List{T}.Exists(Predicate{T})"/>
        public bool Exists(Predicate<T> predicate) => _list.Exists(predicate);
        /// <inheritdoc cref="List{T}.Find(Predicate{T})"/>
        public T? Find(Predicate<T> predicate) => _list.Find(predicate);
        /// <inheritdoc cref="List{T}.FindAll(Predicate{T})"/>
        public List<T> FindAll(Predicate<T> predicate) => _list.FindAll(predicate);
        /// <inheritdoc cref="List{T}.FindIndex(Predicate{T})"/>
        public int FindIndex(Predicate<T> predicate) => _list.FindIndex(predicate);
        /// <inheritdoc cref="List{T}.FindLast(Predicate{T})"/>
        public T? FindLast(Predicate<T> predicate) => _list.FindLast(predicate);
        /// <inheritdoc cref="List{T}.FindLastIndex(Predicate{T})"/>
        public int FindLastIndex(Predicate<T> predicate) => _list.FindLastIndex(predicate);

        public List<T>.Enumerator GetEnumerator() => _list.GetEnumerator();
        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        protected virtual void ClearItems() => _list.Clear();

        protected virtual void AddItem(T item, out bool replaced, out int index, out T? oldItem)
        {
            _list.Add(item);
            index = _list.Count - 1;
            replaced = false;
            oldItem = default;
        }

        protected virtual int RemoveItem(T item)
        {
            var index = IndexOf(item);
            if (index is >= 0)
            {
                _list.RemoveAt(index);
            }
            return index;
        }

        void IObservableCollection.RaiseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        bool ICollection<T>.IsReadOnly => false;
        void ICollection.CopyTo(Array array, int index) => (_list as ICollection).CopyTo(array, index);
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot { get; } = new();
    }
}
