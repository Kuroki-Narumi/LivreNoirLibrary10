using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public static partial class ValueOperation
    {
        public const char Operator_Set = '=';
        public const char Operator_Add = '+';
        public const char Operator_Subtract = '-';
        public const char Operator_Multiply = '*';
        public const char Operator_Divide = '/';
        public const char Operator_Modulo = '%';
        public const char Operator_Samller = '<';
        public const char Operator_Greater = '>';

        public static bool TryGetOperator<T>(ValueOperationMode mode, T value, [MaybeNullWhen(false)] out Func<T, T> func)
            where T : INumber<T>
        {
            func = null;
            switch (mode)
            {
                case ValueOperationMode.Set:
                    func = Set(value);
                    break;
                case ValueOperationMode.Add:
                    if (!T.IsZero(value))
                    {
                        func = Add(value);
                    }
                    break;
                case ValueOperationMode.Subtract:
                    if (!T.IsZero(value))
                    {
                        func = Subtract(value);
                    }
                    break;
                case ValueOperationMode.Multiply:
                    if (value != T.MultiplicativeIdentity)
                    {
                        func = Multiply(value);
                    }
                    break;
                case ValueOperationMode.Divide:
                    if (!T.IsZero(value) && value != T.MultiplicativeIdentity)
                    {
                        func = Divide(value);
                    }
                    break;
                case ValueOperationMode.Modulo:
                    if (!T.IsZero(value) && value != T.MultiplicativeIdentity)
                    {
                        func = Modulo(value);
                    }
                    break;
                case ValueOperationMode.Smaller:
                    func = Smaller(value);
                    break;
                case ValueOperationMode.Greater:
                    func = Greater(value);
                    break;
            }
            return func is not null;
        }

        public static Func<T, T> Set<T>(T o) => v => o;
        public static Func<T, T> Add<T>(T o) where T : IAdditionOperators<T, T, T> => v => v + o;
        public static Func<T, T> Subtract<T>(T o) where T : ISubtractionOperators<T, T, T> => v => v - o;
        public static Func<T, T> Multiply<T>(T o) where T : IMultiplyOperators<T, T, T> => v => v * o;
        public static Func<T, T> Divide<T>(T o) where T : IDivisionOperators<T, T, T> => v => v / o;
        public static Func<T, T> Modulo<T>(T o) where T : IModulusOperators<T, T, T> => v => v % o;
        public static Func<T, T> Smaller<T>(T o) where T : IComparisonOperators<T, T, bool> => v => v < o ? v : o;
        public static Func<T, T> Greater<T>(T o) where T : IComparisonOperators<T, T, bool> => v => v > o ? v : o;


        private static readonly Dictionary<ValueOperationMode, char> _replacer = new()
        {
            { ValueOperationMode.Set, Operator_Set },
            { ValueOperationMode.Add, Operator_Add },
            { ValueOperationMode.Subtract, Operator_Subtract },
            { ValueOperationMode.Multiply, Operator_Multiply },
            { ValueOperationMode.Divide, Operator_Divide },
            { ValueOperationMode.Modulo, Operator_Modulo },
            { ValueOperationMode.Smaller, Operator_Samller },
            { ValueOperationMode.Greater, Operator_Greater },
        };
        private static readonly Dictionary<char, ValueOperationMode> _replacer_i = _replacer.Invert();

        public static string GetText(ValueOperationMode mode) => _replacer.TryGetValue(mode, out var op) ? op.ToString() : "";
        public static bool TryGetMode(char op, out ValueOperationMode value) => _replacer_i.TryGetValue(op, out value);

        public static string GetText<T>(ValueOperationMode mode, T value) => mode is ValueOperationMode.None ? "" : $"{GetText(mode)}{value}";

        public static bool TryParse(ReadOnlySpan<char> text, out ValueOperationMode mode, out Rational value)
        {
            text = text.Trim();
            if (text.Length is > 0)
            {
                if (TryGetMode(text[0], out mode))
                {
                    text = text[1..].Trim();
                }
                return Rational.TryParse(text, out value);
            }
            mode = default;
            value = default;
            return false;
        }

        public static bool TryParseToDouble(string? text, out ValueOperationMode mode, out double value)
        {
            var ret = TryParse(text, out mode, out var v);
            value = (double)v;
            return ret;
        }
    }
}
