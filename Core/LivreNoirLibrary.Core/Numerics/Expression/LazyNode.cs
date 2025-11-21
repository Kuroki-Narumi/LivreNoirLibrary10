using System;
using System.Linq;

namespace LivreNoirLibrary.Numerics
{
    public abstract class LazyNode<T>(string symbol, ReadOnlySpan<LazyNode<T>> operands)
    {
        private readonly LazyNode<T>[] _operands = operands.ToArray();

        public string Symbol { get; } = symbol;
        public ReadOnlySpan<LazyNode<T>> Operands => _operands;
        public abstract FuncResult<T> Execute(TryGetFunc<T> variables);

        public override string ToString() => _operands.Length is 0 ? Symbol : $"{Symbol}({string.Join(", ", _operands.Select(o => o.ToString()))})";
    }
}
