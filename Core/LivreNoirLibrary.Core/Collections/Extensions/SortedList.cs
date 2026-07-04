using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using IEble = System.Collections.IEnumerable;
using IEtor = System.Collections.IEnumerator;

namespace LivreNoirLibrary.Collections
{
    public static partial class SortedList
    {
        /// <summary>
        /// Determines whether the specified key exists in a sorted list using a binary search algorithm.
        /// </summary>
        /// <remarks>The list must be sorted in ascending order prior to calling this method. 
        /// If the list is not sorted, the result is undefined.</remarks>
        /// <typeparam name="TKey">The type of the keys in the list.</typeparam>
        /// <param name="keys">A list of keys, which must be sorted in ascending order.</param>
        /// <param name="key">The key to locate in the list.</param>
        /// <param name="comparer">The comparer to use when comparing keys, or <see langword="null"/> to use the default comparer for the <typeparamref name="TKey"/>.</param>
        /// <returns><see langword="true"/> if the key is found in the list; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsKey<TKey>(List<TKey> keys, TKey key, IComparer<TKey>? comparer = null) => keys.BinarySearch(key, comparer) is >= 0;

        /// <inheritdoc cref="ContainsKey{TKey}(List{TKey}, TKey, IComparer{TKey}?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsKey<TKey>(ReadOnlySpan<TKey> keys, TKey key, IComparer<TKey>? comparer = null) => keys.BinarySearch(key, comparer ?? Comparer<TKey>.Default) is >= 0;

        /// <summary>
        /// Attempts to find the index of the specified key in a sorted list using binary search.
        /// </summary>
        /// <remarks>The list must be sorted in ascending order prior to calling this method. 
        /// If the list is not sorted, the result is undefined.</remarks>
        /// <typeparam name="TKey">The type of the keys in the list.</typeparam>
        /// <param name="keys">A list of keys, which must be sorted in ascending order.</param>
        /// <param name="key">The key to locate in the list.</param>
        /// <param name="index">When this method returns, contains the zero-based index of the key if found; otherwise, a negative number
        /// that is the bitwise complement of the index of the next element that is larger than key, or, if there is no
        /// larger element, the bitwise complement of Count.</param>
        /// <param name="comparer">The comparer to use when comparing keys, or <see langword="null"/> to use the default comparer for <typeparamref name="TKey"/>.</param>
        /// <returns><see langword="true"/> if the key is found in the list; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetIndex<TKey>(List<TKey> keys, TKey key, out int index, IComparer<TKey>? comparer = null)
        {
            index = keys.BinarySearch(key, comparer);
            return index >= 0;
        }

        /// <inheritdoc cref="TryGetIndex{TKey}(List{TKey}, TKey, out int, IComparer{TKey}?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetIndex<TKey>(ReadOnlySpan<TKey> keys, TKey key, out int index, IComparer<TKey>? comparer = null)
        {
            index = keys.BinarySearch(key, comparer ?? Comparer<TKey>.Default);
            return index >= 0;
        }

        /// <summary>
        /// Attempts to retrieve the value associated with the specified key from the provided sorted key and value lists.
        /// </summary>
        /// <remarks>The keys list must be sorted in ascending order prior to calling this method. 
        /// If the list is not sorted, the result is undefined.
        /// The values list must be the same length as the keys list, with each value corresponding to the key at the same index.</remarks>
        /// <typeparam name="TKey">The type of the keys in the list.</typeparam>
        /// <typeparam name="TValue">The type of the values in the list.</typeparam>
        /// <param name="keys">A list of keys, which must be sorted in ascending order.</param>
        /// <param name="values">A list of values corresponding to the keys. Must have the same number of elements as the keys list.</param>
        /// <param name="key">The key to locate in the keys list.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found;
        /// otherwise, the default value for <typeparamref name="TValue"/></param>
        /// <param name="comparer">The comparer to use when comparing keys, or <see langword="null"/> to use the default comparer for <typeparamref name="TKey"/>.</param>
        /// <returns><see langword="true"/> if the key is found in the list; otherwise, <see langword="false"/>.</returns>
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

