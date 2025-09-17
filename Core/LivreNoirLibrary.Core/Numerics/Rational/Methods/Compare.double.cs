using System;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational : IEquatable<double>, IComparable<double>, IEqualityOperators<Rational, double, bool>, IComparisonOperators<Rational, double, bool>
    {
        public bool Equals(double other) => _numerator == other * Denominator;
        public static bool operator ==(Rational left, double right) => left._numerator == right * left.Denominator;
        public static bool operator !=(Rational left, double right) => left._numerator != right * left.Denominator;

        /// <inheritdoc cref="operator==(Rational, Rational)"/>
        public static bool operator ==(double left, Rational right) => left * right.Denominator == right._numerator;
        /// <inheritdoc cref="operator!=(Rational, Rational)"/>
        public static bool operator !=(double left, Rational right) => left * right.Denominator != right._numerator;

        public int CompareTo(double other) => ((double)_numerator).CompareTo(other * Denominator);

        public static bool operator <(Rational left, double right) => left._numerator < right * left.Denominator;
        public static bool operator <=(Rational left, double right) => left._numerator <= right * left.Denominator;
        public static bool operator >(Rational left, double right) => left._numerator > right * left.Denominator;
        public static bool operator >=(Rational left, double right) => left._numerator >= right * left.Denominator;

        /// <inheritdoc cref="operator&lt;(Rational, Rational)"/>
        public static bool operator <(double left, Rational right) => left * right.Denominator < right._numerator;
        /// <inheritdoc cref="operator&lt;=(Rational, Rational)"/>
        public static bool operator <=(double left, Rational right) => left * right.Denominator <= right._numerator;
        /// <inheritdoc cref="operator&gt;(Rational, Rational)"/>
        public static bool operator >(double left, Rational right) => left * right.Denominator > right._numerator;
        /// <inheritdoc cref="operator&gt;=(Rational, Rational)"/>
        public static bool operator >=(double left, Rational right) => left * right.Denominator >= right._numerator;
    }
}
