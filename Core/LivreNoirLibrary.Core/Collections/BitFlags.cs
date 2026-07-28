using System;
using System.Buffers;

namespace LivreNoirLibrary.Collections
{
    public static class BitFlags
    {
        const int BitsPerInt = sizeof(int) * 8;

        public static int GetArrayLength(int count) => (count + BitsPerInt - 1) / BitsPerInt;
        public static int GetLength(ReadOnlySpan<int> span) => span.Length * BitsPerInt;

        public static bool IsSet(ReadOnlySpan<int> span, int index)
        {
            if ((uint)index < (uint)GetLength(span))
            {
                var (i, j) = Math.DivRem(index, BitsPerInt);
                return (span[i] & (1 << j)) is not 0;
            }
            return false;
        }

        public static void Set(Span<int> span, int index)
        {
            if ((uint)index < (uint)GetLength(span))
            {
                var (i, j) = Math.DivRem(index, BitsPerInt);
                span[i] |= 1 << j;
            }
        }

        public static void Break(Span<int> span, int index)
        {
            if ((uint)index < (uint)GetLength(span))
            {
                var (i, j) = Math.DivRem(index, BitsPerInt);
                span[i] &= ~(1 << j);
            }
        }

        public static void Set(Span<int> span, int index, int count)
        {
            if (index is < 0)
            {
                count += index;
                index = 0;
            }
            count = Math.Min(GetLength(span) - index, count);
            if (count is <= 0)
            {
                return;
            }
            var (arrayStart, bitStart) = Math.DivRem(index, BitsPerInt);
            var (arrayEnd, bitEnd) = Math.DivRem(index + count, BitsPerInt);

            ref var value = ref span[arrayStart];
            if (arrayStart == arrayEnd)
            {
                for (var j = bitStart; j < bitEnd; j++)
                {
                    value |= 1 << j;
                }
            }
            else
            {
                if (bitStart > 0)
                {
                    for (var j = bitStart; j < BitsPerInt; j++)
                    {
                        value |= 1 << j;
                    }
                    arrayStart++;
                }

                span[arrayStart..arrayEnd].Fill(~0);

                if (bitEnd > 0)
                {
                    value = ref span[arrayEnd];
                    for (var j = 0; j < bitEnd; j++)
                    {
                        value |= 1 << j;
                    }
                }
            }
        }

        public static void Break(Span<int> span, int index, int count)
        {
            if (index is < 0)
            {
                count += index;
                index = 0;
            }
            count = Math.Min(GetLength(span) - index, count);
            if (count is <= 0)
            {
                return;
            }
            var (arrayStart, bitStart) = Math.DivRem(index, BitsPerInt);
            var (arrayEnd, bitEnd) = Math.DivRem(index + count, BitsPerInt);

            ref var value = ref span[arrayStart];
            if (arrayStart == arrayEnd)
            {
                for (var j = bitStart; j < bitEnd; j++)
                {
                    value &= ~(1 << j);
                }
            }
            else
            {
                if (bitStart > 0)
                {
                    for (var j = bitStart; j < BitsPerInt; j++)
                    {
                        value &= ~(1 << j);
                    }
                    arrayStart++;
                }

                span[arrayStart..arrayEnd].Clear();

                if (bitEnd > 0)
                {
                    value = ref span[arrayEnd];
                    for (var j = 0; j < bitEnd; j++)
                    {
                        value &= ~(1 << j);
                    }
                }
            }
        }

        public static Enumerator EnumerateFlags(int[] array) => new(array);
        public static IndexEnumerator EnumerateSetIndex(int[] array) => new(array);

        public struct Enumerator(int[] array) : ISafeEnumerator<bool>
        {
            private readonly int[] _array = array;
            private int _arrayIndex;
            private int _currentValue;
            private int _currentMask;
            private bool _current;

            public readonly bool Current => _current;

            public bool MoveNext()
            {
                if (_currentMask is not 0)
                {
                    _current = (_currentValue & _currentMask) is not 0;
                    _currentMask <<= 1;
                    return true;
                }
                if (_arrayIndex < _array.Length)
                {
                    _currentValue = _array[_arrayIndex];
                    _arrayIndex++;
                    _current = (_currentValue & 1) is not 0;
                    _currentMask = 2;
                    return true;
                }
                return false;
            }
        }

        public struct IndexEnumerator(int[] array) : ISafeEnumerator<int>
        {
            private readonly int[] _array = array;
            private int _arrayIndex;
            private int _currentValue;
            private int _currentMask = 1;
            private int _current = -1;

            public readonly int Current => _current;

            public bool MoveNext()
            {
                while (_arrayIndex < _array.Length)
                {
                    if (_currentMask is 1)
                    {
                        _currentValue = _array[_arrayIndex];
                    }
                    while (_currentMask is not 0)
                    {
                        var flag = (_currentValue & _currentMask) is not 0;
                        _currentMask <<= 1;
                        _current++;
                        if (flag)
                        {
                            return true;
                        }
                    }
                    _currentMask = 1;
                    _arrayIndex++;
                }
                return false;
            }
        }
    }
}
