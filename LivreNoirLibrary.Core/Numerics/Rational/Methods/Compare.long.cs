using System;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational : IEquatable<long>, IComparable<long>, IEqualityOperators<Rational, long, bool>, IComparisonOperators<Rational, long, bool>
    {
        public bool Equals(long other) => _denominatorMinusOne is 0 && _numerator == other;
        public static bool operator ==(Rational left, long right) => left._denominatorMinusOne is 0 && left._numerator == right;
        public static bool operator !=(Rational left, long right) => left._denominatorMinusOne is not 0 || left._numerator != right;
        /// <inheritdoc cref="operator==(Rational, Rational)"/>
        public static bool operator ==(long left, Rational right) => right._denominatorMinusOne is 0 && left == right._numerator;
        /// <inheritdoc cref="operator!=(Rational, Rational)"/>
        public static bool operator !=(long left, Rational right) => right._denominatorMinusOne is not 0 || left != right._numerator;

        public int CompareTo(long other) => ((double)_numerator).CompareTo((double)other * Denominator);

        public static bool operator <(Rational left, long right) => left._numerator < (double)right * left.Denominator;
        public static bool operator <=(Rational left, long right) => left._numerator <= (double)right * left.Denominator;
        public static bool operator >(Rational left, long right) => left._numerator > (double)right * left.Denominator;
        public static bool operator >=(Rational left, long right) => left._numerator >= (double)right * left.Denominator;

        /// <inheritdoc cref="operator&lt;(Rational, Rational)"/>
        public static bool operator <(long left, Rational right) => (double)left * right.Denominator < right._numerator;
        /// <inheritdoc cref="operator&lt;=(Rational, Rational)"/>
        public static bool operator <=(long left, Rational right) => (double)left * right.Denominator <= right._numerator;
        /// <inheritdoc cref="operator&gt;(Rational, Rational)"/>
        public static bool operator >(long left, Rational right) => (double)left * right.Denominator > right._numerator;
        /// <inheritdoc cref="operator&gt;=(Rational, Rational)"/>
        public static bool operator >=(long left, Rational right) => (double)left * right.Denominator >= right._numerator;
    }
}
