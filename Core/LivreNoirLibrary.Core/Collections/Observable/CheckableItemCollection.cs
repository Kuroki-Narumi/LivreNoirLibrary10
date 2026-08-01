using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace LivreNoirLibrary.Collections
{
    public abstract class CheckableItemCollection<TKey, TValue> : ISafeEnumerable<TValue>, IObservableCollection
        where TValue : IClear, INotifyIsCheckedChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private readonly ObjectCache<TValue> _cache;

        protected readonly List<TValue> _items = [];
        protected readonly HashSet<TKey> _checkedItems = [];

        private static readonly ThreadLocal<HashSet<TKey>> _checkedBuffer = new(() => []);

        public int Count => _items.Count;
        public int CheckedCount
        {
            get;
            private set
            {
                if (field != value)
                {
                    field = value;
                    this.NotifyPropertyChanged(nameof(CheckedCount));
                }
            }
        }

        public CheckableItemCollection()
        {
            _cache = new(CreateNewItem);
        }

        public void Clear()
        {
            _cache.Clear();
            _items.Clear();
            _checkedItems.Clear();
            CheckedCount = 0;
            this.NotifyCollectionReset();
        }

        public void ClearFlags()
        {
            foreach (var item in _items.AsSpan())
            {
                item.IsChecked = false;
            }
            CheckedCount = 0;
        }

        private TValue CreateNewItem()
        {
            var item = CreateItem();
            item.IsCheckedChanged += Item_IsCheckedChanged;
            return item;
        }

        private void Item_IsCheckedChanged(object? sender, bool value)
        {
            if (sender is not TValue item)
            {
                return;
            }
            if (item.IsChecked)
            {
                _checkedItems.Add(GetKey(item));
                CheckedCount++;
            }
            else
            {
                _checkedItems.Remove(GetKey(item));
                CheckedCount--;
            }
        }

        protected void RefreshItems<T>(IEnumerable<T> source, Action<TValue, T> initializer)
        {
            var @checked = _checkedItems;
            var buffer = _checkedBuffer.Value!;
            buffer.UnionWith(@checked);
            @checked.Clear();

            var cache = _cache;
            var items = _items;
            cache.Clear();
            items.Clear();
            foreach (var obj in source)
            {
                var item = cache.GetNext();
                initializer(item, obj);
                item.IsChecked = @checked.Contains(GetKey(item));
                items.Add(item);
            }
            this.NotifyCollectionReset();
        }

        protected abstract TValue CreateItem();
        protected abstract TKey GetKey(TValue item);

        public IEnumerator<TValue> GetEnumerator() => _items.GetEnumerator();
        void IObservableCollection.RaiseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);
        void IObservableObject.RaisePropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(sender, e);

        public IEnumerable<TValue> EnumerateChecked()
        {
            foreach (var item in _items)
            {
                if (item.IsChecked)
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<TKey> EnumerateCheckedKeys() => _checkedItems;

        public ReadOnlySpan<TValue> AsSpan() => _items.AsSpan();
    }
}
