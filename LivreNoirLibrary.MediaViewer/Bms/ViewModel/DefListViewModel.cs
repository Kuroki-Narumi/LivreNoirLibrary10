using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class DefListViewModel : ObservableObjectBase, IEnumerable<DefListItem>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private readonly DefListItem[] _items;
        [ObservableProperty(SetterScope = Scope.Private)]
        private int _count = Constants.DefMax_Default;

        public DefListViewModel(DefType type)
        {
            _items = new DefListItem[Constants.DefMax_Extended];
            for (var i = 0; i < Constants.DefMax_Extended; i++)
            {
                _items[i] = new(type, i);
            }
        }

        public ArrayEnumerator<DefListItem> GetEnumerator() => new(_items, 0, _count);
        IEnumerator<DefListItem> IEnumerable<DefListItem>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal void Update(DefListCollection source, DefType type, int radix)
        {
            foreach (var item in _items)
            {
                var index = item.Index;
                item.Radix = radix;
                item.Value = source.Get(type, index);
                item.DefaultValue = source.GetParent(type, index);
            }
            Count = radix * radix;
            CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
        }

        internal DefListItem GetItemAt(int index) => _items[index];
        internal DefListItem Update(int index, string? value)
        {
            var item = _items[index];
            item.Value = value;
            return item;
        }

        internal void Clear()
        {
            foreach (var item in _items)
            {
                item.Clear();
            }
        }
    }
}
