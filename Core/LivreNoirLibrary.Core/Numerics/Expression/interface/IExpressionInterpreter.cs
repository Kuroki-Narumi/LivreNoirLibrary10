using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Numerics
{
    public interface IExpressionInterpreter
    {
        public void Clear();
        public bool IsEffective();
        /// <summary>
        /// Parses a string expression into an internal form that can be evaluated.
        /// </summary>
        /// <param name="expression">Expression to parse as a <see cref="Span{T}"/>.</param>
        /// <param name="exception">Set an exception if parse failed.</param>
        /// <returns><see cref="bool">true</see> if parse succeeded.</returns>
        public bool TryParse(ReadOnlySpan<char> expression, [MaybeNullWhen(true)] out Exception exception);
        public bool IsWhiteSpace(char c);
        public bool IsOpenBracket(char c);
        public bool IsCloseBracket(char c);
        public bool IsArgumentDelimiter(char c);
        public bool IsNumberCharacter(char c);
        public bool IsIdentifierCharacter(char c);
        public bool IsOperatorCharacter(char c);
    }

    public static partial class IExpressionExtensions
    {
        public static bool TryParse(this IExpressionInterpreter obj, ReadOnlySpan<char> expression)
            => obj.TryParse(expression, out _);
        public static bool TryParse(this IExpressionInterpreter obj, [NotNullWhen(true)] string? expression, [MaybeNullWhen(true)] out Exception exception)
            => obj.TryParse(expression.AsSpan(), out exception);
        public static bool TryParse(this IExpressionInterpreter obj, [NotNullWhen(true)] string? expression) => obj.TryParse(expression.AsSpan(), out _);

        public static void Parse(this IExpressionInterpreter obj, ReadOnlySpan<char> expression)
        {
            if (!obj.TryParse(expression, out var exception))
            {
                throw exception;
            }
        }

        public static void Parse(this IExpressionInterpreter obj, string expression) => Parse(obj, expression.AsSpan());

        public static SymbolEnumerator EnumSymbol(this IExpressionInterpreter obj, ReadOnlySpan<char> span) => new(span, obj);
    }
}