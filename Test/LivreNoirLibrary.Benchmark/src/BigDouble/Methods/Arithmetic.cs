using System;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct BigDouble :
        IAdditionOperators<BigDouble, BigDouble, BigDouble>,
        ISubtractionOperators<BigDouble, BigDouble, BigDouble>,
        IMultiplyOperators<BigDouble, BigDouble, BigDouble>,
        IDivisionOperators<BigDouble, BigDouble, BigDouble>,
        IModulusOperators<BigDouble, BigDouble, BigDouble>
    {
        public static BigDouble operator +(BigDouble left, BigDouble right)
        {
            var (lm, le) = left;
            var (rm, re) = right;
            if (lm is 0)
            {
                return right;
            }
            if (rm is 0)
            {
                return left;
            }
            // infinite
            if (NeedsHandleDoubleMethod(lm, rm))
            {
                return lm + rm;
            }
            // set left to bigger exponent
            if (re > le)
            {
                (left, right) = (right, left);
                (le, re) = (re, le);
                (lm, rm) = (rm, lm);
            }
            var diff = le - re;
            if (diff > MaxSignificantDigits)
            {
                return left;
            }
            return Normalize(1e14 * lm + 1e14 * rm * PowerOf10(-diff), le - 14);
        }

        public static BigDouble operator -(BigDouble left, BigDouble right) => left + -right;

        public static BigDouble operator *(BigDouble left, BigDouble right)
        {
            var (lm, le) = left;
            var (rm, re) = right;
            // infinite
            if (NeedsHandleDoubleMethod(lm, rm))
            {
                return lm * rm;
            }
            return Normalize(lm * rm, le + re);
        }

        public static BigDouble Reciprocate(BigDouble value) => Normalize(1 / value.Mantissa, -value.Exponent);

        public static BigDouble operator /(BigDouble left, BigDouble right) => left * new BigDouble(false, 1 / right.Mantissa, -right.Exponent);

        public static BigDouble operator %(BigDouble left, BigDouble right)
        {
            throw new NotImplementedException();
        }
    }
}
