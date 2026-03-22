using BenchmarkDotNet.Attributes;
using LivreNoirLibrary.Numerics;
using System;

namespace LivreNoirLibrary.Benchmark
{
    public class ShuffleTest
    {
        const int N = 44100 * 2 * 240;

        private readonly float[] _array1 = new float[N];
        private readonly float[] _array2 = new float[N];

        [GlobalSetup]
        public void Setup()
        { 
            var random = new XorShift(123456789);
            var array1 = _array1.AsSpan();
            for (var i = 0; i < N; i++)
            {
                array1[i] = random.NextSingle();
            }
        }

        void Shuffle_Source(int stride)
        {
            var array1 = _array1.AsSpan();
            var array2 = _array2.AsSpan();
            var length = N / stride;
            var srcIndex = 0;
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < stride; j++)
                {
                    array2[i + j * length] = array1[srcIndex];
                    srcIndex++;
                }
            }
        }

        unsafe void Shuffle_Source_Fixed(int stride)
        {
            var length = N / stride;
            fixed (float* array1 = _array1)
            fixed (float* array2 = _array2)
            {
                var srcIndex = 0;
                for (var i = 0; i < length; i++)
                {
                    for (var j = 0; j < stride; j++)
                    {
                        array2[i + j * length] = array1[srcIndex];
                        srcIndex++;
                    }
                }
            }
        }

        void Shuffle_Target(int stride)
        {
            var array1 = _array1.AsSpan();
            var array2 = _array2.AsSpan();
            var length = N / stride;
            var dstIndex = 0;
            for (var j = 0; j < stride; j++)
            {
                var srcIndex = j;
                for (var i = 0; i < length; i++)
                {
                    array2[dstIndex] = array1[srcIndex];
                    dstIndex++;
                    srcIndex += stride;
                }
            }
        }

        [Benchmark]
        public void Source_2() => Shuffle_Source(2);
        [Benchmark]
        public void Source_4() => Shuffle_Source(4);
        [Benchmark]
        public void Source_10() => Shuffle_Source(10);
        [Benchmark]
        public void Source_2f() => Shuffle_Source_Fixed(2);
        [Benchmark]
        public void Source_4f() => Shuffle_Source_Fixed(4);
        [Benchmark]
        public void Source_10f() => Shuffle_Source_Fixed(10);
        [Benchmark]
        public void Target_2() => Shuffle_Target(2);
        [Benchmark]
        public void Target_4() => Shuffle_Target(4);
        [Benchmark]
        public void Target_10() => Shuffle_Target(10);

        public static void Validate()
        {
            var s = new ShuffleTest();
            var t = new ShuffleTest();
            s.Setup();
            t.Setup();
            var strokes = (stackalloc int[] { 2, 4, 10 });
            var ok = true;
            for (var i = 0; i < strokes.Length; i++)
            {
                var stroke = strokes[i];
                s.Shuffle_Source(stroke);
                t.Shuffle_Target(stroke);
                if (!s._array2.AsSpan().SequenceEqual(t._array2.AsSpan()))
                {
                    Console.WriteLine($"Validation failed for stroke {stroke}");
                    ok = false;
                }
            }
            if (ok)
            {
                Console.WriteLine("Validation succeeded");
            }
        }
    }
}
