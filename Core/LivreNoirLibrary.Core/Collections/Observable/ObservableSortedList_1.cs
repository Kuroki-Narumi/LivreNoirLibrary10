using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Collections
{
    public class ObservableSortedList<T> : ObservableCollectionBase<T>, ICollection<T>
    {
        protected readonly IComparer<T>? _comparer = null;

        public ObservableSortedList(IComparer<T>? comparer = null) : base(0)
        {
            _comparer = comparer;
        }

        public ObservableSortedList(int capacity, IComparer<T>? comparer = null) : base(capacity)
        {
            _comparer = comparer;
        }

        public ObservableSortedList(IEnumerable<T> collection, IComparer<T>? comparer = null) : base(0)
        {
            _comparer = comparer;
            foreach (var item in collection)
            {
                AddWithoutNotify(item);
            }
        }

        public sealed override int IndexOf(T item) => _list.BinarySearch(item, _comparer);

        public void Add(T item, out int index)
        {
            AddItem(item, out var replaced, out index, out var oldItem);
            if (replaced)
            {
                this.NotifyCollectionReplaced(index, oldItem, item);
            }
            else
            {
                this.NotifyCollectionAdded(index, item);
            }
        }

        protected override void AddItem(T item, out bool replaced, out int index, out T? oldItem)
        {
            index = IndexOf(item);
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
                _list.Insert(index, item);
            }
        }

        protected override int RemoveItem(T item)
        {
            var index = IndexOf(item);
            if (index is >= 0 && EqualityComparer<T>.Default.Equals(_list[index], item))
            {
                _list.RemoveAt(index);
                return index;
            }
            return -1;
        }

        /// <inheritdoc cref="System.Collections.Generic.CollectionExtensions.AddRange"/>
        public void AddRange(params ReadOnlySpan<T> items)
        {
            var modified = false;
            foreach (var item in items)
            {
                AddItem(item, out _, out _, out _);
                modified = true;
            }
            if (modified)
            {
                this.NotifyCollectionReset();
            }
        }

        /// <inheritdoc cref="List{T}.AddRange"/>
        public void AddRange(IEnumerable<T> items)
        {
            var modified = false;
            foreach (var item in items)
            {
                AddItem(item, out _, out _, out _);
                modified = true;
            }
            if (modified)
            {
                this.NotifyCollectionReset();
            }
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
                this.NotifyCollectionReset();
            }
            return count;
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
                this.NotifyCollectionReset();
            }
            return count;
        }

        public int RemoveAll(Func<T, bool> match)
        {
            var count = 0;
            for (var i = 0; i < _list.Count;)
            {
                if (match(_list[i]))
                {
                    _list.RemoveAt(i);
                    count++;
                }
                else
                {
                    i++;
                }
            }
            if (count is > 0)
            {
                this.NotifyCollectionReset();
            }
            return count;
        }
    }
}
