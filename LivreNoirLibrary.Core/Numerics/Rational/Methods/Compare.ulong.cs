using System;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational : IEquatable<ulong>, IComparable<ulong>, IEqualityOperators<Rational, ulong, bool>, IComparisonOperators<Rational, ulong, bool>
    {
        public bool Equals(ulong other) => _denominatorMinusOne is 0 && other <= long.MaxValue && _numerator == (long)other;
        public static bool operator ==(Rational left, ulong right) => left._denominatorMinusOne is 0 && right is <= long.MaxValue && left._numerator == (long)right;
        public static bool operator !=(Rational left, ulong right) => left._denominatorMinusOne is not 0 || right is > long.MaxValue || left._numerator != (long)right;
        /// <inheritdoc cref="operator==(Rational, Rational)"/>
        public static bool operator ==(ulong left, Rational right) => right._denominatorMinusOne is 0 && left is <= long.MaxValue && (long)left == right._numerator;
        /// <inheritdoc cref="operator!=(Rational, Rational)"/>
        public static bool operator !=(ulong left, Rational right) => right._denominatorMinusOne is not 0 || left is > long.MaxValue || (long)left != right._numerator;

        public int CompareTo(ulong other) => ((double)_numerator).CompareTo((double)other * Denominator);

        public static bool operator <(Rational left, ulong right) => left._numerator < (double)right * left.Denominator;
        public static bool operator <=(Rational left, ulong right) => left._numerator <= (double)right * left.Denominator;
        public static bool operator >(Rational left, ulong right) => left._numerator > (double)right * left.Denominator;
        public static bool operator >=(Rational left, ulong right) => left._numerator >= (double)right * left.Denominator;

        /// <inheritdoc cref="operator&lt;(Rational, Rational)"/>
        public static bool operator <(ulong left, Rational right) => (double)left * right.Denominator < right._numerator;
        /// <inheritdoc cref="operator&lt;=(Rational, Rational)"/>
        public static bool operator <=(ulong left, Rational right) => (double)left * right.Denominator <= right._numerator;
        /// <inheritdoc cref="operator&gt;(Rational, Rational)"/>
        public static bool operator >(ulong left, Rational right) => (double)left * right.Denominator > right._numerator;
        /// <inheritdoc cref="operator&gt;=(Rational, Rational)"/>
        public static bool operator >=(ulong left, Rational right) => (double)left * right.Denominator >= right._numerator;
    }
}
