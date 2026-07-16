using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace LivreNoirLibrary.ObjectModel
{
    partial class ObjectPool
    {
        public static PooledObject<MemoryStream> RentMemoryStream(out MemoryStream ms, int capacity = 0)
        {
            var ret = PooledObject.Create(MemoryStreamPool.Rent, MemoryStreamPool.Return);
            ms = ret.Value;
            if (capacity > ms.Capacity)
            {
                ms.Capacity = capacity;
            }
            return ret;
        }
    }

    internal static class MemoryStreamPool
    {
        private static readonly ThreadLocal<Stack<MemoryStream>> _stored = new(static () => new(1));
        
        public static MemoryStream Rent()
        {
            if (_stored.Value!.TryPop(out var sb))
            {
                return sb;
            }
            else
            {
                return new();
            }
        }

        public static void Return(MemoryStream sb)
        {
            var stack = _stored.Value!;
            if (stack.Count < ObjectPool.MaxStoreCount)
            {
                sb.SetLength(0);
                stack.Push(sb);
            }
        }
    }
}
