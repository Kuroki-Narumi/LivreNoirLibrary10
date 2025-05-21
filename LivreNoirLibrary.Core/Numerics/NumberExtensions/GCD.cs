using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Numerics
{
    public static partial class NumberExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GCD(this int val1, int val2)
        {
            if (val1 is not (0 or int.MinValue) && val2 is not int.MinValue)
            {
                var v1 = Math.Abs(val1);
                var v2 = Math.Abs(val2);
                while (v2 is > 0)
                {
                    (v1, v2) = (v2, v1 % v2);
                }
                return v1;
            }
            return GCD_Rare(val1, val2);
        }

        private static int GCD_Rare(int val1, int val2)
        {
            if (val1 is int.MinValue || val2 is int.MinValue)
            {
                return (int)GCD((long)val1, (long)val2);
            }
            return val2 is 0 ? 1 : Math.Abs(val2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GCD(this long val1, long val2)
        {
            if (val1 is not (0 or long.MinValue) && val2 is not long.MinValue)
            {
                var v1 = Math.Abs(val1);
                var v2 = Math.Abs(val2);
                while (v2 is > 0)
                {
                    (v1, v2) = (v2, v1 % v2);
                }
                return v1;
            }
            return GCD_Rare(val1, val2);
        }

        private static long GCD_Rare(long val1, long val2)
        {
            if (val1 is long.MinValue || val2 is long.MinValue)
            {
                return (long)GCD((Int128)val1, (Int128)val2);
            }
            return val2 is 0 ? 1 : Math.Abs(val2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint GCD(this uint val1, uint val2)
        {
            if (val1 is not 0)
            {
                while (val2 is > 0)
                {
                    (val1, val2) = (val2, val1 % val2);
                }
                return val2;
            }
            return val2 is 0 ? 1 : val2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong GCD(this ulong val1, ulong val2)
        {
            if (val1 is not 0)
            {
                while (val2 is > 0)
                {
                    (val1, val2) = (val2, val1 % val2);
                }
                return val2;
            }
            return val2 is 0 ? 1 : val2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GCD<T>(this T val1, T val2)
            where T : INumber<T>
        {
            if (!T.IsZero(val1))
            {
                var v1 = T.Abs(val1);
                var v2 = T.Abs(val2);
                while (!T.IsZero(v2))
                {
                    (v1, v2) = (v2, v1 % v2);
                }
                return v1;
            }
            return T.IsZero(val2) ? T.One : T.Abs(val2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int128 GCD(this Int128 val1, Int128 val2) => BinaryGCD(Int128.Abs(val1), Int128.Abs(val2), new Tzc_Int128());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 GCD(this UInt128 val1, UInt128 val2) => BinaryGCD(val1, val2, new Tzc_UInt128());

        public interface ITrailingZeroCount<T>
        {
            public int Tzc(T value);
        }

        public readonly struct Tzc_Int128 : ITrailingZeroCount<Int128>
        {
            public int Tzc(Int128 value) => (int)Int128.TrailingZeroCount(value);
        }

        public readonly struct Tzc_UInt128 : ITrailingZeroCount<UInt128>
        {
            public int Tzc(UInt128 value) => (int)UInt128.TrailingZeroCount(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T BinaryGCD<T, TTzc>(this T val1, T val2, TTzc tzc)
            where T : IBinaryInteger<T>
            where TTzc : ITrailingZeroCount<T>
        {
            if (T.IsZero(val1))
            {
                return T.IsZero(val2) ? T.One : val2;
            }
            if (T.IsZero(val2))
            {
                return val1;
            }
            var shift1 = tzc.Tzc(val1);
            val1 >>>= shift1;
            var shift2 = tzc.Tzc(val2);
            val2 >>>= shift2;
            while (val1 != val2)
            {
                if (val1 > val2)
                {
                    val1 -= val2;
                    val1 >>>= tzc.Tzc(val1);
                }
                else
                {
                    val2 -= val1;
                    val2 >>>= tzc.Tzc(val2);
                }
            }
            return val1 << Math.Min(shift1, shift2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T LCM<T>(this T val1, T val2)
            where T : INumber<T>
        {
            var gcd = GCD(val1, val2);
            return (val1 / gcd) * (val2 / gcd) * gcd;
        }
    }
}
