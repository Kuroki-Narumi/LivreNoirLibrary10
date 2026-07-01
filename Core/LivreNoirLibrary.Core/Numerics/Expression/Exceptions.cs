using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Numerics
{
    public static class ExpressionExceptions
    {
        public static ExpressionParseException UnexpectedToken(string symbol, ReadOnlySpan<char> expression, int index) => new($"unexpected token detected {symbol}", expression, index);

        public static ExpressionParseException OpenBracketMissing(ReadOnlySpan<char> expression, int index) => new($"open bracket missing", expression, index);

        public static ExpressionParseException UnexpectedCloseBracket(string symbol, string expected, ReadOnlySpan<char> expression, int index)
            => new($"unexpected close bracket: {symbol} expected: {expected})", expression, index);

        public static ExpressionParseException UnexpectedDelimiter(ReadOnlySpan<char> expression, int index) => new($"argument delimiter must be written in a function", expression, index);

        public static ExpressionParseException UnparsableSymbol(string symbol, ReadOnlySpan<char> expression, int index) => new($"unparsable number symbol ({symbol})", expression, index);

        public static ExpressionParseException UnexpectedTernarySymbol(string symbol, string expected, ReadOnlySpan<char> expression, int index) 
            => new($"unexpected ternary symbol: {symbol} expected: {expected})", expression, index);

        public static ExpressionParseException TernarySymbolMissing(ReadOnlySpan<char> expression, int index) => new($"ternary operator symbol missing", expression, index);

        public static ExpressionParseException UnresolvedTernaryOperator(string symbol, ReadOnlySpan<char> expression, int index)
            => new($"unresolved ternary operator: {symbol}", expression, index);

        public static ExpressionParseException UnclosedOpenBracket(string symbol, ReadOnlySpan<char> expression, int index)
            => new($"unclosed open bracket: {symbol}", expression, index);

        public static ExpressionParseException Unhandled(ReadOnlySpan<char> expression, int index) => new("unhandled exception", expression, 0);

        public static NotImplementedException UnknownOperatorSymbol(string symbol) => new($"unknown operator symbol: {symbol}");

        public static NotImplementedException FunctionNotSupported(string symbol, int operandCount) => new($"function \"{symbol}{{{operandCount}}}\" is not supported.");

        public static readonly Exception ExpressionEmpty = new("the expression is empty.");

        public static ArgumentException ArgumentTooFew(int actual, int expected) => new($"too few arguments ({actual}, expected:{expected}).");

        public static KeyNotFoundException VariableNotFound(string symbol) => new($"the variable \"{symbol}\" is not found.");

        public static readonly OverflowException ResultInfinite = new("result must be a finite number.");
    }
}
