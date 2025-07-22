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
        public const string Operator_Set = "=";
        public const string Operator_Add = "+";
        public const string Operator_Subtract = "-";
        public const string Operator_Multiply = "*";
        public const string Operator_Divide = "/";
        public const string Operator_Modulo = "%";
        public const string Operator_Samller = "<";
        public const string Operator_Greater = ">";

        public static bool TryGetOperator<T>(ValueOperationMode mode, T value, [MaybeNullWhen(false)]out Func<T, T> func)
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


        private static readonly Dictionary<ValueOperationMode, string> _replacer = new()
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
        private static readonly Dictionary<string, ValueOperationMode> _replacer_i = _replacer.Invert();

        public static string GetText(ValueOperationMode mode) => _replacer.TryGetValue(mode, out var op) ? op : "";
        public static ValueOperationMode GetMode(string op) => _replacer_i.TryGetValue(op, out var m) ? m : 0;

        public static string GetText(ValueOperationMode mode, Rational value) => mode is ValueOperationMode.None ? "" : $"{GetText(mode)}{value}";

        [GeneratedRegex(@"(?<op>[=+\-*/%<>])?(?<val>.+)")]
        private static partial Regex GR_Replace { get; }

        public static bool TryParse(string? text, out ValueOperationMode mode, out Rational value)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var match = GR_Replace.Match(text);
                if (match.Success)
                {
                    var op = match.Groups["op"];
                    mode = op.Success ? GetMode(op.Value) : ValueOperationMode.Set;
                    var val = match.Groups["val"];
                    if (Rational.TryParse(val.Value, out value))
                    {
                        return true;
                    }
                }
            }
            mode = default;
            value = default;
            return false;
        }
    }
}
