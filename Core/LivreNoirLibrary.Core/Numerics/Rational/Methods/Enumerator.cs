using System;

namespace LivreNoirLibrary.Numerics
{
    partial struct Rational
    {
        /// <summary>
        /// Enumerates all rational numbers between 0 and 1 with denominators up to a specified limit.
        /// </summary>
        /// <remarks>
        /// 0 (0/1) and 1 (1/1) are excluded from the enumeration. The enumerator generates fractions in their simplest form, ensuring that each fraction is unique and irreducible.
        /// </remarks>
        /// <param name="maxDenominator">The maximum allowed value(inclusive) for the denominator of the rational numbers to enumerate.</param>
        /// <returns>An enumerator for the rational numbers.</returns>
        public static Enumerator EnumerateZeroToOne(long maxDenominator = long.MaxValue)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxDenominator);
            return new(maxDenominator);
        }

        public struct Enumerator
        {
            private readonly ulong _limit;
            private ulong _prevN, _prevD, _nextN, _nextD;

            public readonly (long Numerator, long Denominator) Current => ((long)_prevN, (long)_prevD);

            internal Enumerator(long limit)
            {
                _prevN = 0;
                _prevD = 1;
                _nextN = 1;
                _nextD = _limit = (ulong)limit;
            }

            public bool MoveNext()
            {
                if (_nextN <= _limit)
                {
                    var k = (_limit + _prevD) / _nextD;
                    var newC = k * _nextN - _prevN;
                    var newD = k * _nextD - _prevD;
                    _prevN = _nextN;
                    _prevD = _nextD;
                    _nextN = newC;
                    _nextD = newD;
                    return _prevN != _prevD; // Skip 1/1
                }
                return false;
            }

            public readonly Enumerator GetEnumerator() => this;
        }
    }
}
