using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Midi
{
    public interface IBarLengthProvider
    {
        Rational GetBarLength(int number);
    }

    public interface IBarPositionProvider : IBarLengthProvider
    {
        Rational GetAbsolutePosition(BarPosition position);
        BarPosition GetBarPosition(Rational absolutePosition);
    }

    public static class IBarPositionProviderExtensions
    {
        extension (IBarPositionProvider provider)
        {
            public Rational GetHead(int number) => provider.GetAbsolutePosition(new BarPosition(number));

            public Rational GetHead(BarPosition position) => provider.GetAbsolutePosition(new BarPosition(position.Bar));

            public Rational GetTail(int number) => provider.GetAbsolutePosition(new BarPosition(number + 1));

            public Rational GetTail(BarPosition position) => provider.GetAbsolutePosition(new BarPosition(position.Bar + 1));

            public IEnumerable<(int Number, Rational Head, Rational Length)> EnumerateBars(int start, int endInclusive)
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
}
