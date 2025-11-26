using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public unsafe class UnmanagedArray<T> : DisposableBase, IEnumerable<T>, IClear
        where T : unmanaged
    {
        /// <summary>
        /// Gets a value that indicates whether <see cref="Vector{T}"/> is supported.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if <see cref="Vector{T}"/> is supported; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsHardwareAccelerated { get; } = Vector<T>.IsSupported;

        private T* _ptr;
        private int _size;

        /// <summary>
        /// Gets the pointer to the first element of the allocated memory.
        /// </summary>
        public T* Pointer => _ptr;

        /// <summary>
        /// Gets the number of elements contained in this <see cref="UnmanagedArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// This value matches the size in bytes in memory only if <typeparamref name="T"/> is <see cref="byte"/>; otherwise, the actual size in memory is this value multiplied by <see langword="sizeof"/>(<typeparamref name="T"/>).
        /// </remarks>
        public int Length => _size;

        /// <summary>
        /// Gets a reference to the element of specified index.
        /// </summary>
        /// <param name="index">index to get reference.</param>
        /// <returns>a reference to the element of specified index.</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public ref T this[int index]
        {
            get
            {
                if ((uint)index < (uint)_size)
                {
                    return ref _ptr[index];
                }
                throw new IndexOutOfRangeException($"Index out of range (given:{index}, size:{_size})");
            }
        }

        /// <inheritdoc cref="this[int]"/>
        public ref T this[Index index] => ref this[index.GetOffset(_size)];

        /// <inheritdoc cref="Slice(Range)"/>
        public Span<T> this[Range range] => Slice(range);

        /// <summary>
        /// Create an instance of <see cref="UnmanagedArray{T}"/> with the specified size.
        /// </summary>
        /// <param name="size">number of elements.</param>
        public UnmanagedArray(int size = 0) => Realloc(size);

        /// <summary>
        /// Create an instance of <see cref="UnmanagedArray{T}"/> from an existing buffer.
        /// </summary>
        /// <param name="source">the buffer to be copied into as the initial state of the instance.</param>
        public UnmanagedArray(ReadOnlySpan<T> source)
        {
            Realloc(source.Length);
            CopyFrom(source);
        }

        /// <summary>
        /// Frees the allocated memory and sets its <see cref="Length"/> to 0.
        /// </summary>
        public void Free()
        {
            NativeMemory.Free(_ptr);
            _ptr = null;
            _size = 0;
        }

        private void ReallocCore(int size, bool clear)
        {
            var newPtr = (T*)NativeMemory.Realloc(_ptr, (nuint)(size * sizeof(T)));
            if (clear && size > _size)
            {
                ClearCore(newPtr + _size, size - _size);
            }
            _ptr = newPtr;
            _size = size;
        }

        /// <summary>
        /// Reallocates memory to be the specified size.
        /// </summary>
        /// <param name="size">the new number of elements.</param>
        /// <param name="clear">if <see langword="true"/>, when reallocating a larger area, the enlarged area will be set to 0; 
        /// if <see langword="false"/>, the enlarged area will remain as is.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="OutOfMemoryException"/>
        public void Realloc(int size, bool clear = true)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(size, 0);
            if (size is 0)
            {
                Free();
            }
            else
            {
                ReallocCore(size, clear);
            }
        }

        /// <summary>
        /// Reallocates memory so that its size is a power of 2 greater than or equal to the specified size.
        /// </summary>
        /// <param name="requiredSize">the minimum number of elements.</param>
        /// <param name="clear">if <see langword="true"/>, when reallocating a larger area, the enlarged area will be set to 0; 
        /// if <see langword="false"/>, the enlarged area will remain as is.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="OutOfMemoryException"/>
        public void ReallocToPowerOf2(int requiredSize, bool clear = true)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(requiredSize, 0);
            ReallocCore((int)BitOperations.RoundUpToPowerOf2((uint)requiredSize), clear);
        }

        /// <summary>
        /// If current <see cref="Length"/> is less than the specified size, reallocates to be at least the specified size.
        /// </summary>
        /// <param name="size">the minimum number of elements.</param>
        /// <param name="clear">if <see langword="true"/>, the enlarged/excess area will be set to 0; 
        /// if <see langword="false"/>, the enlarged/excess area will remain as is.</param>
        /// <returns>
        /// <see langword="true"/> if reallocated; otherwise, <see langword="false"/>.
        /// </returns>
        public bool EnsureSize(int size, bool clear = true)
        {
            if (size > _size)
            {
                ReallocToPowerOf2(size, clear);
                return true;
            }
            else if (clear)
            {
                Clear(size);
            }
            return false;
        }

        /// <summary>
        /// Creates a new span over this <see cref="UnmanagedArray{T}"/>.
        /// </summary>
        public Span<T> AsSpan() => _ptr is null ? [] : new(_ptr, _size);

        private Span<T> ThrowOutOfRange(int start)
        {
            throw new ArgumentOutOfRangeException($"index must be >= 0 and < {_size}. (given:{start})");
        }

        private Span<T> ThrowOutOfRange(int start, int length)
        {
            if (start is < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start), $"index must be >= 0. (given:{start})");
            }
            throw new ArgumentOutOfRangeException(nameof(length), $"count must be < {_size - start}. (given:{length})");
        }

        /// <summary>
        /// Forms a slice out of this <see cref="UnmanagedArray{T}"/>.
        /// </summary>
        /// <param name="start">The index at which to begin the Span.</param>
        /// <param name="length">The number of elements in the Span.</param>
        /// <returns>The span representation of this <see cref="UnmanagedArray{T}"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public Span<T> Slice(int start, int length)
        {
            if (start is >= 0 && start + length <= _size)
            {
                return new(_ptr + start, length);
            }
            else
            {
                return ThrowOutOfRange(start, length);
            }
        }

        /// <inheritdoc cref="Slice(int, int)"/>
        public Span<T> Slice(int start)
        {
            if ((uint)start <= (uint)_size)
            {
                return new(_ptr + start, _size - start);
            }
            else
            {
                return ThrowOutOfRange(start);
            }
        }

        /// <inheritdoc cref="Slice(int, int)"/>
        public Span<T> Slice(Index start) => Slice(start.GetOffset(_size));

        /// <inheritdoc cref="Slice(int, int)"/>
        /// <param name="range">The range of slice.</param>
        public Span<T> Slice(Range range)
        {
            var (offset, length) = range.GetOffsetAndLength(_size);
            return Slice(offset, length);
        }

        public static implicit operator Span<T>(UnmanagedArray<T> obj) => obj.AsSpan();
        public static implicit operator ReadOnlySpan<T>(UnmanagedArray<T> obj) => obj.AsSpan();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ClearCore(T* ptr, int count)
        {
            if (IsHardwareAccelerated)
            {
                SimdOperations.Clear(ptr, count);
            }
            else
            {
                NativeMemory.Clear(ptr, (nuint)(count * sizeof(T)));
            }
        }

        /// <summary>
        /// Sets all elements of this <see cref="UnmanagedArray{T}"/> to 0.
        /// </summary>
        public void Clear() => ClearCore(_ptr, _size);

        /// <summary>
        /// Sets elements of this <see cref="UnmanagedArray{T}"/> in the specified range to 0.
        /// </summary>
        /// <param name="start">The index to start clearing from.</param>
        /// <param name="length">The length to clear.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public void Clear(int start, int length)
        {
            if (start is >= 0 && start + length <= _size)
            {
                ClearCore(_ptr + start, length);
            }
            else
            {
                ThrowOutOfRange(start, length);
            }
        }

        /// <inheritdoc cref="Clear(int, int)"/>
        public void Clear(int start)
        {
            if ((uint)start < (uint)_size)
            {
                ClearCore(_ptr + start, _size - start);
            }
            else if (start != _size)
            {
                ThrowOutOfRange(start);
            }
        }

        /// <inheritdoc cref="Clear(int, int)"/>
        public void Clear(Index index) => Clear(index.GetOffset(_size));

        /// <inheritdoc cref="Clear(int, int)"/>
        /// <param name="range">The range to clear.</param>
        public void Clear(Range range)
        {
            var (offset, length) = range.GetOffsetAndLength(_size);
            Clear(offset, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FillCore(T value, T* ptr, int count)
        {
            if (IsHardwareAccelerated)
            {
                SimdOperations.CopyFrom(ptr, value, count);
            }
            else
            {
                new Span<T>(ptr, count).Fill(value);
            }
        }

        /// <summary>
        /// Sets all elements of this <see cref="UnmanagedArray{T}"/> to the specified value.
        /// </summary>
        /// <param name="value">The value to fill.</param>
        public void Fill(T value) => FillCore(value, _ptr, _size);

        /// <summary>
        /// Sets elements of this <see cref="UnmanagedArray{T}"/> in the specified range to the specified value.
        /// </summary>
        /// <param name="value">The value to fill.</param>
        /// <param name="start">The index to start filling from.</param>
        /// <param name="length">The length to fill.</param>
        public void Fill(T value, int start, int length)
        {
            if (start is >= 0 && start + length <= _size)
            {
                FillCore(value, _ptr + start, length);
            }
            else
            {
                ThrowOutOfRange(start, length);
            }
        }

        /// <inheritdoc cref="Fill(T, int, int)"/>
        public void Fill(T value, int index)
        {
            if ((uint)index < (uint)_size)
            {
                FillCore(value, _ptr + index, _size - index);
            }
            else if (index != _size)
            {
                ThrowOutOfRange(index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CopyFromCore(T* ptr, T* src, int length)
        {
            if (IsHardwareAccelerated)
            {
                SimdOperations.CopyFrom(ptr, src, length);
            }
            else
            {
                new Span<T>(src, length).CopyTo(new Span<T>(ptr, length));
            }
        }

        /// <summary>
        /// Copies the values of the specified source into this <see cref="UnmanagedArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// If the source is longer than this <see cref="UnmanagedArray{T}"/>, the excess elements are not copied.
        /// </remarks>
        /// <param name="source">The source of values.</param>
        public void CopyFrom(ReadOnlySpan<T> source)
        {
            fixed (T* src = source)
            {
                CopyFromCore(_ptr, src, Math.Min(_size, source.Length));
            }
        }

        /// <inheritdoc cref="CopyFrom(ReadOnlySpan{T})"/>
        public void CopyFrom(T[] source)
        {
            fixed (T* src = source)
            {
                CopyFromCore(_ptr, src, Math.Min(_size, source.Length));
            }
        }

        /// <inheritdoc cref="CopyFrom(ReadOnlySpan{T})"/>
        public void CopyFrom(List<T> source)
        {
            var span = source.AsSpan();
            fixed (T* src = span)
            {
                CopyFromCore(_ptr, src, Math.Min(_size, source.Count));
            }
        }

        /// <summary>
        /// Copies the values of the specified source into this <see cref="UnmanagedArray{T}"/>, starting at the specified position.
        /// </summary>
        /// <remarks>
        /// If the source is longer than the rest of this <see cref="UnmanagedArray{T}"/>, the excess elements are not copied.
        /// </remarks>
        /// <param name="source">The source of values.</param>
        /// <param name="start">The starting index of the destination.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public void CopyFrom(ReadOnlySpan<T> source, int start)
        {
            if ((uint)start < (uint)_size)
            {
                fixed (T* src = source)
                {
                    CopyFromCore(_ptr + start, src, Math.Min(_size - start, source.Length));
                }
            }
            else if (start != _size)
            {
                ThrowOutOfRange(start);
            }
        }

        /// <inheritdoc cref="CopyFrom(ReadOnlySpan{T}, int)"/>
        public void CopyFrom(T[] source, int start)
        {
            if ((uint)start < (uint)_size)
            {
                fixed (T* src = source)
                {
                    CopyFromCore(_ptr + start, src, Math.Min(_size - start, source.Length));
                }
            }
            else if (start != _size)
            {
                ThrowOutOfRange(start);
            }
        }

        /// <inheritdoc cref="CopyFrom(ReadOnlySpan{T}, int)"/>
        public void CopyFrom(List<T> source, int start)
        {
            if ((uint)start < (uint)_size)
            {
                var span = source.AsSpan();
                fixed (T* src = span)
                {
                    CopyFromCore(_ptr + start, src, Math.Min(_size - start, source.Count));
                }
            }
            else if (start != _size)
            {
                ThrowOutOfRange(start);
            }
        }

        protected override void DisposeUnmanaged()
        {
            Free();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public Enumerator GetEnumerator() => new(this);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<T>
        {
            private readonly T* _ptr;
            private readonly int _size;
            private int _index;
            private T _current;

            public readonly T Current => _current;
            readonly object IEnumerator.Current => Current;

            internal Enumerator(UnmanagedArray<T> array)
            {
                _ptr = array._ptr;
                _size = array._size;
            }

            public bool MoveNext()
            {
                if (_index < _size)
                {
                    _current = _ptr[_index];
                    _index++;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _index = 0;
                _current = default;
            }

            public readonly void Dispose() { }
        }
    }
}
