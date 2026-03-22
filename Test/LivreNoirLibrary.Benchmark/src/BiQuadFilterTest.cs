using BenchmarkDotNet.Attributes;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Benchmark
{
    public class BiQuadFilterTest
    {
        const int SampleRate = 44100;
        const int Channels = 2;
        const int Seconds = 120;
        const int N = SampleRate * Channels * Seconds;

        private readonly float[] _array1 = new float[N];
        private readonly float[] _array2 = new float[N];
        private readonly BiQuadFilter _filter = BiQuadFilter.LowPass(SampleRate, 1000);
        private readonly BiQuadFilterState[] _states = new BiQuadFilterState[Channels];

        [GlobalSetup]
        public void Setup()
        {
            var random = new XorShift(123456789);
            var array1 = _array1.AsSpan();
            for (var i = 0; i < N; i++)
            {
                array1[i] = (float)(random.NextDouble() * 2 - 1);
            }
        }

        [Benchmark]
        public void ChannelFirst_NoShuffle()
        {
            //_filter.ApplyMultiChannel2(_array1, _array2, _states, false);
        }

        [Benchmark]
        public void ChannelFirst_Transpose()
        {
            //_filter.ApplyMultiChannel2(_array1, _array2, _states, true);
        }

        [Benchmark]
        public void SampleFirst_NoShuffle()
        {
            _filter.ApplyMultiChannel(_array1, _array2, _states, false);
        }

        [Benchmark]
        public void SampleFirst_Transpose()
        {
            _filter.ApplyMultiChannel(_array1, _array2, _states, true);
        }

        public static void Validate()
        {
            var t1 = new BiQuadFilterTest();
            var t2 = new BiQuadFilterTest();
            t1.Setup();
            t2.Setup();

            t1.SampleFirst_NoShuffle();
            t2.ChannelFirst_NoShuffle();
            if (t1._array2.SequenceEqual(t2._array2))
            {
                Console.WriteLine($"NoShuffle: validation successed");
            }
            else
            {
                Console.WriteLine($"NoShuffle: validation failed");
            }

            t1.SampleFirst_Transpose();
            t2.ChannelFirst_Transpose();
            if (t1._array2.SequenceEqual(t2._array2))
            {
                Console.WriteLine($"Transpose: validation successed");
            }
            else
            {
                Console.WriteLine($"Transpose: validation failed");
            }
        }
    }
}
