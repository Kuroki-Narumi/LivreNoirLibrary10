using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using IEble = System.Collections.IEnumerable;
using IEtor = System.Collections.IEnumerator;

namespace LivreNoirLibrary.Collections
{
    public static partial class SortedList
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsKey<TKey>(List<TKey> keys, TKey key, IComparer<TKey>? comparer = null) => keys.BinarySearch(key, comparer) is >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetIndex<TKey>(List<TKey> keys, TKey key, out int index, IComparer<TKey>? comparer = null)
        {
            index = keys.BinarySearch(key, comparer);
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
        public static bool AddOrReplace<TKey, TValue>(List<TKey> keys, List<TValue> values, TKey key, TValue value, [MaybeNullWhen(false)]out TValue oldValue, IComparer<TKey>? comparer = null)
        {
            var index = keys.BinarySearch(key, comparer);
            if (index is >= 0)
            {
                oldValue = values[index];
                values[index] = value;
                return true;
            }
            else
            {
                index = ~index;
                keys.Insert(index, key);
                values.Insert(index, value);
                oldValue = default;
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
