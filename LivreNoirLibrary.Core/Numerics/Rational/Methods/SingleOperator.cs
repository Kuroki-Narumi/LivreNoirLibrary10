using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational : 
        IAdditiveIdentity<Rational, Rational>, IMultiplicativeIdentity<Rational, Rational>, 
        IUnaryPlusOperators<Rational, Rational>, IUnaryNegationOperators<Rational, Rational>, 
        IIncrementOperators<Rational>, IDecrementOperators<Rational>,
        IMinMaxValue<Rational>
    {
        public static Rational Zero { get; } = new(false, 0, 1);
        public static Rational One { get; } = new(false, 1, 1);
        public static Rational MinusOne { get; } = new(false, -1, 1);
        public static Rational MaxValue { get; } = new(false, long.MaxValue, 1);
        public static Rational MinValue { get; } = new(false, long.MinValue, 1);
        public static Rational Epsilon { get; } = new(false, 1, long.MaxValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational operator +(Rational value) => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational operator -(Rational value) => new(false, -value._numerator, value.Denominator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational operator ++(Rational value)
        {
            var den = value.Denominator;
            return new(value._numerator + den, den);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational operator --(Rational value)
        {
            var den = value.Denominator;
            return new(value._numerator - den, den);
        }

        /// <inheritdoc cref="Abs(Rational)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rational Abs() => _numerator is >= 0 ? this : new(false, -_numerator, Denominator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rational Invert()
        {
            var num = _numerator;
            var den = _denominatorMinusOne + 1;
            if (den is <= 0)
            {
                ThrowDivideByZeroException(DenominatorMustBeNonZero);
            }
            if (num is < 0)
            {
                den = -den;
                num = -num;
            }
            return new(false, den, num);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float InvertToFloat() => (float)Denominator / _numerator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double InvertToDouble() => (double)Denominator / _numerator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal InvertToDecimal() => (decimal)Denominator / _numerator;

        /// <inheritdoc cref="IsZero(Rational)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsZero() => _numerator is 0;

        /// <inheritdoc cref="IsPositive(Rational)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsPositive() => _numerator is >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsPositiveThanZero() => _numerator is > 0;

        /// <inheritdoc cref="IsNegative(Rational)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNegative() => _numerator is < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNegativeOrZero() => _numerator is <= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rational Remainder()
        {
            var den = Denominator;
            var r = _numerator % den;
            return new(false, r, den);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (long Quotient, Rational Remainder) DivRem()
        {
            var den = Denominator;
            var (q, r) = Math.DivRem(_numerator, den);
            return (q, new(false, r, den));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (long Quotient, long RemainderNum, long RemainderDen) DivRem2()
        {
            var den = Denominator;
            var (q, r) = Math.DivRem(_numerator, den);
            return (q, r, den);
        }

        public Rational Round(Rational unit, MidpointRounding mode = MidpointRounding.ToEven)
        {
            var un = unit._numerator;
            var ud = unit.Denominator;
            return new((long)Math.Round((double)_numerator * ud / ((double)Denominator * un), mode) * un, ud);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Round(long factor = 1, MidpointRounding mode = MidpointRounding.ToEven) => (long)Math.Round((double)_numerator * factor / Denominator, mode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Truncate(long factor = 1) => (long)Math.Truncate((double)_numerator * factor / Denominator);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Floor(long factor = 1) => (long)Math.Floor((double)_numerator * factor / Denominator);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Ceiling(long factor = 1) => (long)Math.Ceiling((double)_numerator * factor / Denominator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational Abs(Rational value) => value.Abs();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(Rational value) => value.IsZero();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositive(Rational value) => value.IsPositive();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositiveThanZero(Rational value) => value.IsPositiveThanZero();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegative(Rational value) => value.IsNegative();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegativeOrZero(Rational value) => value.IsNegativeOrZero();

        static Rational IAdditiveIdentity<Rational, Rational>.AdditiveIdentity => Zero;
        static Rational IMultiplicativeIdentity<Rational, Rational>.MultiplicativeIdentity => One;
        static int INumberBase<Rational>.Radix => 10;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsInteger(Rational value) => value._denominatorMinusOne is 0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsOddInteger(Rational value) => value._denominatorMinusOne is 0 && long.IsOddInteger(value._numerator);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsEvenInteger(Rational value) => value._denominatorMinusOne is 0 && long.IsEvenInteger(value._numerator);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsRealNumber(Rational value) => true;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsImaginaryNumber(Rational value) => false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsComplexNumber(Rational value) => true;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsCanonical(Rational value) => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsNormal(Rational value) => true;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsSubnormal(Rational value) => false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsFinite(Rational value) => true;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsInfinity(Rational value) => false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsPositiveInfinity(Rational value) => false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsNegativeInfinity(Rational value) => false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.IsNaN(Rational value) => false;
    }
}
