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
        public bool IsEffective => _rpn.IsEffective();

        public Exception? EvalException { get; private set => SetValue(ref field, value); }

        private void OnExpressionChanged(string? oldValue, string? newValue)
        {
            _nodes.Clear();
            IsValid = _rpn.TryParse(newValue, out var ex) &&
                      _rpn.TryGetLazyNode(_nodes, out ex) &&
                      TryEvaluate(_testCard, out _, out ex);
            EvalException = ex;
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
            dic["l"] = dic["level"] = card.Level;
            dic["a"] = dic["atk"] = card.Atk;
            dic["d"] = dic["def"] = card.CardType is CardType.Link_Monster ? double.NaN : card.Def;
            dic["p"] = dic["pscale"] =  card.PendulumScale;

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

        public bool IsMatch(ICard card) => TryEvaluate(card, out var value, out _) && value != 0;

        private static readonly Card _testCard = new()
        {
            Level = 4,
            Atk = 1000,
            Def = 500,
            PendulumScale = 8,
        };
    }
}
