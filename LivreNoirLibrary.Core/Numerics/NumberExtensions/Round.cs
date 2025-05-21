using System;

namespace LivreNoirLibrary.Numerics
{
    public static partial class NumberExtensions
    {
        public static byte RoundToByte(this float value)
        {
            return (byte)MathF.Round(value);
        }

        public static byte RoundToByte(this double value)
        {
            return (byte)Math.Round(value);
        }

        public static byte RoundToByte(this decimal value)
        {
            return (byte)Math.Round(value);
        }

        public static int RoundToInt(this float value)
        {
            return (int)MathF.Round(value);
        }

        public static int RoundToInt(this double value)
        {
            return (int)Math.Round(value);
        }

        public static int RoundToInt(this decimal value)
        {
            return (int)Math.Round(value);
        }

        public static long RoundToLong(this float value)
        {
            return (long)MathF.Round(value);
        }

        public static long RoundToLong(this double value)
        {
            return (long)Math.Round(value);
        }

        public static long RoundToLong(this decimal value)
        {
            return (long)Math.Round(value);
        }
    }
}
