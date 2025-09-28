using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using System.Text;

namespace LivreNoirLibrary.Benchmark
{
    public class PointerTest
    {
        public const int Number = 10000;
        private readonly Point[] _points1 = new Point[Number];
        private readonly Point[] _points2 = new Point[Number];
        private readonly bool[] _results = new bool[Number];

        public readonly struct Point(int x, int y, int w, int h)
        {
            public readonly int X = x;
            public readonly int Y = y;
            public readonly int W = w;
            public readonly int H = h;

            public static bool Equals(Point left, Point right) => left.X == right.X && left.Y == right.Y && left.W == right.W && left.H == right.H;
            public static unsafe bool UnsafeEquals(Point left, Point right) => *(UInt128*)&left == *(UInt128*)&right;
            public static unsafe bool UnsafeEquals2(Point left, Point right)
            {
                var lp = (ulong*)&left;
                var rp = (ulong*)&right;
                return lp[0] == rp[0] && lp[1] == rp[1];
            }
        }

        [GlobalSetup]
        public void Setup()
        {
            var rand = Random.Shared;
            int Get() => rand.Next(int.MaxValue);
            for (var i = 0; i < Number; i++)
            {
                _points1[i] = new(Get(), Get(), Get(), Get());
                if (i % 2 is 0)
                {
                    _points2[i] = _points1[i];
                }
                else
                {
                    _points2[i] = new(Get(), Get(), Get(), Get());
                }
            }
        }

        [Benchmark]
        public void UnsafeConpare()
        {
            var ary1 = _points1;
            var ary2 = _points2;
            var result = _results;
            for (var i = 0; i < Number; i++)
            {
                result[i] = Point.UnsafeEquals(ary1[i], ary2[i]);
            }
        }

        [Benchmark]
        public void SimpleCompare()
        {
            var ary1 = _points1;
            var ary2 = _points2;
            var result = _results;
            for (var i = 0; i < Number; i++)
            {
                result[i] = Point.Equals(ary1[i], ary2[i]);
            }
        }

        [Benchmark]
        public void UnsafeCompare2()
        {
            var ary1 = _points1;
            var ary2 = _points2;
            var result = _results;
            for (var i = 0; i < Number; i++)
            {
                result[i] = Point.UnsafeEquals2(ary1[i], ary2[i]);
            }
        }

        public static void Run()
        {
            BenchmarkRunner.Run<PointerTest>();
            var p = new PointerTest();
            p.Setup();
            p.SimpleCompare();
            bool[] result1 = [.. p._results];
            p.UnsafeConpare();
            bool[] result2 = [.. p._results];
            p.UnsafeCompare2();
            bool[] result3 = [.. p._results];

            Console.WriteLine($"Simple vs Unsafe: {result1.AsSpan().SequenceEqual(result2)}");
            Console.WriteLine($"Simple vs Vector: {result1.AsSpan().SequenceEqual(result3)}");
            Console.WriteLine($"Unsafe vs Vector: {result2.AsSpan().SequenceEqual(result3)}");
        }
    }
}
