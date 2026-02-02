using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Collections
{
    public class DoubleEndedQueue<T> : IReadOnlyCollection<T>
    {
        public const int DefaultCapacity = 4;

        private T[] _items;
        private int _head;
        private int _tail;
        private int _size;

        public int Count => _size;

        public int Capacity
        {
            get => _items.Length;
            set
            {
                if (value < _size)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Capacity cannot be set to a value less than Count.");
                }
                SetCapacity(value, false);
            }
        }

        public DoubleEndedQueue() : this(DefaultCapacity) { }

        public DoubleEndedQueue(int capacity)
        {
            _items = new T[(capacity is <= 0 ? DefaultCapacity : capacity)];
        }

        public DoubleEndedQueue(IEnumerable<T> collection, int capacity = 0)
        {
            if (collection is ICollection<T> coll)
            {
                var count = coll.Count;
                capacity = Math.Max(Math.Max(capacity, count), 1);
                _items = new T[capacity];
                coll.CopyTo(_items, 0);
                _head = 0;
                _tail = count;
                _size = count;
            }
            else
            {
                _items = new T[(capacity is <= 0 ? DefaultCapacity : capacity)];
                foreach (var item in collection)
                {
                    Push(item);
                }
            }
        }

        public void Clear()
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                Array.Clear(_items);
            }
            _head = 0;
            _tail = 0;
            _size = 0;
        }

        /// <summary>
        /// Ensures that the capacity of this <see cref="DoubleEndedQueue{T}"/> is at least the specified <paramref name="capacity"/>.
        /// </summary>
        /// <param name="capacity">The minimum capacity to ensure.</param>
        /// <returns>The new capacity of this queue.</returns>
        public int EnsureCapacity(int capacity)
        {
            if (Capacity < capacity)
            {
                Grow(capacity, false);
            }
            return Capacity;
        }

        private void Grow(int capacity, bool needsPushFront)
        {
            SetCapacity(capacity < DefaultCapacity ? DefaultCapacity : (int)BitOperations.RoundUpToPowerOf2((uint)capacity), needsPushFront);
        }

        private void SetCapacity(int capacity, bool needsPushFront)
        {
            var newItems = new T[capacity];
            var offset = needsPushFront ? 1 : 0;
            CopyToImpl(newItems, offset);
            _items = newItems;
            _head = offset;
            _tail = _size + offset;
        }

        public void PushFront(T item)
        {
            if (_size == _items.Length)
            {
                Grow(_size + 1, true);
            }
            _head = GetPreviousIndex(_head);
            _items[_head] = item;
            _size++;
        }

        public void Push(T item)
        {
            if (_size == _items.Length)
            {
                Grow(_size + 1, false);
            }
            _items[_tail] = item;
            Advance(ref _tail);
            _size++;
        }

        public void Enqueue(T item) => Push(item);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnPopFront()
        {
            ClearItemAt(_head);
            Advance(ref _head);
            _size--;
        }

        public bool TryDequeue([MaybeNullWhen(false)] out T result)
        {
            if (_size is > 0)
            {
                result = _items[_head];
                OnPopFront();
                return true;
            }
            result = default;
            return false;
        }

        public bool DequeueIf(Predicate<T> match, [MaybeNullWhen(false)] out T result)
        {
            var item = _items[_head];
            if (_size is > 0 && match(item))
            {
                result = item;
                OnPopFront();
                return true;
            }
            result = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnPopBack(int index)
        {
            ClearItemAt(index);
            _tail = index;
            _size--;
        }

        public bool TryPop([MaybeNullWhen(false)]out T result)
        {
            if (_size is > 0)
            {
                var index = GetPreviousIndex(_tail);
                result = _items[index];
                OnPopBack(index);
                return true;
            }
            result = default;
            return false;
        }

        public bool PopIf(Predicate<T> match, [MaybeNullWhen(false)] out T result)
        {
            var index = GetPreviousIndex(_tail);
            var item = _items[index];
            if (_size is > 0 && match(item))
            {
                result = item;
                OnPopBack(index);
                return true;
            }
            result = default;
            return false;
        }

        public bool TryPeakBottom([MaybeNullWhen(false)] out T result)
        {
            if (_size is > 0)
            {
                result = _items[_head];
                return true;
            }
            result = default;
            return false;
        }

        public bool TryPeakTop([MaybeNullWhen(false)] out T result)
        {
            if (_size is > 0)
            {
                result = _items[GetPreviousIndex(_tail)];
                return true;
            }
            result = default;
            return false;
        }

        public T Dequeue() => TryDequeue(out var result) ? result : ThrowForEmpty();

        public T Pop() => TryPop(out var result) ? result : ThrowForEmpty();

        public T PeakBottom() => _size is 0 ? ThrowForEmpty() : _items[_head];

        public T PeakTop() => _size is 0 ? ThrowForEmpty() : _items[GetPreviousIndex(_tail)];

        private void CopyToImpl(T[] array, int index)
        {
            if (_size > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_items, _head, array, index, _size);
                }
                else
                {
                    var headToEnd = _items.Length - _head;
                    Array.Copy(_items, _head, array, index, headToEnd);
                    Array.Copy(_items, 0, array, headToEnd + index, _tail);
                }
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ArgumentNullException.ThrowIfNull(array);
            if ((uint)arrayIndex >= (uint)array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Index is out of range.");
            }
            if (array.Length - arrayIndex < _size)
            {
                throw new ArgumentException("The number of elements in the source DoubleEndedQueue<T> is greater than the available space from index to the end of the destination array.");
            }
            CopyToImpl(array, arrayIndex);
        }

        public T[] ToArray()
        {
            if (_size is 0)
            {
                return [];
            }
            var array = new T[_size];
            CopyToImpl(array, 0);
            return array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Advance(ref int field) => field = (field + 1) % _items.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetPreviousIndex(int current) => (current is 0 ? _items.Length : current) - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ClearItemAt(int index)
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                _items[index] = default!;
            }
        }

        private static T ThrowForEmpty() => throw new InvalidOperationException("The queue is empty.");

        public Enumerator GetEnumerator() => new(this);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<T>
        {
            private readonly DoubleEndedQueue<T> _source;
            private int _index;
            private T _current;

            internal Enumerator(DoubleEndedQueue<T> source)
            {
                _source = source;
                _index = source._head;
                _current = default!;
            }

            public readonly T Current => _current;
            readonly object IEnumerator.Current => Current!;

            public bool MoveNext()
            {
                if (_index == _source._tail)
                {
                    _current = default!;
                    return false;
                }
                _current = _source._items[_index];
                _index = (_index + 1) % _source._items.Length;
                return true;
            }

            public void Reset()
            {
                _index = _source._head;
                _current = default!;
            }

            public readonly void Dispose() { }
        }

        public static void Test(List<TestClass> list, int value)
        {
            var index = list.FindIndex(item => item.Value == value);
            Console.WriteLine(index);
        }

        public class TestClass
        {
            public int Value { get; set; }
        }
    }
}
