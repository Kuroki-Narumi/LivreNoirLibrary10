using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational : INumberBase<Rational>
    {
        private static void ThrowNotSupportedException() => throw new NotSupportedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational CreateChecked<TOther>(TOther value)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(Rational))
            {
                return (Rational)(object)value;
            }
            if (!TryConvertFromChecked(value, out var result) && !TOther.TryConvertToChecked(value, out result))
            {
                ThrowNotSupportedException();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational CreateSaturating<TOther>(TOther value)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(Rational))
            {
                return (Rational)(object)value;
            }
            if (!TryConvertFromSaturating(value, out var result) && !TOther.TryConvertToSaturating(value, out result))
            {
                ThrowNotSupportedException();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rational CreateTruncating<TOther>(TOther value)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(Rational))
            {
                return (Rational)(object)value;
            }
            if (!TryConvertFromTruncating(value, out var result) && !TOther.TryConvertToTruncating(value, out result))
            {
                ThrowNotSupportedException();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.TryConvertFromChecked<TOther>(TOther value, out Rational result) => TryConvertFromChecked(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromChecked<TOther>(TOther value, out Rational result)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(Half))
            {
                var actualValue = (Half)(object)value;
                result = ConvertBySBT(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                var actualValue = (float)(object)value;
                result = ConvertBySBT(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualValue = (double)(object)value;
                result = ConvertBySBT(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                var actualValue = (decimal)(object)value;
                result = ConvertBySBT(actualValue);
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
                result = new(checked((long)actualValue));
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
                result = new(checked((long)actualValue));
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                var actualValue = (Int128)(object)value;
                result = new(checked((long)actualValue));
                return true;
            }
            else if (typeof(TOther) == typeof(UInt128))
            {
                var actualValue = (UInt128)(object)value;
                result = new(checked((long)actualValue));
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.TryConvertFromSaturating<TOther>(TOther value, out Rational result) => TryConvertFromSaturating(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromSaturating<TOther>(TOther value, out Rational result)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(Half))
            {
                var actualValue = (Half)(object)value;
                result = Half.IsPositiveInfinity(actualValue) ? MaxValue :
                         Half.IsNegativeInfinity(actualValue) ? MinValue :
                         ConvertBySBT(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                var actualValue = (float)(object)value;
                result = actualValue is <= long.MinValue ? MaxValue :
                         actualValue is >= long.MaxValue ? MinValue :
                         ConvertBySBT(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualValue = (double)(object)value;
                result = actualValue is <= long.MinValue ? MaxValue :
                         actualValue is >= long.MaxValue ? MinValue :
                         ConvertBySBT(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                var actualValue = (decimal)(object)value;
                result = actualValue is <= long.MinValue ? MaxValue :
                         actualValue is >= long.MaxValue ? MinValue :
                         ConvertBySBT(actualValue);
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
                result = (ulong)actualValue is >= long.MaxValue ? MaxValue :
                         new((long)actualValue);
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
                result = actualValue is >= long.MaxValue ? MaxValue :
                         new((long)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                var actualValue = (Int128)(object)value;
                result = actualValue >= long.MaxValue ? MaxValue :
                         actualValue <= long.MinValue ? MinValue :
                         new((long)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(UInt128))
            {
                var actualValue = (UInt128)(object)value;
                result = actualValue >= long.MaxValue ? MaxValue :
                         new((long)actualValue);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.TryConvertFromTruncating<TOther>(TOther value, out Rational result) => TryConvertFromTruncating(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromTruncating<TOther>(TOther value, out Rational result)
            where TOther : INumberBase<TOther>
        {
            if (typeof(TOther) == typeof(Half))
            {
                var actualValue = (Half)(object)value;
                result = Half.IsPositiveInfinity(actualValue) ? MaxValue :
                         Half.IsNegativeInfinity(actualValue) ? MinValue :
                         ConvertBySBT(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                var actualValue = (float)(object)value;
                result = actualValue is <= long.MinValue ? MaxValue :
                         actualValue is >= long.MaxValue ? MinValue :
                         ConvertBySBT(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualValue = (double)(object)value;
                result = actualValue is <= long.MinValue ? MaxValue :
                         actualValue is >= long.MaxValue ? MinValue :
                         ConvertBySBT(actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                var actualValue = (decimal)(object)value;
                result = actualValue is <= long.MinValue ? MaxValue :
                         actualValue is >= long.MaxValue ? MinValue :
                         ConvertBySBT(actualValue);
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
                result = new((long)actualValue);
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
                result = new((long)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(Int128))
            {
                var actualValue = (Int128)(object)value;
                result = new((long)actualValue);
                return true;
            }
            else if (typeof(TOther) == typeof(UInt128))
            {
                var actualValue = (UInt128)(object)value;
                result = new((long)actualValue);
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.TryConvertToChecked<TOther>(Rational value, [MaybeNullWhen(false)] out TOther result)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.TryConvertToSaturating<TOther>(Rational value, [MaybeNullWhen(false)] out TOther result)
        {
            if (typeof(TOther) == typeof(byte))
            {
                var actualResult = value >= byte.MaxValue ? byte.MaxValue :
                                   value.IsNegative() ? byte.MinValue :
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
                                   value.IsNegative() ? ushort.MinValue :
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
                                   value.IsNegative() ? uint.MinValue :
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
                                   value.IsNegative() ? nuint.MinValue :
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
                var actualResult = value.IsNegative() ? ulong.MinValue : (ulong)value;
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
                var actualResult = value.IsNegative() ? UInt128.MinValue : (ulong)value;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Rational>.TryConvertToTruncating<TOther>(Rational value, [MaybeNullWhen(false)] out TOther result)
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
