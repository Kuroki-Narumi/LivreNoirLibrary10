using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public class IntTextBlock : NumberTextBlockBase<int>
    {
        public static readonly DependencyProperty DefaultValueProperty = IntPropertyHolder.DefaultValueProperty.AddOwner(typeof(IntTextBlock));
        public static readonly DependencyProperty ValueProperty = IntPropertyHolder.ValueProperty.AddOwner(typeof(IntTextBlock));
        public static readonly DependencyProperty MinimumProperty = IntPropertyHolder.MinimumProperty.AddOwner(typeof(IntTextBlock));
        public static readonly DependencyProperty MaximumProperty = IntPropertyHolder.MaximumProperty.AddOwner(typeof(IntTextBlock));
        public static readonly DependencyProperty WheelStepProperty = IntPropertyHolder.WheelStepProperty.AddOwner(typeof(IntTextBlock));

        protected override void InitializeFields()
        {
            _default = (int)DefaultValueProperty.GetMetadata(this).DefaultValue;
            _minimum = (int)MinimumProperty.GetMetadata(this).DefaultValue;
            _maximum = (int)MaximumProperty.GetMetadata(this).DefaultValue;
            _wheel_step = (int)WheelStepProperty.GetMetadata(this).DefaultValue;
        }

        public sealed override int DefaultValue { get => _default; set => SetValue(DefaultValueProperty, value); }
        public sealed override int Value { get => _value; set => SetValue(ValueProperty, value); }
        public sealed override int Minimum { get => _minimum; set => SetValue(MinimumProperty, value); }
        public sealed override int Maximum { get => _maximum; set => SetValue(MaximumProperty, value); }
        public sealed override int WheelStep { get => _wheel_step; set => SetValue(WheelStepProperty, value); }
    }

    public class IntTextBox : NumberTextBoxBase<int>
    {
        public static readonly DependencyProperty DefaultValueProperty = IntPropertyHolder.DefaultValueProperty.AddOwner(typeof(IntTextBox));
        public static readonly DependencyProperty ValueProperty = IntPropertyHolder.ValueProperty.AddOwner(typeof(IntTextBox));
        public static readonly DependencyProperty MinimumProperty = IntPropertyHolder.MinimumProperty.AddOwner(typeof(IntTextBox));
        public static readonly DependencyProperty MaximumProperty = IntPropertyHolder.MaximumProperty.AddOwner(typeof(IntTextBox));
        public static readonly DependencyProperty WheelStepProperty = IntPropertyHolder.WheelStepProperty.AddOwner(typeof(IntTextBox));

        protected override void InitializeFields()
        {
            _default = (int)DefaultValueProperty.GetMetadata(this).DefaultValue;
            _minimum = (int)MinimumProperty.GetMetadata(this).DefaultValue;
            _maximum = (int)MaximumProperty.GetMetadata(this).DefaultValue;
            _wheel_step = (int)WheelStepProperty.GetMetadata(this).DefaultValue;
        }

        public sealed override int DefaultValue { get => _default; set => SetValue(DefaultValueProperty, value); }
        public sealed override int Value { get => _value; set => SetValue(ValueProperty, value); }
        public sealed override int Minimum { get => _minimum; set => SetValue(MinimumProperty, value); }
        public sealed override int Maximum { get => _maximum; set => SetValue(MaximumProperty, value); }
        public sealed override int WheelStep { get => _wheel_step; set => SetValue(WheelStepProperty, value); }
    }
}
