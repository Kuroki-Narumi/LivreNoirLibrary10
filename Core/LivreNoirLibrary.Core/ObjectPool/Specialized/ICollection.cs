using System;
using System.Collections.Generic;
using System.Threading;

namespace LivreNoirLibrary.ObjectModel
{
    partial class ObjectPool
    {
        private static PooledObject<TCollection> RentCollection<T, TCollection>(Func<TCollection> factory)
            where TCollection : class, ICollection<T>
            => new(CollectionPool<T, TCollection>.Rent, factory, CollectionPool<T, TCollection>.Return);

        public static PooledObject<List<T>> RentList<T>(out List<T> obj, int capacity = 0)
        {
            var o = RentCollection<T, List<T>>(DefaultFactory<List<T>>.Create);
            obj = o.Value;
            obj.EnsureCapacity(capacity);
            return o;
        }

        public static PooledObject<HashSet<T>> RentHashSet<T>(out HashSet<T> obj, int capacity = 0)
        {
            var o = RentCollection<T, HashSet<T>>(DefaultFactory<HashSet<T>>.Create);
            obj = o.Value;
            obj.EnsureCapacity(capacity);
            return o;
        }

        public static PooledObject<SortedSet<T>> RentSortedSet<T>(out SortedSet<T> obj)
        {
            var o = RentCollection<T, SortedSet<T>>(DefaultFactory<SortedSet<T>>.Create);
            obj = o.Value;
            return o;
        }

        public static PooledObject<Dictionary<TKey, TValue>> RentDictionary<TKey, TValue>(out Dictionary<TKey, TValue> obj, int capacity = 0)
            where TKey : notnull
        {
            var o = RentCollection<KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>>(DefaultFactory<Dictionary<TKey, TValue>>.Create);
            obj = o.Value;
            obj.EnsureCapacity(capacity);
            return o;
        }

        public static PooledObject<SortedDictionary<TKey, TValue>> RentSortedDictionary<TKey, TValue>(out SortedDictionary<TKey, TValue> obj, int capacity = 0)
            where TKey : notnull
        {
            var o = RentCollection<KeyValuePair<TKey, TValue>, SortedDictionary<TKey, TValue>>(DefaultFactory<SortedDictionary<TKey, TValue>>.Create);
            obj = o.Value;
            return o;
        }

        public static PooledObject<SortedList<TKey, TValue>> RentSortedList<TKey, TValue>(out SortedList<TKey, TValue> obj, int capacity = 0)
            where TKey : notnull
        {
            var o = RentCollection<KeyValuePair<TKey, TValue>, SortedList<TKey, TValue>>(DefaultFactory<SortedList<TKey, TValue>>.Create);
            obj = o.Value;
            return o;
        }
    }

    internal class CollectionPool<T, TCollection>
        where TCollection : class, ICollection<T>
    {
        private static readonly ThreadLocal<Stack<TCollection>> _stored = new(static () => new(1));

        public static TCollection Rent(Func<TCollection> factory)
        {
            if (_stored.Value!.TryPop(out var obj))
            {
                return obj;
            }
            else
            {
                return factory();
            }
        }

        public static void Return(TCollection obj)
        {
            var stack = _stored.Value!;
            if (stack.Count < ObjectPool.MaxStoreCount)
            {
                obj.Clear();
                stack.Push(obj);
            }
        }
    }
}