        /// <inheritdoc cref="TryGetValue{TKey, TValue}(List{TKey}, List{TValue}, TKey, out TValue, IComparer{TKey}?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetValue<TKey, TValue>(ReadOnlySpan<TKey> keys, ReadOnlySpan<TValue> values, TKey key, [MaybeNullWhen(false)]out TValue value, IComparer<TKey>? comparer = null)
        {
            var index = keys.BinarySearch(key, comparer ?? Comparer<TKey>.Default);
            if (index is >= 0)
            {
                value = values[index];
                return true;
            }
            value = default;
            return false;
        }

        /// <summary>
        /// Retrieves the value associated with the specified key from the provided sorted key and value lists.
        /// or returns a default value if the key is not found.
        /// </summary>
        /// <remarks>The keys list must be sorted in ascending order prior to calling this method. 
        /// If the list is not sorted, the result is undefined.
        /// The values list must be the same length as the keys list, with each value corresponding to the key at the same index.</remarks>
        /// <typeparam name="TKey">The type of the keys in the list.</typeparam>
        /// <typeparam name="TValue">The type of the values in the list.</typeparam>
        /// <param name="keys">A list of keys, which must be sorted in ascending order.</param>
        /// <param name="values">A list of values corresponding to the keys. Must have the same number of elements as the keys list.</param>
        /// <param name="key">The key to locate in the keys list.</param>
        /// <param name="defaultValue">The value to return if the specified key is not found in the keys list.</param>
        /// <param name="comparer">The comparer to use when comparing keys, or <see langword="null"/> to use the default comparer for <typeparamref name="TKey"/>.</param>
        /// <returns>The value associated with the specified key if found; otherwise, the specified default value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetValueOrDefault<TKey, TValue>(List<TKey> keys, List<TValue> values, TKey key, TValue defaultValue, IComparer<TKey>? comparer = null)
        {
            var index = keys.BinarySearch(key, comparer);
            if (index is >= 0)
            {
                return values[index];
            }
            return defaultValue;
        }

        /// <summary>
        /// Attempts to find the key associated with the specified value in the provided key and value lists.
        /// </summary>
        /// <remarks>The keys and values lists must be of equal length, and each key at a given index is
        /// associated with the value at the same index. If the specified value occurs multiple times in the values
        /// list, the key corresponding to its first occurrence is returned.</remarks>
        /// <typeparam name="TKey">The type of the keys in the list.</typeparam>
        /// <typeparam name="TValue">The type of the values in the list.</typeparam>
        /// <param name="keys">A list of keys.</param>
        /// <param name="values">A list of values corresponding to the keys. Must have the same number of elements as the keys list.</param>
        /// <param name="value">The value to locate in the values list.</param>
        /// <param name="key">When this method returns, contains the key associated with the specified value, if the value is found;
        /// otherwise, the default value for <typeparamref name="TKey"/></param>
        /// <returns><see langword="true"/> if the value is found and the associated key is returned in <paramref name="key"/>;
        /// otherwise, <see langword="false"/>.</returns>
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

