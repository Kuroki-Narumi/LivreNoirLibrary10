using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows.Media.Imaging;

namespace LivreNoir.WinCapture
{
    public class CapturedItemCollection : ISafeEnumerable<CapturedItem>, IObservableCollection
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private static readonly CapturedItem _current = new(null, "(current)");
        private readonly List<CapturedItem> _items = [];

        public void Clear()
        {
            _items.Clear();
            this.NotifyCollectionReset();
        }

        public void Add(WriteableBitmap bitmap, string name)
        {
            CapturedItem item = new(bitmap, name);
            _items.Add(item);
            this.NotifyCollectionAdded(_items.Count, item);
        }

        public CapturedItem? LastAddedItem => _items.Count is 0 ? null : _items[^1];

        public void RaiseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);
        public void RaisePropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(sender, e);

        public IEnumerator<CapturedItem> GetEnumerator()
        {
            yield return _current;
            foreach (var item in _items)
            {
                yield return item;
            }
        }
    }
}
