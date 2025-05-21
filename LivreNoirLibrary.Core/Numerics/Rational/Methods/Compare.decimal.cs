using System;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational : IEquatable<decimal>, IComparable<decimal>, IEqualityOperators<Rational, decimal, bool>, IComparisonOperators<Rational, decimal, bool>
    {
        public bool Equals(decimal other) => _numerator == other * Denominator;
        public static bool operator ==(Rational left, decimal right) => left._numerator == right * left.Denominator;
        public static bool operator !=(Rational left, decimal right) => left._numerator != right * left.Denominator;

        /// <inheritdoc cref="operator==(Rational, Rational)"/>
        public static bool operator ==(decimal left, Rational right) => left * right.Denominator == right._numerator;
        /// <inheritdoc cref="operator!=(Rational, Rational)"/>
        public static bool operator !=(decimal left, Rational right) => left * right.Denominator != right._numerator;

        public int CompareTo(decimal other) => ((decimal)_numerator).CompareTo(other * Denominator);

        public static bool operator <(Rational left, decimal right) => left._numerator < right * left.Denominator;
        public static bool operator <=(Rational left, decimal right) => left._numerator <= right * left.Denominator;
        public static bool operator >(Rational left, decimal right) => left._numerator > right * left.Denominator;
        public static bool operator >=(Rational left, decimal right) => left._numerator >= right * left.Denominator;

        /// <inheritdoc cref="operator&lt;(Rational, Rational)"/>
        public static bool operator <(decimal left, Rational right) => left * right.Denominator < right._numerator;
        /// <inheritdoc cref="operator&lt;=(Rational, Rational)"/>
        public static bool operator <=(decimal left, Rational right) => left * right.Denominator <= right._numerator;
        /// <inheritdoc cref="operator&gt;(Rational, Rational)"/>
        public static bool operator >(decimal left, Rational right) => left * right.Denominator > right._numerator;
        /// <inheritdoc cref="operator&gt;=(Rational, Rational)"/>
        public static bool operator >=(decimal left, Rational right) => left * right.Denominator >= right._numerator;
    }
}
