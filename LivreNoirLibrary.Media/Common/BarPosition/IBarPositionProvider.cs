using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public interface IBarPositionProvider
    {
        public static readonly Rational DefaultBarLength = Rational.One;

        public static IBarPositionProvider Default { get; } = new DefaultProvider();

        public Rational GetBarLength(int number);
        public Rational GetAbsolutePosition(BarPosition position);
        public BarPosition GetBarPosition(Rational absolutePosition);

        private sealed class DefaultProvider : IBarPositionProvider
        {
            public Rational GetBarLength(int number) => DefaultBarLength;
            public Rational GetAbsolutePosition(BarPosition position) => position.Bar + position.Offset;
            public BarPosition GetBarPosition(Rational absolutePosition)
            {
                var bar = (long)absolutePosition;
                var inner = absolutePosition - bar;
                return new((int)bar, inner);
            }
        }
    }
}
