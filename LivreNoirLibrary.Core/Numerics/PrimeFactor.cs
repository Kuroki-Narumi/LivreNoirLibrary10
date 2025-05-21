using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Numerics
{
    public readonly struct PrimeFactor(long @base, long exponent)
    {
        public readonly long Base = @base;
        public readonly long Exponent = exponent;

        public PrimeFactor((long, long) tuple) : this(tuple.Item1, tuple.Item2) { }
        public PrimeFactor(Tuple<long, long> tuple) : this(tuple.Item1, tuple.Item2) { }
        public PrimeFactor(KeyValuePair<long, long> kv) : this(kv.Key, kv.Value) { }

        public static implicit operator PrimeFactor((long, long) tuple) => new(tuple);
        public static implicit operator PrimeFactor(Tuple<long, long> tuple) => new(tuple);
        public static implicit operator PrimeFactor(KeyValuePair<long, long> kv) => new(kv);

        public void Deconstruct(out long b, out long e)
        {
            b = Base;
            e = Exponent;
        }

        public override string ToString()
        {
            return Exponent == 1 ? $"{Base}" : $"{Base}^{Exponent}";
        }
    }
}