        /// <summary>
        /// Retrieves the value associated with the specified key, or adds a new key and value if the key does not
        /// exist. The value is created using the default constructor of the value type.
        /// </summary>
        /// <remarks>The keys list must be sorted in ascending order prior to calling this method. 
        /// If the list is not sorted, the result is undefined.
        /// The values list must be the same length as the keys list, with each value corresponding to the key at the same index.</remarks>
        /// <typeparam name="TKey">The type of the keys in the list.</typeparam>
        /// <typeparam name="TValue">The type of the values in the list.</typeparam>
        /// <param name="keys">A list of keys, which must be sorted in ascending order.</param>
        /// <param name="values">A list of values corresponding to the keys. Must have the same number of elements as the keys list.</param>
        /// <param name="key">The key to locate in the keys list. If not found, a new entry is added.</param>
        /// <param name="comparer">The comparer to use when comparing keys, or <see langword="null"/> to use the default comparer for <typeparamref name="TKey"/>.</param>
        /// <returns>The value associated with the specified key if it exists; 
        /// otherwise, a new value created using the default constructor and added to the lists.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(List<TKey> keys, List<TValue> values, TKey key, IComparer<TKey>? comparer = null)
            where TValue : new()
        {
            var index = keys.BinarySearch(key, comparer);
            if (index is >= 0)
            {
                return values[index];
            }
            else
            {
                index = ~index;
                keys.Insert(index, key);
                var value = new TValue();
                values.Insert(index, value);
                return value;
            }
        }

        /// <summary>
        /// Retrieves the value associated with the specified key, or adds a new key and value if the key does not
        /// exist. The value is created using the provided factory function.
        /// </summary>
        /// <remarks>The keys list must be sorted in ascending order prior to calling this method. 
        /// If the list is not sorted, the result is undefined.
        /// The values list must be the same length as the keys list, with each value corresponding to the key at the same index.</remarks>
        /// <typeparam name="TKey">The type of the keys in the list.</typeparam>
        /// <typeparam name="TValue">The type of the values in the list.</typeparam>
        /// <param name="keys">A list of keys, which must be sorted in ascending order.</param>
        /// <param name="values">A list of values corresponding to the keys. Must have the same number of elements as the keys list.</param>
        /// <param name="key">The key to locate in the keys list. If not found, a new entry is added.</param>
        /// <param name="valueFactory">A function that is called to produce a value when the specified key does not exist in the keys list.</param>
        /// <param name="comparer">The comparer to use when comparing keys, or <see langword="null"/> to use the default comparer for <typeparamref name="TKey"/>.</param>
        /// <returns>The value associated with the specified key if it exists; 
        /// otherwise, a new value created using the provided factory function constructor and added to the lists.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(List<TKey> keys, List<TValue> values, TKey key, Func<TKey, TValue> valueFactory, IComparer<TKey>? comparer = null)
        {
            var index = keys.BinarySearch(key, comparer);
            if (index is >= 0)
            {
                return values[index];
            }
            else
            {
                index = ~index;
                keys.Insert(index, key);
                var value = valueFactory(key);
                values.Insert(index, value);
                return value;
            }
        }

        /// <summary>
        /// Adds a key-value pair to the specified lists or replaces the value for an existing key. 
        /// Returns a value indicating whether the operation resulted in an addition or replacement.
        /// </summary>
        /// <remarks>The keys list must be sorted in ascending order prior to calling this method. 
        /// If the list is not sorted, the result is undefined.
        /// The values list must be the same length as the keys list, with each value corresponding to the key at the same index.</remarks>
        /// <typeparam name="TKey">The type of the keys in the list.</typeparam>
        /// <typeparam name="TValue">The type of the values in the list.</typeparam>
        /// <param name="keys">A list of keys, which must be sorted in ascending order.</param>
        /// <param name="values">A list of values corresponding to the keys. Must have the same number of elements as the keys list.</param>
        /// <param name="key">The key to add or whose value to replace.</param>
        /// <param name="value">The value to associate with the specified key.</param>
        /// <param name="oldValue">When replacing an existing value, contains the previous value associated with the key; 
        /// otherwise, contains the default value for <typeparamref name="TValue"/>.</param>
        /// <param name="comparer">The comparer to use when comparing keys, or <see langword="null"/> to use the default comparer for <typeparamref name="TKey"/>.</param>
        /// <param name="valueComparer">The equality comparer used to determine whether the existing value is equal to the new value. 
        /// If <see langword="null"/>, the default equality comparer for <typeparamref name="TValue"/> is used.</param>
        /// <returns><see langword="true"/> if the key-value pair was added or the value was replaced; 
        /// <see langword="false"/> if the existing value was equal to the new value and no change was made.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddOrReplace<TKey, TValue>(List<TKey> keys, List<TValue> values, TKey key, TValue value, [MaybeNullWhen(true)]out TValue oldValue, IComparer<TKey>? comparer = null, IEqualityComparer<TValue>? valueComparer = null)
        {
            var index = keys.BinarySearch(key, comparer);
            if (index is >= 0)
            {
                oldValue = values[index];
                if ((valueComparer ?? EqualityComparer<TValue>.Default).Equals(oldValue, value))
                {
                    return false;
                }
                values[index] = value;
                return true;
            }
            else
            {
                index = ~index;
                keys.Insert(index, key);
                values.Insert(index, value);
                oldValue = default;
                return true;
            }
        }

