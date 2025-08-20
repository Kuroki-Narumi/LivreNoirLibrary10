using System;
using System.Collections;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public struct ArrayEnumerator<T> : IEnumerator<T>
    {
        private readonly T[] _source;
        private readonly int _start;
        private readonly int _max;
        private int _index;

        public ArrayEnumerator(T[] source, int start = 0, int count = -1)
        {
            _source = source;
            if ((uint)start >= (uint)source.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "Start index is out of range.");
            }
            _start = start;
            _max = (count is < 0 ? source.Length - start : Math.Min(source.Length - start, count)) - 1;
            _index = -1;
        }

        public readonly T Current => _source[_start + _index];
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
