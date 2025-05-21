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
            _default = (long)DefaultValueProperty.GetMetadata(this).DefaultValue;
            _minimum = (long)MinimumProperty.GetMetadata(this).DefaultValue;
            _maximum = (long)MaximumProperty.GetMetadata(this).DefaultValue;
            _wheel_step = (long)WheelStepProperty.GetMetadata(this).DefaultValue;
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
            _default = (long)DefaultValueProperty.GetMetadata(this).DefaultValue;
            _minimum = (long)MinimumProperty.GetMetadata(this).DefaultValue;
            _maximum = (long)MaximumProperty.GetMetadata(this).DefaultValue;
            _wheel_step = (long)WheelStepProperty.GetMetadata(this).DefaultValue;
        }

        public sealed override long DefaultValue { get => _default; set => SetValue(DefaultValueProperty, value); }
        public sealed override long Value { get => _value; set => SetValue(ValueProperty, value); }
        public sealed override long Minimum { get => _minimum; set => SetValue(MinimumProperty, value); }
        public sealed override long Maximum { get => _maximum; set => SetValue(MaximumProperty, value); }
        public sealed override long WheelStep { get => _wheel_step; set => SetValue(WheelStepProperty, value); }
    }
}
