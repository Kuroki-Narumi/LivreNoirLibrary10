using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Numerics
{
    public delegate bool TryGetFunc<T>(string symbol, [MaybeNullWhen(false)] out T value);

    public interface IExpressionInterpreter<T> : IExpressionInterpreter
    {
        public bool TryEvaluate(TryGetFunc<T> variables, out T result, [MaybeNullWhen(true)] out Exception exception);
        public bool TryLazyEvaluate(TryGetFunc<T> variables, out T result, [MaybeNullWhen(true)] out Exception exception);
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

        public static bool TryLazyEvaluate<T>(this IExpressionInterpreter<T> ev, TryGetFunc<T> variables, [MaybeNullWhen(false)] out T result) => ev.TryLazyEvaluate(variables, out result, out _);
        public static T LazyEvaluate<T>(this IExpressionInterpreter<T> ev, TryGetFunc<T> variables)
        {
            if (ev.TryLazyEvaluate(variables, out var result, out var exception))
            {
                return result;
            }
            throw exception;
        }
    }
}
