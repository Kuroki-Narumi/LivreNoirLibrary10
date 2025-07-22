using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BarSplitOptions : ObservableObjectBase
    {
        private static readonly BarRpnVariables _dummy_vals = [];

        internal readonly SortedSet<int> _numbers = [];
        internal readonly BarRpn _rpn = new();

        [ObservableProperty(Related = [nameof(Mode_Once), nameof(Mode_Division), nameof(Mode_Interval), nameof(Mode_Expression)])]
        private BarSplitMode _mode;
        [ObservableProperty]
        private Rational _firstLength;
        [ObservableProperty]
        private int _maxCount = 4;
        [ObservableProperty]
        private string? _expression;

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
        public bool Mode_Once { get => _mode is BarSplitMode.Once; set => SetMode(BarSplitMode.Once, value); }
        [JsonIgnore]
        public bool Mode_Division { get => _mode is BarSplitMode.Division; set => SetMode(BarSplitMode.Division, value); }
        [JsonIgnore]
        public bool Mode_Interval { get => _mode is BarSplitMode.Interval; set => SetMode(BarSplitMode.Interval, value); }
        [JsonIgnore]
        public bool Mode_Expression { get => _mode is BarSplitMode.Expression; set => SetMode(BarSplitMode.Expression, value); }

        private void SetMode(BarSplitMode mode, bool value)
        {
            if (value)
            {
                Mode = mode;
            }
        }

        public bool VerifyExpression(string expr)
        {
            _expression = expr;
            var rpn = _rpn;
            if (string.IsNullOrEmpty(expr))
            {
                rpn.Clear();
                return true;
            }
            return rpn.TryParse(expr) && rpn.TryEvaluate(_dummy_vals, out _, out _);
        }

        public bool IsEffective()
        {
            return
                _numbers.Count is > 0 &&
                (_firstLength.IsPositiveThanZero() || _rpn.IsEffective()) &&
                _maxCount is > 1;
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
}
