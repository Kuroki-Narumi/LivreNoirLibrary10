using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using System.Numerics;
using System.Buffers.Binary;

namespace LivreNoirLibrary.Benchmark
{
    /*
    public unsafe class ColorTest
    {
        public const int Count = 1000;
        private readonly Color[] _colors = new Color[Count];
        private readonly uint[] _numbers = new uint[Count];
        private readonly uint[] _color_numbers = new uint[Count];

        [GlobalSetup]
        public void Setup()
        {
            var rand = Random.Shared;
            var colors = _colors;
            var numbers = _numbers;
            for (var i = 0; i < Count; i++)
            {
                var value = unchecked((uint)rand.Next());
                numbers[i] = value;
                var (a, r, g, b) = ColorUtils.ToColor(value);
                colors[i] = Color.FromArgb(a, r, g, b);
            }
        }

        [Benchmark]
        public void NormalConvert()
        {
            var colors = _colors;
            var numbers = _color_numbers;
            for (var i = 0; i < Count; i++)
            {
                var c = colors[i];
                numbers[i] = ColorUtils.ToUInt(c.A, c.R, c.G, c.B);
            }
        }

        [Benchmark]
        public unsafe void UnsafeConvert()
        {
            var colors = _colors;
            var numbers = _color_numbers;
            for (var i = 0; i < Count; i++)
            {
                var value = colors[i];
                numbers[i] = BinaryPrimitives.ReadUInt32BigEndian(new Span<byte>(&value, 4));
            }
        }

        public static void Check()
        {
            var ary = new uint[Count];
            var instance = new ColorTest();
            instance.Setup();
            instance.NormalConvert();
            SimdOperations.CopyFrom(ary, instance._color_numbers);
            instance.UnsafeConvert();
            if (SimdOperations.EqualsAll(ary, instance._color_numbers))
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("NG");
            }
        }
    }
    */
}
