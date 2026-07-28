using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Collections;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class TokenCollection : ISafeEnumerable<Token>, IObservableCollection
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public int Count => _activeCount;

        public ReadOnlySpan<Token> ActiveItems => _items.AsSpan(0, _activeCount);
        public SortedCardList Referers { get; } = [];
        public SortedCardList NegativeReferers { get; } = [];

        private readonly List<Token> _items = [];
        private int _activeCount = 0;
        private readonly Dictionary<string, int> _indexes = [];

        public void Clear()
        {
            Referers.Clear();
            NegativeReferers.Clear();
            // トークンオブジェクトそのものは削除しない(GC回避のため)
            foreach (var item in _items.AsSpan())
            {
                item.Clear();
            }
            _activeCount = 0;
            _indexes.Clear();
            this.NotifyCollectionReset();
        }

        public Token GetOrAdd(ReadOnlySpan<char> name)
        {
            var items = _items;
            Token token;
            if (!_indexes.GetAlternateLookup<ReadOnlySpan<char>>().TryGetValue(name, out var index))
            {
                index = _activeCount;
                var nameStr = name.ToString();
                _indexes[nameStr] = index;
                if (index >= items.Count)
                {
                    token = new(index + 1, nameStr);
                    items.Add(token);
                }
                else
                {
                    token = items[index];
                    token.Name = nameStr;
                }
                _activeCount++;
                this.NotifyCollectionAdded(index, token);
            }
            return items[index];
        }

        public IEnumerator<Token> GetEnumerator() => new Enumerator(this);
        void IObservableObject.RaisePropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(sender, e);
        void IObservableCollection.RaiseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);

        private class Enumerator(TokenCollection source) : ISafeEnumerator<Token>
        {
            private readonly int _count = source._activeCount;
            private readonly List<Token> _list = source._items;
            private int _index = 0;

            public Token Current { get; private set; } = null!;

            public bool MoveNext()
            {
                if (_index < _count)
                {
                    Current = _list[_index];
                    _index++;
                    return true;
                }
                return false;
            }
        }
    }
}
