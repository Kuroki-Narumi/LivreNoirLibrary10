using System;
using System.Collections;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public class ConcatEnumerable<T> : IEnumerable<T>
    {
        private readonly List<IEnumerable<T>> _enumerables;
        private int _version;

        public ConcatEnumerable(IEnumerable<T> enumer) => _enumerables = [enumer];
        public ConcatEnumerable(IEnumerable<IEnumerable<T>> enumers) => _enumerables = [.. enumers];
        public ConcatEnumerable(params ReadOnlySpan<IEnumerable<T>> enumers) => _enumerables = [.. enumers];

        public void Clear()
        {
            _version++;
            _enumerables.Clear();
        }
        
        public void Concat(IEnumerable<T> enumer)
        {
            _version++;
            _enumerables.Add(enumer);
        }

        public Enumerator GetEnumerator() => new(this);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<T>
        {
            private readonly ConcatEnumerable<T> _source;
            private readonly int _version;
            private int _enum_index = 0;
            private IEnumerator<T>? _enumerator;
            private T? _current;
            
            internal Enumerator(ConcatEnumerable<T> source)
            {
                _source = source;
                _version = source._version;
            }

            public readonly T Current => _current!;
            readonly object IEnumerator.Current => _current!;

            public bool MoveNext()
            {
                CheckVersion();
                var enumer = _enumerator;
                if (enumer is not null && enumer.MoveNext())
                {
                    _current = enumer.Current;
                    return true;
                }
                return MoveNextEnumerator();
            }

            private bool MoveNextEnumerator()
            {
                var list = _source._enumerables;
                while (_enum_index < list.Count)
                {
                    _enumerator?.Dispose();
                    var enumer = _enumerator = list[_enum_index].GetEnumerator();
                    if (enumer.MoveNext())
                    {
                        _current = enumer.Current;
                        return true;
                    }
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                _current = default;
                _enum_index = _source._enumerables.Count + 1;
                return false;
            }

            public void Reset()
            {
                CheckVersion();
                _enum_index = 0;
                _enumerator = null;
                _current = default;
            }

            private readonly void CheckVersion()
            {
                if (_version != _source._version)
                {
                    throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
                }
            }

            public readonly void Dispose()
            {
                _enumerator?.Dispose();
            }
        }
    }
}
