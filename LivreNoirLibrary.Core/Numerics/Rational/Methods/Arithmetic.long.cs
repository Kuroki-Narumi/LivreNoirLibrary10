using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational :
        IAdditionOperators<Rational, long, Rational>,
        ISubtractionOperators<Rational, long, Rational>,
        IMultiplyOperators<Rational, long, Rational>,
        IDivisionOperators<Rational, long, Rational>,
        IModulusOperators<Rational, long, Rational>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Rational AddInt(long num, long den, long other, bool check) => new(false, check ? checked(num + other * den) : num + other * den, den);

        public static Rational operator +(Rational left, long right) => AddInt(left._numerator, left.Denominator, right, false);
        public static Rational operator checked +(Rational left, long right) => AddInt(left._numerator, left.Denominator, right, true);

        public static Rational operator -(Rational left, long right) => AddInt(left._numerator, left.Denominator, -right, false);
        public static Rational operator checked -(Rational left, long right) => AddInt(left._numerator, left.Denominator, -right, true);

        /// <inheritdoc cref="operator +(Rational, Rational)"/>
        public static Rational operator +(long left, Rational right) => AddInt(right._numerator, right.Denominator, left, false);
        /// <inheritdoc cref="operator checked +(Rational, Rational)"/>
        public static Rational operator checked +(long left, Rational right) => AddInt(right._numerator, right.Denominator, left, true);

        /// <inheritdoc cref="operator -(Rational, Rational)"/>
        public static Rational operator -(long left, Rational right) => AddInt(-right._numerator, right.Denominator, left, false);
        /// <inheritdoc cref="operator checked -(Rational, Rational)"/>
        public static Rational operator checked -(long left, Rational right) => AddInt(-right._numerator, right.Denominator, left, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Rational MultipyInt(long num, long den, long other, bool check)
        {
            var gcd = den.GCD(other);
            return new(false, check ? checked(num * (other / gcd)) : num * (other / gcd), den / gcd);
        }

        public static Rational operator *(Rational left, long right) => MultipyInt(left._numerator, left.Denominator, right, false);
        public static Rational operator checked *(Rational left, long right) => MultipyInt(left._numerator, left.Denominator, right, true);

        /// <inheritdoc cref="operator *(Rational, Rational)"/>
        public static Rational operator *(long left, Rational right) => MultipyInt(right._numerator, right.Denominator, left, false);
        /// <inheritdoc cref="operator checked *(Rational, Rational)"/>
        public static Rational operator checked *(long left, Rational right) => MultipyInt(right._numerator, right.Denominator, left, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Rational DivideInt(long num, long den, long other, bool check)
        {
            if (other is 0)
            {
                ThrowDivideByZeroException();
            }
            var gcd = num.GCD(other);
            return new(false, num / gcd, check ? checked(den * (other / gcd)) : den * (other / gcd));
        }

        public static Rational operator /(Rational left, long right) => DivideInt(left._numerator, left.Denominator, right, false);
        public static Rational operator checked /(Rational left, long right) => DivideInt(left._numerator, left.Denominator, right, true);

        /// <inheritdoc cref="operator /(Rational, Rational)"/>
        public static Rational operator /(long left, Rational right)
        {
            if (right._numerator is 0)
            {
                ThrowDivideByZeroException();
            }
            return MultipyInt(right.Denominator, right._numerator, left, false);
        }
        /// <inheritdoc cref="operator checked /(Rational, Rational)"/>
        public static Rational operator checked /(long left, Rational right)
        {
            if (right._numerator is 0)
            {
                ThrowDivideByZeroException();
            }
            return MultipyInt(right.Denominator, right._numerator, left, true);
        }

        public static Rational operator %(Rational left, long right)
        {
            if (right is 0)
            {
                ThrowDivideByZeroException();
            }
            var num = left._numerator;
            if (num is 0)
            {
                return Zero;
            }
            else
            {
                var den = left.Denominator;
                var d = den * right;
                return new(num - (num / d) * d, den);
            }
        }

        /// <inheritdoc cref="DivRem(in Rational, in Rational)"/>
        public static (long Quotient, Rational Remainder) DivRem(Rational left, long right)
        {
            if (right is 0)
            {
                ThrowDivideByZeroException();
            }
            var d1 = left.Denominator;
            var n1 = left._numerator;
            var d = d1 * right;
            var a = n1 / d;
            return (a, new(n1 - a * d, d1));
        }
    }
}
