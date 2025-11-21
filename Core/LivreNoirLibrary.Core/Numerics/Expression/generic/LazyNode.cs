using System;

namespace LivreNoirLibrary.Numerics
{
    public partial class ReversePolishNotation<T>
    {
        public sealed class LazyFunctionNode(string symbol, Func func, ReadOnlySpan<LazyNode<T>> operands) : LazyNode<T>(symbol, operands)
        {
            private readonly Func _func = func;
            public override FuncResult<T> Execute(TryGetFunc<T> variables)
            {
                var operands = Operands;
                var span = (stackalloc T[operands.Length]);
                for (var i = 0; i < operands.Length; i++)
                {
                    var r = operands[i].Execute(variables);
                    if (r.HasException)
                    {
                        return r;
                    }
                    span[i] = r.Value;
                }
                return _func(span, variables);
            }
        }

        public sealed class LazyAndNode(ReadOnlySpan<LazyNode<T>> operands) : LazyNode<T>(AndSymbol, operands)
        {
            public override FuncResult<T> Execute(TryGetFunc<T> variables)
            {
                var operands = Operands;
                var left = operands[0].Execute(variables);
                return (left.HasException || T.IsZero(left.Value)) ? left : operands[1].Execute(variables);
            }
        }

        public sealed class LazyOrNode(ReadOnlySpan<LazyNode<T>> operands) : LazyNode<T>(OrSymbol, operands)
        {
            public override FuncResult<T> Execute(TryGetFunc<T> variables)
            {
                var operands = Operands;
                var left = operands[0].Execute(variables);
                return (left.HasException || !T.IsZero(left.Value)) ? left : operands[1].Execute(variables);
            }
        }

        public sealed class LazyConditionalNode(ReadOnlySpan<LazyNode<T>> operands) : LazyNode<T>(ConditionalSymbol, operands)
        {
            public override FuncResult<T> Execute(TryGetFunc<T> variables)
            {
                var operands = Operands;
                var condition = operands[0].Execute(variables);
                if (condition.HasException)
                {
                    return condition;
                }
                return T.IsZero(condition.Value) ? operands[2].Execute(variables) : operands[1].Execute(variables);
            }
        }

        protected virtual LazyNode<T> CreateLazyNode(FunctionNode token, ReadOnlySpan<LazyNode<T>> operands) => token.Symbol switch
        {
            AndSymbol => new LazyAndNode(operands),
            OrSymbol => new LazyOrNode(operands),
            ConditionalSymbol => new LazyConditionalNode(operands),
            _ => new LazyFunctionNode(token.Symbol, token.Func, operands),
        };
    }
}
