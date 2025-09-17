using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Numerics
{
    public class FloatRpn : ReversePolishNotation<float>
    {
        public const float DegreeToRadian = MathF.PI / 180.0f;
        public const float RadianToDegree = 180.0f / MathF.PI;

        public bool AllowsInfinite { get; set; } = false;

        protected override bool CheckResult(float result, [MaybeNullWhen(true)] out Exception exception)
        {
            if (AllowsInfinite || float.IsFinite(result))
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
            Add2(PowerToken, (s, v) => MathF.Pow(s[0], s[1]));
            Add(new(SignSymbol, 1, (s, v) => MathF.Sign(s[0])));
            Add(new(FloorSymbol, 1, (s, v) => MathF.Floor(s[0])));
            Add(new(CeilingSymbol, 1, (s, v) => MathF.Ceiling(s[0])));
            Add(new(TruncateSymbol, 1, (s, v) => MathF.Truncate(s[0])));
            Add(new(RoundSymbol, 1, (s, v) => MathF.Round(s[0])));
            Add(new(RoundSymbol, 2, (s, v) => MathF.Round(s[0], (int)s[1])));

            Add(new(SquareRootSymbol, 1, (s, v) => MathF.Sqrt(s[0])));
            Add(new(CubeRootSymbol, 1, (s, v) => MathF.Cbrt(s[0])));
            Add(new(HypotSymbol, 2, (s, v) => float.Hypot(s[0], s[1])));
            Add(new(ExponentSymbol, 1, (s, v) => MathF.Exp(s[0])));
            Add(new(ScaleBSymbol, 2, (s, v) => MathF.ScaleB(s[0], (int)s[1])));
            Add(new(ILogBSymbol, 1, (s, v) => MathF.ILogB(s[0])));
            Add(new(LogSymbol, 1, (s, v) => MathF.Log(s[0])));
            Add(new(LogSymbol, 2, (s, v) => MathF.Log(s[0], s[1])));
            Add(new(Log2Symbol, 1, (s, v) => MathF.Log2(s[0])));
            Add(new(Log10Symbol, 1, (s, v) => MathF.Log10(s[0])));
            Add(new(SinSymbol, 1, (s, v) => MathF.Sin(s[0])));
            Add(new(CosSymbol, 1, (s, v) => MathF.Cos(s[0])));
            Add(new(TanSymbol, 1, (s, v) => MathF.Tan(s[0])));
            Add(new(AsinSymbol, 1, (s, v) => MathF.Asin(s[0])));
            Add(new(AcosSymbol, 1, (s, v) => MathF.Acos(s[0])));
            Add(new(AtanSymbol, 1, (s, v) => MathF.Atan(s[0])));
            Add(new(AtanSymbol, 2, (s, v) => MathF.Atan2(s[0], s[1])));
            Add(new(SinhSymbol, 1, (s, v) => MathF.Sinh(s[0])));
            Add(new(CoshSymbol, 1, (s, v) => MathF.Cosh(s[0])));
            Add(new(TanhSymbol, 1, (s, v) => MathF.Tanh(s[0])));
            Add(new(AsinhSymbol, 1, (s, v) => MathF.Asinh(s[0])));
            Add(new(AcoshSymbol, 1, (s, v) => MathF.Acosh(s[0])));
            Add(new(AtanhSymbol, 1, (s, v) => MathF.Atanh(s[0])));

            Add(new(PiSymbol, 0, (s, v) => MathF.PI));
            Add(new(NapierSymbol, 0, (s, v) => MathF.E));
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