        /// <inheritdoc cref="AddOrReplace{TKey, TValue}(List{TKey}, List{TValue}, TKey, TValue, out TValue, IComparer{TKey}?, IEqualityComparer{TValue}?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddOrReplace<TKey, TValue>(List<TKey> keys, List<TValue> values, TKey key, TValue value, IComparer<TKey>? keyComparer = null, IEqualityComparer<TValue>? valueComparer = null)
            => AddOrReplace(keys, values, key, value, out _, keyComparer, valueComparer);

        /// <summary>
        /// Removes the element with the specified key from the provided key and value lists.
        /// </summary>
        /// <remarks>The keys list must be sorted in ascending order prior to calling this method. 
        /// If the list is not sorted, the result is undefined.
        /// The values list must be the same length as the keys list, with each value corresponding to the key at the same index.</remarks>
        /// <typeparam name="TKey">The type of the keys in the list.</typeparam>
        /// <typeparam name="TValue">The type of the values in the list.</typeparam>
        /// <param name="keys">A list of keys, which must be sorted in ascending order.</param>
        /// <param name="values">A list of values corresponding to the keys. Must have the same number of elements as the keys list.</param>
        /// <param name="key">The key of the element to remove.</param>
        /// <param name="removed">If the element removed, contains the previous value associated with the key; 
        /// otherwise, contains the default value for <typeparamref name="TValue"/>.</param>
        /// <param name="comparer">The comparer to use when comparing keys, or <see langword="null"/> to use the default comparer for <typeparamref name="TKey"/>.</param>
        /// <returns><see langword="true"/> if the element with the specified key was found and removed; otherwise, <see langword="false"/>.</returns>
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

        /// <inheritdoc cref="Remove{TKey, TValue}(List{TKey}, List{TValue}, TKey, out TValue, IComparer{TKey}?)"/>
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

