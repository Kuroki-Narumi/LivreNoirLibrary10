using System;
using System.Windows;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Controls
{
    public class RationalTextBlock : NumberTextBlockBase<Rational>
    {
        public static readonly DependencyProperty DefaultValueProperty = RationalPropertyHolder.DefaultValueProperty.AddOwner(typeof(RationalTextBlock));
        public static readonly DependencyProperty ValueProperty = RationalPropertyHolder.ValueProperty.AddOwner(typeof(RationalTextBlock));
        public static readonly DependencyProperty MinimumProperty = RationalPropertyHolder.MinimumProperty.AddOwner(typeof(RationalTextBlock));
        public static readonly DependencyProperty MaximumProperty = RationalPropertyHolder.MaximumProperty.AddOwner(typeof(RationalTextBlock));
        public static readonly DependencyProperty WheelStepProperty = RationalPropertyHolder.WheelStepProperty.AddOwner(typeof(RationalTextBlock));

        protected override void InitializeFields()
        {
            this.SetDefaultValue(DefaultValueProperty, ref _default);
            this.SetDefaultValue(MinimumProperty, ref _minimum);
            this.SetDefaultValue(MaximumProperty, ref _maximum);
            this.SetDefaultValue(WheelStepProperty, ref _wheel_step);
        }

        public sealed override Rational DefaultValue { get => _default; set => SetValue(DefaultValueProperty, value); }
        public sealed override Rational Value { get => _value; set => SetValue(ValueProperty, value); }
        public sealed override Rational Minimum { get => _minimum; set => SetValue(MinimumProperty, value); }
        public sealed override Rational Maximum { get => _maximum; set => SetValue(MaximumProperty, value); }
        public sealed override Rational WheelStep { get => _wheel_step; set => SetValue(WheelStepProperty, value); }
    }

    public class RationalTextBox : NumberTextBoxBase<Rational>
    {
        public static readonly DependencyProperty DefaultValueProperty = RationalPropertyHolder.DefaultValueProperty.AddOwner(typeof(RationalTextBox));
        public static readonly DependencyProperty ValueProperty = RationalPropertyHolder.ValueProperty.AddOwner(typeof(RationalTextBox));
        public static readonly DependencyProperty MinimumProperty = RationalPropertyHolder.MinimumProperty.AddOwner(typeof(RationalTextBox));
        public static readonly DependencyProperty MaximumProperty = RationalPropertyHolder.MaximumProperty.AddOwner(typeof(RationalTextBox));
        public static readonly DependencyProperty WheelStepProperty = RationalPropertyHolder.WheelStepProperty.AddOwner(typeof(RationalTextBox));

        protected override void InitializeFields()
        {
            this.SetDefaultValue(DefaultValueProperty, ref _default);
            this.SetDefaultValue(MinimumProperty, ref _minimum);
            this.SetDefaultValue(MaximumProperty, ref _maximum);
            this.SetDefaultValue(WheelStepProperty, ref _wheel_step);
        }

        public sealed override Rational DefaultValue { get => _default; set => SetValue(DefaultValueProperty, value); }
        public sealed override Rational Value { get => _value; set => SetValue(ValueProperty, value); }
        public sealed override Rational Minimum { get => _minimum; set => SetValue(MinimumProperty, value); }
        public sealed override Rational Maximum { get => _maximum; set => SetValue(MaximumProperty, value); }
        public sealed override Rational WheelStep { get => _wheel_step; set => SetValue(WheelStepProperty, value); }

        protected override string? ToString(Rational value) => value.ToString();
        protected override bool TryParse(string? text, out Rational value) => Rational.TryParse(text, out value);
    }
}
