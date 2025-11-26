using BenchmarkDotNet.Attributes;
using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LivreNoirLibrary.Benchmark
{
    public unsafe class Lanczos3Test
    {
        const int TableSize = 65536;
        const float Scale = TableSize / 3f;

        static readonly float[] _lanczos3Table = CreateLanczos3Table();

        static float[] CreateLanczos3Table()
        {
            var result = new float[TableSize];
            result[0] = 1;
            for (var i = 1; i < TableSize; i++)
            {
                var x = i / Scale;
                if (x is >= 3)
                {
                    result[i] = 0;
                }
                else
                {
                    var pix = MathF.PI * x;
                    result[i] = (MathF.Sin(pix) / pix) * (MathF.Sin(pix / 3) / (pix / 3));
                }
            }
            return result;
        }

        private static float Lanczos3Kernel(float x)
        {
            x = Math.Abs(x);
            if (x is >= 3)
            {
                return 0;
            }
            x *= Scale;
            var index = (int)x;
            var y0 = _lanczos3Table[index];
            var y1 = _lanczos3Table[index + 1];
            return y0 + (y1 - y0) * (x - index);
        }

        static readonly float[] _values = CreateValues();

        static float[] CreateValues()
        {
            var ary = new float[1920 * 1080];
            for (var i = 0; i < ary.Length; i++)
            {
                ary[i] = (i + 1) / (float)ary.Length;
            }
            return ary;
        }

        readonly float[] _results = new float[_values.Length];

        [Benchmark]
        public void MathFSin()
        {
            var count = _results.Length;
            fixed (float* source = _values)
            fixed (float* dest = _results)
            {
                for (var i = 0; i < count; i++)
                {
                    var x = Math.Abs(source[i]) * MathF.PI;
                    var x3 = x / 3;
                    dest[i] = MathF.Sin(x) / x * MathF.Sin(x3) / x3;
                }
            }
        }

        [Benchmark]
        public void FloatSinPi()
        {
            var count = _results.Length;
            fixed (float* source = _values)
            fixed (float* dest = _results)
            {
                for (var i = 0; i < count; i++)
                {
                    var x = Math.Abs(source[i]);
                    var x3 = x / 3;
                    dest[i] = float.SinPi(x) * float.SinPi(x3) / (MathF.PI * MathF.PI * x * x3);
                }
            }
        }

        [Benchmark]
        public void Table()
        {
            var count = _results.Length;
            fixed (float* source = _values)
            fixed (float* dest = _results)
            {
                for (var i = 0; i < count; i++)
                {
                    dest[i] = Lanczos3Kernel(source[i]);
                }
            }
        }

        public static void Check()
        {
            Lanczos3Test t1 = new();
            Lanczos3Test t2 = new();
            Lanczos3Test t3 = new();
            t1.MathFSin();
            t2.FloatSinPi();
            t3.Table();

            static float Rms(float[] ary) => MathF.Sqrt(ary.MeanSquare());

            Console.WriteLine($"src: {Rms(_values)}");
            Console.WriteLine($"t1: {Rms(t1._results)}");
            Console.WriteLine($"t2: {Rms(t2._results)}");
            Console.WriteLine($"t3: {Rms(t3._results)}");

            float[] t1_t2 = [.. t1._results];
            t1_t2.Subtract(t2._results);
            var t1_t3 = t1._results;
            t1_t3.Subtract(t3._results);
            var t2_t3 = t2._results;
            t2_t3.Subtract(t3._results);

            Console.WriteLine($"t1 vs t2: {Rms(t1_t2)}");
            Console.WriteLine($"t1 vs t3: {Rms(t1_t3)}");
            Console.WriteLine($"t2 vs t3: {Rms(t2_t3)}");
        }
    }
}