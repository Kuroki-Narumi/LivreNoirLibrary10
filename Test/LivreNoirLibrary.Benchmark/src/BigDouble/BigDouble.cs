using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct BigDouble : INumber<BigDouble>
    {
        private readonly double _mantissa;
        private readonly long _exponent;

        /// <summary>
        /// A <see langword="double"/> value representing the mantissa of this instance, its absolute value normalized to 1 &lt;= m &lt; 10 whenever possible.
        /// </summary>
        public double Mantissa => _mantissa;
        /// <summary>
        /// A <see langword="long"/> value representing the power of 10 multiplied by this instance.
        /// </summary>
        public long Exponent => _exponent;

        private BigDouble(bool _, double mantissa, long exponent)
        {
            _mantissa = mantissa;
            _exponent = exponent;
        }

        public BigDouble(double mantissa, long exponent = 0)
        {
            this = Normalize(mantissa, exponent);
        }

        public void Deconstruct(out double mantissa, out long exponent)
        {
            mantissa = _mantissa;
            exponent = _exponent;
        }

        public const int MaxSignificantDigits = 17;
        public const long MaxDoubleExp = 308;
        public const long MinDoubleExp = -324;
        public const long MaxExponent = long.MaxValue - MaxDoubleExp;
        public const long MinExponent = -long.MaxValue - MinDoubleExp;

        public static BigDouble Normalize(double mantissa, long exponent)
        {
            if (double.IsNaN(mantissa))
            {
                return NaN;
            }
            if (double.IsPositiveInfinity(mantissa))
            {
                return PositiveInfinity;
            }
            if (double.IsNegativeInfinity(mantissa))
            {
                return NegativeInfinity;
            }
            if (mantissa is 0)
            {
                return Zero;
            }
            if (mantissa is >= 1 and < 10)
            {
                return new(false, mantissa, exponent);
            }
            var exp = (long)Math.Floor(Math.Log10(Math.Abs(mantissa)));
            mantissa /= PowerOf10(exp);
            return new(false, mantissa, Math.Clamp(exponent, MinExponent, MaxExponent) + exp);
        }

        private static readonly double[] _powersOf10 = CreatePowersOf10();

        private static double[] CreatePowersOf10()
        {
            var ary = new double[MaxDoubleExp - MinDoubleExp + 1];
            for (var exp = MinDoubleExp; exp <= MaxDoubleExp; exp++)
            {
                ary[exp - MinDoubleExp] = Math.Pow(10, exp);
            }
            return ary;
        }

        private static double PowerOf10(long exp) => _powersOf10[exp - MinDoubleExp];
    }
}