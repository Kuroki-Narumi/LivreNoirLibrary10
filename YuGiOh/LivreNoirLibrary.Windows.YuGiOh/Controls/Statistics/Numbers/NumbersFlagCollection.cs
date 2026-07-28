using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class NumbersFlagCollection : ISafeEnumerable<NumbersFlag>, IObservableCollection
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private readonly ObjectCache<NumbersFlag> _cache;
        private readonly List<NumbersFlag> _items = [];
        private readonly HashSet<int> _checkedItems = [];

        public int Count => _items.Count;
        public int CheckedCount
        {
            get;
            private set
            {
                field = value;
                this.NotifyPropertyChanged(nameof(CheckedCount));
            }
        }

        public NumbersFlagCollection()
        {
            _cache = new(CreateNumberFlag);
        }

        public void Clear()
        {
            foreach( var item in _items)
            {
                item.IsChecked = false;
            }
            CheckedCount = 0;
        }

        private NumbersFlag CreateNumberFlag()
        {
            NumbersFlag item = new();
            item.IsCheckedChanged += Item_IsCheckedChanged;
            return item;
        }

        private void Item_IsCheckedChanged(object? sender, bool e)
        {
            var item = (sender as NumbersFlag)!;
            if (item.IsChecked)
            {
                _checkedItems.Add(item.Number);
                CheckedCount++;
            }
            else
            {
                _checkedItems.Remove(item.Number);
                CheckedCount--;
            }
        }

        public void UpdateItems(IEnumerable<int> numbers)
        {
            var cache = _cache;
            var items = _items;
            _checkedItems.Clear();
            cache.Clear();
            items.Clear();
            foreach (var number in numbers)
            {
                var item = cache.GetNext();
                item.Number = number;
                items.Add(item);
            }
            this.NotifyCollectionReset();
        }

        public bool Contains(NumbersKey obj)
        {
            var set = _checkedItems;
            return set.Count is 0 || set.Contains(obj.Value1) || set.Contains(obj.Value2) || set.Contains(obj.Value3) || set.Contains(obj.Value4);
        }

        public bool IsMatch(NumbersKey obj, MatchType type)
        {
            var set = _checkedItems;
            var setCount = set.Count;
            if (setCount is 0)
            {
                return true;
            }
            var matchCount = 0;
            if (set.Contains(obj.Value1)) matchCount++;
            if (set.Contains(obj.Value2)) matchCount++;
            if (set.Contains(obj.Value3)) matchCount++;
            if (set.Contains(obj.Value4)) matchCount++;
            return type switch
            {
                MatchType.All => matchCount == setCount,
                MatchType.Minimum => matchCount is 4,
                MatchType.Perfect => matchCount is 4 && setCount is 4,
                _ => matchCount is > 0,
            };
        }

        public IEnumerator<NumbersFlag> GetEnumerator() => _items.GetEnumerator();

        void IObservableCollection.RaiseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);
        void IObservableObject.RaisePropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(sender, e);
    }
}
