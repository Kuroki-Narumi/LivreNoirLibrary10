using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public unsafe class UnmanagedArray<T> : DisposableBase, IClear
        where T : unmanaged
    {
        /// <summary>
        /// Gets a value that indicates whether <see cref="Vector{T}"/> is supported.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if <see cref="Vector{T}"/> is supported; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsHardwareAccelerated { get; } = Vector<T>.IsSupported;

        /// <summary>
        /// Gets the maximum allowable length for instances of <see cref="UnmanagedArray{T}"/>, 
        /// calculated as the largest value that can be safely represented based on <see langword="sizeof"/>(<typeparamref name="T"/>).
        /// </summary>
        public static nuint MaxLength { get; } = nuint.MaxValue / (nuint)sizeof(T);

        private T* _ptr;
        private nuint _size;

        /// <summary>
        /// Create an instance of <see cref="UnmanagedArray{T}"/> without allocation.
        /// </summary>
        public UnmanagedArray() { }

        /// <summary>
        /// Create an instance of <see cref="UnmanagedArray{T}"/> with the specified size allocated.
        /// </summary>
        /// <param name="length">number of elements.</param>
        public UnmanagedArray(nuint length) => Realloc(length);

        ///<inheritdoc cref="UnmanagedArray(nuint)"/>
        public UnmanagedArray(long length) => Realloc(length);

        ///<inheritdoc cref="UnmanagedArray(nuint)"/>
        public UnmanagedArray(int length) => Realloc(length);

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
        /// Gets the pointer to the first element of the allocated memory.
        /// </summary>
        public T* Pointer => _ptr;

        /// <summary>
        /// Gets the number of elements contained in this <see cref="UnmanagedArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// This value matches the size in bytes in memory only if <typeparamref name="T"/> is <see cref="byte"/>; 
        /// otherwise, the actual size in memory is this value multiplied by <see langword="sizeof"/>(<typeparamref name="T"/>).
        /// </remarks>
        public nuint Length => _size;

        /// <summary>
        /// Gets a reference to the element of specified index.
        /// </summary>
        /// <param name="index">index to get reference.</param>
        /// <returns>a reference to the element of specified index.</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public ref T this[nuint index] => ref GetRefByIndex(index);

        /// <inheritdoc cref="this[nuint]"/>
        public ref T this[int index]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfNegative(index);
                return ref GetRefByIndex((nuint)index);
            }
        }

        /// <inheritdoc cref="this[nuint]"/>
        public ref T this[long index]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfNegative(index);
                return ref GetRefByIndex((nuint)index);
            }
        }

        /// <inheritdoc cref="this[nuint]"/>
        public ref T this[Index index]
        {
            get
            {
                var actualIndex = ConvertIndexSafe(index);
                return ref _ptr[actualIndex];
            }
        }

        /// <inheritdoc cref="Slice(Range)"/>
        public Span<T> this[Range range] => Slice(range);

        /// <summary>
        /// Frees the allocated memory and sets its <see cref="Length"/> to 0.
        /// </summary>
        public void Free()
        {
            NativeMemory.Free(_ptr);
            _ptr = null;
            _size = 0;
        }

        /// <summary>
        /// Reallocates memory to be the specified size.
        /// </summary>
        /// <param name="newCount">the new number of elements.</param>
        /// <param name="clear">if <see langword="true"/>, when reallocating a larger area, the enlarged area will be set to 0; 
        /// if <see langword="false"/>, the enlarged area will remain as is.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="OutOfMemoryException"/>
        public void Realloc(nuint newCount, bool clear = true) => ReallocImpl(newCount, clear);

        /// <inheritdoc cref="Realloc(nuint, bool)"/>
        public void Realloc(int newCount, bool clear = true)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(newCount);
            ReallocImpl((nuint)newCount, clear);
        }

        /// <inheritdoc cref="Realloc(nuint, bool)"/>
        public void Realloc(long newCount, bool clear = true)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(newCount);
            ReallocImpl((nuint)newCount, clear);
        }

        /// <summary>
        /// Reallocates memory so that its size is a power of 2 greater than or equal to the specified size.
        /// </summary>
        /// <param name="requiredCount">the minimum number of elements.</param>
        /// <param name="clear">if <see langword="true"/>, when reallocating a larger area, the enlarged area will be set to 0; 
        /// if <see langword="false"/>, the enlarged area will remain as is.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="OutOfMemoryException"/>
        public void ReallocToPowerOf2(nuint requiredCount, bool clear = true)
        {
            nuint size;
            if (sizeof(nuint) == sizeof(ulong))
            {
                var r = (ulong)requiredCount;
                size = r is > 0x8000_0000_0000_0000 ? nuint.MaxValue : (nuint)BitOperations.RoundUpToPowerOf2(r);
            }
            else
            {
                var r = (uint)requiredCount;
                size = r is > 0x8000_0000 ? nuint.MaxValue : BitOperations.RoundUpToPowerOf2(r);
            }
            Realloc(size, clear);
        }

        /// <inheritdoc cref="ReallocToPowerOf2(nuint, bool)"/>
        public void ReallocToPowerOf2(int requiredCount, bool clear = true)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(requiredCount);
            ReallocToPowerOf2((nuint)requiredCount, clear);
        }

        /// <inheritdoc cref="ReallocToPowerOf2(nuint, bool)"/>
        public void ReallocToPowerOf2(long requiredCount, bool clear = true)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(requiredCount);
            ReallocToPowerOf2((nuint)requiredCount, clear);
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
        public bool EnsureSize(nuint size, bool clear = true)
        {
            if (size > _size)
            {
                ReallocToPowerOf2(size, clear);
                return true;
            }
            else if (size < _size && clear)
            {
                Clear(size);
            }
            return false;
        }

        /// <inheritdoc cref="EnsureSize(nuint, bool)"/>
        public bool EnsureSize(int size, bool clear = true)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(size);
            return EnsureSize((nuint)size, clear);
        }

        /// <inheritdoc cref="EnsureSize(nuint, bool)"/>
        public bool EnsureSize(long size, bool clear = true)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(size);
            return EnsureSize((nuint)size, clear);
        }

        /// <summary>
        /// Creates a new span over this <see cref="UnmanagedArray{T}"/>.
        /// </summary>
        /// <exception cref="OverflowException"/>
        public Span<T> AsSpan()
        {
            if (_ptr is null)
            {
                return [];
            }
            var length = checked((int)_size);
            return new(_ptr, length);
        }

        /// <summary>
        /// Forms a slice out of this <see cref="UnmanagedArray{T}"/>.
        /// </summary>
        /// <param name="start">The index at which to begin the Span.</param>
        /// <param name="count">The number of elements in the Span.</param>
        /// <returns>The span representation of this <see cref="UnmanagedArray{T}"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="OverflowException"/>
        public Span<T> Slice(nuint start, nuint count)
        {
            ThrowIfIndexOutOfRange(start, nameof(start));
            ThrowIfCountOutOfRange(start, count);
            var length = checked((int)count);
            return new(_ptr + start, length);
        }

        /// <inheritdoc cref="Slice(nuint, nuint)"/>
        public Span<T> Slice(int start, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(start);
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            return Slice((nuint)start, (nuint)count);
        }

        /// <inheritdoc cref="Slice(nuint, nuint)"/>
        public Span<T> Slice(long start, long count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(start);
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            return Slice((nuint)start, (nuint)count);
        }

        /// <inheritdoc cref="Slice(nuint, nuint)"/>
        public Span<T> Slice(nuint start)
        {
            ThrowIfIndexOutOfRange(start, nameof(start));
            var length = checked((int)_size);
            return new(_ptr + start, length);
        }

        /// <inheritdoc cref="Slice(nuint)"/>
        public Span<T> Slice(int start)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(start);
            return Slice((nuint)start);
        }

        /// <inheritdoc cref="Slice(nuint)"/>
        public Span<T> Slice(long start)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(start);
            return Slice((nuint)start);
        }

        /// <inheritdoc cref="Slice(nuint)"/>
        public Span<T> Slice(Index start)
        {
            var index = ConvertIndexSafe(start);
            var length = checked((int)(_size - index));
            return new(_ptr + index, length);
        }

        /// <inheritdoc cref="Slice(nuint, nuint)"/>
        /// <param name="range">The range of slice.</param>
        public Span<T> Slice(Range range)
        {
            var (start, length) = ConvertRangeSafe(range);
            return new(_ptr + start, checked((int)length));
        }

        public static implicit operator Span<T>(UnmanagedArray<T> obj) => obj.AsSpan();
        public static implicit operator ReadOnlySpan<T>(UnmanagedArray<T> obj) => obj.AsSpan();

        /// <summary>
        /// Sets all elements of this <see cref="UnmanagedArray{T}"/> to 0.
        /// </summary>
        public void Clear() => ClearImpl(_ptr, _size);

        /// <summary>
        /// Sets elements of this <see cref="UnmanagedArray{T}"/> in the specified range to 0.
        /// </summary>
        /// <param name="start">The index to start clearing from.</param>
        /// <param name="count">The elements count to clear.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public void Clear(nuint start, nuint count)
        {
            ThrowIfIndexOutOfRange(start, nameof(start));
            ThrowIfCountOutOfRange(start, count);
            ClearImpl(_ptr + start, count);
        }

        /// <inheritdoc cref="Clear(nuint, nuint)"/>
        public void Clear(int start, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(start);
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            Clear((nuint)start, (nuint)count);
        }

        /// <inheritdoc cref="Clear(nuint, nuint)"/>
        public void Clear(long start, long count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(start);
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            Clear((nuint)start, (nuint)count);
        }

        /// <inheritdoc cref="Clear(nuint, nuint)"/>
        public void Clear(nuint start)
        {
            ThrowIfIndexOutOfRange(start, nameof(start));
            ClearImpl(_ptr + start, _size - start);
        }

        /// <inheritdoc cref="Clear(nuint)"/>
        public void Clear(int start)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(start);
            Clear((nuint)start);
        }

        /// <inheritdoc cref="Clear(nuint)"/>
        public void Clear(long start)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(start);
            Clear((nuint)start);
        }

        /// <inheritdoc cref="Clear(int, int)"/>
        public void Clear(Index index) => Clear(ConvertIndexSafe(index));

        /// <inheritdoc cref="Clear(int, int)"/>
        /// <param name="range">The range to clear.</param>
        public void Clear(Range range)
        {
            var (start, length) = ConvertRangeSafe(range);
            Clear(start, length);
        }

        /// <summary>
        /// Sets all elements of this <see cref="UnmanagedArray{T}"/> to the specified value.
        /// </summary>
        /// <param name="value">The value to fill.</param>
        public void Fill(T value) => FillImpl(value, _ptr, _size);

        /// <summary>
        /// Sets elements of this <see cref="UnmanagedArray{T}"/> in the specified range to the specified value.
        /// </summary>
        /// <param name="value">The value to fill.</param>
        /// <param name="start">The index to start filling from.</param>
        /// <param name="count">The number of elements to fill.</param>
        public void Fill(T value, nuint start, nuint count)
        {
            ThrowIfIndexOutOfRange(start, nameof(start));
            ThrowIfCountOutOfRange(start, count);
            FillImpl(value, _ptr + start, count);
        }

        /// <inheritdoc cref="Fill(T, nuint, nuint)"/>
        public void Fill(T value, int start, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(start);
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            Fill(value, (nuint)start, (nuint)count);
        }

        /// <inheritdoc cref="Fill(T, nuint, nuint)"/>
        public void Fill(T value, long start, long count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(start);
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            Fill(value, (nuint)start, (nuint)count);
        }

        /// <inheritdoc cref="Fill(T, nuint, nuint)"/>
        public void Fill(T value, nuint start)
        {
            ThrowIfIndexOutOfRange(start, nameof(start));
            FillImpl(value, _ptr + start, _size - start);
        }

        /// <inheritdoc cref="Fill(T, nuint)"/>
        public void Fill(T value, int start)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(start);
            Fill(value, (nuint)start);
        }

        /// <inheritdoc cref="Fill(T, nuint)"/>
        public void Fill(T value, long start)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(start);
            Fill(value, (nuint)start);
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
                CopyFromImpl(_ptr, src, Math.Min(_size, (nuint)source.Length));
            }
        }

        /// <inheritdoc cref="CopyFrom(ReadOnlySpan{T})"/>
        public void CopyFrom(T[] source)
        {
            fixed (T* src = source)
            {
                CopyFromImpl(_ptr, src, Math.Min(_size, (nuint)source.Length));
            }
        }

        /// <inheritdoc cref="CopyFrom(ReadOnlySpan{T})"/>
        public void CopyFrom(List<T> source)
        {
            var span = source.AsSpan();
            fixed (T* src = span)
            {
                CopyFromImpl(_ptr, src, Math.Min(_size, (nuint)source.Count));
            }
        }

        /// <summary>
        /// Copies the values of the specified source into this <see cref="UnmanagedArray{T}"/>, starting at the specified position.
        /// </summary>
        /// <remarks>
        /// If the source is longer than the rest of this <see cref="UnmanagedArray{T}"/>, the excess elements are not copied.
        /// </remarks>
        /// <param name="source">The source of values.</param>
        /// <param name="offset">The starting index of the destination.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public void CopyFrom(ReadOnlySpan<T> source, nuint offset)
        {
            ThrowIfIndexOutOfRange(offset, nameof(offset));
            fixed (T* src = source)
            {
                CopyFromImpl(_ptr + offset, src, Math.Min(_size - offset, (nuint)source.Length));
            }
        }

        /// <inheritdoc cref="CopyFrom(ReadOnlySpan{T}, nuint)"/>
        public void CopyFrom(ReadOnlySpan<T> source, int offset)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(offset);
            CopyFrom(source, (nuint)offset);
        }

        /// <inheritdoc cref="CopyFrom(ReadOnlySpan{T}, nuint)"/>
        public void CopyFrom(ReadOnlySpan<T> source, long offset)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(offset);
            CopyFrom(source, (nuint)offset);
        }

        protected override void DisposeUnmanaged()
        {
            Free();
        }

        private ref T GetRefByIndex(nuint index)
        {
            ThrowIfIndexOutOfRange(index);
            return ref _ptr[index];
        }

        private void ThrowIfIndexOutOfRange(nuint index, string indexName = "index")
        {
            if (index >= _size)
            {
                throw new IndexOutOfRangeException($"{indexName} must be < {_size} (given:{index})");
            }
        }

        private void ThrowIfCountOutOfRange(nuint index, nuint count, string countName = "count")
        {
            if (index + count > _size)
            {
                throw new IndexOutOfRangeException($"{countName} must be <= {_size - index} (given:{count})");
            }
        }

        private nuint ConvertIndexSafe(Index index)
        {
            var actualIndex = (nuint)index.Value;
            if (index.IsFromEnd)
            {
                if (actualIndex > _size)
                {
                    throw new IndexOutOfRangeException($"index must be >= ^{_size} (given:{index})");
                }
                actualIndex = _size - actualIndex;
            }
            else
            {
                ThrowIfIndexOutOfRange(actualIndex);
            }
            return actualIndex;
        }

        private (nuint, nuint) ConvertRangeSafe(Range range)
        {
            var start = ConvertIndexSafe(range.Start);
            var end = ConvertIndexSafe(range.End);
            return (start, end - start);
        }

        private void ReallocImpl(nuint count, bool clear)
        {
            if (count is 0)
            {
                Free();
                return;
            }
            if (count > MaxLength)
            {
                throw new ArgumentOutOfRangeException($"count must be <= {MaxLength} (given:{count})");
            }
            var newPtr = (T*)NativeMemory.Realloc(_ptr, count * (nuint)sizeof(T));
            if (clear && count > _size)
            {
                ClearImpl(newPtr + _size, count - _size);
            }
            _ptr = newPtr;
            _size = count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ClearImpl(T* ptr, nuint count)
        {
            if (IsHardwareAccelerated)
            {
                SimdOperations.Clear(ptr, count);
            }
            else
            {
                NativeMemory.Clear(ptr, count * (nuint)sizeof(T));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FillImpl(T value, T* ptr, nuint count)
        {
            if (IsHardwareAccelerated)
            {
                SimdOperations.CopyFrom(ptr, value, count);
            }
            else
            {
                foreach (var span in new ChunkEnumerator(ptr, count))
                {
                    span.Fill(value);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CopyFromImpl(T* ptr, T* src, nuint count)
        {
            if (IsHardwareAccelerated)
            {
                SimdOperations.CopyFrom(ptr, src, count);
            }
            else
            {
                nuint index = 0;
                foreach (var srcSpan in new ChunkEnumerator(src, count))
                {
                    var dstSpan = new Span<T>(ptr + index, srcSpan.Length);
                    srcSpan.CopyTo(dstSpan);
                    index += (nuint)srcSpan.Length;
                }
            }
        }

        public ChunkEnumerator EnumerateChunks(int chunkSize = int.MaxValue)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(chunkSize);
            return new ChunkEnumerator(_ptr, _size, (nuint)chunkSize);
        }

        public ref struct ChunkEnumerator
        {
            private readonly T* _ptr;
            private readonly nuint _size;
            private readonly nuint _chunkSize;
            private nuint _index;
            private Span<T> _current;

            public readonly Span<T> Current => _current;

            internal ChunkEnumerator(T* ptr, nuint size, nuint chunkSize = int.MaxValue)
            {
                _ptr = ptr;
                _size = size;
                _chunkSize = chunkSize;
            }

            public bool MoveNext()
            {
                if (_index < _size)
                {
                    var currentChunkSize = Math.Min(_chunkSize, _size - _index);
                    _current = new Span<T>(_ptr + _index, (int)currentChunkSize);
                    _index += currentChunkSize;
                    return true;
                }
                return false;
            }

            public readonly ChunkEnumerator GetEnumerator() => this;
        }
    }
}
