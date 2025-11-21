using System;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Numerics
{
    public delegate bool TryGetFunc<T>(string symbol, [MaybeNullWhen(false)] out T value);

    public interface IExpressionInterpreter<T> : IExpressionInterpreter
    {
        /// <summary>
        /// Attempts to evaluate the expression using the provided variable resolver, returning a value if successful.
        /// </summary>
        /// <remarks>If evaluation fails due to an error, <paramref name="exception"/> will contain the
        /// exception describing the failure, and <paramref name="result"/> will be set to its default value. This
        /// method does not throw exceptions for evaluation errors; instead, it reports them via the out
        /// parameter.</remarks>
        /// <param name="variables">A delegate that resolves variable values required for evaluation. Must not be null.</param>
        /// <param name="result">When this method returns, contains the evaluated result if the operation succeeds; otherwise, the default
        /// value for type <typeparamref name="T"/>.</param>
        /// <param name="exception">When this method returns, contains the exception encountered during evaluation if the operation fails;
        /// otherwise, <see langword="null"/>.</param>
        /// <returns>true if the evaluation succeeds and a result is produced; otherwise, false.</returns>
        bool TryEvaluate(TryGetFunc<T> variables, out T result, [MaybeNullWhen(true)] out Exception exception);
    }

    public static partial class IExpressionExtensions
    {
        public static bool TryEvaluate<T>(this IExpressionInterpreter<T> ev, TryGetFunc<T> variables, [MaybeNullWhen(false)] out T result) => ev.TryEvaluate(variables, out result, out _);
        public static T Evaluate<T>(this IExpressionInterpreter<T> ev, TryGetFunc<T> variables)
        {
            if (ev.TryEvaluate(variables, out var result, out var exception))
            {
                return result;
            }
            throw exception;
        }
    }
}
