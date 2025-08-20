using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Numerics
{
    public interface IExpressionEvaluator<T>
    {
        public bool TryParse(string expression, [MaybeNullWhen(true)] out Exception exception);
        public bool TryEvaluate(IDictionary<string, T> variables, [MaybeNullWhen(false)] out T result, [MaybeNullWhen(true)] out Exception exception);
        public bool TryLazyEvaluate(IDictionary<string, T> variables, [MaybeNullWhen(false)] out T result, [MaybeNullWhen(true)] out Exception exception);
    }

    public static class IExpressionEvaluatorExtensions
    {
        public static bool TryParse<T>(this IExpressionEvaluator<T> ev, string expression)
            => ev.TryParse(expression, out _);
        public static bool TryEvaluate<T>(this IExpressionEvaluator<T> ev, IDictionary<string, T> variables, [MaybeNullWhen(false)] out T result)
            => ev.TryEvaluate(variables, out result, out _);
        public static bool TryLazyEvaluate<T>(this IExpressionEvaluator<T> ev, IDictionary<string, T> variables, [MaybeNullWhen(false)] out T result) 
            => ev.TryLazyEvaluate(variables, out result, out _);
    }
}
