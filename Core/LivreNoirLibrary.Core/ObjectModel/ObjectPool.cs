using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace LivreNoirLibrary.ObjectModel
{
    public static partial class ObjectPool
    {
        public static int MaxStoreCount { get; set; } = 16;

        public static PooledObject<T> Rent<T>() where T : class, new() => new(DefaultFactory<T>.Instance);
        public static PooledObject<T> Rent<T>(Func<T> factory) where T : class => new(factory);

        public static PooledObject<T> Rent<T>(out T obj)
            where T : class, new()
        {
            var ret = Rent<T>();
            obj = ret.Value;
            return ret;
        }

        public static PooledObject<T> Rent<T>(Func<T> factory, out T obj)
            where T : class
        {
            var ret = Rent(factory);
            obj = ret.Value;
            return ret;
        }

        private static class DefaultFactory<T>
            where T : class, new()
        {
            public static readonly Func<T> Instance = () => new T();
        }
    }

    public readonly struct PooledObject<T>(Func<T> factory) : IDisposable
        where T : class
    {
        public readonly T Value = ObjectPool<T>.Rent(factory);

        public void Dispose()
        {
            ObjectPool<T>.Return(Value);
        }
    }

    internal static class ObjectPool<T>
        where T : class
    {
        private static readonly ThreadLocal<Stack<T>> _stored = new(static () => new(4));
        private static readonly Action<T>? _clearMethod;
        private static readonly Func<T, T>? _clearMethod2;

        static ObjectPool()
        {
            if (typeof(T).GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public, Type.EmptyTypes) is { } info)
            {
                if (info.ReturnType == typeof(T))
                {
                    _clearMethod2 = info.CreateDelegate<Func<T, T>>();
                }
                else if (info.ReturnType == typeof(void))
                {
                    _clearMethod = info.CreateDelegate<Action<T>>();
                }
            }
        }

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
            _clearMethod?.Invoke(obj);
            _clearMethod2?.Invoke(obj);
            var stack = _stored.Value!;
            if (stack.Count < ObjectPool.MaxStoreCount)
            {
                stack.Push(obj);
            }
        }
    }
}
