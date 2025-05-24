using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Collections
{
    public class SharedBuffer<T> : DisposableBase, IList<T>
    {
        public const int DefaultCapacity = 32;
        protected static readonly bool IsRefType = RuntimeHelpers.IsReferenceOrContainsReferences<T>();

        protected T[] _array;
        protected int _size;
        protected readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.SupportsRecursion);

        public int Capacity => _array.Length;
        public int Count => _size;

        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)_size)
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
                if ((uint)index >= (uint)_size)
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
            get => this[index.GetOffset(_size)];
            set => this[index.GetOffset(_size)] = value;
        }

        public SharedBuffer(int capacity = DefaultCapacity)
        {
            _array = capacity is > 0 ? ArrayPool<T>.Shared.Rent(capacity) : [];
            // 借りてきた配列に入っているゴミを削除
            if (IsRefType)
            {
                Array.Clear(_array);
            }
        }

        protected override void DisposeUnmanaged()
        {
            ArrayPool<T>.Shared.Return(_array, IsRefType);
            base.DisposeUnmanaged();
        }

        protected virtual void ProcessCopy(ReadOnlySpan<T> source, Span<T> destination)
        {
            source.CopyTo(destination);
        }

        public void Clear() => SetSize(0);

        public void SetSize(int size)
        {
            _lock.EnterWriteLock();
            try
            {
                var oldLength = _array.Length;
                if (size > oldLength)
                {
                    var newArray = ArrayPool<T>.Shared.Rent(size);
                    if (IsRefType)
                    {
                        Array.Clear(newArray, oldLength, newArray.Length - oldLength);
                    }
                    ProcessCopy(_array, newArray);
                    ArrayPool<T>.Shared.Return(_array, IsRefType);
                    _array = newArray;
                }
                else if (size < _size && IsRefType)
                {
                    Array.Clear(_array, size, _size - size);
                }
            }
            finally
            {
                _size = size;
                _lock.ExitWriteLock();
            }
        }

        public void EnsureSize(int size)
        {
            if (_size < size)
            {
                SetSize(size);
            }
        }

        public void SetData(ReadOnlySpan<T> source)
        {
            SetSize(source.Length);
            ProcessCopy(source, _array);
        }

        public void Append(ReadOnlySpan<T> source)
        {
            var size = _size;
            SetSize(_size + source.Length);
            ProcessCopy(source, _array.AsSpan(size));
        }

        public Span<T> AsSpan() => _array.AsSpan(0, _size);
        public Span<T> AsSpan(int offset) => _array.AsSpan(offset, _size - offset);
        public Span<T> AsSpan(int offset, int length) => _array.AsSpan(offset, length);
        public static implicit operator Span<T>(SharedBuffer<T> buffer) => buffer.AsSpan();
        public static implicit operator ReadOnlySpan<T>(SharedBuffer<T> buffer) => buffer.AsSpan();

        public Enumerator GetEnumerator() => new(this);

        #region Interface Methods
        public int IndexOf(T item) => Array.IndexOf(_array, item, 0, _size);
        public bool Contains(T item) => IndexOf(item) is >= 0;

        public void Add(T item)
        {
            var size = _size;
            SetSize(_size + 1);
            _array[size] = item;
        }

        public void Insert(int index, T item)
        {
            if ((uint)index < (uint)_size)
            {
                SetSize(_size + 1);
                Array.Copy(_array, index, _array, index + 1, _size - index - 1);
                _array[index] = item;
            }
            else if (index == _size)
            {
                Add(item);
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index is >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if ((uint)index < (uint)_size)
            {
                SetSize(_size - 1);
                if (index < _size)
                {
                    Array.Copy(_array, index + 1, _array, index, _size - index);
                }
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                {
                    _array[_size] = default!;
                }
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        bool ICollection<T>.IsReadOnly => false;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void CopyTo(T[] array, int arrayIndex) => ProcessCopy(AsSpan(), array.AsSpan(arrayIndex));

        #endregion

        public struct Enumerator : IEnumerator<T>
        {
            private readonly SharedBuffer<T> _source;
            private readonly int _size;
            private int _index;

            internal Enumerator(SharedBuffer<T> source)
            {
                _source = source;
                _size = source._size;
                _index = -1;
            }

            public readonly T Current => _source._array[_index];
            readonly object? IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (++_index < _size)
                {
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _index = -1;
            }

            public readonly void Dispose() { }
        }
    }
}
