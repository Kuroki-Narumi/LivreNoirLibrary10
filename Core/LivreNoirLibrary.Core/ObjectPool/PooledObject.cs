using System;

namespace LivreNoirLibrary.ObjectModel
{
    internal static class PooledObject
    {
        public static PooledObject<T> Create<T>(Func<Func<T>, T> getMethod, Func<T> factory, Action<T> returnMethod) where T : class => new(getMethod, factory, returnMethod);
        public static PooledObject<T> Create<T>(Func<T> getMethod, Action<T> returnMethod) where T : class => new(getMethod, returnMethod);
    }

    public readonly struct PooledObject<T> : IDisposable
        where T : class
    {
        public readonly T Value;
        private readonly Action<T> _return;

        internal PooledObject(Func<Func<T>, T> getMethod, Func<T> factory, Action<T> returnMethod)
        {
            Value = getMethod(factory);
            _return = returnMethod;
        }

        internal PooledObject(Func<T> getMethod, Action<T> returnMethod)
        {
            Value = getMethod();
            _return = returnMethod;
        }

        public void Dispose() => _return(Value);
    }
}
