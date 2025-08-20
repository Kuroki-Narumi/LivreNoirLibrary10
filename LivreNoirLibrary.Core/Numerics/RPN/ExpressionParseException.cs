using System;

namespace LivreNoirLibrary.Numerics
{
    public class ExpressionParseException(string message, string expression, int charIndex) : Exception(message)
    {
        public string Expression { get; } = expression;
        public int CharIndex { get; } = charIndex;
    }
}
