using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct BigDouble
    {
        public static implicit operator BigDouble(byte value) => new((double)value);
        public static implicit operator BigDouble(sbyte value) => new((double)value);
        public static implicit operator BigDouble(short value) => new((double)value);
        public static implicit operator BigDouble(ushort value) => new((double)value);
        public static implicit operator BigDouble(int value) => new((double)value);
        public static implicit operator BigDouble(uint value) => new((double)value);
        public static implicit operator BigDouble(nint value) => new((double)value);
        public static implicit operator BigDouble(nuint value) => new((double)value);
        public static implicit operator BigDouble(long value) => new((double)value);
        public static implicit operator BigDouble(ulong value) => new((double)value);
        public static implicit operator BigDouble(Int128 value) => new((double)value);
        public static implicit operator BigDouble(UInt128 value) => new((double)value);
        public static implicit operator BigDouble(Half value) => new((double)value);
        public static implicit operator BigDouble(float value) => new((double)value);
        public static implicit operator BigDouble(double value) => new((double)value);
        public static implicit operator BigDouble(decimal value) => new((double)value);

        public static explicit operator byte(BigDouble value) => (byte)value.ToDouble();
        public static explicit operator checked byte(BigDouble value) => checked((byte)value.ToDouble());
        public static explicit operator sbyte(BigDouble value) => (sbyte)value.ToDouble();
        public static explicit operator checked sbyte(BigDouble value) => checked((sbyte)value.ToDouble());
        public static explicit operator short(BigDouble value) => (short)value.ToDouble();
        public static explicit operator checked short(BigDouble value) => checked((short)value.ToDouble());
        public static explicit operator ushort(BigDouble value) => (ushort)value.ToDouble();
        public static explicit operator checked ushort(BigDouble value) => checked((ushort)value.ToDouble());
        public static explicit operator int(BigDouble value) => (int)value.ToDouble();
        public static explicit operator checked int(BigDouble value) => checked((int)value.ToDouble());
        public static explicit operator uint(BigDouble value) => (uint)value.ToDouble();
        public static explicit operator checked uint(BigDouble value) => checked((uint)value.ToDouble());
        public static explicit operator long(BigDouble value) => (long)value.ToDouble();
        public static explicit operator checked long(BigDouble value) => checked((long)value.ToDouble());
        public static explicit operator ulong(BigDouble value) => (ulong)value.ToDouble();
        public static explicit operator checked ulong(BigDouble value) => checked((ulong)value.ToDouble());
        public static explicit operator Int128(BigDouble value) => (Int128)value.ToDouble();
        public static explicit operator checked Int128(BigDouble value) => checked((Int128)value.ToDouble());
        public static explicit operator UInt128(BigDouble value) => (UInt128)value.ToDouble();
        public static explicit operator checked UInt128(BigDouble value) => checked((UInt128)value.ToDouble());
        public static explicit operator Half(BigDouble value) => (Half)value.ToDouble();
        public static explicit operator float(BigDouble value) => (float)value.ToDouble();
        public static explicit operator double(BigDouble value) => value.ToDouble();
        public static explicit operator decimal(BigDouble value) => (decimal)value.ToDouble();

        public double ToDouble()
        {
            if (IsNaN(this))
            {
                return double.NaN;
            }
            if (Exponent is > MaxDoubleExp)
            {
                return Mantissa is > 0 ? double.PositiveInfinity : double.NegativeInfinity;
            }
            if (Exponent is < MinDoubleExp)
            {
                return 0;
            }
            if (IsPositiveInfinity(this))
            {
                return double.PositiveInfinity;
            }
            if (IsNegativeInfinity(this))
            {
                return double.NegativeInfinity;
            }
            return Mantissa * PowerOf10(Exponent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigDouble CreateChecked<TOther>(TOther value)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(BigDouble))
            {
                return (BigDouble)(object)value;
            }
            if (!TryConvertFrom(value, out var result) && !TOther.TryConvertToChecked(value, out result))
            {
                Rational.ThrowNotSupportedException();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigDouble CreateSaturating<TOther>(TOther value)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(BigDouble))
            {
                return (BigDouble)(object)value;
            }
            if (!TryConvertFrom(value, out var result) && !TOther.TryConvertToSaturating(value, out result))
            {
                Rational.ThrowNotSupportedException();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigDouble CreateTruncating<TOther>(TOther value)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(BigDouble))
            {
                return (BigDouble)(object)value;
            }
            if (!TryConvertFrom(value, out var result) && !TOther.TryConvertToTruncating(value, out result))
            {
                Rational.ThrowNotSupportedException();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<BigDouble>.TryConvertFromChecked<TOther>(TOther value, out BigDouble result) => TryConvertFrom(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<BigDouble>.TryConvertFromSaturating<TOther>(TOther value, out BigDouble result) => TryConvertFrom(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<BigDouble>.TryConvertFromTruncating<TOther>(TOther value, out BigDouble result) => TryConvertFrom(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<BigDouble>.TryConvertToChecked<TOther>(BigDouble value, [MaybeNullWhen(false)] out TOther result) => TryConvertToChecked(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<BigDouble>.TryConvertToSaturating<TOther>(BigDouble value, [MaybeNullWhen(false)] out TOther result) => TryConvertToSaturating(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<BigDouble>.TryConvertToTruncating<TOther>(BigDouble value, [MaybeNullWhen(false)] out TOther result) => TryConvertToTruncating(value, out result);

        private static bool TryConvertFrom<TOther>(TOther value, out BigDouble result) where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(Half))
            {
                var actualValue = (Half)(object)value;
                result = new((double)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                var actualValue = (float)(object)value;
                result = new(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualValue = (double)(object)value;
                result = new(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                var actualValue = (decimal)(object)value;
                result = new((double)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(byte))
            {
                var actualValue = (byte)(object)value;
                result = new(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                var actualValue = (sbyte)(object)value;
                result = new(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                var actualValue = (short)(object)value;
                result = new(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                var actualValue = (ushort)(object)value;
                result = new(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                var actualValue = (int)(object)value;
                result = new(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                var actualValue = (uint)(object)value;
                result = new(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                var actualValue = (nint)(object)value;
                result = new(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                var actualValue = (nuint)(object)value;
                result = new(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                var actualValue = (long)(object)value;
                result = new(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                var actualValue = (ulong)(object)value;
                result = new(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                var actualValue = (Int128)(object)value;
                result = new((double)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(UInt128))
            {
                var actualValue = (UInt128)(object)value;
                result = new((double)actualValue);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        private static bool TryConvertToChecked<TOther>(BigDouble value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(byte))
            {
                var actualResult = checked((byte)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                var actualResult = checked((sbyte)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                var actualResult = checked((short)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                var actualResult = checked((ushort)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                var actualResult = checked((int)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                var actualResult = checked((uint)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                var actualResult = checked((nint)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                var actualResult = checked((nuint)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                var actualResult = (long)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                var actualResult = checked((ulong)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                var actualResult = checked((Int128)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(UInt128))
            {
                var actualResult = checked((UInt128)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Half))
            {
                var actualResult = checked((Half)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                var actualResult = (float)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualResult = (double)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                var actualResult = (decimal)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        private static bool TryConvertToSaturating<TOther>(BigDouble value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(byte))
            {
                var actualResult = value >= byte.MaxValue ? byte.MaxValue :
                                   IsNegative(value) ? byte.MinValue :
                                   (byte)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                var actualResult = value >= sbyte.MaxValue ? sbyte.MaxValue :
                                   value <= sbyte.MinValue ? sbyte.MinValue :
                                   (sbyte)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                var actualResult = value >= short.MaxValue ? short.MaxValue :
                                   value <= short.MinValue ? short.MinValue :
                                   (short)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                var actualResult = value >= ushort.MaxValue ? ushort.MaxValue :
                                   IsNegative(value) ? ushort.MinValue :
                                   (ushort)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                var actualResult = value >= int.MaxValue ? int.MaxValue :
                                   value <= int.MinValue ? int.MinValue :
                                   (int)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                var actualResult = value >= uint.MaxValue ? uint.MaxValue :
                                   IsNegative(value) ? uint.MinValue :
                                   (uint)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                var actualResult = value >= nint.MaxValue ? nint.MaxValue :
                                   value <= nint.MinValue ? nint.MinValue :
                                   (nint)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                var actualResult = value >= nuint.MaxValue ? nuint.MaxValue :
                                   IsNegative(value) ? nuint.MinValue :
                                   (nuint)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                var actualResult = (long)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                var actualResult = IsNegative(value) ? ulong.MinValue : (ulong)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                var actualResult = (Int128)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(UInt128))
            {
                var actualResult = IsNegative(value) ? UInt128.MinValue : (ulong)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Half))
            {
                var actualResult = value >= (float)Half.MaxValue ? Half.MaxValue :
                                   value <= (float)Half.MinValue ? Half.MinValue :
                                   (Half)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                var actualResult = (float)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualResult = (double)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                var dValue = (double)value;
                var actualResult = double.IsNaN(dValue) ? 0 :
                                   dValue > (double)decimal.MaxValue ? decimal.MaxValue : 
                                   dValue < (double)decimal.MinValue ? decimal.MinValue :
                                   (decimal)dValue;
                result = (TOther)(object)actualResult;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        private static bool TryConvertToTruncating<TOther>(BigDouble value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(byte))
            {
                var actualResult = (byte)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                var actualResult = (sbyte)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                var actualResult = (short)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                var actualResult = (ushort)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(int))
            {
                var actualResult = (int)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                var actualResult = (uint)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                var actualResult = (nint)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                var actualResult = (nuint)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                var actualResult = (long)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                var actualResult = (ulong)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                var actualResult = (Int128)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(UInt128))
            {
                var actualResult = (UInt128)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(Half))
            {
                var actualResult = (Half)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                var actualResult = (float)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualResult = (double)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                var actualResult = (decimal)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
    }
}