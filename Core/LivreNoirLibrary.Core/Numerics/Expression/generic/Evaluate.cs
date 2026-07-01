using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Numerics
{
    public partial class ReversePolishNotation<T> : IExpressionInterpreter<T>
    {
        protected virtual bool CheckResult(T result, [MaybeNullWhen(true)] out Exception exception)
        {
            exception = null;
            return true;
        }

        public bool TryGetLazyNode(List<LazyNode<T>> output, [MaybeNullWhen(true)]out Exception exception)
        {
            if (_nodes.Count is 0)
            {
                exception = ExpressionExceptions.ExpressionEmpty;
                return false;
            }
            foreach (var token in _nodes.AsSpan())
            {
                var currentCount = output.Count;
                var opCount = token.OperandCount;
                if (currentCount < opCount)
                {
                    exception = ExpressionExceptions.ArgumentTooFew(currentCount, opCount);
                    return false;
                }
                var index = currentCount - opCount;
                var operands = output.AsSpan()[index..];
                var node = CreateLazyNode(token, operands);
                output.RemoveRange(index, opCount);
                output.Add(node);
            }
            exception = null;
            return true;
        }

        public bool TryEvaluate(TryGetFunc<T> variables, out T result, [MaybeNullWhen(true)] out Exception exception)
        {
            result = default;
            using var obj = ObjectPool.Rent<List<LazyNode<T>>>(out var nodes);
            try
            {
                if (!TryGetLazyNode(nodes, out exception))
                {
                    return false;
                }
                var r = nodes[^1].Execute(variables);
                if (r.Exception is { } ex)
                {
                    exception = ex;
                    return false;
                }
                result = r.Value;
                return CheckResult(result, out exception);
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
        }

        public IEnumerable<T> EvaluateAll(TryGetFunc<T> variables)
        {
            using var obj = ObjectPool.Rent<List<LazyNode<T>>>(out var nodes);
            if (TryGetLazyNode(nodes, out _))
            {
                foreach (var node in nodes)
                {
                    var r = node.Execute(variables);
                    if (r.IsSuccessful && CheckResult(r.Value, out _))
                    {
                        yield return r.Value;
                    }
                }
            }
        }
    }
}
