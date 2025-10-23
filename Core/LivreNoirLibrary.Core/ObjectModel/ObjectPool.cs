using System;
using System.Collections.Generic;
using System.Threading;

namespace LivreNoirLibrary.ObjectModel
{
    public static class ObjectPool
    {
        public static T Rent<T>() where T : new() => Pool<T>.Rent();
        public static void Return<T>(T value) where T : new() => Pool<T>.Return(value);

        private static class Pool<T>
            where T : new()
        {
            private static readonly Lock _lock = new();
            private static readonly List<T> _stored = [];

            public static T Rent()
            {
                lock (_lock)
                {
                    if (_stored.Count is > 0)
                    {
                        var value = _stored[^1];
                        _stored.RemoveAt(_stored.Count - 1);
                        return value;
                    }
                    else
                    {
                        return new();
                    }
                }
            }

            public static void Return(T value)
            {
                lock (_lock)
                {
                    _stored.Add(value);
                }
            }
        }
    }
}
