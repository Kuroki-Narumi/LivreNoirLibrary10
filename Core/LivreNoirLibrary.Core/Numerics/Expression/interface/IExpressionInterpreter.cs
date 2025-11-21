using LivreNoirLibrary.ObjectModel;
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
        /// <summary>
        /// Clears the internal state of this <see cref="IExpressionInterpreter"/>.
        /// </summary>
        /// <remarks>
        /// After calling this method, the user must call <see cref="TryParse(ReadOnlySpan{char}, out Exception)"/> before evaluation.
        /// </remarks>
        void Clear();

        /// <summary>
        /// Gets whether this <see cref="IExpressionInterpreter"/> is ready to evaluation.
        /// </summary>
        /// <returns><see langword="true"/> if this <see cref="IExpressionInterpreter"/> is ready; otherwise, <see langword="false"/>.</returns>
        bool IsEffective();

        /// <summary>
        /// Parses a string expression into an internal form that can be evaluated.
        /// </summary>
        /// <param name="expression">Expression to parse as a <see cref="Span{T}"/>.</param>
        /// <param name="exception">Set an exception if parse failed.</param>
        /// <returns><see langword="true"/> if parse succeeded; otherwise, <see langword="false"/>.</returns>
        bool TryParse(ReadOnlySpan<char> expression, [MaybeNullWhen(true)] out Exception exception);

        /// <summary>
        /// Determines whether the specified character is classified as a white space.
        /// </summary>
        /// <param name="c">The character to evaluate.</param>
        /// <returns><see langword="true"/> if the specified character is a white-space character; otherwise, <see langword="false"/>.</returns>
        bool IsWhiteSpace(char c) => char.IsWhiteSpace(c);

        /// <summary>
        /// Determines whether the specified character is classified as an open bracket.
        /// </summary>
        /// <param name="c">The character to evaluate.</param>
        /// <returns><see langword="true"/> if the specified character is an open-bracket character; otherwise <see langword="false"/>.</returns>
        bool IsOpenBracket(char c) => c is '(';

        /// <summary>
        /// Determines whether the specified character is classified as an close bracket.
        /// </summary>
        /// <param name="c">The character to evaluate.</param>
        /// <returns><see langword="true"/> if the specified character is a close-bracket character; otherwise <see langword="false"/>.</returns>
        bool IsCloseBracket(char c) => c is ')';

        /// <summary>
        /// Determines whether the specified character is classified as a delimiter for arguments.
        /// </summary>
        /// <param name="c">The character to evaluate.</param>
        /// <returns><see langword="true"/> if the specified character is a delimiter; otherwise <see langword="false"/>.</returns>
        bool IsArgumentDelimiter(char c) => c is ',';

        /// <summary>
        /// Determines whether the specified character is classified as a digit or a decimal point.
        /// </summary>
        /// <param name="c">The character to evaluate.</param>
        /// <returns><see langword="true"/> if the specified character is a digit character or a decimal point character; otherwise <see langword="false"/>.</returns>
        bool IsNumberCharacter(char c) => c is '.' or (>= '0' and <= '9');

        /// <summary>
        /// Determines whether the specified character can use as an identifier.
        /// </summary>
        /// <param name="c">The character to evaluate.</param>
        /// <returns><see langword="true"/> if the specified character can use in an identifier; otherwise <see langword="false"/>.</returns>
        bool IsIdentifierCharacter(char c) => c is '.' or (>= '0' and <= '9') or (>= 'A' and <= 'Z') or '_' or (>= 'a' and <= 'z');

        /// <summary>
        /// Determines whether the specified character can use as an operator.
        /// </summary>
        /// <param name="c">The character to evaluate.</param>
        /// <returns><see langword="true"/> if the specified character can use in an operator; otherwise <see langword="false"/>.</returns>
        bool IsOperatorCharacter(char c) => c is '!' or '"' or '#' or '$' or '%' or '&' or '\'' or '+' or '*' or '-' or '/' or ':' or ';' or '<' or '=' or '>' or '?' or '@' or '\\' or '^' or '`' or '|' or '~';
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