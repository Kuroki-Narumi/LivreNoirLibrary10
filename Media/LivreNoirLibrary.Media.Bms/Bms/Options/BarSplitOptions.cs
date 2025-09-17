using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BarSplitOptions : ObservableObjectBase
    {
        private static readonly BarLengthVariable _dummy_vals = new();

        internal readonly SortedSet<int> _numbers = [];
        internal readonly ReversePolishNotation<Rational> _rpn = new();

        public BarSplitMode Mode { get; set => SetValue(ref field, value, [nameof(Mode_Once), nameof(Mode_Division), nameof(Mode_Interval), nameof(Mode_Expression)]); }
        public Rational FirstLength { get; set => SetValue(ref field, value); }
        public int MaxCount { get; set => SetValue(ref field, value); } = 4;
        public string? Expression { get; set => SetValue(ref field, value); }

        [JsonIgnore]
        public IEnumerable<int> Numbers
        {
            get => _numbers;
            set
            {
                _numbers.Clear();
                _numbers.UnionWith(value);
                SendPropertyChanged();
            }
        }

        [JsonIgnore]
        public bool Mode_Once { get => Mode is BarSplitMode.Once; set => SetMode(BarSplitMode.Once, value); }
        [JsonIgnore]
        public bool Mode_Division { get => Mode is BarSplitMode.Division; set => SetMode(BarSplitMode.Division, value); }
        [JsonIgnore]
        public bool Mode_Interval { get => Mode is BarSplitMode.Interval; set => SetMode(BarSplitMode.Interval, value); }
        [JsonIgnore]
        public bool Mode_Expression { get => Mode is BarSplitMode.Expression; set => SetMode(BarSplitMode.Expression, value); }

        private void SetMode(BarSplitMode mode, bool value)
        {
            if (value)
            {
                Mode = mode;
            }
        }

        public bool VerifyExpression(string expr)
        {
            Expression = expr;
            var rpn = _rpn;
            if (string.IsNullOrEmpty(expr))
            {
                rpn.Clear();
                return true;
            }
            return rpn.TryParse(expr) && rpn.TryEvaluate(_dummy_vals.TryGetValue, out _, out _);
        }

        public bool IsEffective()
        {
            return
                _numbers.Count is > 0 &&
                (FirstLength.IsPositiveThanZero() || _rpn.IsEffective()) &&
                MaxCount is > 1;
        }

        public void SetExpression_DivEqual()
        {
            Expression = "l * i / m";
        }

        public void SetExpression_RegInterval()
        {
            Expression = "f * i";
        }
    }

    public enum BarSplitMode
    {
        Once,
        Division,
        Interval,
        Expression,
    }
}
