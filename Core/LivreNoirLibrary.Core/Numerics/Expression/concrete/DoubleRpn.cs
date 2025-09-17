using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Numerics
{
    public class DoubleRpn : ReversePolishNotation<double>
    {
        public const double DegreeToRadian = Math.PI / 180.0;
        public const double RadianToDegree = 180.0 / Math.PI;

        public bool AllowsInfinite { get; set; } = false;

        protected override bool CheckResult(double result, [MaybeNullWhen(true)] out Exception exception)
        {
            if (AllowsInfinite || double.IsFinite(result))
            {
                exception = null;
                return true;
            }
            else
            {
                exception = new OverflowException("result must be a finite number.");
                return false;
            }
        }

        private static readonly Dictionary<(string, int), FunctionNode> _node_map = InitializeNodeMap();
        private static Dictionary<(string, int), FunctionNode> InitializeNodeMap()
        {
            Dictionary<(string, int), FunctionNode> map = [];
            void Add(FunctionNode node) => map.Add((node.Symbol, node.OperandCount), node);
            void Add2(OperatorToken token, Func func) => Add(new(token.Symbol, token.OperandCount, func));
            Add2(PowerToken, (s, v) => Math.Pow(s[0], s[1]));
            Add(new(SignSymbol, 1, (s, v) => Math.Sign(s[0])));
            Add(new(FloorSymbol, 1, (s, v) => Math.Floor(s[0])));
            Add(new(CeilingSymbol, 1, (s, v) => Math.Ceiling(s[0])));
            Add(new(TruncateSymbol, 1, (s, v) => Math.Truncate(s[0])));
            Add(new(RoundSymbol, 1, (s, v) => Math.Round(s[0])));
            Add(new(RoundSymbol, 2, (s, v) => Math.Round(s[0], (int)s[1])));

            Add(new(SquareRootSymbol, 1, (s, v) => Math.Sqrt(s[0])));
            Add(new(CubeRootSymbol, 1, (s, v) => Math.Cbrt(s[0])));
            Add(new(HypotSymbol, 2, (s, v) => double.Hypot(s[0], s[1])));
            Add(new(ExponentSymbol, 1, (s, v) => Math.Exp(s[0])));
            Add(new(ScaleBSymbol, 2, (s, v) => Math.ScaleB(s[0], (int)s[1])));
            Add(new(ILogBSymbol, 1, (s, v) => Math.ILogB(s[0])));
            Add(new(LogSymbol, 1, (s, v) => Math.Log(s[0])));
            Add(new(LogSymbol, 2, (s, v) => Math.Log(s[0], s[1])));
            Add(new(Log2Symbol, 1, (s, v) => Math.Log2(s[0])));
            Add(new(Log10Symbol, 1, (s, v) => Math.Log10(s[0])));
            Add(new(SinSymbol, 1, (s, v) => Math.Sin(s[0])));
            Add(new(CosSymbol, 1, (s, v) => Math.Cos(s[0])));
            Add(new(TanSymbol, 1, (s, v) => Math.Tan(s[0])));
            Add(new(AsinSymbol, 1, (s, v) => Math.Asin(s[0])));
            Add(new(AcosSymbol, 1, (s, v) => Math.Acos(s[0])));
            Add(new(AtanSymbol, 1, (s, v) => Math.Atan(s[0])));
            Add(new(AtanSymbol, 2, (s, v) => Math.Atan2(s[0], s[1])));
            Add(new(SinhSymbol, 1, (s, v) => Math.Sinh(s[0])));
            Add(new(CoshSymbol, 1, (s, v) => Math.Cosh(s[0])));
            Add(new(TanhSymbol, 1, (s, v) => Math.Tanh(s[0])));
            Add(new(AsinhSymbol, 1, (s, v) => Math.Asinh(s[0])));
            Add(new(AcoshSymbol, 1, (s, v) => Math.Acosh(s[0])));
            Add(new(AtanhSymbol, 1, (s, v) => Math.Atanh(s[0])));

            Add(new(PiSymbol, 0, (s, v) => Math.PI));
            Add(new(NapierSymbol, 0, (s, v) => Math.E));
            Add(new(RadianSymbol, 1, (s, v) => s[0] * DegreeToRadian));
            Add(new(DegreeSymbol, 1, (s, v) => s[0] * RadianToDegree));
            return map;
        }

        protected override bool TryGetFunctionNode(string symbol, int operandCount, out FunctionNode node, [MaybeNullWhen(true)] out Exception exception)
        {
            if (_node_map.TryGetValue((symbol, operandCount), out node))
            {
                exception = null;
                return true;
            }
            return base.TryGetFunctionNode(symbol, operandCount, out node, out exception);
        }
    }
}
