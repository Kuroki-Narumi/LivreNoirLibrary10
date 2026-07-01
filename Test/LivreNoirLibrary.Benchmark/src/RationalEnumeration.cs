using BenchmarkDotNet.Attributes;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Benchmark
{
    public class RationalEnumeration
    {
        const int MaxDen = 1000;
        private double _value;

        [Benchmark]
        public void ZeroToOne_Enumerator()
        {
            foreach (var (num, den) in EnumerateZeroToOne(MaxDen))
            {
                _value = (double)num / den;
            }
        }

        [Benchmark]
        public void ZeroToOne_Enumerator2()
        {
            foreach (var (num, den) in Rational.EnumerateZeroToOne(MaxDen))
            {
                _value = (double)num / den;
            }
        }

        [Benchmark]
        public void ZeroToOne_IEnumerable()
        {
            foreach (var (num, den) in EnumerateZeroToOne_IEnumerable(MaxDen))
            {
                _value = (double)num / den;
            }
        }

        public static OrderdEnumerator EnumerateZeroToOne(int maxDenominator = int.MaxValue)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxDenominator);
            return new(maxDenominator);
        }

        public static IEnumerable<(int Numerator, int Denominator)> EnumerateZeroToOne_IEnumerable(int maxDenominator = int.MaxValue)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxDenominator);
            int a = 0, b = 1, c = 1, d = maxDenominator;
            while (c <= maxDenominator)
            {
                var k = (maxDenominator + b) / d;
                var newA = c;
                var newB = d;
                var newC = k * c - a;
                var newD = k * d - b;
                a = newA;
                b = newB;
                c = newC;
                d = newD;
                if (newA == newB) // Skip 1/1
                {
                    yield break;
                }
                yield return (a, b);
            }
        }

        public struct OrderdEnumerator
        {
            private long _a, _b, _c, _d;
            private readonly long _limit;

            public readonly (int Numerator, int Denominator) Current => ((int)_a, (int)_b);

            internal OrderdEnumerator(int limit)
            {
                _a = 0;
                _b = 1;
                _c = 1;
                _d = limit;
                _limit = limit;
            }

            public bool MoveNext()
            {
                if (_c <= _limit)
                {
                    var k = (_limit + _b) / _d;
                    var newA = _c;
                    var newB = _d;
                    var newC = k * _c - _a;
                    var newD = k * _d - _b;
                    _a = newA;
                    _b = newB;
                    _c = newC;
                    _d = newD;
                    return newA != newB; // Skip 1/1
                }
                return false;
            }

            public readonly OrderdEnumerator GetEnumerator() => this;
        }
    }
}
