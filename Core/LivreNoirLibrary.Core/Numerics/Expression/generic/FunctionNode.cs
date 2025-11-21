using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Numerics
{
    public partial class ReversePolishNotation<T>
    {
        public delegate FuncResult<T> Func(ReadOnlySpan<T> operands, TryGetFunc<T> variables);

        protected readonly struct FunctionNode(string symbol, int operandCount, Func func)
        {
            public readonly string Symbol = symbol;
            public readonly int OperandCount = operandCount;
            public readonly Func Func = func;

            public override string ToString() => Symbol;
        }

        private static readonly Dictionary<(string, int), FunctionNode> _node_map = InitializeNodeMap();
        private static Dictionary<(string, int), FunctionNode> InitializeNodeMap()
        {
            Dictionary<(string, int), FunctionNode> map = [];
            void Add(FunctionNode node) => map.Add((node.Symbol, node.OperandCount), node);
            void Add2(OperatorToken token, Func func) => Add(new(token.Symbol, token.OperandCount, func));
            Add2(AddToken, (s, v) => s[0] + s[1]);
            Add2(SubtractToken, (s, v) => s[0] - s[1]);
            Add2(MultiplyToken, (s, v) => s[0] * s[1]);
            Add2(DivideToken, (s, v) => s[0] / s[1]);
            Add2(ModuloToken, (s, v) => s[0] % s[1]);
            Add2(UnaryPlusToken, (s, v) => +s[0]);
            Add2(UnaryMinusToken, (s, v) => -s[0]);
            Add2(AndToken, (s, v) => T.IsZero(s[0]) ? s[0] : s[1]);
            Add2(OrToken, (s, v) => T.IsZero(s[0]) ? s[1] : s[0]);
            Add2(XorToken, (s, v) => T.IsZero(s[0]) ? s[1] : T.IsZero(s[1]) ? s[0] : T.Zero);
            Add2(NotToken, (s, v) => T.IsZero(s[0]) ? T.One : T.Zero);
            Add2(LessToken, (s, v) => s[0] < s[1] ? T.One : T.Zero);
            Add2(LessOrEqualToken, (s, v) => s[0] <= s[1] ? T.One : T.Zero);
            Add2(GreaterToken, (s, v) => s[0] > s[1] ? T.One : T.Zero);
            Add2(GreaterOrEqualToken, (s, v) => s[0] >= s[1] ? T.One : T.Zero);
            Add2(CompareToken, (s, v) => s[0] < s[1] ? -T.One : s[0] > s[1] ? T.One : T.Zero);
            Add2(EqualToken, (s, v) => s[0] == s[1] ? T.One : T.Zero);
            Add2(NotEqualToken, (s, v) => s[0] == s[1] ? T.Zero : T.One);
            Add(new(ConditionalSymbol, 3, (s, v) => T.IsZero(s[0]) ? s[2] : s[1]));
            Add(new(MaxSymbol, 2, (s, v) => T.Max(s[0], s[1])));
            Add(new(MinSymbol, 2, (s, v) => T.Min(s[0], s[1])));
            Add(new(AbsSymbol, 1, (s, v) => T.Abs(s[0])));
            return map;
        }

        protected virtual bool TryGetFunctionNode(string symbol, int operandCount, out FunctionNode node, [MaybeNullWhen(true)]out Exception exception)
        {
            if (_node_map.TryGetValue((symbol, operandCount), out node))
            {
                exception = null;
                return true;
            }
            exception = new NotImplementedException($"function \"{symbol}{{{operandCount}}}\" is not supported.");
            return false;
        }

        protected static FunctionNode CreateValueNode(T value) => new(value.ToString()!, 0, (s, v) => value);
        protected static FunctionNode CreateVariableNode(string symbol) => new(symbol, 0, 
            (s, v) => v(symbol, out var value) ? value : new KeyNotFoundException($"the variable \"{symbol}\" is not found."));
    }
}
