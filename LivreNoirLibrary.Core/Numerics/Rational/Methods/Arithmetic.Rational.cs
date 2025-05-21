using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational : 
        IAdditionOperators<Rational, Rational, Rational>, 
        ISubtractionOperators<Rational, Rational, Rational>, 
        IMultiplyOperators<Rational, Rational, Rational>, 
        IDivisionOperators<Rational, Rational, Rational>, 
        IModulusOperators<Rational, Rational, Rational>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Rational Add(long n1, long d1, long n2, long d2, bool check)
        {
            if (n1 is 0)
            {
                return new(false, n2, d2);
            }
            else if (n2 is 0)
            {
                return new(false, n1, d1);
            }
            else if (d1 == d2)
            {
                var n12 = check ? checked(n1 + n2) : n1 + n2;
                var gcd = n12.GCD(d1);
                return new(false, n12 / gcd, d1 / gcd);
            }
            else if (d1 is 1)
            {
                return new(false, check ? checked(n2 + n1 * d2) : n2 + n1 * d2, d2);
            }
            else if (d2 is 1)
            {
                return new(false, check ? checked(n1 + n2 * d1) : n1 + n2 * d1, d1);
            }
            else
            {
                var denGcd = d1.GCD(d2);
                d1 /= denGcd;
                d2 /= denGcd;
                var newN = check ? checked(n1 * d2 + n2 * d1) : n1 * d2 + n2 * d1;
                var newD = check ? checked(d1 * d2 * denGcd) : d1 * d2 * denGcd;
                var gcd = newN.GCD(newD);
                return new(false, newN / gcd, newD / gcd);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Rational Multiply(long n1, long d1, long n2, long d2, bool check)
        {
            if (d2 is 0)
            {
                ThrowDivideByZeroException();
            }
            if (n1 is 0 || n2 is 0)
            {
                return Zero;
            }
            else
            {
                var n1d2Gcd = n1.GCD(d2);
                var n2d1Gcd = n2.GCD(d1);
                n1 /= n1d2Gcd;
                n2 /= n2d1Gcd;
                d1 /= n2d1Gcd;
                d2 /= n1d2Gcd;
                if (check)
                {
                    return new(false, checked(n1 * n2), checked(d1 * d2));
                }
                else
                {
                    return new(false, n1 * n2, d1 * d2);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational operator +(Rational left, Rational right) => Add(left._numerator, left.Denominator, right._numerator, right.Denominator, false);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational operator checked +(Rational left, Rational right) => Add(left._numerator, left.Denominator, right._numerator, right.Denominator, true);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational operator -(Rational left, Rational right) => Add(left._numerator, left.Denominator, -right._numerator, right.Denominator, false);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational operator checked -(Rational left, Rational right) => Add(left._numerator, left.Denominator, -right._numerator, right.Denominator, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational operator *(Rational left, Rational right) => Multiply(left._numerator, left.Denominator, right._numerator, right.Denominator, false);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational operator checked *(Rational left, Rational right) => Multiply(left._numerator, left.Denominator, right._numerator, right.Denominator, true);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational operator /(Rational left, Rational right) => Multiply(left._numerator, left.Denominator, right.Denominator, right._numerator, false);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational operator checked /(Rational left, Rational right) => Multiply(left._numerator, left.Denominator, right.Denominator, right._numerator, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational operator %(Rational left, Rational right) => (left / right).Remainder();

        /// <inheritdoc cref="int.DivRem"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (long Quotient, Rational Remainder) DivRem(in Rational left, in Rational right) => (left / right).DivRem();
    }
}
