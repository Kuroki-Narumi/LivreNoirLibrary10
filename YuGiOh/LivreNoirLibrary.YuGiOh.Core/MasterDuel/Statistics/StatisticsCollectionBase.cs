using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public abstract class StatisticsCollectionBase : IObservableCollection
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        protected readonly Dictionary<int, double> _rowTotals = [];

        public abstract void Clear();
        public abstract void UpdateRatio(int totalCount);
        public bool IsEmptyRow(int row) => !_rowTotals.TryGetValue(row, out var value) || value <= 0;
        public abstract void AppendItemLines(StringBuilder sb);

        void IObservableCollection.RaiseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);
        void IObservableObject.RaisePropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(sender, e);
    }

    public abstract class StatisticsCollectionBase<TKey, TValue> : StatisticsCollectionBase, ISafeEnumerable<TValue>
        where TKey : IComparable<TKey>
        where TValue : StatisticsItemBase, new()
    {

        private readonly ObjectCache<TValue> _cache = new(() => new());
        private readonly List<TKey> _keys = [];
        private readonly List<TValue> _values = [];

        public override void Clear()
        {
            _cache.Clear();
            _keys.Clear();
            _values.Clear();
            _rowTotals.Clear();
        }

        protected TValue GetOrAdd(TKey key) => SortedList.GetOrAdd(_keys, _values, key, _ => _cache.GetNext(), out _);

        public override void UpdateRatio(int totalCount)
        {
            var dic = _rowTotals;
            foreach (var value in _values.AsSpan())
            {
                value.CountRatio = StatisticsItemBase.GetRatio(value.Count, totalCount);
                CountValidRow(value, dic);
            }
        }

        protected void CountValidRow(TValue item, Dictionary<int, double> counts)
        {
            foreach (var (i, value) in item.EnumerateIndexAndValue())
            {
                counts[i] = counts.GetValueOrDefault(i) + value;
            }
        }

        public override void AppendItemLines(StringBuilder sb)
        {
            foreach (var value in _values.AsSpan())
            {
                value.AppendLine(sb);
            }
        }

        public virtual IEnumerator<TValue> GetEnumerator() => _values.GetEnumerator();
    }
}
