using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace LivreNoirLibrary.ObjectModel
{
    public static class ObjectPool
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Rent<T>() where T : new() => Pool<T>.Rent();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Return<T>(T obj) where T : new() => Pool<T>.Return(obj);

        private static class Pool<T>
            where T : new()
        {
            private static readonly Stack<T> _stored = [];
            private static readonly Action<T>? _clearMethod;

            static Pool()
            {
                if (typeof(T).GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public, Type.EmptyTypes) is { } info && info.ReturnType == typeof(void))
                {
                    _clearMethod = info.CreateDelegate<Action<T>>();
                }
            }

            public static T Rent()
            {
                if (_stored.TryPop(out var obj))
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
                _clearMethod?.Invoke(obj);
                _stored.Push(obj);
            }
        }
    }
}
