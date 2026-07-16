using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class DeckCardList : ISafeEnumerable<CountedCard>, IObservableCollection, ICardEnumerable
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public void RaiseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);
        public void RaisePropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(sender, e);

        private readonly List<int> _keyList = [];
        private readonly List<CountedCard> _valueList = [];

        public int MaxCount { get; set; }
        public int UniqueCount => _keyList.Count;
        public int Count { get; private set; }

        private void NotifyUniqueCountChanged() => this.NotifyPropertyChanged(nameof(UniqueCount));
        public void NotifyCollectionReset()
        {
            IObservableCollectionExtensions.NotifyCollectionReset(this);
            NotifyUniqueCountChanged();
        }

        public void Clear()
        {
            ClearWithoutNotify();
            NotifyCollectionReset();
        }

        internal void ClearWithoutNotify()
        {
            _keyList.Clear();
            _valueList.Clear();
            Count = 0;
        }

        public bool TryGetItem(Card card, [MaybeNullWhen(false)] out CountedCard item) => SortedList.TryGetValue(_keyList, _valueList, card.TypeIdIndex, out _, out item);
        public int GetCount(Card card) => TryGetItem(card, out var item) ? item.Count : 0;

        public bool Add(Card card)
        {
            switch (AddWithoutNotify(card, out var index, out var current))
            {
                case NotifyCollectionChangedAction.Add:
                    this.NotifyCollectionAdded(index, current);
                    NotifyUniqueCountChanged();
                    return true;
                case NotifyCollectionChangedAction.Replace:
                    this.NotifyCollectionReplaced(index, current, current);
                    this.NotifyCountChanged();
                    NotifyUniqueCountChanged();
                    return true;
            }
            return false;
        }

        public NotifyCollectionChangedAction AddWithoutNotify(Card card, out int index, out CountedCard? current)
        {
            var key = card.TypeIdIndex;
            var keys = _keyList;
            var values = _valueList;
            if (SortedList.TryGetValue(keys, values, key, out index, out current))
            {
                if (current.Count < MaxCount)
                {
                    current.Count++;
                    Count++;
                    return NotifyCollectionChangedAction.Replace;
                }
            }
            else
            {
                Count++;
                index = ~index;
                keys.Insert(index, key);
                current = GetItem(card);
                current.Count = 1;
                values.Insert(index, current);
                return NotifyCollectionChangedAction.Add;
            }
            return NotifyCollectionChangedAction.Reset;
        }

        public bool Remove(Card card)
        {
            switch (AddWithoutNotify(card, out var index, out var current))
            {
                case NotifyCollectionChangedAction.Replace:
                    this.NotifyCollectionReplaced(index, current, current);
                    this.NotifyCountChanged();
                    NotifyUniqueCountChanged();
                    return true;
                case NotifyCollectionChangedAction.Remove:
                    this.NotifyCollectionAdded(index, current);
                    NotifyUniqueCountChanged();
                    return true;
            }
            return false;
        }

        public NotifyCollectionChangedAction RemoveWithoutNotify(Card card, out int index, out CountedCard? current)
        {
            var key = card.TypeIdIndex;
            var keys = _keyList;
            var values = _valueList;
            if (SortedList.TryGetValue(keys, values, key, out index, out current))
            {
                current.Count--;
                Count--;
                if (current.Count is <= 0)
                {
                    keys.RemoveAt(index);
                    values.RemoveAt(index);
                    return NotifyCollectionChangedAction.Remove;
                }
                return NotifyCollectionChangedAction.Replace;
            }
            return NotifyCollectionChangedAction.Reset;
        }

        public bool Set(Card card, int count)
        {
            switch (SetWithoutNotify(card, count, out var index, out var current))
            {
                case NotifyCollectionChangedAction.Add:
                    this.NotifyCollectionAdded(index, current);
                    NotifyUniqueCountChanged();
                    return true;
                case NotifyCollectionChangedAction.Replace:
                    this.NotifyCollectionReplaced(index, current, current);
                    this.NotifyCountChanged();
                    NotifyUniqueCountChanged();
                    return true;
                case NotifyCollectionChangedAction.Remove:
                    this.NotifyCollectionRemoved(index, current);
                    NotifyUniqueCountChanged();
                    return true;
            }
            return false;
        }

        public NotifyCollectionChangedAction SetWithoutNotify(Card card, int count, out int index, out CountedCard? current)
        {
            var key = card.TypeIdIndex;
            var keys = _keyList;
            var values = _valueList;
            if (SortedList.TryGetValue(keys, values, key, out index, out current))
            {
                var currentCount = current.Count;
                if (currentCount != count)
                {
                    if (count is <= 0)
                    {
                        Count -= currentCount;
                        keys.RemoveAt(index);
                        values.RemoveAt(index);
                        return NotifyCollectionChangedAction.Remove;
                    }
                    else
                    {
                        Count += count - currentCount;
                        current.Count = count;
                        return NotifyCollectionChangedAction.Replace;
                    }
                }
            }
            else if (count is > 0)
            {
                Count += count;
                index = ~index;
                keys.Insert(index, key);
                current = GetItem(card);
                current.Count = count;
                values.Insert(index, current);
                return NotifyCollectionChangedAction.Add;
            }
            return NotifyCollectionChangedAction.Reset;
        }

        private readonly Dictionary<int, CountedCard> _cache = [];
        private CountedCard GetItem(Card card) => _cache.GetOrAdd(card.Id, id => new(card));

        public ReadOnlySpan<CountedCard> AsSpan() => _valueList.AsSpan();
        public List<CountedCard>.Enumerator GetEnumerator() => _valueList.GetEnumerator();
        IEnumerator<CountedCard> IEnumerable<CountedCard>.GetEnumerator() => GetEnumerator();

        public void WriteJson(string propertyName, Utf8JsonWriter writer)
        {
            if (_keyList.Count > 0)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteStartArray();
                foreach (var value in _valueList.AsSpan())
                {
                    var id = value.ThisCard.Id;
                    for (var i = value.Count; i > 0; i--)
                    {
                        writer.WriteNumberValue(id);
                    }
                }
                writer.WriteEndArray();
            }
        }

        public IEnumerable<int> EnumerateIds() => new IdEnumerator(this);
        public IEnumerable<ICard> EnumerateCards() => new CardEnumerator(this);

        private sealed class IdEnumerator(DeckCardList source) : ISafeEnumerator<int>
        {
            private readonly List<CountedCard> _list = source._valueList;
            private readonly int _maxIndex = source._valueList.Count;
            private int _index;
            private int _count;
            
            public int Current { get; private set; }

            public bool MoveNext()
            {
                if (_count > 0)
                {
                    _count--;
                    return true;
                }
                if (_index < _maxIndex)
                {
                    var item = _list[_index];
                    _index++;
                    _count = item.Count - 1;
                    Current = item.ThisCard.Id;
                    return true;
                }
                Current = -1;
                return false;
            }

            public void Reset()
            {
                _index = 0;
                _count = 0;
            }
        }

        private sealed class CardEnumerator(DeckCardList source) : ISafeEnumerator<ICard>
        {
            private readonly List<CountedCard> _list = source._valueList;
            private readonly int _maxIndex = source._valueList.Count;
            private int _index;
            private int _count;

            public ICard Current { get; private set; } = null!;

            public bool MoveNext()
            {
                if (_count > 0)
                {
                    _count--;
                    return true;
                }
                while (_index < _maxIndex)
                {
                    var item = _list[_index];
                    _index++;
                    _count = item.Count - 1;
                    Current = item.ThisCard;
                    return true;
                }
                Current = null!;
                return false;
            }

            public void Reset()
            {
                _index = 0;
                _count = 0;
            }
        }
    }
}
