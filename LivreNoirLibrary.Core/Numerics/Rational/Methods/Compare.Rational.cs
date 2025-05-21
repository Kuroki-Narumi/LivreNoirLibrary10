using System;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational : IEquatable<Rational>, IComparable<Rational>, IEqualityOperators<Rational, Rational, bool>, IComparisonOperators<Rational, Rational, bool>
    {
        public bool Equals(Rational other) => _numerator == other._numerator && _denominatorMinusOne == other._denominatorMinusOne;
        public static bool operator ==(Rational left, Rational right) => left._numerator == right._numerator && left._denominatorMinusOne == right._denominatorMinusOne;
        public static bool operator !=(Rational left, Rational right) => left._numerator != right._numerator || left._denominatorMinusOne != right._denominatorMinusOne;

        public int CompareTo(Rational other) => ((double)_numerator * other.Denominator).CompareTo((double)other._numerator * Denominator);

        public static bool operator <(Rational left, Rational right) => ((double)left._numerator * right.Denominator) < ((double)right._numerator * left.Denominator);
        public static bool operator <=(Rational left, Rational right) => ((double)left._numerator * right.Denominator) <= ((double)right._numerator * left.Denominator);
        public static bool operator >(Rational left, Rational right) => ((double)left._numerator * right.Denominator) > ((double)right._numerator * left.Denominator);
        public static bool operator >=(Rational left, Rational right) => ((double)left._numerator * right.Denominator) >= ((double)right._numerator * left.Denominator);

        /// <inheritdoc cref="INumberBase{Rational}.MaxMagnitude(Rational, Rational)"/>
        public static Rational Max(in Rational x, in Rational y) => x < y ? y : x;
        /// <inheritdoc cref="INumberBase{Rational}.MinMagnitude(Rational, Rational)"/>
        public static Rational Min(in Rational x, in Rational y) => x > y ? y : x;
        
        static Rational INumberBase<Rational>.MaxMagnitude(Rational x, Rational y) => Max(x, y);
        static Rational INumberBase<Rational>.MaxMagnitudeNumber(Rational x, Rational y) => Max(x, y);
        static Rational INumberBase<Rational>.MinMagnitude(Rational x, Rational y) => Min(x, y);
        static Rational INumberBase<Rational>.MinMagnitudeNumber(Rational x, Rational y) => Min(x, y);
    }
}
