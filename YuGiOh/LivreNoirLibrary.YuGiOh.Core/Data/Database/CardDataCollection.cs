using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Text.Convert;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class CardDataCollection : IObservableCollection, ICollection<Card>, ICardProvider, IWriteJson
    {
        public const int Capacity = 32768;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private static TextForSearchStringConverter NameConverter { get; } = new(true, false);
        private static ConvertingStringComparer<TextForSearchStringConverter> NameComparer { get; } = new(NameConverter);

        private readonly Card?[] _cards = new Card[Capacity];
        private readonly List<int> _validIds = [];
        private readonly Dictionary<string, List<int>>.AlternateLookup<ReadOnlySpan<char>> _name2id;

        public int Count => _validIds.Count;
        public int MaxCardId => _validIds.Count is > 0 ? _validIds[^1] : 0;
        public ReadOnlySpan<int> Ids => _validIds.AsSpan();

        public CardDataCollection()
        {
            Dictionary<string, List<int>> dic = new(NameComparer);
            _name2id = dic.GetAlternateLookup<ReadOnlySpan<char>>();
        }

        public void ClearWithoutNotify()
        {
            _cards.AsSpan().Clear();
            _validIds.Clear();
            _name2id.Dictionary.Clear();
        }

        public void Clear()
        {
            ClearWithoutNotify();
            this.NotifyCollectionReset();
        }

        private bool AddValidId(int id, out int index)
        {
            index = _validIds.BinarySearch(id);
            if (index >= 0)
            {
                return false;
            }
            index = ~index;
            _validIds.Insert(index, id);
            return true;
        }

        private bool RemoveValidId(int id, out int index)
        {
            index = _validIds.BinarySearch(id);
            if (index >= 0)
            {
                _validIds.RemoveAt(index);
                return true;
            }
            return false;
        }

        private void RegisterName(Card card)
        {
            var id = card.Id;
            _name2id.GetOrAdd(card.Name).Add(id);
            _name2id.GetOrAdd(card.EnName).Add(id);
        }

        private void RemoveName(Card card)
        {
            var id = card.Id;
            _name2id.Remove(card.Name.ToHalf(), id);
            _name2id.Remove(card.EnName, id);
        }

        public Card Add(Card card)
        {
            var id = card.Id;
            var current = _cards[id];
            if (current is null)
            {
                _cards[id] = current = card;
            }
            else
            {
                RemoveName(current);
                current.CopyFrom(card);
            }
            RegisterName(current);
            if (AddValidId(id, out var index))
            {
                this.NotifyCollectionAdded(index, current);
            }
            else
            {
                this.NotifyCollectionReplaced(index, current, current);
            }
            return current;
        }

        void ICollection<Card>.Add(Card card) => Add(card);

        public bool Contains(Card card) => GetOrDefault(card.Id) == card;

        public bool Remove(int id)
        {
            if (GetOrDefault(id) is { } old)
            {
                RemoveValidId(id, out var index);
                RemoveName(old);
                _cards[id] = null;
                this.NotifyCollectionRemoved(index, old);
                return true;
            }
            return false;
        }

        public bool Remove(Card card)
        {
            if (GetOrDefault(card.Id) == card)
            {
                return Remove(card.Id);
            }
            return false;
        }

        public Card? GetOrDefault(int id) => (uint)id < Capacity ? _cards[id] : default;

        public bool TryGetByName(string name, [MaybeNullWhen(false)] out Card card) => TryGetByName(name.AsSpan(), out card);
        public bool TryGetByName(ReadOnlySpan<char> name, [MaybeNullWhen(false)] out Card card)
        {
            if (_name2id.TryGetValue(name, out var list))
            {
                card = _cards[list[0]]!;
                return true;
            }
            card = default;
            return false;
        }

        public void Load(List<Serializable.Card> source)
        {
            ClearWithoutNotify();
            var ids = _validIds;
            var cards = _cards.AsSpan();
            foreach (var s in source.AsSpan())
            {
                var id = s.Id;
                var card = new Card(s);
                ids.Add(id);
                cards[id] = card;
                RegisterName(card);
            }
        }

        void IObservableObject.RaisePropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(sender, e);
        void IObservableCollection.RaiseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var item in this)
            {
                JsonSerializer.Serialize(writer, item, options);
            }
            writer.WriteEndArray();
        }

        public Enumerator GetEnumerator() => new(this);
        public SafeEnumerator GetSaveEnumerator() => new(this);
        IEnumerator<Card> IEnumerable<Card>.GetEnumerator() => GetSaveEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetSaveEnumerator();

        public ref struct Enumerator(CardDataCollection source)
        {
            private readonly ReadOnlySpan<Card?> _cards = source._cards;
            private readonly ReadOnlySpan<int> _validIds = source._validIds.AsSpan();
            private int _index = 0;
            private Card? _current = default;

            public readonly Card Current => _current!;

            public bool MoveNext()
            {
                var ids = _validIds;
                if (_index < ids.Length)
                {
                    _current = _cards[ids[_index]];
                    _index++;
                    return true;
                }
                _current = null;
                return false;
            }
        }

        public sealed class SafeEnumerator(CardDataCollection source) : ISafeEnumerator<Card>
        {
            private readonly Card?[] _cards = source._cards;
            private readonly List<int> _validIds = source._validIds;
            private int _index = 0;
            private Card? _current = default;

            public Card Current => _current!;

            public bool MoveNext()
            {
                var ids = _validIds;
                if (_index < ids.Count)
                {
                    _current = _cards[ids[_index]];
                    _index++;
                    return true;
                }
                _current = null;
                return false;
            }

            public void Reset() => _index = 0;
        }

        bool ICollection<Card>.IsReadOnly => false;
        void ICollection<Card>.CopyTo(Card[] array, int arrayIndex)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(arrayIndex, array.Length - arrayIndex);
            foreach (var card in this)
            {
                array[arrayIndex++] = card;
            }
        }
    }
}
