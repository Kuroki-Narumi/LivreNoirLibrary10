using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Numerics
{
    public partial class ExpressionBase
    {
        protected class Token(string symbol)
        {
            public string Symbol { get; } = symbol;
            public override string ToString() => Symbol;
        }

        protected class OpenBracketToken(string symbol, string closeSymbol) : Token(symbol)
        {
            public string CloseSymbol { get; } = closeSymbol;
        }

        protected class FunctionToken(string symbol) : Token(symbol);

        protected class OperatorToken(string symbol, int operandCount, OperatorPriority priority, bool leftAssoc) : Token(symbol)
        {
            public int OperandCount { get; } = operandCount;
            public OperatorPriority Priority { get; } = priority;
            public bool LeftAssoc { get; } = leftAssoc;
        }

        protected class TernaryOperator1Token(string symbol, string secondSymbol, OperatorPriority priority = OperatorPriority.Ternary, bool leftAssoc = false) : OperatorToken(symbol, 3, priority, leftAssoc)
        {
            public string SecondSymbol { get; } = secondSymbol;
            public TernaryOperator2Token SecondToken { get; } = new(secondSymbol, priority, leftAssoc);
            public OperatorToken ToResolved() => new(Symbol + SecondSymbol, OperandCount, Priority, LeftAssoc);
        }

        protected class TernaryOperator2Token(string symbol, OperatorPriority priority, bool leftAssoc) : OperatorToken(symbol, 0, priority, leftAssoc) { }

        private static readonly Dictionary<string, OpenBracketToken> _brackets = new()
        {
            [OpenBracketSymbol1] = new(OpenBracketSymbol1, CloseBracketSymbol1),
            [OpenBracketSymbol2] = new(OpenBracketSymbol2, CloseBracketSymbol2),
            [OpenBracketSymbol3] = new(OpenBracketSymbol3, CloseBracketSymbol3),
        };
        protected virtual OpenBracketToken GetBracketToken(string symbol) => _brackets[symbol];

        protected static OperatorToken CreateUnaryOperatorToken(string symbol, OperatorPriority priority = OperatorPriority.Unary, bool leftAssoc = false)
            => new(symbol, 1, priority, leftAssoc);
        protected static OperatorToken CreateBinaryOperatorToken(string symbol, OperatorPriority priority = OperatorPriority.Addition, bool leftAssoc = true)
            => new(symbol, 2, priority, leftAssoc);

        protected static readonly OperatorToken AddToken = CreateBinaryOperatorToken(PlusSymbol, OperatorPriority.Addition);
        protected static readonly OperatorToken SubtractToken = CreateBinaryOperatorToken(MinusSymbol, OperatorPriority.Addition);
        protected static readonly OperatorToken MultiplyToken = CreateBinaryOperatorToken(MultiplySymbol, OperatorPriority.Multiply);
        protected static readonly OperatorToken DivideToken = CreateBinaryOperatorToken(DivideSymbol, OperatorPriority.Multiply);
        protected static readonly OperatorToken ModuloToken = CreateBinaryOperatorToken(ModuloSymbol, OperatorPriority.Multiply);
        protected static readonly OperatorToken PowerToken = CreateBinaryOperatorToken(PowerSymbol, OperatorPriority.Power, false);

        protected static readonly OperatorToken UnaryPlusToken = CreateUnaryOperatorToken(UnaryPlusSymbol);
        protected static readonly OperatorToken UnaryMinusToken = CreateUnaryOperatorToken(UnaryMinusSymbol);

        protected static readonly OperatorToken AndToken = CreateBinaryOperatorToken(AndSymbol, OperatorPriority.And);
        protected static readonly OperatorToken OrToken = CreateBinaryOperatorToken(OrSymbol, OperatorPriority.Or);
        protected static readonly OperatorToken XorToken = CreateBinaryOperatorToken(XorSymbol, OperatorPriority.Xor);
        protected static readonly OperatorToken NotToken = CreateUnaryOperatorToken(NotSymbol);

        protected static readonly OperatorToken ShiftLeftToken = CreateBinaryOperatorToken(ShiftLeftSymbol, OperatorPriority.BitShift);
        protected static readonly OperatorToken ShiftRightArithmeticToken = CreateBinaryOperatorToken(ShiftRightArithmeticSymbol, OperatorPriority.BitShift);
        protected static readonly OperatorToken ShiftRightLogicalToken = CreateBinaryOperatorToken(ShiftRightLogicalSymbol, OperatorPriority.BitShift);
        protected static readonly OperatorToken OnesComplementToken = CreateUnaryOperatorToken(OnesComplementSymbol);

        protected static readonly OperatorToken LessToken = CreateBinaryOperatorToken(LessSymbol, OperatorPriority.Comparison);
        protected static readonly OperatorToken LessOrEqualToken = CreateBinaryOperatorToken(LessOrEqualSymbol, OperatorPriority.Comparison);
        protected static readonly OperatorToken GreaterToken = CreateBinaryOperatorToken(GreaterSymbol, OperatorPriority.Comparison);
        protected static readonly OperatorToken GreaterOrEqualToken = CreateBinaryOperatorToken(GreaterOrEqualSymbol, OperatorPriority.Comparison);
        protected static readonly OperatorToken CompareToken = CreateBinaryOperatorToken(CompareSymbol, OperatorPriority.Comparison);

        protected static readonly OperatorToken EqualToken = CreateBinaryOperatorToken(EqualSymbol, OperatorPriority.Equality);
        protected static readonly OperatorToken NotEqualToken = CreateBinaryOperatorToken(NotEqualSymbol, OperatorPriority.Equality);

        protected static readonly TernaryOperator1Token Conditional1Token = new(Conditional1Symbol, Conditional2Symbol);
        protected static readonly OperatorToken Conditional2Token = Conditional1Token.SecondToken;

        private static readonly Dictionary<string, OperatorToken> _unary_map = new()
        {
            [UnaryPlusSymbol] = UnaryPlusToken,
            [PlusSymbol] = UnaryPlusToken,
            [UnaryMinusSymbol] = UnaryMinusToken,
            [MinusSymbol] = UnaryMinusToken,
            [NotSymbol] = NotToken,
        };

        private static readonly Dictionary<string, OperatorToken> _operator_map = InitializeOperatorMap();
        private static Dictionary<string, OperatorToken> InitializeOperatorMap()
        {
            Dictionary<string, OperatorToken> map = [];
            void Add(OperatorToken token, string? key = null) => map.Add(key ?? token.Symbol, token);

            Add(AddToken);
            Add(SubtractToken);
            Add(MultiplyToken);
            Add(DivideToken);
            Add(ModuloToken);

            Add(AndToken);
            Add(AndToken, AndSymbol2);
            Add(OrToken);
            Add(OrToken, OrSymbol2);
            Add(XorToken);

            Add(LessToken);
            Add(LessOrEqualToken);
            Add(GreaterToken);
            Add(GreaterOrEqualToken);
            Add(CompareToken);

            Add(EqualToken);
            Add(EqualToken, EqualSymbol2);
            Add(NotEqualToken);
            Add(NotEqualToken, NotSymbol);

            Add(Conditional1Token);
            Add(Conditional2Token);

            return map;
        }

        protected bool TryGetOperatorToken(string symbol, bool expectValueToken, [MaybeNullWhen(false)] out OperatorToken token, [MaybeNullWhen(true)] out Exception exception)
            => expectValueToken
                ? TryGetUnaryOperatorToken(symbol, out token, out exception)
                : TryGetMultiOperatorToken(symbol, out token, out exception);

        protected virtual bool TryGetUnaryOperatorToken(string symbol, [MaybeNullWhen(false)] out OperatorToken token, [MaybeNullWhen(true)] out Exception exception)
        {
            if (_unary_map.TryGetValue(symbol, out token))
            {
                exception = null;
                return true;
            }
            token = null;
            exception = ExpressionExceptions.UnknownOperatorSymbol(symbol);
            return false;
        }

        protected virtual bool TryGetMultiOperatorToken(string symbol, [MaybeNullWhen(false)] out OperatorToken token, [MaybeNullWhen(true)] out Exception exception)
        {
            if (_operator_map.TryGetValue(symbol, out token))
            {
                exception = null;
                return true;
            }
            token = null;
            exception = ExpressionExceptions.UnknownOperatorSymbol(symbol);
            return false;
        }
    }
}
