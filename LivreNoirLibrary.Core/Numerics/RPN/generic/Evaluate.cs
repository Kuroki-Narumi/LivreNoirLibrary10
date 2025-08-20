using LivreNoirLibrary.Debug;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Numerics
{
    public partial class ReversePolishNotation<T> : IExpressionEvaluator<T>
    {
        protected virtual bool CheckResult(T result, out Exception? exception)
        {
            exception = null;
            return true;
        }

        private static void ThrowExpressionEmptyException() => throw new Exception("the expression is empty.");
        private static void ThrowArgumentTooFewException(int actual, int expected) => throw new ArgumentException($"too few arguments ({actual}, expected:{expected}).");

        public bool TryEvaluate(IDictionary<string, T> variables, out T result, [MaybeNullWhen(true)] out Exception exception)
        {
            try
            {
                if (_nodes.Count is 0)
                {
                    ThrowExpressionEmptyException();
                }
                List<T> stack = [];
                foreach (var token in CollectionsMarshal.AsSpan(_nodes))
                {
                    var currentCount = stack.Count;
                    var opCount = token.OperandCount;
                    if (currentCount < opCount)
                    {
                        ThrowArgumentTooFewException(currentCount, opCount);
                    }
                    var index = currentCount - opCount;
                    var operands = CollectionsMarshal.AsSpan(stack)[index..];
                    var value = token.Func(operands, variables);
                    stack.RemoveRange(index, opCount);
                    stack.Add(value);
                }
                result = stack[^1];
                return CheckResult(result, out exception);
            }
            catch (Exception ex)
            {
                result = default;
                exception = ex;
                return false;
            }
        }

        public LazyNode CreateLazyNode()
        {
            if (_nodes.Count is 0)
            {
                ThrowExpressionEmptyException();
            }
            List<LazyNode> stack = [];
            foreach (var token in CollectionsMarshal.AsSpan(_nodes))
            {
                var currentCount = stack.Count;
                var opCount = token.OperandCount;
                if (currentCount < opCount)
                {
                    ThrowArgumentTooFewException(currentCount, opCount);
                }
                var index = currentCount - opCount;
                var operands = CollectionsMarshal.AsSpan(stack)[index..];
                var node = CreateLazyNode(token, operands);
                stack.RemoveRange(index, opCount);
                stack.Add(node);
            }
            return stack[^1];
        }

        public bool TryLazyEvaluate(IDictionary<string, T> variables, out T result, [MaybeNullWhen(true)] out Exception exception)
        {
            try
            {
                result = CreateLazyNode().Execute(variables);
                return CheckResult(result, out exception);
            }
            catch (Exception ex)
            {
                result = default;
                exception = ex;
                return false;
            }
        }
    }
}
