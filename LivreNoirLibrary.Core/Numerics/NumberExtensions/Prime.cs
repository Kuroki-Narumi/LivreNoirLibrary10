using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Numerics
{
    public static partial class NumberExtensions
    {
        public static bool IsPrime(this int val)
        {
            if (val is <= 3) { return val is >= 2; }
            if (val is 5) { return true; }
            int v1 = 30;
            var v2 = val % v1;
            while (v2 is > 0)
            {
                (v1, v2) = (v2, v1 % v2);
            }
            if (v1 is not 1) { return false; }
            var limit = (int)Math.Sqrt(val);
            for (int p = 7; p <= limit; p += 30)
            {
                if (val % p is 0 || val % (p + 4) is 0 || val % (p + 6) is 0 || val % (p + 10) is 0 ||
                    val % (p + 12) is 0 || val % (p + 16) is 0 || val % (p + 22) is 0 || val % (p + 24) is 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsPrime(this long val)
        {
            if (val is <= 3) { return val is >= 2; }
            if (val is 5) { return true; }
            long v1 = 30;
            var v2 = val % v1;
            while (v2 is > 0)
            {
                (v1, v2) = (v2, v1 % v2);
            }
            if (v1 is not 1) { return false; }
            var limit = (long)Math.Sqrt(val);
            for (long p = 7; p <= limit; p += 30L)
            {
                if (val % p is 0L || val % (p + 4L) is 0L || val % (p + 6L) is 0L || val % (p + 10L) is 0L ||
                    val % (p + 12L) is 0L || val % (p + 16L) is 0L || val % (p + 22L) is 0L || val % (p + 24L) is 0L)
                {
                    return false;
                }
            }
            return true;
        }

        private static readonly Dictionary<long, PrimeFactor[]> _division_cache = [];
        private static readonly Dictionary<long, long[]> _divisor_cache_long = [];
        private static readonly Dictionary<int, int[]> _divisor_cache_int = [];

        public static ReadOnlySpan<PrimeFactor> Division(this long val)
        {
            if (!_division_cache.TryGetValue(val, out var ary))
            {
                var v = val;
                Dictionary<long, long> list = [];

                void AddFactor(long n)
                {
                    if (list.TryGetValue(n, out long value))
                    {
                        list[n] = ++value;
                    }
                    else
                    {
                        list.Add(n, 1);
                    }
                }

                while (v % 2 == 0)
                {
                    AddFactor(2);
                    v /= 2;
                }
                var limit = (long)Math.Sqrt(v);
                for (long p = 3; p <= limit; p += 2)
                {
                    while (v % p == 0)
                    {
                        AddFactor(p);
                        v /= p;
                    }
                }
                if (v > 1)
                {
                    AddFactor(v);
                }
                ary = new PrimeFactor[list.Count];
                int i = 0;
                foreach (var kv in list)
                {
                    ary[i++] = new(kv);
                }
                _division_cache.Add(val, ary);
            }
            return ary;
        }

        public static ReadOnlySpan<long> Divisors(this long val)
        {
            if (!_divisor_cache_long.TryGetValue(val, out var ary))
            {
                List<long> result = [1];
                foreach (var (b, exp) in Division(val))
                {
                    var count = result.Count;
                    for (int i = 0; i < count; i++)
                    {
                        var a = result[i];
                        for (long e = 1; e <= exp; e++)
                        {
                            a *= b;
                            result.Add(a);
                        }
                    }
                }
                result.Sort();
                ary = [.. result];
                _divisor_cache_long.Add(val, ary);
            }
            return ary;
        }

        public static ReadOnlySpan<int> Divisors(this int val)
        {
            if (!_divisor_cache_int.TryGetValue(val, out var ary))
            {
                var src = Divisors((long)val);
                ary = new int[src.Length];
                for (int i = 0; i < src.Length; i++)
                {
                    ary[i] = (int)src[i];
                }
                _divisor_cache_int.Add(val, ary);
            }
            return ary;
        }
    }
}
