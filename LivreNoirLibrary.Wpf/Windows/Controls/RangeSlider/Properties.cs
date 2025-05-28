using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class RangeSlider
    {
        public const double DefaultMinimum = 0;
        public const double DefaultMaximum = 100;
        public const double DefaultTickFrequency = 1;
        public const string DefaultExclusiveText = "Exclusive";

        public static readonly DependencyProperty Value1Property = PropertyUtils.RegisterTwoWay(typeof(RangeSlider), DefaultMinimum, OnValue1Changed);
        public static readonly DependencyProperty Value2Property = PropertyUtils.RegisterTwoWay(typeof(RangeSlider), DefaultMaximum, OnValue2Changed);
        public static readonly DependencyProperty MinimumProperty = RangeBase.MinimumProperty.AddOwner(typeof(RangeSlider), DefaultMinimum, OnMinimumChanged);
        public static readonly DependencyProperty MaximumProperty = RangeBase.MaximumProperty.AddOwner(typeof(RangeSlider), DefaultMaximum, OnMaximumChanged);
        public static readonly DependencyProperty TickFrequencyProperty = Slider.TickFrequencyProperty.AddOwner(typeof(RangeSlider), DefaultTickFrequency, OnTickFrequencyChanged);

        private static void OnValue1Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RangeSlider)?.VerifyValue1((double)e.NewValue);
        }

        private static void OnValue2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RangeSlider)?.VerifyValue2((double)e.NewValue);
        }

        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RangeSlider)?.VerifyMinimum((double)e.NewValue);
        }

        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RangeSlider)?.VerifyMaximum((double)e.NewValue);
        }

        private static void OnTickFrequencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RangeSlider s)
            {
                s._tick_frequency = (double)e.NewValue;
            }
        }

        private void VerifyValue1(double target) => VerifyValue(target, ref _value1, Value1Property, _minimum, Math.Min(_maximum, _value2));
        private void VerifyValue2(double target) => VerifyValue(target, ref _value2, Value2Property, Math.Max(_minimum, _value1), _maximum);
        private void VerifyMinimum(double target) => VerifyValue(target, ref _minimum, MinimumProperty, double.MinValue, _maximum);
        private void VerifyMaximum(double target) => VerifyValue(target, ref _maximum, MaximumProperty, _minimum, double.MaxValue);

        private bool VerifyValue(double value, ref double field, DependencyProperty property, double min, double max)
        {
            if (_initialized)
            {
                if (value < min)
                {
                    SetValue(property, min);
                    return false;
                }
                if (value > max)
                {
                    SetValue(property, max);
                    return false;
                }
            }
            field = value;
            ReserveRefresh();
            return true;
        }

        private double _value1 = DefaultMinimum;
        private double _value2 = DefaultMaximum;
        private double _minimum = DefaultMinimum;
        private double _maximum = DefaultMaximum;
        private double _tick_frequency = DefaultTickFrequency;

        [DependencyProperty]
        private bool _withExclusiveButton;
        [DependencyProperty]
        private object? _exclusiveText = DefaultExclusiveText;
        [DependencyProperty]
        private bool _isExclusive;
        [DependencyProperty]
        private bool _withText;
        [DependencyProperty]
        private double _textWidth = double.NaN;
        [DependencyProperty]
        private Thickness _textMargin;
        [DependencyProperty]
        private bool _isTextEditable;
        [DependencyProperty]
        private double _thumbWidth;
        [DependencyProperty]
        private double _thumbHeight;
        [DependencyProperty(SetterScope = ObjectModel.Scope.Private)]
        private Thickness _thumb1Margin;
        [DependencyProperty(SetterScope = ObjectModel.Scope.Private)]
        private Thickness _thumb2Margin;
        [DependencyProperty(SetterScope = ObjectModel.Scope.Private)]
        private Thickness _selection1Margin;
        [DependencyProperty(SetterScope = ObjectModel.Scope.Private)]
        private double _selection1Width;
        [DependencyProperty(SetterScope = ObjectModel.Scope.Private)]
        private Thickness _selection2Margin;
        [DependencyProperty(SetterScope = ObjectModel.Scope.Private)]
        private double _selection2Width;

        public double Value1 { get => _value1; set => SetValue(Value1Property, value); }
        public double Value2 { get => _value2; set => SetValue(Value2Property, value); }
        public double Minimum { get => _minimum; set => SetValue(MinimumProperty, value); }
        public double Maximum { get => _maximum; set => SetValue(MaximumProperty, value); }
        public double TickFrequency { get => _tick_frequency; set => SetValue(TickFrequencyProperty, value); }

        private void OnIsExclusiveChanged(bool value) => ReserveRefresh();
        private void OnThumbWidthChanged(double value) => ReserveRefresh();
        private void OnThumbHeightChanged(double value) => ReserveRefresh();
    }
}
