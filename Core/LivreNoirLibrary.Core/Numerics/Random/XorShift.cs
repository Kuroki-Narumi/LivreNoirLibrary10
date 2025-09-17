using System;

namespace LivreNoirLibrary.Numerics
{
    public sealed class XorShift(ulong x) : RandomBase
    {
        private ulong _x = x;

        public XorShift() : this((ulong)System.Diagnostics.Stopwatch.GetTimestamp()) { }

        protected override ulong Generate()
        {
            var x = _x;
            x ^= x << 13;
            x ^= x >> 7;
            x ^= x << 17;
            _x = x;
            return x;
        }
    }
}
