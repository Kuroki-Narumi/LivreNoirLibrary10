using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace LivreNoirLibrary.Media
{
    public static class ColorOperation
    {
        public static int GetClearMask(this ColorIndex index) => ~(255 << ((int)index * 8));
        public static int GetSetMask(this ColorIndex index, byte value) => value << ((int)index * 8);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(Color color)
        {
            var a = color.A;
            var r = Multiply(color.R, a) << 16;
            var g = Multiply(color.G, a) << 8;
            var b = Multiply(color.B, a);
            return (a << 24) | r | g | b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ToColor(int value)
        {
            var a = (value >> 24) & 0xFF;
            var r = Divide((value >> 16) & 0xFF, a);
            var g = Divide((value >> 8) & 0xFF, a);
            var b = Divide(value & 0xFF, a);
            return Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Multiply(int left, int right) => left * right / byte.MaxValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Divide(int left, int right) => right is 0 ? left : left * byte.MaxValue / right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte LimitMinimum(int value) => value is < byte.MinValue ? byte.MinValue : (byte)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte LimitMaximum(int value) => value is > byte.MaxValue ? byte.MaxValue : (byte)value;
    }
}
