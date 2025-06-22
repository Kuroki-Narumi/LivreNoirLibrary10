using System;
using System.Collections;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public struct ArrayEnumerator<T>(T[] source, int length = -2) : IEnumerator<T>
        where T : notnull
    {
        private readonly T[] _source = source;
        private readonly int _max = Math.Min(length is >= -1 ? length : int.MaxValue, source.Length - 1);
        private int _index = -1;

        public readonly T Current => _source[_index];
        readonly object IEnumerator.Current => Current!;

        public readonly void Dispose() { }

        public bool MoveNext()
        {
            if (_index < _max)
            {
                _index++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
