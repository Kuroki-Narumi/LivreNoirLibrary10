using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;
using static LivreNoirLibrary.Numerics.RpnConstants;

namespace LivreNoirLibrary.Numerics
{
    public partial class ReversePolishNotation<T>
    {
        public bool TryParse(string expression)
        {
            var index = -1;
            var length = expression.Length;
            var list = _tokens;
            list.Clear();
            Stack<IRpnToken> stack = [];
            IRpnToken? token;
            var after_value = false;
            var is_negative = false;
            var chars = _char_tokens;
            var regexs = _regex_tokens;

            bool PushToken(IRpnToken token)
            {
                var flag = token.IsValue;
                if (flag)
                {
                    list.Add(token);
                }
                else
                {
                    if (token is IRpnInfixOperatorToken<T> op)
                    {
                        while (stack.TryPeek(out var top) && (op.Priority < top.Priority || (op.LeftAssoc && op.Priority == top.Priority)))
                        {
                            list.Add(stack.Pop());
                        }
                    }
                    else if (token is IRpnFunctionToken<T> func)
                    {
                        while (stack.TryPeek(out var top) && (func.Priority < top.Priority))
                        {
                            list.Add(stack.Pop());
                        }
                    }
                    stack.Push(token);
                }
                if (after_value == flag)
                {
                    return false;
                }
                after_value = flag;
                return true;
            }

            bool IsMatch(Regex regex, [MaybeNullWhen(false)]out Match match)
            {
                match = regex.Match(expression, index);
                if (match.Success && match.Index == index)
                {
                    index += match.Length - 1;
                    return true;
                }
                match = null;
                return false;
            }

            while ((++index) < length)
            {
                var c = expression[index];
                switch (c)
                {
                    case Space:
                        break;
                    case OpenBracket:
                        if (after_value)
                        {
                            goto ReturnInvalid;
                        }
                        stack.Push(RpnOpenBracketToken.Instance);
                        after_value = false;
                        break;
                    case CloseBracket:
                        while (true)
                        {
                            if (stack.Count is 0)
                            {
                                goto ReturnInvalid;
                            }
                            token = stack.Pop();
                            if (token is RpnOpenBracketToken)
                            {
                                break;
                            }
                            else
                            {
                                list.Add(token);
                            }
                        }
                        break;
                    case Plus:
                        if (after_value)
                        {
                            if (!PushToken(AdditionToken))
                            {
                                goto ReturnInvalid;
                            }
                        }
                        else
                        {
                            is_negative = false;
                        }
                        break;
                    case Minus:
                        if (after_value)
                        {
                            if (!PushToken(SubtractionToken))
                            {
                                goto ReturnInvalid;
                            }
                        }
                        else
                        {
                            is_negative = true;
                        }
                        break;
                    default:
                        if (chars.TryGetValue(c, out token))
                        {
                            if (!PushToken(token))
                            {
                                goto ReturnInvalid;
                            }
                        }
                        else if (IsMatch(Regex_Number, out var match) && T.TryParse(match.Value, System.Globalization.NumberStyles.Number, null, out var value))
                        {
                            if (!PushToken(new ValueToken(is_negative ? -value : value)))
                            {
                                goto ReturnInvalid;
                            }
                            is_negative = false;
                        }
                        else
                        {
                            var flag = false;
                            foreach (var (regex, ft) in regexs)
                            {
                                if (IsMatch(regex, out _))
                                {
                                    flag = PushToken(ft);
                                    break;
                                }
                            }
                            if (!flag)
                            {
                                goto ReturnInvalid;
                            }
                        }
                        break;
                }
            }
            while (stack.Count is > 0)
            {
                token = stack.Pop();
                if (token is RpnOpenBracketToken)
                {
                    goto ReturnInvalid;
                }
                list.Add(token);
            }
            return true;
        ReturnInvalid:
            list.Clear();
            return false;
        }
    }
}
