using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public static partial class SortedList
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsKey<TKey>(List<TKey> keys, TKey key) => keys.BinarySearch(key) is >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetIndex<TKey>(List<TKey> keys, TKey key, out int index)
        {
            index = keys.BinarySearch(key);
            return index >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetValue<TKey, TValue>(List<TKey> keys, List<TValue> values, TKey key, [MaybeNullWhen(false)]out TValue value, IComparer<TKey>? comparer = null)
        {
            var index = keys.BinarySearch(key, comparer);
            if (index is >= 0)
            {
                value = values[index];
                return true;
            }
            value = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetKey<TKey, TValue>(List<TKey> keys, List<TValue> values, TValue value, [MaybeNullWhen(false)] out TKey key)
        {
            var index = values.IndexOf(value);
            if (index is >= 0)
            {
                key = keys[index];
                return true;
            }
            key = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddOrReplace<TKey, TValue>(List<TKey> keys, List<TValue> values, TKey key, TValue value, IComparer<TKey>? comparer = null)
        {
            var index = keys.BinarySearch(key, comparer);
            if (index is >= 0)
            {
                values[index] = value;
                return true;
            }
            else
            {
                index = ~index;
                keys.Insert(index, key);
                values.Insert(index, value);
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddOrReplace<TKey, TValue>(List<TKey> keys, List<TValue> values, TKey key, TValue value, [MaybeNullWhen(false)]out TValue current, IComparer<TKey>? comparer = null)
        {
            var index = keys.BinarySearch(key, comparer);
            if (index is >= 0)
            {
                current = values[index];
                values[index] = value;
                return true;
            }
            else
            {
                index = ~index;
                keys.Insert(index, key);
                values.Insert(index, value);
                current = default;
                return false;
            }
        }

        public delegate bool UpdateFunc<T>(ref T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Update<TKey, TValue>(List<TKey> keys, List<TValue> values, TKey key, Func<TValue> createFunc, UpdateFunc<TValue> updateFunc, IComparer<TKey>? comparer = null)
        {
            var index = keys.BinarySearch(key, comparer);
            if (index is >= 0)
            {
                var span = CollectionsMarshal.AsSpan(values);
                return updateFunc(ref span[index]);
            }
            else
            {
                index = ~index;
                keys.Insert(index, key);
                values.Insert(index, createFunc());
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Remove<TKey, TValue>(List<TKey> keys, List<TValue> values, TKey key, IComparer<TKey>? comparer = null)
        {
            var index = keys.BinarySearch(key, comparer);
            if (index is >= 0)
            {
                keys.RemoveAt(index);
                values.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Remove<TKey, TValue>(List<TKey> keys, List<TValue> values, TKey key, [MaybeNullWhen(false)]out TValue removed, IComparer<TKey>? comparer = null)
        {
            var index = keys.BinarySearch(key, comparer);
            if (index is >= 0)
            {
                removed = values[index];
                keys.RemoveAt(index);
                values.RemoveAt(index);
                return true;
            }
            else
            {
                removed = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Remove<TKey, TValue>(List<TKey> keys, List<TValue> values, TKey key, Predicate<TValue> removePredicate, IComparer<TKey>? comparer = null)
        {
            var index = keys.BinarySearch(key, comparer);
            if (index is >= 0)
            {
                if (removePredicate(values[index]))
                {
                    keys.RemoveAt(index);
                    values.RemoveAt(index);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo<TKey, TValue>(List<TKey> sourceKeys, List<TValue> sourceValues, List<TKey> targetKeys, List<TValue> targetValues)
            where TKey : IComparable<TKey>
        {
            var sk = CollectionsMarshal.AsSpan(sourceKeys);
            var sv = CollectionsMarshal.AsSpan(sourceValues);
            var sourceLength = sk.Length;
            var sourceIndex = 0;
            for (var targetIndex = 0; sourceIndex < sourceLength && targetIndex < targetKeys.Count; targetIndex++)
            {
                var key = sk[sourceIndex];
                var value = sv[sourceIndex];
                switch (targetKeys[targetIndex].CompareTo(key))
                {
                    case < 0:
                        continue;
                    case 0:
                        targetValues[targetIndex] = value;
                        break;
                    default:
                        targetKeys.Insert(targetIndex, key);
                        targetValues.Insert(targetIndex, value);
                        break;
                }
                sourceIndex++;
            }
            if (sourceIndex < sourceLength)
            {
                targetKeys.AddRange(sk[sourceIndex..]);
                targetValues.AddRange(sv[sourceIndex..]);
            }
        }

        public static Enumerator<TKey, TValue> GetEnumerator<TKey, TValue>(List<TKey> keys, List<TValue> values) => new(keys, values);

        public struct Enumerator<TKey, TValue>(List<TKey> keys, List<TValue> values) : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly List<TKey> _keys = keys;
            private readonly List<TValue> _values = values;
            private int _index = 0;
            private KeyValuePair<TKey, TValue> _current;

            public readonly KeyValuePair<TKey, TValue> Current => _current;
            readonly object System.Collections.IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (_index < _keys.Count)
                {
                    _current = new(_keys[_index], _values[_index]);
                    _index++;
                    return true;
                }
                return false;
            }

            public void Reset() => _index = 0;
            public readonly void Dispose() { }
        }
    }
}
