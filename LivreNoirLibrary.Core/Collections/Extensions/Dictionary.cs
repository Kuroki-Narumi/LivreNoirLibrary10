using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dic, Predicate<KeyValuePair<TKey, TValue>> selector)
        {
            var list = dic.ToArray();
            foreach (var kv in list)
            {
                if (selector(kv))
                {
                    dic.Remove(kv.Key);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, Func<TKey, TValue> createFunc)
        {
            if (!dic.TryGetValue(key, out var value))
            {
                value = createFunc(key);
                dic.Add(key, value);
            }
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
            where TValue: new()
        {
            if (!dic.TryGetValue(key, out var value))
            {
                value = new();
                dic.Add(key, value);
            }
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<TValue, TKey> Invert<TKey, TValue>(this IDictionary<TKey, TValue> dic)
            where TValue : notnull
        {
            Dictionary<TValue, TKey> result = [];
            foreach (var kv in dic)
            {
                result[kv.Value] = kv.Key;
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
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> dest, IDictionary<TKey, TValue> source)
        {
            foreach (var (key, value) in source)
            {
                dest[key] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> dest, IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            foreach (var (key, value) in source)
            {
                dest[key] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> dest, IEnumerable<(TKey, TValue)> source)
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
    }
}
