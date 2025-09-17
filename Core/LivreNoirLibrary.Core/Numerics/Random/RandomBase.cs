using System;

namespace LivreNoirLibrary.Numerics
{
    public abstract class RandomBase : Random
    {
        private const double DoubleDenominator = 1.0 / ((double)ulong.MaxValue + 1);
        private const float FloatDenominator = 1 / ((float)ulong.MaxValue + 1);

        private static void ThrowMinMaxValueSwapped<T>(T minValue, T maxValue) =>
            throw new ArgumentOutOfRangeException(nameof(minValue), $"{nameof(minValue)}({minValue}) cannot be greater than {nameof(maxValue)}({maxValue}).");

        protected abstract ulong Generate();

        protected sealed override double Sample() => Generate() * DoubleDenominator;

        public sealed override int Next() => (int)(Generate() >>> 33);

        public sealed override int Next(int maxValue)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(maxValue, nameof(maxValue));
            if (maxValue is > 1)
            {
                return (int)(((Generate() >> 32) * (ulong)maxValue) >> 32);
            }
            return 0;
        }

        public sealed override int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                ThrowMinMaxValueSwapped(minValue, maxValue);
            }
            return Next(maxValue - minValue) + minValue;
        }

        public sealed override long NextInt64() => (long)(Generate() >>> 1);

        public sealed override long NextInt64(long maxValue)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(maxValue, nameof(maxValue));
            if (maxValue is > 1)
            {
                var m = (ulong)maxValue;
                var x = Generate();
                var high = Math.BigMul(x, m, out var low);
                return (long)high;
            }
            return 0;
        }

        public sealed override long NextInt64(long minValue, long maxValue)
        {
            if (minValue > maxValue)
            {
                ThrowMinMaxValueSwapped(minValue, maxValue);
            }
            return NextInt64(maxValue - minValue) + minValue;
        }

        public sealed override float NextSingle() => Generate() * FloatDenominator;
        public sealed override double NextDouble() => Sample();

        public sealed override void NextBytes(byte[] buffer) => NextBytes(buffer.AsSpan());

        public sealed override unsafe void NextBytes(Span<byte> buffer)
        {
            fixed (byte* ptr = buffer)
            {
                var len = buffer.Length;
                var count = len / sizeof(ulong);
                var remain = len - (count * sizeof(ulong));
                var ulPtr = (ulong*)ptr;
                while (len is >= sizeof(ulong))
                {
                    *ulPtr = Generate();
                    ulPtr++;
                    len -= sizeof(ulong);
                }
                if (remain is > 0)
                {
                    var value = Generate();
                    var bPtr = (byte*)ulPtr;
                    for (var i = 0; i < remain; i++)
                    {
                        bPtr[i] = unchecked((byte)value);
                        value >>>= 8;
                    }
                }
            }
        }
    }
}
