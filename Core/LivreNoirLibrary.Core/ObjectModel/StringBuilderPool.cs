using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LivreNoirLibrary.ObjectModel
{
    public static partial class ObjectPool
    {
        public static PooledStringBuilder RentStringBuilder(out StringBuilder sb)
        {
            var ret = new PooledStringBuilder();
            sb = ret.Value;
            return ret;
        }
    }

    public readonly struct PooledStringBuilder() : IDisposable
    {
        public readonly StringBuilder Value = StringBuilderPool.RentInternal();

        public void Dispose()
        {
            StringBuilderPool.ReturnInternal(Value);
        }
    }

    internal static class StringBuilderPool
    {
        private static readonly ThreadLocal<Stack<StringBuilder>> _stored = new(static () => new(4));
        
        internal static StringBuilder RentInternal()
        {
            if (_stored.Value!.TryPop(out var sb))
            {
                sb.Length = 0;
                return sb;
            }
            else
            {
                return new();
            }
        }

        internal static void ReturnInternal(StringBuilder sb)
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
