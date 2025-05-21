using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational
    {
        private const long FloatDenominator = 4503599627370496L; // 2^52
        private const int DecimalMaxScale = 18;
        public const decimal DecimalMaxDenominator = 100_000_000_000_000_000;
        public const ulong DefaultConvertDenLimit = 10_000_000_000;
        public static readonly Int128 Int128MaxValueAsLong = long.MaxValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out long numerator, out long denominator)
        {
            numerator = _numerator;
            denominator = Denominator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Rational(in (int num, int den) tuple) => new(tuple.num, tuple.den);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Rational(in (long num, long den) tuple) => new(tuple.num, tuple.den);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Rational(byte value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Rational(sbyte value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Rational(short value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Rational(ushort value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Rational(int value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Rational(uint value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rational(nint value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rational(nuint value) => new((long)value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Rational(long value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rational(ulong value) => new((long)value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked Rational(ulong value) => new(checked((long)value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked Rational(nuint value) => new(checked((long)value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rational(Int128 value) => new((long)value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked Rational(Int128 value) => new(checked((long)value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rational(UInt128 value) => new((long)value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked Rational(UInt128 value) => new(checked((long)value));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rational(Half value) => ConvertBySBT(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rational(float value) => ConvertBySBT(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rational(double value) => ConvertBySBT(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Rational(decimal value) => ConvertBySBT(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator byte(in Rational value) => (byte)(value._numerator / value.Denominator);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked byte(in Rational value) => checked((byte)(value._numerator / value.Denominator));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator sbyte(in Rational value) => (sbyte)(value._numerator / value.Denominator);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked sbyte(in Rational value) => checked((sbyte)(value._numerator / value.Denominator));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator short(in Rational value) => (short)(value._numerator / value.Denominator);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked short(in Rational value) => checked((short)(value._numerator / value.Denominator));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ushort(in Rational value) => (ushort)(value._numerator / value.Denominator);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked ushort(in Rational value) => checked((ushort)(value._numerator / value.Denominator));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(in Rational value) => (int)value._numerator / (int)value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked int(in Rational value) => checked((int)(value._numerator / value.Denominator));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(in Rational value) => (uint)value._numerator / (uint)value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked uint(in Rational value) => checked((uint)(value._numerator / value.Denominator));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator nint(in Rational value) => (nint)value._numerator / (nint)value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked nint(in Rational value) => checked((nint)value._numerator) / (nint)value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator nuint(in Rational value) => (nuint)value._numerator / (nuint)value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked nuint(in Rational value) => checked((nuint)value._numerator) / (nuint)value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(in Rational value) => value._numerator / value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ulong(in Rational value) => (ulong)value._numerator / (ulong)value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked ulong(in Rational value) => checked((ulong)value._numerator) / (ulong)value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Int128(in Rational value) => (Int128)value._numerator / (Int128)value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator UInt128(in Rational value) => (UInt128)value._numerator / (UInt128)value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked UInt128(in Rational value) => checked((UInt128)value._numerator) / (UInt128)value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Half(in Rational value) => (Half)value._numerator / (Half)value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator checked Half(in Rational value) => checked((Half)value._numerator) / checked((Half)value.Denominator);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(in Rational value) => (float)value._numerator / value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(in Rational value) => (double)value._numerator / value.Denominator;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator decimal(in Rational value) => (decimal)value._numerator / value.Denominator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckValue(Half value)
        {
            if (!Half.IsFinite(value))
            {
                ThrowOverflowException();
            }
            return value == Half.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckValue(float value)
        {
            if (!float.IsFinite(value) || (value is > long.MaxValue or < long.MinValue))
            {
                ThrowOverflowException();
            }
            return value is 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckValue(double value)
        {
            if (!double.IsFinite(value) || (value is > long.MaxValue or < long.MinValue))
            {
                ThrowOverflowException();
            }
            return value is 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool CheckValue(decimal value)
        {
            if (value is > long.MaxValue or < long.MinValue)
            {
                ThrowOverflowException();
            }
            return value is 0;
        }

        public static Rational ConvertByDecimal(float value)
        {
            if (CheckValue(value))
            {
                return Zero;
            }
            var den = FloatDenominator;
            value *= den;
            while (value is > long.MaxValue or < long.MinValue)
            {
                value /= 2;
                den >>= 1;
            }
            return new((long)value, den);
        }

        public static Rational ConvertByDecimal(double value)
        {
            if (CheckValue(value))
            {
                return Zero;
            }
            var den = FloatDenominator;
            value *= den;
            while (value is > long.MaxValue or < long.MinValue)
            {
                value /= 2;
                den >>= 1;
            }
            return new((long)value, den);
        }

        public static Rational ConvertByDecimal(decimal value)
        {
            if (CheckValue(value))
            {
                return Zero;
            }
            var scale = value.Scale;
            decimal den;
            var flip2 = true;
            if (scale is 0)
            {
                den = 1m;
            }
            else if (scale is >= DecimalMaxScale)
            {
                den = DecimalMaxDenominator;
            }
            else
            {
                den = decimal.Parse($"1{new string('0', scale)}");
            }
            value *= den;
            while (value is > long.MaxValue or < long.MinValue)
            {
                if (flip2)
                {
                    den /= 2m;
                    value /= 2m;
                }
                else
                {
                    den /= 5m;
                    value /= 5m;
                }
                flip2 = !flip2;
            }
            return new((long)value, (long)den);
        }

        private interface ISbtConverter<T>
        {
            T Convert2T(Int128 value);
            Int128 Convert2Int128(T value);
        }

        private static Rational ConvertBySBTCore<T, TConv>(T value, Int128 limit, TConv conv)
            where T : INumber<T>
            where TConv : ISbtConverter<T>
        {
            var negative = false;
            if (T.IsNegative(value))
            {
                negative = true;
                value = -value;
            }
            var intPart = conv.Convert2Int128(value);
            var decPart = value - conv.Convert2T(intPart);
            if (T.IsZero(decPart))
            {
                return new((long)(negative ? -intPart : intPart));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            Int128 Clamp(T num1, T den1, Int128 num2, Int128 den2)
            {
                if (T.IsZero(den1))
                {
                    return 1;
                }
                var x = conv.Convert2Int128(num1 / den1) - 1;
                var y = (limit - num2) / den2;
                if (x > y)
                {
                    x = y;
                }
                return x < 1 ? 1 : x;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            Rational Return((Int128 num, Int128 den) tuple)
            {
                var n = (long)(intPart * tuple.den + tuple.num);
                return new(true, negative ? -n : n, (long)tuple.den);
            }

            /*
             * left : (  p  ,   q  , p + r, q + s)
             * right: (p + r, q + s,   r  ,   s  )
             */
            Int128 p = 0, q, r, s;
            q = r = s = 1;

            T dp = T.Zero, dq, dr, ds;
            dq = dr = ds = T.One;

            // main loop
            while (true)
            {
                var pr = p + r;
                var qs = q + s;
                // denominator is over the limit
                if (pr > limit || qs > limit)
                {
                    break;
                }
                // actual current value in the target type
                /*
                 *  var current = conv.Convert2T(pr) / conv.Convert2T(qs);
                 *  Compare(decPart, current)
                 *  
                 *  <=> Compare(decPart * dqs, dpr)
                 */
                var compare = (decPart * conv.Convert2T(qs)).CompareTo(conv.Convert2T(pr));
                if (compare is 0)
                {
                    return Return((pr, qs));
                }
                /*
                 * Reference: https://qiita.com/okaponta_/items/36d485004d04b37519a3
                 * 
                 * t = pt / qt
                 * pt * qm < qt * pm <=> (pt / qt) * qm < pm
                 *                   <=> (pt / qt) < (pm / qm)
                 */
                var drs = dr - decPart * ds;
                var dqp = decPart * dq - dp;
                if (compare is < 0)
                {
                    var x = Clamp(drs, dqp, s, q);
                    r += p * x;
                    s += q * x;
                    dr = conv.Convert2T(r);
                    ds = conv.Convert2T(s);
                }
                else
                {
                    var x = Clamp(dqp, drs, q, s);
                    p += r * x;
                    q += s * x;
                    dp = conv.Convert2T(p);
                    dq = conv.Convert2T(q);
                }
            }
            /*
             *  var d1 = decPart - dp / dq;
             *  var d2 = dr / ds - decPart;
             *  if (d1 < d2)
             */
            var d0 = decPart * dq * ds;
            return Return((d0 - dp * ds) < (dr * dq - d0) ? (p, q) : (r, s));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational ConvertBySBT(Half value, ulong denLimit = DefaultConvertDenLimit) =>
            CheckValue(value) ? Zero : ConvertBySBTCore(value, denLimit, new HalfSbtConverter());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational ConvertBySBT(float value, ulong denLimit = DefaultConvertDenLimit) => 
            CheckValue(value) ? Zero : ConvertBySBTCore(value, denLimit, new FloatSbtConverter());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational ConvertBySBT(double value, ulong denLimit = DefaultConvertDenLimit) => 
            CheckValue(value) ? Zero : ConvertBySBTCore(value, denLimit, new DoubleSbtConverter());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational ConvertBySBT(decimal value, ulong denLimit = DefaultConvertDenLimit) => 
            CheckValue(value) ? Zero : ConvertBySBTCore(value, denLimit, new DecimalSbtConverter());

        private readonly struct HalfSbtConverter : ISbtConverter<Half>
        {
            public Half Convert2T(Int128 value) => (Half)value;
            public Int128 Convert2Int128(Half value) => (Int128)value;
        }

        private readonly struct FloatSbtConverter : ISbtConverter<float>
        {
            public float Convert2T(Int128 value) => (float)value;
            public Int128 Convert2Int128(float value) => (Int128)value;
        }

        private readonly struct DoubleSbtConverter : ISbtConverter<double>
        {
            public double Convert2T(Int128 value) => (double)value;
            public Int128 Convert2Int128(double value) => (Int128)value;
        }

        private readonly struct DecimalSbtConverter : ISbtConverter<decimal>
        {
            public decimal Convert2T(Int128 value) => (decimal)value;
            public Int128 Convert2Int128(decimal value) => (Int128)value;
        }

        public Rational LimitDen(ulong denLimit = DefaultConvertDenLimit)
        {
            var n = _numerator;
            var negative = n is < 0;
            var num = (Int128)(negative ? -n : n);
            var den = (Int128)(_denominatorMinusOne + 1);
            var limit = (Int128)denLimit;
            if (den <= limit)
            {
                return this;
            }

            // Reference: https://atcoder.jp/contests/abc333/editorial/7937
            Int128 p, q, n1, n2, d1, d2, xn, xd, yn, yd;
            p = num;
            q = den;
            n1 = 1;
            n2 = 0;
            d1 = 0;
            d2 = 1;
            var depth_parity = false;

            while (true)
            {
                var quo = p / q;
                var max_q = d1 == 0 ? (limit - n2) / n1 : (limit - d2) / d1;
                if (quo >= max_q)
                {
                    if (depth_parity)
                    {
                        xn = n1;
                        xd = d1;
                        yn = n1 * max_q + n2;
                        yd = d1 * max_q + d2;
                    }
                    else
                    {
                        xn = n1 * max_q + n2;
                        xd = d1 * max_q + d2;
                        yn = n1;
                        yd = d1;
                    }
                    break;
                }
                (n1, n2) = (n1 * quo + n2, n1);
                (d1, d2) = (d1 * quo + d2, d1);
                (p, q) = (q, p % q);
                depth_parity = !depth_parity;
            }

            /**
             * a - x = num/den - xn/xd
             * y - a = yn/yd - num/den
             * 
             * k = *den*xd*yd
             * X = (a-x)k = num*xd*yd - xn*den*yd
             * Y = (y-a)k = yn*den*xd - num*xd*yd
             */

            var a2 = num * xd * yd * 2;
            var xx = xn * den * yd;
            var yy = yn * den * xd;
            if (a2 < (xx + yy))
            {
                return new(true, (long)(negative ? -xn : xn), (long)xd);
            }
            else
            {
                return new(true, (long)(negative ? -yn : yn), (long)yd);
            }
        }
    }
}
