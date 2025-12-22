using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct BigDouble :
        IComparable<BigDouble>, IComparisonOperators<BigDouble, BigDouble, bool>,
        IEquatable<BigDouble>, IEqualityOperators<BigDouble, BigDouble, bool>
    {
        public override int GetHashCode() => HashCode.Combine(_mantissa, _exponent);
        public bool Equals(BigDouble other) => Mantissa == other.Mantissa && Exponent == other.Exponent;

        public static bool operator ==(BigDouble left, BigDouble right) => left.Equals(right);
        public static bool operator !=(BigDouble left, BigDouble right) => !left.Equals(right);

        private static bool NeedsHandleDoubleMethod(BigDouble left, BigDouble right) => !double.IsFinite(left.Mantissa) || !double.IsFinite(right.Mantissa);
        private static bool NeedsHandleDoubleMethod(double left, double right) => !double.IsFinite(left) || !double.IsFinite(right);

        public int CompareTo(BigDouble other)
        {
            var (lm, le) = this;
            var (rm, re) = other;
            if (NeedsHandleDoubleMethod(lm, rm))
            {
                return lm.CompareTo(rm);
            }
            if (lm is < 0 && rm is > 0)
            {
                return -1;
            }
            if (lm is > 0 && rm is < 0)
            {
                return 1;
            }
            var c = le.CompareTo(re);
            if (c is not 0)
            {
                return lm is < 0 ? -c : c;
            }
            return lm.CompareTo(rm);
        }

        public override bool Equals([NotNullWhen(true)] object? obj) => obj switch
        {
            BigDouble v => Equals(v),
            byte v => Equals(v),
            sbyte v => Equals(v),
            short v => Equals(v),
            ushort v => Equals(v),
            int v => Equals(v),
            uint v => Equals(v),
            long v => Equals(v),
            ulong v => Equals(v),
            float v => Equals(v),
            double v => Equals(v),
            Int128 v => Equals(v),
            UInt128 v => Equals(v),
            Half v => Equals(v),
            _ => false,
        };

        int IComparable.CompareTo(object? obj) => obj switch
        {
            BigDouble v => CompareTo(v),
            byte v => CompareTo(v),
            sbyte v => CompareTo(v),
            short v => CompareTo(v),
            ushort v => CompareTo(v),
            int v => CompareTo(v),
            uint v => CompareTo(v),
            long v => CompareTo(v),
            ulong v => CompareTo(v),
            float v => CompareTo(v),
            double v => CompareTo(v),
            Int128 v => CompareTo(v),
            UInt128 v => CompareTo(v),
            Half v => CompareTo(v),
            null => 1,
            _ => Rational.ThrowIncomparableException(obj),
        };

        private static bool CheckNaN(BigDouble left, BigDouble right) => !(IsNaN(left) || IsNaN(right));

        public static bool operator <(BigDouble left, BigDouble right) => CheckNaN(left, right) && left.CompareTo(right) is < 0;
        public static bool operator <=(BigDouble left, BigDouble right) => CheckNaN(left, right) && left.CompareTo(right) is <= 0;
        public static bool operator >(BigDouble left, BigDouble right) => CheckNaN(left, right) && left.CompareTo(right) is > 0;
        public static bool operator >=(BigDouble left, BigDouble right) => CheckNaN(left, right) && left.CompareTo(right) is >= 0;

        public static BigDouble Max(BigDouble left, BigDouble right)
        {
            if (NeedsHandleDoubleMethod(left, right))
            {
                return Math.Max(left.Mantissa, right.Mantissa);
            }
            return left > right ? left : right;
        }

        public static BigDouble Min(BigDouble left, BigDouble right)
        {
            if (NeedsHandleDoubleMethod(left, right))
            {
                return Math.Min(left.Mantissa, right.Mantissa);
            }
            return left < right ? left : right;
        }

        public static BigDouble MaxMagnitude(BigDouble left, BigDouble right) => Max(left, right);
        public static BigDouble MinMagnitude(BigDouble left, BigDouble right) => Max(left, right);

        public static BigDouble MaxMagnitudeNumber(BigDouble left, BigDouble right)
        {
            if (IsNaN(left))
            {
                return right;
            }
            if (IsNaN(right))
            {
                return left;
            }
            return left > right ? left : right;
        }

        public static BigDouble MinMagnitudeNumber(BigDouble left, BigDouble right)
        {
            if (IsNaN(left))
            {
                return right;
            }
            if (IsNaN(right))
            {
                return left;
            }
            return left < right ? left : right;
        }
    }
}
