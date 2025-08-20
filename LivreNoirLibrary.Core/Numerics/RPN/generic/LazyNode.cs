using System;
using System.Collections.Generic;
using System.Linq;

namespace LivreNoirLibrary.Numerics
{
    public partial class ReversePolishNotation<T>
    {
        public abstract class LazyNode(string symbol, ReadOnlySpan<LazyNode> operands)
        {
            private readonly LazyNode[] _operands = operands.ToArray();

            public string Symbol { get; } = symbol;
            public ReadOnlySpan<LazyNode> Operands => _operands;
            public abstract T Execute(IDictionary<string, T> variables);

            public override string ToString() => _operands.Length is 0 ? Symbol : $"{Symbol}({string.Join(", ", _operands.Select(o => o.ToString()))})";
        }

        public sealed class LazyFunctionNode(string symbol, Func func, ReadOnlySpan<LazyNode> operands) : LazyNode(symbol, operands)
        {
            private readonly Func _func = func;
            public override T Execute(IDictionary<string, T> variables)
            {
                var operands = Operands;
                var span = (stackalloc T[operands.Length]);
                for (var i = 0; i <  operands.Length; i++)
                {
                    span[i] = operands[i].Execute(variables);
                }
                return _func(span, variables);
            }
        }

        public sealed class LazyAndNode(ReadOnlySpan<LazyNode> operands) : LazyNode(AndSymbol, operands)
        {
            public override T Execute(IDictionary<string, T> variables)
            {
                var operands = Operands;
                var left = operands[0].Execute(variables);
                return T.IsZero(left) ? T.Zero : operands[1].Execute(variables);
            }
        }

        public sealed class LazyOrNode(ReadOnlySpan<LazyNode> operands) : LazyNode(OrSymbol, operands)
        {
            public override T Execute(IDictionary<string, T> variables)
            {
                var operands = Operands;
                var left = operands[0].Execute(variables);
                return T.IsZero(left) ? operands[1].Execute(variables) : left;
            }
        }

        public sealed class LazyConditionalNode(ReadOnlySpan<LazyNode> operands) : LazyNode(ConditionalSymbol, operands)
        {
            public override T Execute(IDictionary<string, T> variables)
            {
                var operands = Operands;
                return T.IsZero(operands[0].Execute(variables)) ? operands[2].Execute(variables) : operands[1].Execute(variables);
            }
        }

        protected virtual LazyNode CreateLazyNode(FunctionNode token, ReadOnlySpan<LazyNode> operands) => token.Symbol switch
        {
            AndSymbol => new LazyAndNode(operands),
            OrSymbol => new LazyOrNode(operands),
            ConditionalSymbol => new LazyConditionalNode(operands),
            _ => new LazyFunctionNode(token.Symbol, token.Func, operands),
        };
    }
}
