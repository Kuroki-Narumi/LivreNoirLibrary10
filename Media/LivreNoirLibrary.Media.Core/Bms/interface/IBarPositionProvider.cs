using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IBarLengthProvider
    {
        double GetBarLength(int number);
    }

    public interface IBarPositionProvider : IBarLengthProvider
    {
        double GetAbsolutePosition(BarPosition position);
        BarPosition GetBarPosition(double absolutePosition);
    }
    
    public static class IBarPositionProviderExtensions
    {
        extension(IBarPositionProvider provider)
        {
            public double GetHead(int number) => provider.GetAbsolutePosition(new BarPosition(number));
            public double GetHead(BarPosition position) => provider.GetAbsolutePosition(new BarPosition(position.Bar));
            public double GetTail(int number) => provider.GetAbsolutePosition(new BarPosition(number + 1));
            public double GetTail(BarPosition position) => provider.GetAbsolutePosition(new BarPosition(position.Bar + 1));

            public IEnumerable<(int Number, double Head, double Length)> EnumerateBars(int start, int endInclusive)
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

    public sealed class DefaultBarPositionProvider : IBarPositionProvider
    {
        public double GetBarLength(int number) => 1;
        public double GetAbsolutePosition(BarPosition position) => position.Bar + position.Offset;
        public BarPosition GetBarPosition(double absolutePosition) => new((int)absolutePosition, absolutePosition % 1);
    }
}
