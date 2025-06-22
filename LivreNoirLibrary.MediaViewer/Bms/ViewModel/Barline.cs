using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public enum BarLineType : byte { Bar, Large, Small }

    public readonly record struct BarLineInfo(Rational Position, Rational Length);
}
