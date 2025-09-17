using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly struct BarResizeInfo(Rational newLength, Rational ratio)
    {
        public readonly Rational NewLength = newLength;
        public readonly Rational Ratio = ratio;

        public void Deconstruct(out Rational newLength, out Rational ratio)
        {
            newLength = NewLength;
            ratio = Ratio;
        }
    }
}
