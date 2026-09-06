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
        public event EventHandler? CheckedItemChanged;

        private readonly ObjectCache<TValue> _cache;

        protected readonly List<TValue> _items = [];
        private HashSet<TKey> _checkedItems = [];
        private HashSet<TKey> _checkedBuffer = [];
        private bool _isCheckedChanging;

        public int Count => _items.Count;
        public int CheckedCount => _checkedItems.Count;

        protected HashSet<TKey> CheckedItems => _checkedItems;

        public CheckableItemCollection()
        {
            _cache = new(CreateNewItem);
        }

        public void Clear()
        {
            _isCheckedChanging = true;
            _cache.Clear();
            _items.Clear();
            _checkedItems.Clear();
            _isCheckedChanging = false;
            this.NotifyCollectionReset();
            NotifyCheckedItemChanged();
        }

        public void ClearFlags()
        {
            _isCheckedChanging = true;
            foreach (var item in _items.AsSpan())
            {
                item.IsChecked = false;
            }
            _checkedItems.Clear();
            _isCheckedChanging = false;
            NotifyCheckedItemChanged();
        }

        private TValue CreateNewItem()
        {
            var item = CreateItem();
            item.IsCheckedChanged += Item_IsCheckedChanged;
            return item;
        }

        private void Item_IsCheckedChanged(object? sender, bool value)
        {
            if (_isCheckedChanging || sender is not TValue item)
            {
                return;
            }
            if (item.IsChecked)
            {
                _checkedItems.Add(GetKey(item));
            }
            else
            {
                _checkedItems.Remove(GetKey(item));
            }
            NotifyCheckedItemChanged();
        }

        protected void RefreshItems<T>(IEnumerable<T> source, Action<TValue, T>? initializer = null)
        {
            // チェック済みだったアイテムset
            var @checked = _checkedItems;
            // sourceに含まれるチェック済みアイテムset
            var buffer = _checkedBuffer;

            _isCheckedChanging = true;
            var cache = _cache;
            var items = _items;
            // ここで全てのアイテムについて IsChecked = false となる
            cache.Clear();
            items.Clear();
            foreach (var obj in source)
            {
                var item = cache.GetNext();
                initializer?.Invoke(item, obj);
                var key = GetKey(item);
                // チェックアイテムsetに含まれる場合
                if (@checked.Contains(key))
                {
                    item.IsChecked = true;
                    buffer.Add(key);
                }
                items.Add(item);
            }
            // バッファ(現在チェック済みのアイテム)setと以前のチェック済みアイテムsetを交換
            _checkedItems = buffer;
            _checkedBuffer = @checked;
            @checked.Clear();

            _isCheckedChanging = false;
            this.NotifyCollectionReset();
            NotifyCheckedItemChanged();
        }

        protected abstract TValue CreateItem();
        protected abstract TKey GetKey(TValue item);

        protected void NotifyCheckedItemChanged()
        {
            this.NotifyPropertyChanged(nameof(CheckedCount));
            CheckedItemChanged?.Invoke(this, EventArgs.Empty);
        }

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
