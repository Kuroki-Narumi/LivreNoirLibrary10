using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public class StatusExpression : ObservableObjectBase
    {
        private readonly DoubleRpn _rpn = new();
        private readonly List<LazyNode<double>> _nodes = [];
        private readonly Dictionary<string, double> _variables = new(StringComparer.OrdinalIgnoreCase);

        public string? Expression { get; set => SetValue(ref field, value, OnExpressionChanged); }
        public bool IsValid { get; private set => SetValue(ref field, value); }
        public bool IsEffective => IsValid && _rpn.IsEffective();

        public Exception? InnerException { get; private set => SetValue(ref field, value); }

        private void OnExpressionChanged(string? oldValue, string? newValue)
        {
            _nodes.Clear();
            IsValid = _rpn.TryParse(newValue, out var ex) &&
                      _rpn.TryGetLazyNode(_nodes, out ex) &&
                      TryEvaluate(_testCard, out _, out ex);
            InnerException = ex;
        }

        public bool TryEvaluate(ICard card, out double result, out Exception? ex)
        {
            result = default;
            ex = null;
            if (_nodes.Count is <= 0)
            {
                ex = ExpressionExceptions.ExpressionEmpty;
                return false;
            }

            var dic = _variables;
            dic["l"] = dic["lv"] = dic["level"] = card.Level;
            dic["a"] = dic["atk"] = card.Atk;
            dic["d"] = dic["def"] = card.Def;
            dic["p"] = dic["scale"] = card.PendulumScale;

            var e = _nodes[^1].Execute(dic.TryGetValue);
            if (e.HasException)
            {
                ex = e.Exception;
                return false;
            }
            result = e.Value;
            if (!double.IsFinite(result))
            {
                ex = ExpressionExceptions.ResultInfinite;
                return false;
            }
            return true;
        }

        public (bool Monster, bool Def, bool Pendulum) CheckRequirements()
        {
            if (!IsEffective)
            {
                return (false, false, false);
            }
            var checker = RequirementChecker.Instance;
            var e = _nodes[^1].Execute(checker.TryGetValue);
            return (checker.Monster, checker.Def, checker.Pendulum);
        }

        public bool IsMatch(ICard card) => TryEvaluate(card, out var value, out _) && value != 0;

        private static readonly Card _testCard = new()
        {
            Level = 4,
            Atk = 1000,
            Def = 500,
            PendulumScale = 8,
        };

        private class RequirementChecker
        {
            public static RequirementChecker Instance { get; } = new();

            public bool Monster { get; private set; }
            public bool Def { get; private set; }
            public bool Pendulum { get; private set; }

            public void Clear()
            {
                Monster = false;
                Def = false;
                Pendulum = false;
            }

            public bool TryGetValue(string symbol, out double value)
            {
                switch (symbol)
                {
                    case "l" or "lv" or "level":
                        Monster = true;
                        value = 4;
                        return true;
                    case "a" or "atk":
                        Monster = true;
                        value = 1000;
                        return true;
                    case "d" or "def":
                        Monster = true;
                        Def = true;
                        value = 500;
                        return true;
                    case "p" or "scale":
                        Monster = true;
                        Pendulum = true;
                        value = 8;
                        return true;
                }
                value = default;
                return false;
            }
        }
    }
}
