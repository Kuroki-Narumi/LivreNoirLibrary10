using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public class LongTextBlock : NumberTextBlockBase<long>
    {
        public static readonly DependencyProperty DefaultValueProperty = LongPropertyHolder.DefaultValueProperty.AddOwner(typeof(LongTextBlock));
        public static readonly DependencyProperty ValueProperty = LongPropertyHolder.ValueProperty.AddOwner(typeof(LongTextBlock));
        public static readonly DependencyProperty MinimumProperty = LongPropertyHolder.MinimumProperty.AddOwner(typeof(LongTextBlock));
        public static readonly DependencyProperty MaximumProperty = LongPropertyHolder.MaximumProperty.AddOwner(typeof(LongTextBlock));
        public static readonly DependencyProperty WheelStepProperty = LongPropertyHolder.WheelStepProperty.AddOwner(typeof(LongTextBlock));

        protected override void InitializeFields()
        {
            this.SetDefaultValue(DefaultValueProperty, ref _default);
            this.SetDefaultValue(MinimumProperty, ref _minimum);
            this.SetDefaultValue(MaximumProperty, ref _maximum);
            this.SetDefaultValue(WheelStepProperty, ref _wheel_step);
        }

        public sealed override long DefaultValue { get => _default; set => SetValue(DefaultValueProperty, value); }
        public sealed override long Value { get => _value; set => SetValue(ValueProperty, value); }
        public sealed override long Minimum { get => _minimum; set => SetValue(MinimumProperty, value); }
        public sealed override long Maximum { get => _maximum; set => SetValue(MaximumProperty, value); }
        public sealed override long WheelStep { get => _wheel_step; set => SetValue(WheelStepProperty, value); }
    }

    public class LongTextBox : NumberTextBoxBase<long>
    {
        public static readonly DependencyProperty DefaultValueProperty = LongPropertyHolder.DefaultValueProperty.AddOwner(typeof(LongTextBox));
        public static readonly DependencyProperty ValueProperty = LongPropertyHolder.ValueProperty.AddOwner(typeof(LongTextBox));
        public static readonly DependencyProperty MinimumProperty = LongPropertyHolder.MinimumProperty.AddOwner(typeof(LongTextBox));
        public static readonly DependencyProperty MaximumProperty = LongPropertyHolder.MaximumProperty.AddOwner(typeof(LongTextBox));
        public static readonly DependencyProperty WheelStepProperty = LongPropertyHolder.WheelStepProperty.AddOwner(typeof(LongTextBox));

        protected override void InitializeFields()
        {
            this.SetDefaultValue(DefaultValueProperty, ref _default);
            this.SetDefaultValue(MinimumProperty, ref _minimum);
            this.SetDefaultValue(MaximumProperty, ref _maximum);
            this.SetDefaultValue(WheelStepProperty, ref _wheel_step);
        }

        public sealed override long DefaultValue { get => _default; set => SetValue(DefaultValueProperty, value); }
        public sealed override long Value { get => _value; set => SetValue(ValueProperty, value); }
        public sealed override long Minimum { get => _minimum; set => SetValue(MinimumProperty, value); }
        public sealed override long Maximum { get => _maximum; set => SetValue(MaximumProperty, value); }
        public sealed override long WheelStep { get => _wheel_step; set => SetValue(WheelStepProperty, value); }
    }
}
