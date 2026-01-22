using BenchmarkDotNet.Attributes;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Benchmark
{
    public class IfTest
    {
        const int Count = 10000;

        readonly (int, int, int)[] _values = new (int, int, int)[Count];
        readonly (int, int, int)[] _results = new (int, int, int)[Count];

        [GlobalSetup]
        public void Setup()
        {
            var random = new XorShift(123456789);

            var values = _values;
            for (var i = 0; i < Count; i++)
            {
                var a = random.Next(1000) - 500;
                var b = random.Next(1000) - 500;
                var c = 2000;
                values[i] = (a, b, c);
            }
        }

        [Benchmark]
        public void If()
        {
            var values = _values;
            var results = _results;
            for (var i = 0; i < Count; i++)
            {
                var (a, b, c) = values[i];
                if (a < 0)
                {
                    b -= a;
                    c += a;
                    a = 0;
                }
                results[i] = (a, b, c);
            }
        }

        [Benchmark]
        public void MathMin()
        {
            var values = _values;
            var results = _results;
            for (var i = 0; i < Count; i++)
            {
                var (a, b, c) = values[i];
                var delta = Math.Min(a, 0);
                b -= delta;
                c += delta;
                a -= delta;
                results[i] = (a, b, c);
            }
        }

        public static void Validate()
        {
            var t1 = new IfTest();
            t1.Setup();
            var t2 = new IfTest();
            t2.Setup();
            Console.WriteLine($"source equals: {t1._values.SequenceEqual(t2._values)}");
            t1.If();
            t2.MathMin();
            Console.WriteLine($"results equals: {t1._results.SequenceEqual(t2._results)}");
        }
    }
}
