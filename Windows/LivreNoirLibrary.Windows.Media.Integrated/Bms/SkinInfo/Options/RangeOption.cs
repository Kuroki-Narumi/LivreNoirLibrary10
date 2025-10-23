using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public sealed class RangeOption : OptionBase
    {
        public double Minimum { get; set => SetValue(ref field, value); } = double.MinValue;
        public double Maximum { get; set => SetValue(ref field, value); } = double.MaxValue;
        public double DefaultValue { get; set => SetValue(ref field, value); }

        public double Value { get; set => SetValue(ref field, double.IsNaN(value) ? value : Math.Clamp(value, Minimum, Maximum)); } = double.NaN;

        public override string? SelectedValue => $"{(double.IsNaN(Value) ? DefaultValue : Value)}";
    }
}
