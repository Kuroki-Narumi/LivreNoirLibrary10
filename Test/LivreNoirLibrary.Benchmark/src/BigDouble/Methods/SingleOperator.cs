using System;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct BigDouble : 
        IAdditiveIdentity<BigDouble, BigDouble>, IMultiplicativeIdentity<BigDouble, BigDouble>,
        IUnaryPlusOperators<BigDouble, BigDouble>, IUnaryNegationOperators<BigDouble, BigDouble>,
        IIncrementOperators<BigDouble>, IDecrementOperators<BigDouble>,
        IMinMaxValue<BigDouble>
    {
        public static BigDouble Zero { get; } = new(false, 0, 0);
        public static BigDouble One { get; } = new(false, 1, 0);
        public static BigDouble MaxValue { get; } = new(false, double.MaxValue, MaxExponent);
        public static BigDouble MinValue { get; } = new(false, double.MinValue, MaxExponent);

        public static BigDouble NaN { get; } = new(false, double.NaN, 0);
        public static BigDouble PositiveInfinity { get; } = new(false, double.PositiveInfinity, 0);
        public static BigDouble NegativeInfinity { get; } = new(false, double.NegativeInfinity, 0);

        public static BigDouble operator +(BigDouble value) => value;
        public static BigDouble operator -(BigDouble value) => new(false, -value.Mantissa, value.Exponent);
        public static BigDouble operator ++(BigDouble value) => value + 1;
        public static BigDouble operator --(BigDouble value) => value - 1;

        public static BigDouble Abs(BigDouble value) => new(false, Math.Abs(value.Mantissa), value.Exponent);
        public static bool IsZero(BigDouble value) => value.Mantissa is 0;
        public static bool IsPositive(BigDouble value) => double.IsPositive(value.Mantissa);
        public static bool IsNegative(BigDouble value) => double.IsNegative(value.Mantissa);

        public static bool IsNormal(BigDouble value) => double.IsNormal(value.Mantissa);
        public static bool IsSubnormal(BigDouble value) => double.IsSubnormal(value.Mantissa);
        public static bool IsFinite(BigDouble value) => double.IsFinite(value.Mantissa);
        public static bool IsInfinity(BigDouble value) => double.IsInfinity(value.Mantissa);
        public static bool IsPositiveInfinity(BigDouble value) => double.IsPositiveInfinity(value.Mantissa);
        public static bool IsNegativeInfinity(BigDouble value) => double.IsNegativeInfinity(value.Mantissa);
        public static bool IsNaN(BigDouble value) => double.IsNaN(value.Mantissa);

        public static bool IsInteger(BigDouble value) => IsInteger(value);
        public static bool IsEvenInteger(BigDouble value) => IsInteger(value) && (Abs(value % 2) == 0);
        public static bool IsOddInteger(BigDouble value) => IsInteger(value) && (Abs(value % 2) != 0);

        static int INumberBase<BigDouble>.Radix => 10;
        static bool INumberBase<BigDouble>.IsCanonical(BigDouble value) => true;
        static bool INumberBase<BigDouble>.IsComplexNumber(BigDouble value) => false;
        static bool INumberBase<BigDouble>.IsRealNumber(BigDouble value) => true;
        static bool INumberBase<BigDouble>.IsImaginaryNumber(BigDouble value) => false;
        static BigDouble IAdditiveIdentity<BigDouble, BigDouble>.AdditiveIdentity => Zero;
        static BigDouble IMultiplicativeIdentity<BigDouble, BigDouble>.MultiplicativeIdentity => One;
    }
}
