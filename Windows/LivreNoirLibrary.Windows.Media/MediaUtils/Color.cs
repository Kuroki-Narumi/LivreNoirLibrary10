using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Media
{
    public static partial class MediaUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt(this Color color) => ((uint)color.A << 24) | ((uint)color.R << 16) | ((uint)color.G << 8) | ((uint)color.B);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ToColor(this uint value) => unchecked(Color.FromArgb((byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Multiply(uint left, uint right) => left * right / byte.MaxValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Divide(uint left, uint right) => right is 0 ? left : left * byte.MaxValue / right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte LimitMinimum(int value) => value is < byte.MinValue ? byte.MinValue : (byte)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte LimitMaximum(uint value) => value is > byte.MaxValue ? byte.MaxValue : (byte)value;
    }
}
