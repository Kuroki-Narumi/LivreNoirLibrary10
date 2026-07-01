using System;
using System.Collections.Generic;
using System.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public class FlowAddress : IEquatable<FlowAddress>, IComparable<FlowAddress>, IComparisonOperators<FlowAddress, FlowAddress, bool>
    {
        private static readonly Dictionary<(int[], int), FlowAddress> _cache = [];
        private static readonly Dictionary<int[], FlowAddress> _reverseCache = [];

        public static FlowAddress Empty { get; } = new();

        public static FlowAddress Append(FlowAddress address, int value)
        {
            var array = address._array;
            var key = (array, value);
            if (!_cache.TryGetValue(key, out var newAddress))
            {
                newAddress = new([.. array, value]);
                _cache.Add(key, newAddress);
                _reverseCache.Add(newAddress._array, address);
            }
            return newAddress;
        }

        public static FlowAddress Back(FlowAddress address)
        {
            var array = address._array;
            if (array.Length <= 1)
            {
                return address;
            }
            if (!_reverseCache.TryGetValue(array, out var newAddress))
            {
                newAddress = Empty;
                foreach (var value in array.AsSpan()[..^1])
                {
                    newAddress = Append(newAddress, value);
                }
            }
            return newAddress;
        }

        public static FlowAddress Create(int value) => Append(Empty, value);

        public static FlowAddress ChangeAt(FlowAddress address, int index, int value)
        {
            var newAddress = Empty;
            var array = address._array.AsSpan();
            for (var i = 0; i < array.Length; i++)
            {
                newAddress = Append(newAddress, i == index ? value : array[i]);
            }
            return newAddress;
        }

        private readonly int[] _array;
        private readonly string _toString;

        public int this[int index] => _array[index];
        public int this[Index index] => _array[index];
        public int Length => _array.Length;
        public bool IsFlow => Length % 2 is 1;
        public bool IsBranch => Length % 2 is 0;

        private FlowAddress()
        {
            _array = [];
            _toString = "[root]";
        }

        private FlowAddress(int[] array)
        {
            _array = array;
            var arrayLength = array.Length;
            var chars = (stackalloc char[11 * arrayLength]);
            chars[0] = '[';
            var index = 1;
            ReadOnlySpan<char> delim = ['-', ';'];
            for (var i = 0; i < arrayLength; i++)
            {
                var charsWritten = 1;
                var value = array[i];
                if (value is BmsConstants.DefaultCondition)
                {
                    chars[index] = '*';
                }
                else
                {
                    value.TryFormat(chars[index..], out charsWritten);
                }
                chars[index + charsWritten] = delim[i % 2];
                index += charsWritten + 1;
            }
            chars[index - 1] = ']';
            _toString = new(chars[..index]);
        }

        public override string ToString() => _toString;
        public ReadOnlySpan<int> AsSpan() => _array.AsSpan();

        public bool IsParentOf(FlowAddress other)
        {
            var child = other._array.AsSpan();
            var parent = _array.AsSpan();
            return parent.Length < child.Length &&
                   parent.SequenceEqual(child[..parent.Length]);
        }

        public FlowAddress Append(int value) => Append(this, value);
        public FlowAddress ChangeAt(int index, int value) => ChangeAt(this, index, value);
        public FlowAddress Back() => Back(this);

        public bool Equals(FlowAddress? other) => ReferenceEquals(_array, other?._array);
        public override bool Equals(object? obj) => obj is FlowAddress other && Equals(other);
        public override int GetHashCode() => _array.GetHashCode();
        public int CompareTo(FlowAddress? other) => _array.AsSpan().SequenceCompareTo(other?._array);

        public static bool operator ==(FlowAddress? left, FlowAddress? right) => ReferenceEquals(left?._array, right?._array);
        public static bool operator !=(FlowAddress? left, FlowAddress? right) => !ReferenceEquals(left?._array, right?._array);

        public static bool operator >(FlowAddress left, FlowAddress right) => left.CompareTo(right) is > 0;
        public static bool operator >=(FlowAddress left, FlowAddress right) => left.CompareTo(right) is >= 0;
        public static bool operator <(FlowAddress left, FlowAddress right) => left.CompareTo(right) is < 0;
        public static bool operator <=(FlowAddress left, FlowAddress right) => left.CompareTo(right) is <= 0;

    }
}
