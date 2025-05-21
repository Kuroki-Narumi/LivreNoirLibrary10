using System;
using System.Collections.Generic;
using System.Threading;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Collections
{
    public class BacketQueue<TIn, TOut>(int capacity, bool autoResize = false) : DisposableBase
        where TIn : allows ref struct
        where TOut : IBacket<TIn, TOut>
    {
        protected override void DisposeUnmanaged()
        {
            if (typeof(TOut).IsAssignableTo(typeof(IDisposable)))
            {
                foreach (var item in _array)
                {
                    (item as IDisposable)?.Dispose();
                }
            }
            base.DisposeUnmanaged();
        }

        private TOut[] _array = new TOut[Math.Max(capacity, 2)];
        protected readonly ReaderWriterLockSlim _lock = new();

        private int _head;       // The index from which to dequeue if the queue isn't empty.
        private int _tail;       // The index at which to enqueue if the queue isn't full.
        private int _size;       // Number of elements.

        public int Capacity => _array.Length;
        public bool AutoResize { get; set; } = autoResize;

        public int Count => _size;
        public bool IsEmpty() => _size is <= 0;
        public bool IsFull() => _size >= _array.Length;

        public void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                foreach (var item in _array)
                {
                    item?.ClearData();
                }
                _head = 0;
                _tail = 0;
                _size = 0;
                OnClear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        protected virtual void OnClear() { }

        public bool SetCapacity(int size)
        {
            var current = _array.Length;
            if (size <= current)
            {
                return false;
            }
            while (current < size)
            {
                current *= 2;
            }
            _lock.EnterWriteLock();
            try
            {
                var newArray = new TOut[current];
                var array = _array;
                if (_head < _tail)
                {
                    Array.Copy(array, _head, newArray, 0, _size);
                }
                else
                {
                    var rem = array.Length - _head;
                    Array.Copy(array, _head, newArray, 0, rem);
                    Array.Copy(array, 0, newArray, rem, _tail);
                }
                _array = newArray;
                _head = 0;
                _tail = _size;
                OnCapacityChanged(newArray);
                return true;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        protected virtual void OnCapacityChanged(TOut[] newArray) { }

        /// <returns>
        /// <see cref="bool">true</see> if item discarded.
        /// </returns>
        public bool Enqueue(in TIn item)
        {
            _lock.EnterWriteLock();
            try
            {
                if (AutoResize)
                {
                    SetCapacity(_size + 1);
                }
                var buffer = _array;
                var tail = _tail;
                if (buffer[tail] is TOut current)
                {
                    current.SetData(item);
                }
                else
                {
                    buffer[tail] = TOut.Create(item);
                }
                _tail = (tail + 1) % buffer.Length;
                var discarded = _size == buffer.Length;
                if (discarded)
                {
                    Console.WriteLine($"***Backet Discarded***");
                    _head = (_head + 1) % buffer.Length;
                }
                else
                {
                    _size++;
                }
                OnEnqueue(discarded, buffer[_head]);
                return discarded;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        protected virtual void OnEnqueue(bool discarded, TOut? next) { }

        public TOut Dequeue()
        {
            if (_size is <= 0)
            {
                ThrowForEmptyQueue();
            }
            return DequeueCore();
        }

        public bool TryDequeue([MaybeNullWhen(false)]out TOut result)
        {
            if (_size is <= 0)
            {
                result = default;
                return false;
            }
            result = DequeueCore();
            return true;
        }

        private TOut DequeueCore()
        {
            _lock.EnterWriteLock();
            try
            {
                var buffer = _array;
                var head = _head;
                var item = buffer[head];
                _head = (head + 1) % buffer.Length;
                _size--;
                OnDequeue(item, buffer[head]);
                return item;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        protected virtual void OnDequeue(TOut removed, TOut? next) { }

        public TOut Peek()
        {
            _lock.EnterReadLock();
            try
            {
                if (_size is <= 0)
                {
                    ThrowForEmptyQueue();
                }
                return _array[_head];
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public bool TryPeek([MaybeNullWhen(false)]out TOut result)
        {
            _lock.EnterReadLock();
            try
            {
                if (_size is <= 0)
                {
                    result = default;
                    return false;
                }
                result = _array[_head];
                return true;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        private static void ThrowForEmptyQueue() => throw new InvalidOperationException("Queue is empty.");
    }
}
