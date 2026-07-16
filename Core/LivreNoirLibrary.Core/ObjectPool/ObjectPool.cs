using System;
using System.Collections.Generic;
using System.Threading;

namespace LivreNoirLibrary.ObjectModel
{
    public static partial class ObjectPool
    {
        public const int InitialStoreCapacity = 4;
        public static int MaxStoreCount { get; set; } = 16;

        public static PooledObject<T> Rent<T>(out T obj)
            where T : class, new()
        {
            var o = PooledObject.Create(ObjectPool<T>.Rent, DefaultFactory<T>.Create, ObjectPool<T>.Return);
            obj = o.Value;
            return o;
        }

        public static PooledObject<T> Rent<T>(Func<T> factory, out T obj)
            where T : class
        {
            var o = PooledObject.Create(ObjectPool<T>.Rent, factory, ObjectPool<T>.Return);
            obj = o.Value;
            return o;
        }
    }

    internal static class DefaultFactory<T>
        where T : class, new()
    {
        public static readonly Func<T> Create = () => new T();
    }

    internal static class ObjectPool<T>
        where T : class
    {
        private static readonly ThreadLocal<Stack<T>> _stored = new(static () => new(1));

        public static T Rent(Func<T> factory)
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

        public static void Return(T obj)
        {
            var stack = _stored.Value!;
            if (stack.Count < ObjectPool.MaxStoreCount)
            {
                stack.Push(obj);
            }
        }
    }
}
