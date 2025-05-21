using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Numerics
{
    public interface IRpnToken
    {
        public string Symbol { get; }
        public int Priority { get; }
        public bool IsValue { get; }
    }

    public interface IRpnValueToken<T> : IRpnToken
    {
        public T Value { get; }
    }

    public interface IRpnInfixOperatorToken<T> : IRpnToken
    {
        public bool LeftAssoc { get; }
        public Func<T, T, T> Func { get; }
    }

    public interface IRpnFunctionToken<T> : IRpnToken
    {
        public int OperandCount { get; }
        public Func<List<T>, T> Func { get; }
    }
}
