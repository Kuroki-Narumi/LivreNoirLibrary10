using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace LivreNoirLibrary.Media
{
    public interface IBarLengthProvider<T>
    {
        T GetBarLength(int number);
    }

    public interface IBarPositionProvider<T> : IBarLengthProvider<T>
    {
        T GetAbsolutePosition(BarPosition position);
        BarPosition GetBarPosition(T absolutePosition);
    }

    public static class IBarPositionProviderExtensions
    {
        extension<T> (IBarPositionProvider<T> provider)
        {
            public T GetHead(int number) => provider.GetAbsolutePosition(new BarPosition(number));
            public T GetHead(BarPosition position) => provider.GetAbsolutePosition(new BarPosition(position.Bar));
            public T GetTail(int number) => provider.GetAbsolutePosition(new BarPosition(number + 1));
            public T GetTail(BarPosition position) => provider.GetAbsolutePosition(new BarPosition(position.Bar + 1));
        }

        extension<T>(IBarPositionProvider<T> provider) where T : INumber<T>
        {
            public IEnumerable<(int Number, T Head, T Length)> EnumerateBars(int start, int endInclusive)
            {
                var head = provider.GetAbsolutePosition(new(start));
                for (; start <= endInclusive; start++)
                {
                    var length = provider.GetBarLength(start);
                    yield return (start, head, length);
                    head += length;
                }
            }
        }
    }

    public sealed class DefaultRationalBarPositionProvider : IBarPositionProvider<Rational>
    {
        public Rational GetBarLength(int number) => Rational.One;
        public Rational GetAbsolutePosition(BarPosition position) => position.Bar + position.Offset;
        public BarPosition GetBarPosition(Rational absolutePosition)
        {
            var (number, offset) = absolutePosition.DivRem();
            return new((int)number, offset);
        }
    }

    public sealed class DefaultDoubleBarPositionProvider : IBarPositionProvider<double>
    {
        public double GetBarLength(int number) => 1;
        public double GetAbsolutePosition(BarPosition position) => position.Bar + (double)position.Offset;
        public BarPosition GetBarPosition(double absolutePosition)
        {
            var number = (int)absolutePosition;
            var offset = absolutePosition - number;
            return new(number, Rational.ConvertBySBT(offset));
        }
    }
}
