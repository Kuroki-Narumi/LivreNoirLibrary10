using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;

namespace LivreNoirLibrary.Benchmark
{
    public class VectorTest
    {
        private readonly (int, int, int, int, int, int, int, int) _values = (1, 2, 3, 4, 5, 6, 7, 8);
        private readonly int[] _dummy = [1];

        [Benchmark]
        public void NaiveCompare()
        {
            var (v1, v2, v3, v4, v5, v6, v7, v8) = _values;
            if (v1 < v2 && v2 < v3 && v3 < v4 && v4 < v5 && v5 < v6 && v6 < v7 && v7 < v8 && 0 < v1)
            {
                Hoge();
            }
        }

        [Benchmark]
        public void VectorCompare()
        {
            var (v1, v2, v3, v4, v5, v6, v7, v8) = _values;
            var vector1 = Vector256.Create(v1, v2, v3, v4, v5, v6, v7, v8);
            var vector2 = Vector256.Create(0, v1, v2, v3, v4, v5, v6, v7);
            if (Vector256.LessThanAll(vector2, vector1))
            {
                Hoge();
            }
        }

        public void Hoge()
        {
            _dummy[0] = 2;
        }
    }
}
