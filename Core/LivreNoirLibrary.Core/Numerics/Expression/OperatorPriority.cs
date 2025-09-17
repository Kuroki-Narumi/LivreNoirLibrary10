using System;

namespace LivreNoirLibrary.Numerics
{
    public enum OperatorPriority
    {
        Ternary = 0,

        Or = 10,
        Xor = 11,
        And = 12,

        BitOr = 20,
        BitXor = 21,
        BitAnd = 22,

        Equality = 30,
        Comparison = 40,

        BitShift = 50,

        Addition = 60,
        Multiply = 61,
        Power = 62,

        Cast = 90,
        Unary = 100,
    }
}
