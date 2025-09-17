using LivreNoirLibrary.Debug;
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

        public void Deconstruct(out long numerator, out long denominator)
        {
            numerator = Numerator;
            denominator = Denominator;
        }

        public static implicit operator Rational((int num, int den) tuple) => new(tuple.num, tuple.den);
        public static implicit operator Rational((long num, long den) tuple) => new(tuple.num, tuple.den);
        public static implicit operator Rational(byte value) => new(value);
        public static implicit operator Rational(sbyte value) => new(value);
        public static implicit operator Rational(short value) => new(value);
        public static implicit operator Rational(ushort value) => new(value);
        public static implicit operator Rational(int value) => new(value);
        public static implicit operator Rational(uint value) => new(value);
        public static explicit operator Rational(nint value) => new(value);
        public static explicit operator Rational(nuint value) => new((long)value);
        public static explicit operator checked Rational(nuint value) => new(checked((long)value));
        public static implicit operator Rational(long value) => new(value);
        public static explicit operator Rational(ulong value) => new((long)value);
        public static explicit operator checked Rational(ulong value) => new(checked((long)value));
        public static explicit operator Rational(Int128 value) => new((long)value);
        public static explicit operator checked Rational(Int128 value) => new(checked((long)value));
        public static explicit operator Rational(UInt128 value) => new((long)value);
        public static explicit operator checked Rational(UInt128 value) => new(checked((long)value));
        public static explicit operator Rational(Half value) => ConvertBySBT(value);
        public static explicit operator Rational(float value) => ConvertBySBT(value);
        public static explicit operator Rational(double value) => ConvertBySBT(value);
        public static explicit operator Rational(decimal value) => ConvertBySBT(value);

        public static implicit operator (long, long)(Rational value) => (value.Numerator, value.Denominator);
        public static explicit operator (int, int)(Rational value) => ((int)value.Numerator, (int)value.Denominator);
        public static explicit operator checked (int, int)(Rational value) => (checked((int)value.Numerator), checked((int)value.Denominator));
        public static explicit operator byte(Rational value) => (byte)(value.Numerator / value.Denominator);
        public static explicit operator checked byte(Rational value) => checked((byte)(value.Numerator / value.Denominator));
        public static explicit operator sbyte( Rational value) => (sbyte)(value.Numerator / value.Denominator);
        public static explicit operator checked sbyte(Rational value) => checked((sbyte)(value.Numerator / value.Denominator));
        public static explicit operator short(Rational value) => (short)(value.Numerator / value.Denominator);
        public static explicit operator checked short(Rational value) => checked((short)(value.Numerator / value.Denominator));
        public static explicit operator ushort(Rational value) => (ushort)(value.Numerator / value.Denominator);
        public static explicit operator checked ushort(Rational value) => checked((ushort)(value.Numerator / value.Denominator));
        public static explicit operator int(Rational value) => (int)value.Numerator / (int)value.Denominator;
        public static explicit operator checked int(Rational value) => checked((int)(value.Numerator / value.Denominator));
        public static explicit operator uint(Rational value) => (uint)value.Numerator / (uint)value.Denominator;
        public static explicit operator checked uint(Rational value) => checked((uint)(value.Numerator / value.Denominator));
        public static explicit operator nint(Rational value) => (nint)value.Numerator / (nint)value.Denominator;
        public static explicit operator checked nint(Rational value) => checked((nint)value.Numerator) / (nint)value.Denominator;
        public static explicit operator nuint(Rational value) => (nuint)value.Numerator / (nuint)value.Denominator;
        public static explicit operator checked nuint(Rational value) => checked((nuint)value.Numerator) / (nuint)value.Denominator;
        public static explicit operator long(Rational value) => value.Numerator / value.Denominator;
        public static explicit operator ulong(Rational value) => (ulong)value.Numerator / (ulong)value.Denominator;
        public static explicit operator checked ulong(Rational value) => checked((ulong)value.Numerator) / (ulong)value.Denominator;
        public static explicit operator Int128(Rational value) => (Int128)value.Numerator / (Int128)value.Denominator;
        public static explicit operator UInt128(Rational value) => (UInt128)value.Numerator / (UInt128)value.Denominator;
        public static explicit operator checked UInt128(Rational value) => checked((UInt128)value.Numerator) / (UInt128)value.Denominator;
        public static explicit operator Half(Rational value) => (Half)value.Numerator / (Half)value.Denominator;
        public static explicit operator checked Half(Rational value) => checked((Half)value.Numerator) / checked((Half)value.Denominator);
        public static explicit operator float(Rational value) => (float)value.Numerator / value.Denominator;
        public static explicit operator double(Rational value) => (double)value.Numerator / value.Denominator;
        public static explicit operator decimal(Rational value) => (decimal)value.Numerator / value.Denominator;

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
        public static Rational ConvertBySBT(Half value, ulong denLimit = DefaultConvertDenLimit) => CheckValue(value) ? Zero : ConvertBySBTCore(value, denLimit, new HalfSbtConverter());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational ConvertBySBT(float value, ulong denLimit = DefaultConvertDenLimit) => CheckValue(value) ? Zero : ConvertBySBTCore(value, denLimit, new FloatSbtConverter());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational ConvertBySBT(double value, ulong denLimit = DefaultConvertDenLimit) => CheckValue(value) ? Zero : ConvertBySBTCore(value, denLimit, new DoubleSbtConverter());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational ConvertBySBT(decimal value, ulong denLimit = DefaultConvertDenLimit) => CheckValue(value) ? Zero : ConvertBySBTCore(value, denLimit, new DecimalSbtConverter());

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

        public static (long Numerator, long Denominator) LimitNumDen(long numerator, long denominator, ulong denLimit = DefaultConvertDenLimit)
        {
            var negative = numerator is < 0;
            var num = (Int128)(negative ? -numerator : numerator);
            var den = (Int128)denominator;
            var limit = (Int128)denLimit;
            if (num <= limit && den <= limit)
            {
                return (numerator, denominator);
            }
            Int128 p, q, r, s;
            p = s = 0;
            q = r = 1;
            while (true)
            {
                var pr = p + r;
                var qs = q + s;
                if (pr > limit || qs > limit)
                {
                    break;
                }
                switch ((pr * den).CompareTo(num * qs))
                {
                    case 0:
                        return ((long)pr, (long)qs);
                    case -1:
                        p = pr;
                        q = qs;
                        break;
                    case 1:
                        r = pr;
                        s = qs;
                        break;

                }
            }
            /**
             * a - pq = num/den - p/q
             * rs - a = r/s - num/den
             * 
             * k = *den*q*s
             * X = (a-pq)k = num*q*s - p*den*s
             * Y = (rs-a)k = r*den*q - num*q*s
             */
            var a2 = num * q * s * 2;
            var xx = p * den * s;
            var yy = r * den * q;
            if (a2 < (xx + yy))
            {
                return ((long)(negative ? -p : p), (long)q);
            }
            else
            {
                return ((long)(negative ? -r : r), (long)s);
            }
        }

        public Rational LimitNumDen(ulong denLimit = DefaultConvertDenLimit)
        {
            var (n, d) = LimitNumDen(Numerator, Denominator, denLimit);
            return new Rational(false, n, d);
        }

        public Rational LimitDen(ulong denLimit = DefaultConvertDenLimit)
        {
            var numerator = Numerator;
            var denominator = Denominator;
            var negative = numerator is < 0;
            var num = (Int128)(negative ? -numerator : numerator);
            var den = (Int128)denominator;
            var limit = (Int128)denLimit;
            if (den <= limit)
            {
                return (numerator, denominator);
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
                return ((long)(negative ? -xn : xn), (long)xd);
            }
            else
            {
                return ((long)(negative ? -yn : yn), (long)yd);
            }
        }
    }
}
