using System;
using System.Collections.Generic;
using System.Threading;

namespace LivreNoirLibrary.ObjectModel
{
    partial class ObjectPool
    {
        public static PooledObject<Stack<T>> RentStack<T>(out Stack<T> obj, int capacity = 0)
        {
            var o = PooledObject.Create(StackPool<T>.Rent, StackPool<T>.Return);
            obj = o.Value;
            obj.EnsureCapacity(capacity);
            return o;
        }

        public static PooledObject<Queue<T>> RentQueue<T>(out Queue<T> obj, int capacity = 0)
        {
            var o = PooledObject.Create(QueuePool<T>.Rent, QueuePool<T>.Return);
            obj = o.Value;
            obj.EnsureCapacity(capacity);
            return o;
        }
    }

    internal class StackPool<T>
    {
        private static readonly ThreadLocal<Stack<Stack<T>>> _stored = new(static () => new(1));

        public static Stack<T> Rent()
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

        public static void Return(Stack<T> obj)
        {
            var stack = _stored.Value!;
            if (stack.Count < ObjectPool.MaxStoreCount)
            {
                obj.Clear();
                stack.Push(obj);
            }
        }
    }

    internal class QueuePool<T>
    {
        private static readonly ThreadLocal<Stack<Queue<T>>> _stored = new(static () => new(1));

        public static Queue<T> Rent()
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

        public static void Return(Queue<T> obj)
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
