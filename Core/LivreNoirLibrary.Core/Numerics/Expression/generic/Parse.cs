using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Numerics
{
    public partial class ReversePolishNotation<T> : ExpressionBase, IExpressionInterpreter<T>
        where T : unmanaged, INumber<T>
    {
        private readonly List<FunctionNode> _nodes = [];

        public void Clear()
        {
            _nodes.Clear();
        }

        public bool IsEffective() => _nodes.Count is > 0;

        public override string ToString() => $"{GetType()}{{{string.Join(" ", _nodes.Select(s => s.Symbol))}}}";

        public bool TryParse(ReadOnlySpan<char> expression, [MaybeNullWhen(true)]out Exception exception)
        {
            Clear();
            var list = _nodes;
            Stack<Token> stack = [];
            Stack<int> functionOperandCounts = [];
            var expectValueToken = true;
            exception = null;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            bool AssertAddNode(Token token, ReadOnlySpan<char> expression, int index, [MaybeNullWhen(false)]out Exception exception)
            {
                int opCount;
                switch (token)
                {
                    case OperatorToken op:
                        opCount = op.OperandCount;
                        break;
                    case FunctionToken:
                        opCount = functionOperandCounts.Pop() + (expectValueToken ? 0 : 1);
                        break;
                    default:
                        exception = ExpressionExceptions.UnexpectedToken(token.Symbol, expression, index);
                        return true;
                }
                if (TryGetFunctionNode(token.Symbol, opCount, out var node, out exception))
                {
                    list.Add(node);
                    return false;
                }
                return true;
            }

            foreach (var (i, type, symbol) in this.EnumSymbol(expression))
            {
                switch (type)
                {
                    case SymbolType.OpenBracket:
                        stack.Push(GetBracketToken(symbol));
                        expectValueToken = true;
                        break;
                    case SymbolType.CloseBracket:
                        // 開き括弧が見つかるまでスタックを掘る
                        while (stack.TryPop(out var top))
                        {
                            if (top is OpenBracketToken open)
                            {
                                // 対応する開き括弧でない場合は不正
                                if (open.CloseSymbol != symbol)
                                {
                                    exception = ExpressionExceptions.UnexpectedCloseBracket(symbol, open.CloseSymbol, expression, i);
                                    goto ReturnInvalid;
                                }
                                // スタックのトップが関数トークンの場合は、それも取り出す
                                if (stack.TryPeek(out top) && top is FunctionToken)
                                {
                                    stack.Pop();
                                    if (AssertAddNode(top, expression, i, out exception))
                                    {
                                        goto ReturnInvalid;
                                    }
                                }
                                // メインループ終了
                                expectValueToken = false;
                                goto EndOfLoop;
                            }
                            else if (AssertAddNode(top, expression, i, out exception))
                            {
                                goto ReturnInvalid;
                            }
                        }
                        // 開き括弧が見つからなかった場合は不正
                        exception = ExpressionExceptions.OpenBracketMissing(expression, i);
                        goto ReturnInvalid;
                    case SymbolType.ArgumentDelimiter:
                        // 関数の内部でない場合は不正
                        if (functionOperandCounts.Count is 0)
                        {
                            exception = ExpressionExceptions.UnexpectedDelimiter(expression, i);
                            goto ReturnInvalid;
                        }
                        // 開き括弧が見つかるまでスタックを掘る
                        while (stack.TryPeek(out var top))
                        {
                            if (top is OpenBracketToken)
                            {
                                // 引数の数を1増やす
                                var count = functionOperandCounts.Pop();
                                functionOperandCounts.Push(count + 1);
                                // メインループ終了
                                expectValueToken = true;
                                goto EndOfLoop;
                            }
                            else
                            {
                                stack.Pop();
                                if (AssertAddNode(top, expression, i, out exception))
                                {
                                    goto ReturnInvalid;
                                }
                            }
                        }
                        // 開き括弧が見つからなかった場合は不正
                        exception = ExpressionExceptions.OpenBracketMissing(expression, i);
                        goto ReturnInvalid;
                    case SymbolType.Number:
                        if (T.TryParse(symbol, null, out var value))
                        {
                            // 数値として解釈可能な場合はリストにそのまま追加
                            list.Add(CreateValueNode(value));
                            expectValueToken = false;
                        }
                        else
                        {
                            // 数値に変換できない場合は不正
                            exception = ExpressionExceptions.UnparsableSymbol(symbol, expression, i);
                            goto ReturnInvalid;
                        }
                        break;
                    case SymbolType.Variable:
                        list.Add(CreateVariableNode(symbol));
                        expectValueToken = false;
                        break;
                    case SymbolType.Function:
                        stack.Push(new FunctionToken(symbol));
                        functionOperandCounts.Push(0);
                        expectValueToken = true;
                        break;
                    case SymbolType.Operator:
                        if (TryGetOperatorToken(symbol, expectValueToken, out var token, out exception))
                        {
                            // 三項演算子の第2記号の場合
                            if (token is TernaryOperator2Token)
                            {
                                // 第一記号か開き括弧が見つかるまでスタックを掘る
                                while (stack.TryPop(out var top) && top is not OpenBracketToken)
                                {
                                    if (top is TernaryOperator1Token op)
                                    {
                                        // 期待と異なる第一記号が見つかった場合
                                        if (op.SecondSymbol != symbol)
                                        {
                                            exception = ExpressionExceptions.UnexpectedTernarySymbol(symbol, op.SecondSymbol, expression, i);
                                            goto ReturnInvalid;
                                        }
                                        // 完成した演算子としてスタックに再プッシュ
                                        stack.Push(op.ToResolved());
                                        expectValueToken = true;
                                        goto EndOfLoop;
                                    }
                                    else if (AssertAddNode(top, expression, i, out exception))
                                    {
                                        goto ReturnInvalid;
                                    }
                                }
                                // 第一記号が見つからなかった場合は不正
                                exception = ExpressionExceptions.TernarySymbolMissing(expression, i);
                                goto ReturnInvalid;
                            }
                            else
                            {
                                // スタックトップの演算子をチェック
                                while (stack.TryPeek(out var top) && top is OperatorToken op &&
                                    // トップの演算子の優先度が現在の演算子よりも高いか、演算子が左結合かつ同じ優先度の場合
                                    (token.Priority < op.Priority || (token.LeftAssoc && token.Priority == op.Priority)))
                                {
                                    // スタックトップの演算子を確定
                                    stack.Pop(); 
                                    if (AssertAddNode(top, expression, i, out exception))
                                    {
                                        goto ReturnInvalid;
                                    }
                                }
                                stack.Push(token);
                                expectValueToken = true;
                                break;
                            }
                        }
                        else
                        {
                            goto ReturnInvalid;
                        }
                    // その他の文字は全て無視する
                }
            EndOfLoop:;
                //ExConsole.Write($"index={i}, list=[{string.Join(", ", list.Select(n => n.Symbol))}], stack=[{string.Join(", ", stack.Select(n => n.Symbol))}]");
            }
            var len = expression.Length;
            while (stack.TryPop(out var token))
            {
                switch (token)
                {
                    case TernaryOperator1Token: // 未処理の三項演算子
                        exception = ExpressionExceptions.UnresolvedTernaryOperator(token.Symbol, expression, len);
                        goto ReturnInvalid;
                    case OpenBracketToken: // 閉じられていない括弧
                        exception = ExpressionExceptions.UnclosedOpenBracket(token.Symbol, expression, len);
                        goto ReturnInvalid;
                    default:
                        if (AssertAddNode(token, expression, len, out exception))
                        {
                            goto ReturnInvalid;
                        }
                        break;
                }
            }
        ReturnInvalid:
            list.Clear();
            exception ??= ExpressionExceptions.Unhandled(expression, 0);
            return false;
        }
    }
}
