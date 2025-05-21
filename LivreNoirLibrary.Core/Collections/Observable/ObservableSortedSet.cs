using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Collections
{
    public class ObservableSortedSet<T> : ObservableCollectionBase<T>
        where T : IComparable<T>
    {
        protected readonly IComparer<T>? _comparer;

        public ObservableSortedSet(IComparer<T>? comparer = null) : base()
        {
            _comparer = comparer;
        }

        public ObservableSortedSet(int capacity, IComparer<T>? comparer = null) : base(capacity)
        {
            _comparer = comparer;
        }

        public ObservableSortedSet(IEnumerable<T> collection, IComparer<T>? comparer = null) : base([.. collection])
        {
            _comparer = comparer;
            _list.Sort(comparer);
        }

        public sealed override int IndexOf(T item) => _list.BinarySearch(item, _comparer);

        public void AddRange(ObservableSortedSet<T> source)
        {
            AddRangeWithoutNotify(source._list);
            NotifyCollectionReset();
        }

        protected override bool AddRangeCore(IEnumerable<T> items, out int index, [MaybeNullWhen(false)] out List<T> addedItems)
        {
            index = 0;
            addedItems = [.. items];
            AddRangeWithoutNotify(items);
            OnUpdate();
            return true;
        }

        protected void AddRangeWithoutNotify(IEnumerable<T> items)
        {
            _list.AddRange(items);
            _list.Sort(_comparer);
        }

        protected sealed override bool TryAdd(T item, out int index, [MaybeNullWhen(false)] out T current) => AddOrReplace(item, out index, out current);
    }
}
