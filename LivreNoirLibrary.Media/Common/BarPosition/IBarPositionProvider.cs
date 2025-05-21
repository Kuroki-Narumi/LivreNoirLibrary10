using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public interface IBarPositionProvider
    {
        public static readonly Rational DefaultBarLength = Rational.One;

        public static IBarPositionProvider Default { get; } = new DefaultProvider();

        public BarPosition MaxBarPosition => BarPosition.MaxValue;
        public Rational GetBarLength(int number);
        public Rational GetBeat(BarPosition positoin);
        public BarPosition GetPosition(Rational beat);

        private sealed class DefaultProvider : IBarPositionProvider
        {
            public Rational GetBarLength(int number) => DefaultBarLength;

            public Rational GetBeat(BarPosition position) => position.Bar + position.Beat;

            public BarPosition GetPosition(Rational beat)
            {
                var bar = (long)beat;
                var inner = beat - bar;
                return new((int)bar, inner);
            }
        }
    }
}