        /// <summary>
        /// Copies key-value pairs from the source lists to the target lists, merging them in sorted order and updating
        /// or inserting entries as needed.
        /// </summary>
        /// <remarks>This method merges the source key-value pairs into the target lists, updating values
        /// for matching keys and inserting new key-value pairs in sorted order. 
        /// Both the source and target key lists must be sorted in ascending order. 
        /// The target lists are modified in place.</remarks>
        /// <typeparam name="TKey">The type of the keys in the list.</typeparam>
        /// <typeparam name="TValue">The type of the values in the list.</typeparam>
        /// <param name="sourceKeys">The list of keys to copy from. Must be sorted in ascending order.</param>
        /// <param name="sourceValues">The list of values to copy from. Each value corresponds to the key at the same index in <paramref name="sourceKeys"/>.</param>
        /// <param name="targetKeys">The list of target keys to merge into. Must be sorted in ascending order.</param>
        /// <param name="targetValues">The list of target values to merge into. Each value corresponds to the key at the same index in <paramref name="targetKeys"/>.</param>
        /// <param name="comparer">The comparer to use when comparing keys, or <see langword="null"/> to use the default comparer for <typeparamref name="TKey"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo<TKey, TValue>(List<TKey> sourceKeys, List<TValue> sourceValues, List<TKey> targetKeys, List<TValue> targetValues, IComparer<TKey>? comparer = null)
        {
            comparer ??= Comparer<TKey>.Default;
            var sk = sourceKeys.AsSpan();
            var sv = sourceValues.AsSpan();
            var sourceLength = sk.Length;
            var sourceIndex = 0;
            for (var targetIndex = 0; sourceIndex < sourceLength && targetIndex < targetKeys.Count; targetIndex++)
            {
                var key = sk[sourceIndex];
                var value = sv[sourceIndex];
                switch (comparer.Compare(targetKeys[targetIndex], key))
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

        public static TKey MinKeyByValue<TKey, TValue>(List<TKey> keys, List<TValue> values, IComparer<TValue>? comparer = null)
        {
            var count = keys.Count;
            if (count is 0)
            {
                return default!;
            }
            comparer ??= Comparer<TValue>.Default;
            var key = keys[0];
            var value = values[0];
            for (var i = 1; i < count; i++)
            {
                var current = values[i];
                if (comparer.Compare(current, value) is < 0)
                {
                    key = keys[i];
                    value = current;
                }
            }
            return key;
        }

        public static TKey MaxKeyByValue<TKey, TValue>(List<TKey> keys, List<TValue> values, IComparer<TValue>? comparer = null)
        {
            var count = keys.Count;
            if (count is 0)
            {
                return default!;
            }
            comparer ??= Comparer<TValue>.Default;
            var key = keys[0];
            var value = values[0];
            for (var i = 1; i < count; i++)
            {
                var current = values[i];
                if (comparer.Compare(current, value) is > 0)
                {
                    key = keys[i];
                    value = current;
                }
            }
            return key;
        }

        public static TKey MinKeyBy<TKey, TValue, TCompare>(List<TKey> keys, List<TValue> values, Func<TValue, TCompare> selector, IComparer<TCompare>? comparer = null)
        {
            var count = keys.Count;
            if (count is 0)
            {
                return default!;
            }
            comparer ??= Comparer<TCompare>.Default;
            var key = keys[0];
            var value = selector(values[0]);
            for (var i = 1; i < count; i++)
            {
                var current = selector(values[i]);
                if (comparer.Compare(current, value) is < 0)
                {
                    key = keys[i];
                    value = current;
                }
            }
            return key;
        }

        public static TKey MaxKeyBy<TKey, TValue, TCompare>(List<TKey> keys, List<TValue> values, Func<TValue, TCompare> selector, IComparer<TCompare>? comparer = null)
        {
            var count = keys.Count;
            if (count is 0)
            {
                return default!;
            }
            comparer ??= Comparer<TCompare>.Default;
            var key = keys[0];
            var value = selector(values[0]);
            for (var i = 1; i < count; i++)
            {
                var current = selector(values[i]);
                if (comparer.Compare(current, value) is > 0)
                {
                    key = keys[i];
                    value = current;
                }
            }
            return key;
        }

        public static Enumerator<TKey, TValue> GetEnumerator<TKey, TValue>(List<TKey> keys, List<TValue> values) => new(keys, values);

        public ref struct Enumerator<TKey, TValue>(List<TKey> keys, List<TValue> values)
        {
            private readonly ReadOnlySpan<TKey> _keys = keys.AsSpan();
            private readonly ReadOnlySpan<TValue> _values = values.AsSpan();
            private readonly int _count = keys.Count;
            private int _index = 0;
            private (TKey, TValue) _current;

            public readonly (TKey, TValue) Current => _current;

            public bool MoveNext()
            {
                if (_index < _count)
                {
                    _current = new(_keys[_index], _values[_index]);
                    _index++;
                    return true;
                }
                return false;
            }

            public readonly Enumerator<TKey, TValue> GetEnumerator() => this;
        }

        public static SafeEnumerator<TKey, TValue> GetSafeEnumerator<TKey, TValue>(List<TKey> keys, List<TValue> values) => new(keys, values);

        public class SafeEnumerator<TKey, TValue>(List<TKey> keys, List<TValue> values) : IEnumerator<(TKey, TValue)>, IEnumerable<(TKey, TValue)>
        {
            private readonly List<TKey> _keys = keys;
            private readonly List<TValue> _values = values;
            private readonly int _count = keys.Count;
            private int _index = 0;
            private (TKey, TValue) _current;

            public (TKey, TValue) Current => _current;

            public bool MoveNext()
            {
                if (_index < _count)
                {
                    _current = new(_keys[_index], _values[_index]);
                    _index++;
                    return true;
                }
                return false;
            }

            public SafeEnumerator<TKey, TValue> GetEnumerator() => this;

            object IEtor.Current => _current;
            void IEtor.Reset() => _index = 0;
            void IDisposable.Dispose() { }

            IEnumerator<(TKey, TValue)> IEnumerable<(TKey, TValue)>.GetEnumerator() => this;
            IEtor IEble.GetEnumerator() => this;
        }

        public static MergedEnumerator<TKey, TValue> GetMergedEnumerator<TKey, TValue>(List<TKey> keys1, List<TValue> values1, List<TKey> keys2, List<TValue> values2, IComparer<TKey>? comparer = null)
            => new(keys1, values1, keys2, values2, comparer);

        public struct MergedEnumerator<TKey, TValue>(List<TKey> keys1, List<TValue> values1, List<TKey> keys2, List<TValue> values2, IComparer<TKey>? comparer) :
            IEnumerator<(TKey, TValue)>, IEnumerable<(TKey, TValue)>
        {
            private readonly List<TKey> _keys1 = keys1;
            private readonly List<TValue> _values1 = values1;
            private readonly List<TKey> _keys2 = keys2;
            private readonly List<TValue> _values2 = values2;
            private readonly int _count1 = keys1.Count;
            private readonly int _count2 = keys2.Count;
            private readonly IComparer<TKey> _comparer = comparer ?? Comparer<TKey>.Default;
            private int _index1 = 0;
            private int _index2 = 0;
            private (TKey, TValue) _current;

            public readonly (TKey, TValue) Current => _current;

            public bool MoveNext()
            {
                var index1 = _index1;
                var index2 = _index2;
                if (index1 >= _count1)
                {
                    if (index2 >= _count2)
                    {
                        _current = default;
                        return false;
                    }
                    _current = (_keys2[index2], _values2[_index2]);
                    _index2 = index2 + 1;
                    return true;
                }
                var key1 = _keys1[index1];
                var value1 = _values1[index1];
                if (index2 >= _count2)
                {
                    _current = (key1, value1);
                    _index1 = index1 + 1;
                    return true;
                }
                var key2 = _keys2[index2];
                var value2 = _values2[index2];
                switch (_comparer.Compare(key1, key2))
                {
                    case <= 0:
                        _current = new(key1, value1);
                        _index1 = index1 + 1;
                        return true;
                    case > 0:
                        _current = new(key2, value2);
                        _index2 = index2 + 1;
                        return true;
                }
            }

            public readonly MergedEnumerator<TKey, TValue> GetEnumerator() => this;

            public readonly record struct Item(TKey Key, bool Value1Exists, TValue? Value1, bool Value2Exists, TValue? Value2);

            readonly object IEtor.Current => _current;
            void IEtor.Reset() => _index1 = _index2 = 0;
            readonly void IDisposable.Dispose() { }

            readonly IEnumerator<(TKey, TValue)> IEnumerable<(TKey, TValue)>.GetEnumerator() => this;
            readonly IEtor IEble.GetEnumerator() => this;
        }
    }
}
