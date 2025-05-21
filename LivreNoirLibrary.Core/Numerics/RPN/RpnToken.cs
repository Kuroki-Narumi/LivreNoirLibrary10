using System;
using System.Collections.Generic;
using System.Linq;

namespace LivreNoirLibrary.Numerics
{
    public abstract class RpnToken(string symbol) : IRpnToken
    {
        public string Symbol { get; } = symbol;
        public virtual bool IsValue => false;
        public virtual int Priority => 0;

        public RpnToken(char c) : this($"{c}") { }
    }

    public sealed class RpnOpenBracketToken : RpnToken, IRpnToken
    {
        public static RpnOpenBracketToken Instance { get; } = new();
        private RpnOpenBracketToken() : base(RpnConstants.OpenBracket) { }
    }

    public class RpnVariableToken : RpnToken, IRpnToken
    {
        public override bool IsValue => true;

        public RpnVariableToken(string symbol) : base(symbol) { }
        public RpnVariableToken(char c) : base(c) { }
    }

    public partial class ReversePolishNotation<T>
    {
        public sealed class ValueToken(T value) : RpnToken($"{value}"), IRpnValueToken<T>
        {
            public T Value { get; } = value;
            public override bool IsValue => true;
        }

        public sealed class InfixOperatorToken(string symbol, int priority, bool leftAssoc, Func<T, T, T> func) : RpnToken(symbol), IRpnInfixOperatorToken<T>
        {
            public bool LeftAssoc { get; } = leftAssoc;
            public override int Priority { get; } = priority;
            public Func<T, T, T> Func { get; } = func;
        }

        public sealed class FunctionToken(string symbol, int opCount, Func<List<T>, T> func) : RpnToken(symbol), IRpnFunctionToken<T>
        {
            public int OperandCount { get; } = opCount;
            public Func<List<T>, T> Func { get; } = func;
        }
    }
}
