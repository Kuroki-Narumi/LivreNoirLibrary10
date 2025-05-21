using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public class DecimalTextBlock : NumberTextBlockBase<decimal>
    {
        public static readonly DependencyProperty DefaultValueProperty = DecimalPropertyHolder.DefaultValueProperty.AddOwner(typeof(DecimalTextBlock));
        public static readonly DependencyProperty ValueProperty = DecimalPropertyHolder.ValueProperty.AddOwner(typeof(DecimalTextBlock));
        public static readonly DependencyProperty MinimumProperty = DecimalPropertyHolder.MinimumProperty.AddOwner(typeof(DecimalTextBlock));
        public static readonly DependencyProperty MaximumProperty = DecimalPropertyHolder.MaximumProperty.AddOwner(typeof(DecimalTextBlock));
        public static readonly DependencyProperty WheelStepProperty = DecimalPropertyHolder.WheelStepProperty.AddOwner(typeof(DecimalTextBlock));

        protected override void InitializeFields()
        {
            _default = (decimal)DefaultValueProperty.GetMetadata(this).DefaultValue;
            _minimum = (decimal)MinimumProperty.GetMetadata(this).DefaultValue;
            _maximum = (decimal)MaximumProperty.GetMetadata(this).DefaultValue;
            _wheel_step = (decimal)WheelStepProperty.GetMetadata(this).DefaultValue;
        }

        public sealed override decimal DefaultValue { get => _default; set => SetValue(DefaultValueProperty, value); }
        public sealed override decimal Value { get => _value; set => SetValue(ValueProperty, value); }
        public sealed override decimal Minimum { get => _minimum; set => SetValue(MinimumProperty, value); }
        public sealed override decimal Maximum { get => _maximum; set => SetValue(MaximumProperty, value); }
        public sealed override decimal WheelStep { get => _wheel_step; set => SetValue(WheelStepProperty, value); }
    }

    public class DecimalTextBox : NumberTextBoxBase<decimal>
    {
        public static readonly DependencyProperty DefaultValueProperty = DecimalPropertyHolder.DefaultValueProperty.AddOwner(typeof(DecimalTextBox));
        public static readonly DependencyProperty ValueProperty = DecimalPropertyHolder.ValueProperty.AddOwner(typeof(DecimalTextBox));
        public static readonly DependencyProperty MinimumProperty = DecimalPropertyHolder.MinimumProperty.AddOwner(typeof(DecimalTextBox));
        public static readonly DependencyProperty MaximumProperty = DecimalPropertyHolder.MaximumProperty.AddOwner(typeof(DecimalTextBox));
        public static readonly DependencyProperty WheelStepProperty = DecimalPropertyHolder.WheelStepProperty.AddOwner(typeof(DecimalTextBox));

        protected override void InitializeFields()
        {
            _default = (decimal)DefaultValueProperty.GetMetadata(this).DefaultValue;
            _minimum = (decimal)MinimumProperty.GetMetadata(this).DefaultValue;
            _maximum = (decimal)MaximumProperty.GetMetadata(this).DefaultValue;
            _wheel_step = (decimal)WheelStepProperty.GetMetadata(this).DefaultValue;
        }

        public sealed override decimal DefaultValue { get => _default; set => SetValue(DefaultValueProperty, value); }
        public sealed override decimal Value { get => _value; set => SetValue(ValueProperty, value); }
        public sealed override decimal Minimum { get => _minimum; set => SetValue(MinimumProperty, value); }
        public sealed override decimal Maximum { get => _maximum; set => SetValue(MaximumProperty, value); }
        public sealed override decimal WheelStep { get => _wheel_step; set => SetValue(WheelStepProperty, value); }
    }
}
