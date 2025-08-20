using LivreNoirLibrary.Debug;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Numerics
{
    public partial class ReversePolishNotation<T> : ReversePolishNotation, IExpressionEvaluator<T>
        where T : unmanaged, INumber<T>
    {
        private readonly List<FunctionNode> _nodes = [];

        public override string ToString() => $"{GetType()}{{{string.Join(" ", _nodes.Select(s => s.Symbol))}}}";

        public bool TryParse(string expression, [MaybeNullWhen(true)] out Exception exception)
        {
            //ExConsole.Write($"expression=\"{expression}\"");
            var span = expression.AsSpan();
            var length = span.Length;
            var list = _nodes;
            list.Clear();
            Stack<Token> stack = [];
            Stack<int> functionOperandCounts = [];
            var expectValueToken = true;
            var index = 0;
            string? exceptionMessage;

            string GetName(ReadOnlySpan<char> span, Predicate<char> selector)
            {
                var start = index;
                for (; index < span.Length && selector(span[index]); index++) ;
                var end = index;
                for (; index < span.Length && IsWhiteSpace(span[index]); index++) ;
                return new(span[start..end]);
            }
            void AddNode(Token token)
            {
                int opCount;
                switch (token)
                {
                    case OperatorToken op:
                        opCount = op.OperandCount;
                        break;
                    case FunctionToken func:
                        opCount = functionOperandCounts.Pop() + (expectValueToken ? 0 : 1);
                        break;
                    default:
                        exceptionMessage = $"unexpected token detected {token.Symbol}";
                        return;
                }
                list.Add(GetFunctionNode(token.Symbol, opCount));
            }
            while (index < length)
            {
                var c = span[index];
                if (IsOpenBracket(c)) // 開き括弧
                {
                    stack.Push(GetBracketToken(c.ToString()));
                    expectValueToken = true;
                }
                else if (IsCloseBracket(c)) // 閉じ括弧
                {
                    // 開き括弧が見つかるまでスタックを掘る
                    while (stack.TryPop(out var top))
                    {
                        if (top is OpenBracketToken open)
                        {
                            // 対応する開き括弧でない場合は不正
                            if (open.CloseSymbol != c.ToString())
                            {
                                exceptionMessage = $"unexpected close bracket (\"{c}\", expected=\"{open.CloseSymbol}\")";
                                goto ReturnInvalid;
                            }
                            // スタックのトップが関数トークンの場合は、それも取り出す
                            if (stack.TryPeek(out top) && top is FunctionToken)
                            {
                                stack.Pop();
                                AddNode(top);
                            }
                            // メインループ終了
                            expectValueToken = false;
                            goto IncrementIndex;
                        }
                        else
                        {
                            AddNode(top);
                        }
                    }
                    // 開き括弧が見つからなかった場合は不正
                    exceptionMessage = $"open bracket missing";
                    goto ReturnInvalid;
                }
                else if (IsArgumentDelimiter(c)) // 引数区切り
                {
                    // 関数の内部でない場合は不正
                    if (functionOperandCounts.Count is 0)
                    {
                        exceptionMessage = $"argument delimiter must be written in a function";
                        goto ReturnInvalid;
                    }
                    // 開き括弧が見つかるまでスタックを掘る
                    while (stack.TryPeek(out var top))
                    {
                        if (top is OpenBracketToken)
                        {
                            var count = functionOperandCounts.Pop();
                            functionOperandCounts.Push(count + 1);
                            // メインループ終了
                            expectValueToken = true;
                            goto IncrementIndex;
                        }
                        else
                        {
                            stack.Pop();
                            AddNode(top);
                        }
                    }
                    // 開き括弧が見つからなかった場合は不正
                    exceptionMessage = $"open bracket missing";
                    goto ReturnInvalid;
                }
                else if (IsNumberCharacter(c)) // 数値
                {
                    var name = GetName(span, IsIdentifierCharacter);
                    if (T.TryParse(name, null, out var value))
                    {
                        // 数値として解釈可能な場合はリストにそのまま追加
                        list.Add(CreateValueNode(value));
                        expectValueToken = false;
                        goto DontUpdateIndex;
                    }
                    else
                    {
                        // 数値に変換できない場合は不正
                        exceptionMessage = $"unparsable number symbol ({name})";
                        goto ReturnInvalid;
                    }
                }
                else if (IsIdentifierCharacter(c)) // 変数か関数
                {
                    var name = GetName(span, IsIdentifierCharacter);
                    // 次のトークンが開き括弧なら関数
                    if (index < span.Length && IsOpenBracket(span[index]))
                    {
                        stack.Push(new FunctionToken(name));
                        functionOperandCounts.Push(0);
                        expectValueToken = true;
                    }
                    else // そうでなければ変数
                    {
                        list.Add(CreateVariableNode(name));
                        expectValueToken = false;
                    }
                    goto DontUpdateIndex;
                }
                else if (IsOperatorCharacter(c)) // 演算子
                {
                    var name = GetName(span, IsOperatorCharacter);
                    // 三項演算子の第二記号
                    if (GetOperatorToken(name, expectValueToken, out var token))
                    {
                        // 第一記号か開き括弧が見つかるまでスタックを掘る
                        while (stack.TryPop(out var top) && top is not OpenBracketToken)
                        {
                            if (top is TernaryOperator1Token op)
                            {
                                // 期待と異なる第一記号が見つかった場合
                                if (op.SecondSymbol != name)
                                {
                                    exceptionMessage = $"unexpected ternary symbol (\"{name}\", expected=\"{op.SecondSymbol}\")";
                                    goto ReturnInvalid;
                                }
                                // 完成した演算子としてスタックに再プッシュ
                                stack.Push(op.ToResolved());
                                expectValueToken = true;
                                goto DontUpdateIndex;
                            }
                            else
                            {
                                AddNode(top);
                            }
                        }
                        // 第一記号が見つからなかった場合は不正
                        exceptionMessage = $"ternary symbol missing";
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
                            AddNode(top);
                        }
                        stack.Push(token);
                        expectValueToken = true;
                        goto DontUpdateIndex;
                    }
                }
                // その他の文字は全て無視する
            IncrementIndex:
                index++;
            DontUpdateIndex:;
                /*
                if (c is not ' ')
                {
                    ExConsole.Write($"index={index}\tlist=[{string.Join(' ', list)}]\tstack=[{string.Join(' ', stack)}]\trest=\"{new string(span[index..])}\"");
                }
                */
            }
            while (stack.TryPop(out var token))
            {
                switch (token)
                {
                    case TernaryOperator1Token: // 未処理の三項演算子
                        exceptionMessage = $"unresolved ternary operator({token.Symbol})";
                        goto ReturnInvalid;
                    case OpenBracketToken: // 閉じられていない括弧
                        exceptionMessage = $"unclosed open bracket";
                        goto ReturnInvalid;
                    default:
                        AddNode(token);
                        break;
                }
            }
            exception = null;
            return true;
        ReturnInvalid:
            list.Clear();
            exception = new ExpressionParseException(exceptionMessage, expression, index);
            return false;
        }
    }
}
