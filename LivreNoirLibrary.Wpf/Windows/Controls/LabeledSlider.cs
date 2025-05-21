using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class LabeledSlider : Slider
    {
        public const string PART_Slider = nameof(PART_Slider);
        public const string PART_ValueText = nameof(PART_ValueText);

        [DependencyProperty]
        private object? _header;
        [DependencyProperty]
        private double _sliderWidth = double.NaN;
        [DependencyProperty]
        private double _textWidth = double.NaN;
        [DependencyProperty]
        private bool _isTextEditable;
        [DependencyProperty]
        private string? _stringFormat;

        private Slider? _slider;
        private TextBlock? _valueText;

        static LabeledSlider()
        {
            PropertyUtils.OverrideDefaultStyleKey<LabeledSlider>();
        }

        private readonly DependencyProperty[] BindingTargets = [
            MaximumProperty,
            MinimumProperty,
            ValueProperty,

            LargeChangeProperty,
            SmallChangeProperty,
            IntervalProperty,
            DelayProperty,

            TicksProperty,
            TickFrequencyProperty,
            TickPlacementProperty,
            IsSnapToTickEnabledProperty,

            SelectionStartProperty,
            SelectionEndProperty,
            IsSelectionRangeEnabledProperty,

            IsMoveToPointEnabledProperty,
            IsDirectionReversedProperty,
            OrientationProperty,
            AutoToolTipPlacementProperty,
            AutoToolTipPrecisionProperty,
        ];

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild(PART_Slider) is Slider slider)
            {
                void Bind(DependencyProperty dp)
                {
                    if (_slider is not null)
                    {
                        BindingOperations.ClearBinding(_slider, dp);
                    }
                    slider.SetBinding(dp, new Binding(dp.Name) { Source = this, Mode = BindingMode.TwoWay });
                }
                foreach (var dp in BindingTargets)
                {
                    Bind(dp);
                }
                _slider = slider;
            }
            _valueText = GetTemplateChild(PART_ValueText) as TextBlock;
            UpdateValueText(Value);
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            this.ChangeByWheel(e);
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            UpdateValueText(newValue);
        }

        private void UpdateValueText(double value)
        {
            if (_valueText is TextBlock t)
            {
                t.Text = value.ToString(StringFormat);
            }
        }
    }
}
