using System;
using System.Collections;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public interface ISafeEnumerable<out T> : IEnumerable<T>
        where T : allows ref struct
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
