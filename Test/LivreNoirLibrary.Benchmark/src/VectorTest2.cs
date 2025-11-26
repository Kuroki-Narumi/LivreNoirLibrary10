using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;

namespace LivreNoirLibrary.Benchmark
{
    public class VectorTest2
    {
        const int Count = 10000;

        static readonly Vector128<int> _shuffle128 = Vector128.Create(3, 3, 3, 3);
        static readonly Vector256<int> _shuffle256 = Vector256.Create(3, 3, 3, 3, 7, 7, 7, 7);
        static readonly Vector512<int> _shuffle512 = Vector512.Create(3, 3, 3, 3, 7, 7, 7, 7, 11, 11, 11, 11, 15, 15, 15, 15);

        internal readonly Vector<float>[] _vectors = new Vector<float>[Count];
        internal readonly Vector<float>[] _results = new Vector<float>[Count];

        [GlobalSetup]
        public void Setup()
        {
            var random = new XorShift(12345678);
            for (var i = 0; i < Count; i++)
            {
                var v1 = random.NextSingle();
                _vectors[i] = VectorUtils.CreateRepeating(v1, v1 + 1, v1 + 2, v1 + 3);
            }
        }

        [Benchmark]
        public unsafe void Manual()
        {
            var count = Vector<float>.Count;
            var buffer = stackalloc float[count];
            for (var i = 0; i < Count; i++)
            {
                var vector = _vectors[i];
                for (var j = 0; j < count; j++)
                {
                    buffer[j] = vector[j / 4 * 4 + 3];
                }
                _results[i] = *(Vector<float>*)buffer;
            }
        }

        [Benchmark]
        public void ShuffleNative()
        {
            for (var i = 0; i < Count; i++)
            {
                _results[i] = FillAlphaToAll(_vectors[i]);
            }
        }

        public static Vector<float> FillAlphaToAll(Vector<float> vector)
        {
            var count = Vector<float>.Count;
            if (count == Vector128<float>.Count)
            {
                return Vector128.ShuffleNative(vector.AsVector128(), _shuffle128).AsVector();
            }
            else if (count == Vector256<float>.Count)
            {
                return Vector256.ShuffleNative(vector.AsVector256(), _shuffle256).AsVector();
            }
            else if (count == Vector512<float>.Count)
            {
                return Vector512.ShuffleNative(vector.AsVector512(), _shuffle512).AsVector();
            }
            return default;
        }
    }
}
