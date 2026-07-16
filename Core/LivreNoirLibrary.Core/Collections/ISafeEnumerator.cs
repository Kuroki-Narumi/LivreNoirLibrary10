using System;
using System.Collections;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public interface ISafeEnumerator<out T> : IEnumerable<T>, IEnumerator<T>
    {
        object? IEnumerator.Current => Current;
        void IEnumerator.Reset() { }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
        void IDisposable.Dispose() { }
    }
}
