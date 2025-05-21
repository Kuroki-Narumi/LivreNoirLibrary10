using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Numerics
{
    public partial class ReversePolishNotation<T>
    {
        private static bool TryEval(Func<T> action, out T result, [NotNullWhen(false)]out Exception? exception)
        {
            try
            {
                result = action();
                exception = null;
                return true;
            }
            catch (Exception e)
            {
                result = default;
                exception = e;
                return false;
            }
        }

        public bool TryEvaluate(Dictionary<string, T> variableList, out T result, [NotNullWhen(false)]out Exception? exception)
        {
            result = default;
            exception = null;
            var tokens = CollectionsMarshal.AsSpan(_tokens);
            if (tokens.Length is 0)
            {
                return true;
            }
            Stack<T> valueStack = [];
            List<T> operands = [];

            static ArgumentException GetArgumnentException(int expected) => new($"Too few arguments (expected:{expected}).");
            //System.Diagnostics.Debug.WriteLine($"variables:{System.Text.Json.JsonSerializer.Serialize(variableList)}");
            //System.Diagnostics.Debug.WriteLine($"start eval:");
            foreach (var token in tokens)
            {
                switch (token)
                {
                    case IRpnValueToken<T> t:
                        valueStack.Push(t.Value);
                        break;
                    case IRpnInfixOperatorToken<T> t:
                        if (valueStack.TryPop(out var operand2) && valueStack.TryPop(out var operand1))
                        {
                            //System.Diagnostics.Debug.Write($"{operand1} {t.Symbol} {operand2} = ");
                            if (TryEval(() => t.Func(operand1, operand2), out var v, out exception))
                            {
                                //System.Diagnostics.Debug.WriteLine($"{v}");
                                valueStack.Push(v);
                            }
                            else
                            {
                                //System.Diagnostics.Debug.WriteLine($"(error)");
                                return false;
                            }
                        }
                        else
                        {
                            exception = GetArgumnentException(2);
                            return false;
                        }
                        break;
                    case IRpnFunctionToken<T> t:
                        operands.Clear();
                        var opc = t.OperandCount;
                        if (opc is < 0)
                        {
                            while (valueStack.TryPop(out var operand))
                            {
                                operands.Add(operand);
                            }
                        }
                        else
                        {
                            for (var i = 0; i < opc; i++)
                            {
                                if (valueStack.TryPop(out var operand))
                                {
                                    operands.Add(operand);
                                }
                                else
                                {
                                    exception = GetArgumnentException(opc);
                                    return false;
                                }
                            }
                        }
                        operands.Reverse();
                        if (TryEval(() => t.Func(operands), out var vv, out exception))
                        {
                            valueStack.Push(vv);
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    default:
                        if (variableList.TryGetValue(token.Symbol, out var value))
                        {
                            valueStack.Push(value);
                        }
                        else
                        {
                            exception = new NotImplementedException($"Not supported token: {token.Symbol}");
                            return false;
                        }
                        break;
                }
            }

            if (valueStack.Count is > 0)
            {
                result = valueStack.Pop();
                return true;
            }
            else
            {
                exception = new InvalidOperationException("No value token exists.");
                return false;
            }
        }
    }
}
