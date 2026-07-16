using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LivreNoirLibrary.ObjectModel
{
    partial class ObjectPool
    {
        public static PooledObject<T> RentClear<T>(out T obj) 
            where T : class, IClear, new()
        {
            var o = PooledObject.Create(IClearObjectPool<T>.Rent, IClearObjectPool<T>.Return);
            obj = o.Value;
            return o;
        }
    }

    internal static class IClearObjectPool<T> where T : IClear, new()
    {
        private static readonly ThreadLocal<Stack<T>> _stored = new(static () => new(1));

        public static T Rent()
        {
            if (_stored.Value!.TryPop(out var obj))
            {
                return obj;
            }
            else
            {
                return new();
            }
        }

        public static void Return(T obj)
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
