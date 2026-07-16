using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LivreNoirLibrary.ObjectModel
{
    partial class ObjectPool
    {
        public static PooledObject<StringBuilder> RentStringBuilder(out StringBuilder sb, int capacity = 0)
        {
            var ret = PooledObject.Create(StringBuilderPool.Rent, StringBuilderPool.Return);
            sb = ret.Value;
            sb.EnsureCapacity(capacity);
            return ret;
        }
    }

    internal static class StringBuilderPool
    {
        private static readonly ThreadLocal<Stack<StringBuilder>> _stored = new(static () => new(1));
        
        public static StringBuilder Rent()
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

        public static void Return(StringBuilder sb)
        {
            var stack = _stored.Value!;
            if (stack.Count < ObjectPool.MaxStoreCount)
            {
                sb.Length = 0;
                stack.Push(sb);
            }
        }
    }
}
