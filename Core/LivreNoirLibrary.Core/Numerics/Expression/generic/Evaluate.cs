using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Numerics
{
    public partial class ReversePolishNotation<T> : IExpressionInterpreter<T>
    {
        protected virtual bool CheckResult(T result, [MaybeNullWhen(true)] out Exception exception)
        {
            exception = null;
            return true;
        }

        private static readonly Exception ExpressionEmptyException = new("the expression is empty.");
        private static ArgumentException ArgumentTooFewException(int actual, int expected) => new($"too few arguments ({actual}, expected:{expected}).");

        public bool TryEvaluate(TryGetFunc<T> variables, out T result, [MaybeNullWhen(true)]out Exception exception)
        {
            result = default;
            if (_nodes.Count is 0)
            {
                exception = ExpressionEmptyException;
                return false;
            }
            List<T> stack = [];
            foreach (var token in CollectionsMarshal.AsSpan(_nodes))
            {
                var currentCount = stack.Count;
                var opCount = token.OperandCount;
                if (currentCount < opCount)
                {
                    exception = ArgumentTooFewException(currentCount, opCount);
                    return false;
                }
                var index = currentCount - opCount;
                var operands = CollectionsMarshal.AsSpan(stack)[index..];
                try
                {
                    var r = token.Func(operands, variables);
                    if (r.Exception is { } ex)
                    {
                        exception = ex;
                        return false;
                    }
                    stack.RemoveRange(index, opCount);
                    stack.Add(r.Value);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    return false;
                }
            }
            result = stack[^1];
            return CheckResult(result, out exception);
        }

        public bool TryGetLazyNode([MaybeNullWhen(false)] out LazyNode node, [MaybeNullWhen(true)]out Exception exception)
        {
            node = default;
            if (_lazyNode is null)
            {
                if (_nodes.Count is 0)
                {
                    exception = ExpressionEmptyException;
                    return false;
                }
                List<LazyNode> stack = [];
                foreach (var token in CollectionsMarshal.AsSpan(_nodes))
                {
                    var currentCount = stack.Count;
                    var opCount = token.OperandCount;
                    if (currentCount < opCount)
                    {
                        exception = ArgumentTooFewException(currentCount, opCount);
                        return false;
                    }
                    var index = currentCount - opCount;
                    var operands = CollectionsMarshal.AsSpan(stack)[index..];
                    node = CreateLazyNode(token, operands);
                    stack.RemoveRange(index, opCount);
                    stack.Add(node);
                }
                _lazyNode = stack[^1];
            }
            node = _lazyNode;
            exception = null;
            return true;
        }

        public bool TryLazyEvaluate(TryGetFunc<T> variables, out T result, [MaybeNullWhen(true)] out Exception exception)
        {
            result = default;
            if (TryGetLazyNode(out var node, out exception))
            {
                try
                {
                    var r = node.Execute(variables);
                    if (r.Exception is { } ex)
                    {
                        exception = ex;
                        return false;
                    }
                    result = r.Value;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    return false;
                }
                return CheckResult(result, out exception);
            }
            return false;
        }
    }
}
