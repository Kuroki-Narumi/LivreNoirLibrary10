using System;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media
{
    public interface IBinaryColorOperation
    {
        public unsafe void Apply(byte* source, byte* target);
    }

    public readonly unsafe struct ColorOperation_AlphaBlend : IBinaryColorOperation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source, byte* target)
        {
            var a = source[3];
            if (a is 0)
            {
            }
            else if (a is byte.MaxValue)
            {
                target[0] = source[0];
                target[1] = source[1];
                target[2] = source[2];
                target[3] = a;
            }
            else
            {
                var na = 255 - a;
                target[0] = (byte)(ColorOperation.Multiply(target[0], na) + source[0]);
                target[1] = (byte)(ColorOperation.Multiply(target[1], na) + source[1]);
                target[2] = (byte)(ColorOperation.Multiply(target[2], na) + source[2]);
                target[3] = (byte)(ColorOperation.Multiply(target[3], na) + a);
            }
        }
    }

    public readonly struct ColorOperation_Add : IBinaryColorOperation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source, byte* target)
        {
            var a = source[3];
            target[0] = ColorOperation.LimitMaximum(target[0] + source[0]);
            target[1] = ColorOperation.LimitMaximum(target[1] + source[1]);
            target[2] = ColorOperation.LimitMaximum(target[2] + source[2]);
        }
    }

    public readonly struct ColorOperation_Subtract : IBinaryColorOperation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source, byte* target)
        {
            var a = source[3];
            target[0] = ColorOperation.LimitMaximum(target[0] - source[0]);
            target[1] = ColorOperation.LimitMaximum(target[1] - source[1]);
            target[2] = ColorOperation.LimitMaximum(target[2] - source[2]);
        }
    }

    public readonly struct ColorOperation_Difference : IBinaryColorOperation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source, byte* target)
        {
            target[0] = (byte)Math.Abs(target[0] - source[0]);
            target[1] = (byte)Math.Abs(target[1] - source[1]);
            target[2] = (byte)Math.Abs(target[2] - source[2]);
        }
    }

    public readonly struct ColorOperation_Multiply : IBinaryColorOperation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source, byte* target)
        {
            target[0] = (byte)ColorOperation.Multiply(target[0], source[0]);
            target[1] = (byte)ColorOperation.Multiply(target[1], source[1]);
            target[2] = (byte)ColorOperation.Multiply(target[2], source[2]);
        }
    }

    public readonly struct ColorOperation_Mask(int index) : IBinaryColorOperation
    {
        private readonly int _index = index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source, byte* target)
        {
            var a = source[_index];
            target[0] = (byte)ColorOperation.Multiply(target[0], a);
            target[1] = (byte)ColorOperation.Multiply(target[1], a);
            target[2] = (byte)ColorOperation.Multiply(target[2], a);
            target[3] = (byte)ColorOperation.Multiply(target[3], a);
        }
    }
}
