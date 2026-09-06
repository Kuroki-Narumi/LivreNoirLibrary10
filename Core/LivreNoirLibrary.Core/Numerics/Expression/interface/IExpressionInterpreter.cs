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
        /// <param name="symbolTypeProvider"></param>
        /// <returns><see langword="true"/> if parse succeeded; otherwise, <see langword="false"/>.</returns>
        bool TryParse(ReadOnlySpan<char> expression, [MaybeNullWhen(true)] out Exception exception, ICharTypeProvider? symbolTypeProvider = null);
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
    }
}