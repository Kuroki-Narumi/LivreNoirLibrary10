using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media
{
    public struct TwoMergedEnumerator<T1, T2> where T1 : IComparable<T1>
    {
        private readonly IEnumerator<(T1, T2)> _enumer1;
        private readonly IEnumerator<(T1, T2)> _enumer2;
        private bool _enumer1Exists;
        private bool _enumer2Exists;
        private (T1, T2?, T2?) _current;

        public readonly (T1 Key, T2? Value1, T2? Value2) Current => _current;

        public TwoMergedEnumerator(IEnumerable<(T1, T2)> left, IEnumerable<(T1, T2)> right)
        {
            _enumer1 = left.GetEnumerator();
            _enumer2 = right.GetEnumerator();
            _enumer1Exists = _enumer1.MoveNext();
            _enumer2Exists = _enumer2.MoveNext();
        }

        public bool MoveNext()
        {
            if (!_enumer1Exists)
            {
                if (!_enumer2Exists)
                {
                    return false;
                }
                var (x, y) = _enumer2.Current;
                _current = (x, default, y);
                _enumer2Exists = _enumer2.MoveNext();
                return true;
            }
            var (x1, y1) = _enumer1.Current;
            if (!_enumer2Exists)
            {
                _current = (x1, y1, default);
                _enumer1Exists = _enumer1.MoveNext();
                return true;
            }
            var (x2, y2) = _enumer2.Current;
            switch (x1.CompareTo(x2))
            {
                case < 0:
                    _current = (x1, y1, default);
                    _enumer1Exists = _enumer1.MoveNext();
                    return true;
                case > 0:
                    _current = (x2, default, y2);
                    _enumer2Exists = _enumer2.MoveNext();
                    return true;
                default:
                    _current = (x1, y1, y2);
                    _enumer1Exists = _enumer1.MoveNext();
                    _enumer2Exists = _enumer2.MoveNext();
                    return true;
            }
        }

        public readonly TwoMergedEnumerator<T1, T2> GetEnumerator() => this;
    }
}
