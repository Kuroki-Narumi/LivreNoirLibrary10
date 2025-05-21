using System;
using System.Windows;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Controls
{
    internal class IntPropertyHolder : DependencyObject
    {
        public static readonly DependencyProperty DefaultValueProperty = PropertyUtils.RegisterAttached(typeof(IntPropertyHolder), default(int), INumberText<int>.OnDefaultValueChanged);
        public static readonly DependencyProperty ValueProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(IntPropertyHolder), default(int), INumberText<int>.OnValueChanged);
        public static readonly DependencyProperty MinimumProperty = PropertyUtils.RegisterAttached(typeof(IntPropertyHolder), int.MinValue, INumberText<int>.OnMinimumChanged);
        public static readonly DependencyProperty MaximumProperty = PropertyUtils.RegisterAttached(typeof(IntPropertyHolder), int.MaxValue, INumberText<int>.OnMaximumChanged);
        public static readonly DependencyProperty WheelStepProperty = PropertyUtils.RegisterAttached(typeof(IntPropertyHolder), 1, INumberText<int>.OnWheelStepChanged);

        public static readonly DependencyProperty RadixProperty = PropertyUtils.RegisterAttached(typeof(IntPropertyHolder), IBasedIntText.DefaultRadix, IBasedIntText.OnRadixChanged, IBasedIntText.OnRadixCoerce);
        public static readonly DependencyProperty DigitsProperty = PropertyUtils.RegisterAttached(typeof(IntPropertyHolder), IBasedIntText.DefaultDigits, IBasedIntText.OnDigitsChanged);
    }

    internal class LongPropertyHolder : DependencyObject
    {
        public static readonly DependencyProperty DefaultValueProperty = PropertyUtils.RegisterAttached(typeof(LongPropertyHolder), default(long), INumberText<long>.OnDefaultValueChanged);
        public static readonly DependencyProperty ValueProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(LongPropertyHolder), default(long), INumberText<long>.OnValueChanged);
        public static readonly DependencyProperty MinimumProperty = PropertyUtils.RegisterAttached(typeof(LongPropertyHolder), long.MinValue, INumberText<long>.OnMinimumChanged);
        public static readonly DependencyProperty MaximumProperty = PropertyUtils.RegisterAttached(typeof(LongPropertyHolder), long.MaxValue, INumberText<long>.OnMaximumChanged);
        public static readonly DependencyProperty WheelStepProperty = PropertyUtils.RegisterAttached(typeof(LongPropertyHolder), 1L, INumberText<long>.OnWheelStepChanged);
    }

    internal class DoublePropertyHolder : DependencyObject
    {
        public static readonly DependencyProperty DefaultValueProperty = PropertyUtils.RegisterAttached(typeof(DoublePropertyHolder), default(double), INumberText<double>.OnDefaultValueChanged);
        public static readonly DependencyProperty ValueProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(DoublePropertyHolder), default(double), INumberText<double>.OnValueChanged);
        public static readonly DependencyProperty MinimumProperty = PropertyUtils.RegisterAttached(typeof(DoublePropertyHolder), double.MinValue, INumberText<double>.OnMinimumChanged);
        public static readonly DependencyProperty MaximumProperty = PropertyUtils.RegisterAttached(typeof(DoublePropertyHolder), double.MaxValue, INumberText<double>.OnMaximumChanged);
        public static readonly DependencyProperty WheelStepProperty = PropertyUtils.RegisterAttached(typeof(DoublePropertyHolder), 1d, INumberText<double>.OnWheelStepChanged);
        public static readonly DependencyProperty StringFormatProperty = PropertyUtils.RegisterAttached<string>(typeof(DoublePropertyHolder), callback: OnStringFormatChanged);

        public static void OnStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as INumberText<double>)?.OnStringFormatChanged(e.NewValue as string);
    }

    internal class DecimalPropertyHolder : DependencyObject
    {
        public static readonly DependencyProperty DefaultValueProperty = PropertyUtils.RegisterAttached(typeof(DecimalPropertyHolder), default(decimal), INumberText<decimal>.OnDefaultValueChanged);
        public static readonly DependencyProperty ValueProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(DecimalPropertyHolder), default(decimal), INumberText<decimal>.OnValueChanged);
        public static readonly DependencyProperty MinimumProperty = PropertyUtils.RegisterAttached(typeof(DecimalPropertyHolder), decimal.MinValue, INumberText<decimal>.OnMinimumChanged);
        public static readonly DependencyProperty MaximumProperty = PropertyUtils.RegisterAttached(typeof(DecimalPropertyHolder), decimal.MaxValue, INumberText<decimal>.OnMaximumChanged);
        public static readonly DependencyProperty WheelStepProperty = PropertyUtils.RegisterAttached(typeof(DecimalPropertyHolder), 1m, INumberText<decimal>.OnWheelStepChanged);
    }

    internal class RationalPropertyHolder : DependencyObject
    {
        public static readonly DependencyProperty DefaultValueProperty = PropertyUtils.RegisterAttached(typeof(RationalPropertyHolder), default(Rational), INumberText<Rational>.OnDefaultValueChanged);
        public static readonly DependencyProperty ValueProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(RationalPropertyHolder), default(Rational), INumberText<Rational>.OnValueChanged);
        public static readonly DependencyProperty MinimumProperty = PropertyUtils.RegisterAttached(typeof(RationalPropertyHolder), Rational.MinValue, INumberText<Rational>.OnMinimumChanged);
        public static readonly DependencyProperty MaximumProperty = PropertyUtils.RegisterAttached(typeof(RationalPropertyHolder), Rational.MaxValue, INumberText<Rational>.OnMaximumChanged);
        public static readonly DependencyProperty WheelStepProperty = PropertyUtils.RegisterAttached(typeof(RationalPropertyHolder), Rational.One, INumberText<Rational>.OnWheelStepChanged);
    }
}
