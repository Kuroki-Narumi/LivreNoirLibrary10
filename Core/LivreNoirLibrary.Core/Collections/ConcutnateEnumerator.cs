using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public static class ConcutnateEnumerator
    {
        public static ConcutnateEnumerator<T> Create<T>(params ReadOnlySpan<IEnumerable<T>> sources) => new(sources);
    }

    public sealed class ConcutnateEnumerator<T>(params ReadOnlySpan<IEnumerable<T>> sources) : ISafeEnumerator<T>
    {
        private readonly IEnumerable<T>[] _sources = [.. sources];
        private int _index;
        private IEnumerator<T>? _currentEnumerator;

        public T Current { get; private set; } = default!;

        public bool MoveNext()
        {
            if (_currentEnumerator is { } enumer && enumer.MoveNext())
            {
                Current = enumer.Current;
                return true;
            }
            while (_index < _sources.Length)
            {
                var e = _sources[_index].GetEnumerator();
                _index++;
                if (e.MoveNext())
                {
                    _currentEnumerator = e;
                    Current = e.Current;
                    return true;
                }
            }
            Current = default!;
            return false;
        }

        public void Reset()
        {
            _index = 0;
            _currentEnumerator = null;
            Current = default!;
        }
    }
}
