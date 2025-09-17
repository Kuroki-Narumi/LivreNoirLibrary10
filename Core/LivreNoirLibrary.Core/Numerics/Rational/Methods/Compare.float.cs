using System;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational : IEquatable<float>, IComparable<float>, IEqualityOperators<Rational, float, bool>, IComparisonOperators<Rational, float, bool>
    {
        public bool Equals(float other) => _numerator == other * Denominator;
        public static bool operator ==(Rational left, float right) => left._numerator == right * left.Denominator;
        public static bool operator !=(Rational left, float right) => left._numerator != right * left.Denominator;

        /// <inheritdoc cref="operator==(Rational, Rational)"/>
        public static bool operator ==(float left, Rational right) => left * right.Denominator == right._numerator;
        /// <inheritdoc cref="operator!=(Rational, Rational)"/>
        public static bool operator !=(float left, Rational right) => left * right.Denominator != right._numerator;

        public int CompareTo(float other) => ((float)_numerator).CompareTo(other * Denominator);

        public static bool operator <(Rational left, float right) => left._numerator < right * left.Denominator;
        public static bool operator <=(Rational left, float right) => left._numerator <= right * left.Denominator;
        public static bool operator >(Rational left, float right) => left._numerator > right * left.Denominator;
        public static bool operator >=(Rational left, float right) => left._numerator >= right * left.Denominator;

        /// <inheritdoc cref="operator&lt;(Rational, Rational)"/>
        public static bool operator <(float left, Rational right) => left * right.Denominator < right._numerator;
        /// <inheritdoc cref="operator&lt;=(Rational, Rational)"/>
        public static bool operator <=(float left, Rational right) => left * right.Denominator <= right._numerator;
        /// <inheritdoc cref="operator&gt;(Rational, Rational)"/>
        public static bool operator >(float left, Rational right) => left * right.Denominator > right._numerator;
        /// <inheritdoc cref="operator&gt;=(Rational, Rational)"/>
        public static bool operator >=(float left, Rational right) => left * right.Denominator >= right._numerator;
    }
}
