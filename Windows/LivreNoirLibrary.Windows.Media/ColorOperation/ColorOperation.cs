using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace LivreNoirLibrary.Media
{
    public static class ColorOperation
    {
        public const uint Mask_Alpha = 0xFF000000;
        public const uint Mask_Red = 0x00FF0000;
        public const uint Mask_Green = 0x0000FF00;
        public const uint Mask_Blue = 0x000000FF;

        private static readonly Dictionary<ColorIndex, uint> _masks = new()
        {
            [ColorIndex.A] = Mask_Alpha,
            [ColorIndex.R] = Mask_Red,
            [ColorIndex.G] = Mask_Green,
            [ColorIndex.B] = Mask_Blue,
        };

        public static uint GetClearMask(this ColorIndex index) => ~_masks[index];
        public static uint GetSetMask(this ColorIndex index, byte value) => unchecked((uint)value << ((int)index * 8));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt(Color color) => ((uint)color.A << 24) | ((uint)color.R << 16) | ((uint)color.G << 8) | ((uint)color.B);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ToColor(uint value) => unchecked(Color.FromArgb((byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)value));

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
