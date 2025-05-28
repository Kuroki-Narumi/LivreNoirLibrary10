using System;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public class DoubleTextBlock : NumberTextBlockBase<double>, INumberText<double>
    {
        public static readonly DependencyProperty DefaultValueProperty = DoublePropertyHolder.DefaultValueProperty.AddOwner(typeof(DoubleTextBlock));
        public static readonly DependencyProperty ValueProperty = DoublePropertyHolder.ValueProperty.AddOwner(typeof(DoubleTextBlock));
        public static readonly DependencyProperty MinimumProperty = DoublePropertyHolder.MinimumProperty.AddOwner(typeof(DoubleTextBlock));
        public static readonly DependencyProperty MaximumProperty = DoublePropertyHolder.MaximumProperty.AddOwner(typeof(DoubleTextBlock));
        public static readonly DependencyProperty WheelStepProperty = DoublePropertyHolder.WheelStepProperty.AddOwner(typeof(DoubleTextBlock));
        public static readonly DependencyProperty StringFormatProperty = DoublePropertyHolder.StringFormatProperty.AddOwner(typeof(DoubleTextBlock));

        protected string? _stringFormat;

        protected override void InitializeFields()
        {
            _default = (double)DefaultValueProperty.GetMetadata(this).DefaultValue;
            _minimum = (double)MinimumProperty.GetMetadata(this).DefaultValue;
            _maximum = (double)MaximumProperty.GetMetadata(this).DefaultValue;
            _wheel_step = (double)WheelStepProperty.GetMetadata(this).DefaultValue;
            _stringFormat = StringFormatProperty.GetMetadata(this).DefaultValue as string;
        }

        public sealed override double DefaultValue { get => _default; set => SetValue(DefaultValueProperty, value); }
        public sealed override double Value { get => _value; set => SetValue(ValueProperty, value); }
        public sealed override double Minimum { get => _minimum; set => SetValue(MinimumProperty, value); }
        public sealed override double Maximum { get => _maximum; set => SetValue(MaximumProperty, value); }
        public sealed override double WheelStep { get => _wheel_step; set => SetValue(WheelStepProperty, value); }
        public string? StringFormat { get => _stringFormat; set => SetValue(StringFormatProperty, value); }

        void INumberText<double>.OnStringFormatChanged(string? value)
        {
            _stringFormat = value;
        }

        protected override string ToString(double value) => value.ToString(_stringFormat);
    }

    public class DoubleTextBox : NumberTextBoxBase<double>, INumberText<double>
    {
        public static readonly DependencyProperty DefaultValueProperty = DoublePropertyHolder.DefaultValueProperty.AddOwner(typeof(DoubleTextBox));
        public static readonly DependencyProperty ValueProperty = DoublePropertyHolder.ValueProperty.AddOwner(typeof(DoubleTextBox));
        public static readonly DependencyProperty MinimumProperty = DoublePropertyHolder.MinimumProperty.AddOwner(typeof(DoubleTextBox));
        public static readonly DependencyProperty MaximumProperty = DoublePropertyHolder.MaximumProperty.AddOwner(typeof(DoubleTextBox));
        public static readonly DependencyProperty WheelStepProperty = DoublePropertyHolder.WheelStepProperty.AddOwner(typeof(DoubleTextBox));
        public static readonly DependencyProperty StringFormatProperty = DoublePropertyHolder.StringFormatProperty.AddOwner(typeof(DoubleTextBox));

        protected string? _stringFormat;

        protected override void InitializeFields()
        {
            _default = (double)DefaultValueProperty.GetMetadata(this).DefaultValue;
            _minimum = (double)MinimumProperty.GetMetadata(this).DefaultValue;
            _maximum = (double)MaximumProperty.GetMetadata(this).DefaultValue;
            _wheel_step = (double)WheelStepProperty.GetMetadata(this).DefaultValue;
            _stringFormat = StringFormatProperty.GetMetadata(this).DefaultValue as string;
        }

        public sealed override double DefaultValue { get => _default; set => SetValue(DefaultValueProperty, value); }
        public sealed override double Value { get => _value; set => SetValue(ValueProperty, value); }
        public sealed override double Minimum { get => _minimum; set => SetValue(MinimumProperty, value); }
        public sealed override double Maximum { get => _maximum; set => SetValue(MaximumProperty, value); }
        public sealed override double WheelStep { get => _wheel_step; set => SetValue(WheelStepProperty, value); }
        public string? StringFormat { get => _stringFormat; set => SetValue(StringFormatProperty, value); }

        void INumberText<double>.OnStringFormatChanged(string? value)
        {
            _stringFormat = value;
        }

        protected override string ToString(double value) => value.ToString(_stringFormat);
    }
}
