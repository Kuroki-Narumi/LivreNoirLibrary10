using System;
using System.ComponentModel;
using System.Numerics;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Numerics
{
    [JsonConverter(typeof(RationalJsonConverter))]
    [TypeConverter(typeof(RationalTypeConverter))]
    public readonly partial struct Rational : INumber<Rational>
    {
        private readonly long _numerator;
        /// <summary>
        /// The denominator is held at its actual value minus one, so that default(<see cref="Rational"/>) represents zero.
        /// </summary>
        private readonly long _denominatorMinusOne;

        /// <summary>
        /// The numerator of a fraction.
        /// </summary>
        public long Numerator => _numerator;
        /// <summary>
        /// The denominator of a fraction.
        /// </summary>
        public long Denominator => _denominatorMinusOne + 1;

        /// <summary>
        /// Create new <see cref="Rational"/> instance with no checking.
        /// </summary>
        /// <param name="_">for overload, discarded.</param>
        /// <param name="n">Value for <see cref="Rational.Numerator"/>.</param>
        /// <param name="d">Value for <see cref="Rational.Denominator"/>.</param>
        internal Rational(bool _, long n, long d)
        {
            _numerator = n;
            _denominatorMinusOne = d - 1;
        }

        /// <summary>
        /// Create new <see cref="Rational"/> instance with denominator = 1.
        /// </summary>
        /// <param name="numerator">Value for <see cref="Rational.Numerator"/>.</param>
        public Rational(long numerator)
        {
            _numerator = numerator;
            _denominatorMinusOne = 0;
        }

        /// <summary>
        /// Create new <see cref="Rational"/> instance with specified <paramref name="numerator"/> and <paramref name="denominator"/>.<br/>
        /// The fractions are automatically reduced so that each value is as small as possible.
        /// </summary>
        /// <param name="numerator">Value for <see cref="Rational.Numerator"/>.</param>
        /// <param name="denominator">Value for <see cref="Rational.Denominator"/>.</param>
        /// <exception cref="DivideByZeroException"/>
        public Rational(long numerator, long denominator)
        {
            if (denominator is 0)
            {
                ThrowDivideByZeroException(DenominatorMustBeNonZero);
            }
            if (numerator is 0)
            {
                _numerator = 0;
                _denominatorMinusOne = 0;
                return;
            }
            if (denominator is < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }
            if (denominator is 1)
            {
                _numerator = numerator;
                _denominatorMinusOne = 0;
                return;
            }
            var gcd = numerator.GCD(denominator);
            _numerator = numerator / gcd;
            _denominatorMinusOne = denominator / gcd - 1;
        }

        /// <summary>
        /// Create new <see cref="Rational"/> instance by a tuple with elements (<paramref name="numerator"/>, <paramref name="denominator"/>).<br/>
        /// The fractions are automatically reduced so that each value is as small as possible.
        /// </summary>
        /// <param name="tuple">A 2-value tuple.</param>
        /// <exception cref="DivideByZeroException"/>
        public Rational((long Numerator, long Denominator) tuple) : this(tuple.Numerator, tuple.Denominator) { }

        /// <inheritdoc cref="Rational(long, long)"/>
        /// <param name="check">If <see cref="bool">true</see>, throw an <see cref="OverflowException"/> 
        /// if either value exceeds <see cref="long.MaxValue"/> or <see cref="long.MinValue"/>.</param>
        /// <exception cref="OverflowException"/>
        public Rational(Int128 numerator, Int128 denominator, bool check = true)
        {
            if (denominator == Int128.Zero)
            {
                ThrowDivideByZeroException(DenominatorMustBeNonZero);
            }
            if (numerator == Int128.Zero || denominator >= numerator * Int128MaxValueAsLong)
            {
                _numerator = 0;
                _denominatorMinusOne = 0;
                return;
            }
            if (denominator < Int128.Zero)
            {
                numerator = -numerator;
                denominator = -denominator;
            }
            if (denominator == Int128.One)
            {
                _numerator = check ? checked((long)numerator) : (long)numerator;
                _denominatorMinusOne = 0;
                return;
            }
            var gcd = numerator.GCD(denominator);
            if (check)
            {
                _numerator = checked((long)(numerator / gcd));
                _denominatorMinusOne = checked((long)(denominator / gcd)) - 1;
            }
            else
            {
                _numerator = (long)(numerator / gcd);
                _denominatorMinusOne = (long)(denominator / gcd) - 1;
            }
        }
    }
}
