using LivreNoirLibrary.ObjectModel;
using System;
using System.Buffers;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace LivreNoirLibrary.Collections
{
    public class SharedBuffer<T> : DisposableBase
    {
        protected override void DisposeUnmanaged()
        {
            ArrayPool<T>.Shared.Return(_array);
            base.DisposeUnmanaged();
        }

        private T[] _array;
        private int _size;
        protected readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.SupportsRecursion);

        public int Capacity => _array.Length;
        public int Length => _size;

        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Capacity)
                {
                    throw new IndexOutOfRangeException();
                }
                _lock.EnterReadLock();
                try
                {
                    return _array[index];
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
            set
            {
                if ((uint)index >= (uint)Capacity)
                {
                    throw new IndexOutOfRangeException();
                }
                _lock.EnterWriteLock();
                try
                {
                    _array[index] = value;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }

        public T this[Index index]
        {
            get => this[index.GetOffset(Capacity)];
            set => this[index.GetOffset(Capacity)] = value;
        }

        public SharedBuffer(int size)
        {
            _array = ArrayPool<T>.Shared.Rent(Math.Max(size, 1));
            _size = size;
            Array.Clear(_array); // 借りてきた配列にゴミが入っている可能性を考慮
        }

        public void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                Array.Clear(_array);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void SetSize(int size, bool clear = false)
        {
            _lock.EnterWriteLock();
            try
            {
                if (size <= _array.Length)
                {
                    if (clear)
                    {
                        Array.Clear(_array, size, _size - size);
                    }
                    _size = size;
                    return;
                }
                var newArray = ArrayPool<T>.Shared.Rent(size);
                if (clear)
                {
                    Array.Clear(newArray);
                }
                else
                {
                    Array.Copy(_array, newArray, _array.Length);
                }
                ArrayPool<T>.Shared.Return(_array);
                _array = newArray;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void EnsureSize(int size, bool clear = false)
        {
            if (_size < size)
            {
                SetSize(size, clear);
            }
        }

        public Span<T> AsSpan() => _array.AsSpan(0, _size);
        public Span<T> AsSpan(int offset) => _array.AsSpan(offset, _size - offset);
        public Span<T> AsSpan(int offset, int length) => _array.AsSpan(offset, length);
        public static implicit operator Span<T>(SharedBuffer<T> buffer) => buffer.AsSpan();
        public static implicit operator ReadOnlySpan<T>(SharedBuffer<T> buffer) => buffer.AsSpan();
    }
}
