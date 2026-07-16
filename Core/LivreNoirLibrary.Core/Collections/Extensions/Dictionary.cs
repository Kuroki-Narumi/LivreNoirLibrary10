using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using static LivreNoirLibrary.Collections.CollectionExtensions;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        /// <summary>
        /// Removes all elements with specified keys.
        /// </summary>
        /// <param name="keys">The keys to remove.</param>
        /// <returns>The number of elements removed from the <see cref="IDictionary{TKey, TValue}"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dic, IEnumerable<TKey> keys)
            where TKey : notnull
        {
            var count = 0;
            foreach (var key in keys)
            {
                if (dic.Remove(key))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Removes all elements with specified keys.
        /// </summary>
        /// <param name="keys">The keys to remove.</param>
        /// <returns>The number of elements removed from the <see cref="IDictionary{TKey, TValue}"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dic, ReadOnlySpan<TKey> keys)
            where TKey : notnull
        {
            var count = 0;
            foreach (var key in keys)
            {
                if (dic.Remove(key))
                {
                    count++;
                }
            }
            return count;
        }
        
        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The delegate that defined the conditions of the elements to remove. </param>
        /// <returns>The number of elements removed from the <see cref="Dictionary{TKey, TValue}"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dic, Func<KeyValuePair<TKey, TValue>, bool> match)
            where TKey : notnull
        {
            var count = 0;
            var keys = new TKey[dic.Count];
            foreach (var kv in dic)
            {
                if (match(kv))
                {
                    keys[count] = kv.Key;
                    count++;
                }
            }
            foreach (var key in keys.AsSpan(0, count))
            {
                dic.Remove(key);
            }
            return count;
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The delegate that defined the conditions of the elements to remove. </param>
        /// <returns>The number of elements removed from the <see cref="SortedDictionary{TKey, TValue}"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RemoveAll<TKey, TValue>(this SortedDictionary<TKey, TValue> dic, Func<KeyValuePair<TKey, TValue>, bool> match)
            where TKey : notnull
        {
            var count = 0;
            var keys = new TKey[dic.Count];
            foreach (var kv in dic)
            {
                if (match(kv))
                {
                    keys[count] = kv.Key;
                    count++;
                }
            }
            foreach (var key in keys.AsSpan(0, count))
            {
                dic.Remove(key);
            }
            return count;
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The delegate that defined the conditions of the elements to remove. </param>
        /// <returns>The number of elements removed from the <see cref="IDictionary{TKey, TValue}"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dic, Func<KeyValuePair<TKey, TValue>, bool> match)
        {
            var count = 0;
            var keys = dic.Where(match).Select(kv => kv.Key).ToArray();
            foreach (var key in keys)
            {
                dic.Remove(key);
                count++;
            }
            return count;
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="Dictionary{TKey,TValue}"/>
        /// if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">the value to be added, if the key does not already exist</param>
        /// <returns>The value for the key.  This will be either the existing value for the key if the
        /// key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
            where TKey : notnull
        {
            if (!dic.TryGetValue(key, out var actualValue))
            {
                actualValue = value;
                dic.Add(key, value);
            }
            return actualValue;
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="Dictionary{TKey,TValue}"/>
        /// if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="valueFactory">The function used to generate a value for the key</param>
        /// <returns>The value for the key.  This will be either the existing value for the key if the
        /// key is already in the dictionary, or the new value for the key as returned by valueFactory
        /// if the key was not in the dictionary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, Func<TKey, TValue> valueFactory)
            where TKey : notnull
        {
            if (!dic.TryGetValue(key, out var value))
            {
                value = valueFactory(key);
                dic.Add(key, value);
            }
            return value;
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="Dictionary{TKey,TValue}"/>
        /// if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <returns>The value for the key.  This will be either the existing value for the key if the
        /// key is already in the dictionary, or the new value for the key as returned by new() 
        /// if the key was not in the dictionary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
            where TKey : notnull where TValue : new() => GetOrAdd(dic, key, key => new());

        /// <summary>
        /// Adds a key/value pair to the <see cref="SortedList{TKey,TValue}"/>
        /// if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">the value to be added, if the key does not already exist</param>
        /// <returns>The value for the key.  This will be either the existing value for the key if the
        /// key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this SortedList<TKey, TValue> dic, TKey key, TValue value)
            where TKey : notnull
        {
            if (!dic.TryGetValue(key, out var actualValue))
            {
                actualValue = value;
                dic.Add(key, value);
            }
            return actualValue;
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="SortedList{TKey,TValue}"/>
        /// if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="valueFactory">The function used to generate a value for the key</param>
        /// <returns>The value for the key.  This will be either the existing value for the key if the
        /// key is already in the dictionary, or the new value for the key as returned by valueFactory
        /// if the key was not in the dictionary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this SortedList<TKey, TValue> dic, TKey key, Func<TKey, TValue> valueFactory)
            where TKey : notnull
        {
            if (!dic.TryGetValue(key, out var value))
            {
                value = valueFactory(key);
                dic.Add(key, value);
            }
            return value;
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="SortedList{TKey,TValue}"/>
        /// if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <returns>The value for the key.  This will be either the existing value for the key if the
        /// key is already in the dictionary, or the new value for the key as returned by new() 
        /// if the key was not in the dictionary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this SortedList<TKey, TValue> dic, TKey key)
            where TKey : notnull where TValue : new() => GetOrAdd(dic, key, key => new());

        /// <summary>
        /// Adds a key/value pair to the <see cref="SortedDictionary{TKey,TValue}"/>
        /// if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">the value to be added, if the key does not already exist</param>
        /// <returns>The value for the key.  This will be either the existing value for the key if the
        /// key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this SortedDictionary<TKey, TValue> dic, TKey key, TValue value)
            where TKey : notnull
        {
            if (!dic.TryGetValue(key, out var actualValue))
            {
                actualValue = value;
                dic.Add(key, value);
            }
            return actualValue;
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="SortedDictionary{TKey,TValue}"/>
        /// if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="valueFactory">The function used to generate a value for the key</param>
        /// <returns>The value for the key.  This will be either the existing value for the key if the
        /// key is already in the dictionary, or the new value for the key as returned by valueFactory
        /// if the key was not in the dictionary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this SortedDictionary<TKey, TValue> dic, TKey key, Func<TKey, TValue> valueFactory)
            where TKey : notnull
        {
            if (!dic.TryGetValue(key, out var value))
            {
                value = valueFactory(key);
                dic.Add(key, value);
            }
            return value;
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="SortedDictionary{TKey,TValue}"/>
        /// if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <returns>The value for the key.  This will be either the existing value for the key if the
        /// key is already in the dictionary, or the new value for the key as returned by new() 
        /// if the key was not in the dictionary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this SortedDictionary<TKey, TValue> dic, TKey key)
            where TKey : notnull where TValue : new() => GetOrAdd(dic, key, key => new());

        /// <summary>
        /// Adds a key/value pair to the <see cref="IDictionary{TKey,TValue}"/>
        /// if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">the value to be added, if the key does not already exist</param>
        /// <returns>The value for the key.  This will be either the existing value for the key if the
        /// key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (!dic.TryGetValue(key, out var actualValue))
            {
                actualValue = value;
                dic.Add(key, value);
            }
            return actualValue;
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="IDictionary{TKey,TValue}"/>
        /// if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="valueFactory">The function used to generate a value for the key</param>
        /// <returns>The value for the key.  This will be either the existing value for the key if the
        /// key is already in the dictionary, or the new value for the key as returned by valueFactory
        /// if the key was not in the dictionary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, Func<TKey, TValue> valueFactory)
        {
            if (!dic.TryGetValue(key, out var value))
            {
                value = valueFactory(key);
                dic.Add(key, value);
            }
            return value;
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="IDictionary{TKey,TValue}"/>
        /// if the key does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <returns>The value for the key.  This will be either the existing value for the key if the
        /// key is already in the dictionary, or the new value for the key as returned by new() 
        /// if the key was not in the dictionary.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
            where TValue : new() => GetOrAdd(dic, key, key => new());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<TValue, TKey> Invert<TKey, TValue>(this Dictionary<TKey, TValue> dic)
            where TKey : notnull
            where TValue : notnull
        {
            Dictionary<TValue, TKey> result = [];
            foreach (var (key, value) in dic)
            {
                result.TryAdd(value, key);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<TValue, TKey> Invert<TKey, TValue>(this SortedDictionary<TKey, TValue> dic)
            where TKey : notnull
            where TValue : notnull
        {
            Dictionary<TValue, TKey> result = [];
            foreach (var (key, value) in dic)
            {
                result.TryAdd(value, key);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<TValue, TKey> Invert<TKey, TValue>(this IDictionary<TKey, TValue> dic)
            where TValue : notnull
        {
            Dictionary<TValue, TKey> result = [];
            foreach (var (key, value) in dic)
            {
                result.TryAdd(value, key);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<TValue, List<TKey>> InvertSafe<TKey, TValue>(this Dictionary<TKey, TValue> dic)
            where TKey : notnull
            where TValue : notnull
        {
            Dictionary<TValue, List<TKey>> result = [];
            foreach (var (key, value) in dic)
            {
                Add(result, value, key);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<TValue, List<TKey>> InvertSafe<TKey, TValue>(this SortedDictionary<TKey, TValue> dic)
            where TKey : notnull
            where TValue : notnull
        {
            Dictionary<TValue, List<TKey>> result = [];
            foreach (var (key, value) in dic)
            {
                Add(result, value, key);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<TValue, List<TKey>> InvertSafe<TKey, TValue>(this IDictionary<TKey, TValue> dic)
            where TValue : notnull
        {
            Dictionary<TValue, List<TKey>> result = [];
            foreach (var (key, value) in dic)
            {
                Add(result, value, key);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dest, IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            foreach (var (key, value) in source)
            {
                dest[key] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dest, IEnumerable<(TKey, TValue)> source)
        {
            foreach (var (key, value) in source)
            {
                dest[key] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<TKey, TValue, TInner>(this IDictionary<TKey, TInner> dic, TKey key, TValue value)
            where TInner : ICollection<TValue>, new()
        {
            if (!dic.TryGetValue(key, out var list))
            {
                list = new();
                dic.Add(key, list);
            }
            list.Add(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddRange<TKey, TValue, TInner>(this IDictionary<TKey, TInner> dic, IEnumerable<KeyValuePair<TKey, TValue>> values)
            where TInner : ICollection<TValue>, new()
        {
            foreach (var (key, value) in values)
            {
                Add(dic, key, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddRange<TKey, TValue, TInner>(this IDictionary<TKey, TInner> dic, IEnumerable<(TKey, TValue)> values)
            where TInner : ICollection<TValue>, new()
        {
            foreach (var (key, value) in values)
            {
                Add(dic, key, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Merge<TKey, TValue, TInner>(this IDictionary<TKey, TInner> dic, TKey key, IEnumerable<TValue> values)
            where TInner : ICollection<TValue>, new()
        {
            if (!dic.TryGetValue(key, out var list))
            {
                list = new();
                dic.Add(key, list);
            }
            foreach (var value in values)
            {
                list.Add(value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Set<TKey1, TKey2, TValue, TInner>(this IDictionary<TKey1, TInner> dic, TKey1 key1, TKey2 key2, TValue value)
            where TInner : IDictionary<TKey2, TValue>, new()
        {
            if (!dic.TryGetValue(key1, out var dic2))
            {
                dic2 = new();
                dic.Add(key1, dic2);
            }
            dic2[key2] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Remove<TKey, TValue, TInner>(this IDictionary<TKey, TInner> dic, TKey key, TValue value)
            where TInner : ICollection<TValue>
        {
            if (dic.TryGetValue(key, out var list) && list.Remove(value))
            {
                if (list.Count == 0)
                {
                    dic.Remove(key);
                }
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Remove<TKey, TValue, TInner>(this IDictionary<TKey, TInner> dic, TKey key, IEnumerable<TValue> values)
            where TInner : ICollection<TValue>
        {
            if (dic.TryGetValue(key, out var list))
            {
                var result = false;
                foreach (var value in values)
                {
                    result |= list.Remove(value);
                }
                if (list.Count == 0)
                {
                    dic.Remove(key);
                }
                return result;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Remove<TKey1, TKey2, TValue, TInner>(this IDictionary<TKey1, TInner> dic, TKey1 key1, TKey2 key2)
            where TInner : IDictionary<TKey2, TValue>, new()
        {
            if (dic.TryGetValue(key1, out var dic2) && dic2.Remove(key2))
            {
                if (dic2.Count == 0)
                {
                    dic.Remove(key1);
                }
                return true;
            }
            return false;
        }

        public static TValue GetOrAdd<TKey, TValue, TAlternateKey>(this Dictionary<TKey, TValue>.AlternateLookup<TAlternateKey> lookup, TAlternateKey key)
            where TKey : notnull
            where TValue : new()
            where TAlternateKey : notnull, allows ref struct
        {
            if (!lookup.TryGetValue(key, out var value))
            {
                value = new();
                lookup[key] = value;
            }
            return value;
        }

        public delegate TValue AlternateValueFactory<TAlternateKey, TValue>(TAlternateKey key) where TAlternateKey : allows ref struct;

        public static TValue GetOrAdd<TKey, TValue, TAlternateKey>(this Dictionary<TKey, TValue>.AlternateLookup<TAlternateKey> lookup, TAlternateKey key, AlternateValueFactory<TAlternateKey, TValue> factory)
            where TKey : notnull
            where TAlternateKey : notnull, allows ref struct
        {
            if (!lookup.TryGetValue(key, out var value))
            {
                value = factory(key);
                lookup[key] = value;
            }
            return value;
        }

        public static bool Remove<TKey, TValue, TAlternateKey>(this Dictionary<TKey, List<TValue>>.AlternateLookup<TAlternateKey> lookup, TAlternateKey key, TValue value)
            where TKey : notnull
            where TValue : new()
            where TAlternateKey : notnull, allows ref struct
        {
            if (lookup.TryGetValue(key, out var list) && list.Remove(value))
            {
                if (list.Count is 0)
                {
                    lookup.Remove(key);
                }
                return true;
            }
            return false;
        }
    }
}
