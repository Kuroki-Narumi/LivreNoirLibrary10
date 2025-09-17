using System;

namespace LivreNoirLibrary.Numerics
{
    public static partial class NumberExtensions
    {
        public static byte RoundToByte(this float value) => (byte)MathF.Round(value);
        public static byte RoundToByte(this double value) => (byte)Math.Round(value);
        public static byte RoundToByte(this decimal value) => (byte)Math.Round(value);

        public static int RoundToInt(this float value) => (int)MathF.Round(value);
        public static int RoundToInt(this double value) => (int)Math.Round(value);
        public static int RoundToInt(this decimal value) => (int)Math.Round(value);

        public static long RoundToLong(this float value) => (long)MathF.Round(value);
        public static long RoundToLong(this double value) => (long)Math.Round(value);
        public static long RoundToLong(this decimal value) => (long)Math.Round(value);
    }
}
