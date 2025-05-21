using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static bool AddOrReplace<TKey, TValue>(this List<TKey> keyList, List<TValue> valueList, TKey key, TValue value)
            where TKey : IComparable<TKey>
        {
            var index = keyList.BinarySearch(key);
            if (index is >= 0)
            {
                valueList[index] = value;
                return true;
            }
            else
            {
                index = ~index;
                keyList.Insert(index, key);
                valueList.Insert(index, value);
                return false;
            }
        }

        public static bool AddOrReplace<TKey, TValue>(this List<TKey> keyList, List<TValue> valueList, TKey key, TValue value, [MaybeNullWhen(false)]out TValue current)
            where TKey : IComparable<TKey>
        {
            var index = keyList.BinarySearch(key);
            if (index is >= 0)
            {
                current = valueList[index];
                valueList[index] = value;
                return true;
            }
            else
            {
                index = ~index;
                keyList.Insert(index, key);
                valueList.Insert(index, value);
                current = default;
                return false;
            }
        }

        public static bool Update<TKey, TValue>(this List<TKey> keyList, List<TValue> valueList, TKey key, Func<TValue> createFunc, Action<TValue> updateFunc)
            where TKey : IComparable<TKey>
        {
            var index = keyList.BinarySearch(key);
            if (index is >= 0)
            {
                updateFunc(valueList[index]);
                return true;
            }
            else
            {
                index = ~index;
                keyList.Insert(index, key);
                valueList.Insert(index, createFunc());
                return false;
            }
        }

        public static bool Remove<TKey, TValue>(this List<TKey> keyList, List<TValue> valueList, TKey key)
            where TKey : IComparable<TKey>
        {
            var index = keyList.BinarySearch(key);
            if (index is >= 0)
            {
                keyList.RemoveAt(index);
                valueList.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Remove<TKey, TValue>(this List<TKey> keyList, List<TValue> valueList, TKey key, [MaybeNullWhen(false)]out TValue removed)
            where TKey : IComparable<TKey>
        {
            var index = keyList.BinarySearch(key);
            if (index is >= 0)
            {
                removed = valueList[index];
                keyList.RemoveAt(index);
                valueList.RemoveAt(index);
                return true;
            }
            else
            {
                removed = default;
                return false;
            }
        }

        public static bool Remove<TKey, TValue>(this List<TKey> keyList, List<TValue> valueList, TKey key, Func<TValue, bool> removeFunc)
            where TKey : IComparable<TKey>
        {
            var index = keyList.BinarySearch(key);
            if (index is >= 0)
            {
                if (removeFunc(valueList[index]))
                {
                    keyList.RemoveAt(index);
                    valueList.RemoveAt(index);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
