using System;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media
{
    public interface IUnaryColorOperation
    {
        public unsafe void Apply(byte* source);
    }

    public readonly struct ColorOperation_Set(int index, byte value) : IUnaryColorOperation
    {
        private readonly int _index = index;
        private readonly byte _value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source)
        {
            source[_index] = (byte)ColorOperation.Multiply(_value, source[3]);
        }
    }

    public readonly struct ColorOperation_SetAlpha(byte value) : IUnaryColorOperation
    {
        private readonly byte _value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source)
        {
            var v = _value;
            var a = source[3];
            source[0] = (byte)ColorOperation.Multiply(v, ColorOperation.Divide(source[0], a));
            source[1] = (byte)ColorOperation.Multiply(v, ColorOperation.Divide(source[1], a));
            source[2] = (byte)ColorOperation.Multiply(v, ColorOperation.Divide(source[2], a));
            source[3] = v;
        }
    }

    public readonly struct ColorOperation_SetFrom(int from, int to) : IUnaryColorOperation
    {
        private readonly int _from = from;
        private readonly int _to = to;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source)
        {
            source[_to] = source[_from];
        }
    }

    public readonly struct ColorOperation_Swap(int from, int to) : IUnaryColorOperation
    {
        private readonly int _from = from;
        private readonly int _to = to;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source)
        {
            var c1 = _from;
            var c2 = _to;
            (source[c1], source[c2]) = (source[c2], source[c1]);
        }
    }

    public readonly struct ColorOperation_InvertAll : IUnaryColorOperation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source)
        {
            var a = source[3];
            source[0] = (byte)(a - source[0]);
            source[1] = (byte)(a - source[1]);
            source[2] = (byte)(a - source[2]);
        }
    }

    public readonly struct ColorOperation_Invert(int index) : IUnaryColorOperation
    {
        private readonly int _index = index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source)
        {
            var a = source[3];
            var i = _index;
            source[i] = (byte)(a - source[i]);
        }
    }

    public readonly struct ColorOperation_InvertAlpha : IUnaryColorOperation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source)
        {
            var a = source[3];
            var na = (byte)~a;
            source[3] = na;
            source[0] = (byte)ColorOperation.Multiply(ColorOperation.Divide(source[0], a), na);
            source[1] = (byte)ColorOperation.Multiply(ColorOperation.Divide(source[1], a), na);
            source[2] = (byte)ColorOperation.Multiply(ColorOperation.Divide(source[2], a), na);
        }
    }

    public readonly struct ColorOperation_ChangeHue(float hue) : IUnaryColorOperation
    {
        private readonly float _hue = hue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source)
        {
            var a = source[3];
            var b = (byte)ColorOperation.Divide(source[0], a);
            var g = (byte)ColorOperation.Divide(source[1], a);
            var r = (byte)ColorOperation.Divide(source[2], a);
            (r, g, b) = ColorUtils.GetHueChanged(r, g, b, _hue);
            source[0] = (byte)ColorOperation.Multiply(b, a);
            source[1] = (byte)ColorOperation.Multiply(g, a);
            source[2] = (byte)ColorOperation.Multiply(r, a);
        }
    }

    public readonly struct ColorOperation_ChangeHsv(float hue, float saturation, float value) : IUnaryColorOperation
    {
        private readonly float _hue = hue;
        private readonly float _saturation = saturation;
        private readonly float _value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Apply(byte* source)
        {
            var a = source[3];
            var b = (byte)ColorOperation.Divide(source[0], a);
            var g = (byte)ColorOperation.Divide(source[1], a);
            var r = (byte)ColorOperation.Divide(source[2], a);
            (r, g, b) = ColorUtils.GetHsvChanged(r, g, b, _hue, _saturation, _value);
            source[0] = (byte)ColorOperation.Multiply(b, a);
            source[1] = (byte)ColorOperation.Multiply(g, a);
            source[2] = (byte)ColorOperation.Multiply(r, a);
        }
    }
}
