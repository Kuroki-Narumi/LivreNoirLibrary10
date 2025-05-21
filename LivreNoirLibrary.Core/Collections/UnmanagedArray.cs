using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Collections
{
    public unsafe class UnmanagedArray<T> : DisposableBase, IEnumerable<T>
        where T : unmanaged
    {
        public static readonly bool IsHardwareAccelerated = Vector<T>.IsSupported;

        private T* _ptr;
        private int _size;

        public T* Pointer => _ptr;
        public int Length => _size;

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

        public ref T this[Index index] => ref this[index.GetOffset(_size)];

        public UnmanagedArray(int size = 0) => Realloc(size);
        public UnmanagedArray(ReadOnlySpan<T> source)
        {
            Realloc(source.Length);
            CopyFrom(source);
        }

        public void Free()
        {
            NativeMemory.Free(_ptr);
            _ptr = null;
            _size = 0;
        }

        private void ReallocCore(int size, bool clear)
        {
            var newPtr = (T*)NativeMemory.Realloc(_ptr, (nuint)(size * sizeof(T)));
            if (clear)
            {
                ClearCore(newPtr + _size, size - _size);
            }
            _ptr = newPtr;
            _size = size;
        }

        public void Realloc(int size, bool clear = true)
        {
            if (size is > 0)
            {
                ReallocCore(size, clear);
                return;
            }
            else if (size is 0)
            {
                Free();
                return;
            }
            throw new ArgumentOutOfRangeException(nameof(size), $"size must be >= 0. (given:{size})");
        }

        public void ReallocToPowerOf2(int requiredSize, bool clear = true)
        {
            if (requiredSize is > 0)
            {
                ReallocCore((int)BitOperations.RoundUpToPowerOf2((uint)requiredSize), clear);
                return;
            }
            throw new ArgumentOutOfRangeException(nameof(requiredSize), $"size must be > 0. (given:{requiredSize})");
        }

        public void EnsureSize(int size, bool clear = true)
        {
            if (size > _size)
            {
                ReallocToPowerOf2(size, clear);
            }
        }

        public Span<T> AsSpan() => _ptr is null ? [] : new(_ptr, _size);
        public Span<T> AsSpan(int index) => Slice(index);
        public Span<T> AsSpan(int index, int count) => Slice(index, count);

        public static implicit operator Span<T>(UnmanagedArray<T> obj) => obj.AsSpan();
        public static implicit operator ReadOnlySpan<T>(UnmanagedArray<T> obj) => obj.AsSpan();

        private Span<T> ThrowOutOfRange(int index)
        {
            throw new IndexOutOfRangeException($"index must be >= 0 and < {_size}. (given:{index})");
        }

        private Span<T> ThrowOutOfRange(int index, int count)
        {
            if (index is < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"index must be >= 0. (given:{index})");
            }
            throw new ArgumentOutOfRangeException(nameof(count), $"count must be < {_size - index}. (given:{count})");
        }

        public Span<T> Slice(int index)
        {
            if ((uint)index <= (uint)_size)
            {
                return new(_ptr + index, _size - index);
            }
            else
            {
                return ThrowOutOfRange(index);
            }
        }

        public Span<T> Slice(int index, int count)
        {
            if (index is >= 0 && index + count <= _size)
            {
                return new(_ptr + index, count);
            }
            else
            {
                return ThrowOutOfRange(index, count);
            }
        }

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

        public void Clear() => ClearCore(_ptr, _size);

        public void Clear(int index)
        {
            if ((uint)index < (uint)_size)
            {
                ClearCore(_ptr + index, _size - index);
            }
            else if (index != _size)
            {
                ThrowOutOfRange(index);
            }
        }

        public void Clear(int index, int count)
        {
            if (index is >= 0 && index + count <= _size)
            {
                ClearCore(_ptr + index, count);
            }
            else
            {
                ThrowOutOfRange(index, count);
            }
        }

        public void Clear(Index index, int count) => Clear(index.GetOffset(_size), count);

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

        public void Fill(T value) => FillCore(value, _ptr, _size);

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

        public void Fill(T value, int index, int count)
        {
            if (index is >= 0 && index + count <= _size)
            {
                FillCore(value, _ptr + index, count);
            }
            else
            {
                ThrowOutOfRange(index, count);
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

        public void CopyFrom(List<T> source)
        {
            var span = CollectionsMarshal.AsSpan(source);
            fixed (T* src = span)
            {
                CopyFromCore(_ptr, src, Math.Min(_size, source.Count));
            }
        }

        public void CopyFrom(T[] source)
        {
            fixed (T* src = source)
            {
                CopyFromCore(_ptr, src, Math.Min(_size, source.Length));
            }
        }

        public void CopyFrom(ReadOnlySpan<T> source)
        {
            fixed (T* src = source)
            {
                CopyFromCore(_ptr, src, Math.Min(_size, source.Length));
            }
        }

        public void CopyFrom(List<T> source, int index)
        {
            if ((uint)index < (uint)_size)
            {
                var span = CollectionsMarshal.AsSpan(source);
                fixed (T* src = span)
                {
                    CopyFromCore(_ptr + index, src, Math.Min(_size - index, source.Count));
                }
            }
            else if (index != _size)
            {
                ThrowOutOfRange(index);
            }
        }

        public void CopyFrom(T[] source, int index)
        {
            if ((uint)index < (uint)_size)
            {
                fixed (T* src = source)
                {
                    CopyFromCore(_ptr + index, src, Math.Min(_size - index, source.Length));
                }
            }
            else if (index != _size)
            {
                ThrowOutOfRange(index);
            }
        }

        public void CopyFrom(ReadOnlySpan<T> source, int index)
        {
            if ((uint)index < (uint)_size)
            {
                fixed (T* src = source)
                {
                    CopyFromCore(_ptr + index, src, Math.Min(_size - index, source.Length));
                }
            }
            else if (index != _size)
            {
                ThrowOutOfRange(index);
            }
        }

        protected override void DisposeUnmanaged() => Free();

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
