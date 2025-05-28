using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class RangeSlider : Control
    {
        public const string PART_TrackBackground = nameof(PART_TrackBackground);
        public const string PART_RepeatButton = nameof(PART_RepeatButton);
        public const string PART_TextBox1 = nameof(PART_TextBox1);
        public const string PART_TextBox2 = nameof(PART_TextBox2);
        public const string PART_Thumb1 = nameof(PART_Thumb1);
        public const string PART_Thumb2 = nameof(PART_Thumb2);

        static RangeSlider()
        {
            PropertyUtils.OverrideDefaultStyleKey<RangeSlider>();
        }

        private bool _initialized;
        private bool _need_refresh;
        private Border _background = new();
        private RepeatButton? _repeatButton;
        private EditableTextBlock? _text1;
        private EditableTextBlock? _text2;
        private Thumb _thumb1 = new();
        private Thumb _thumb2 = new();

        public override void OnApplyTemplate()
        {
            _initialized = false;
            if (_repeatButton is not null)
            {
                _repeatButton.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown_RepeatButton;
                _repeatButton.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp_RepeatButton;
                _repeatButton.Click -= OnClick_RepeatButton;
            }
            if (_text1 is not null)
            {
                _text1.TextChanged -= OnTextChanged;
            }
            if (_text2 is not null)
            {
                _text2.TextChanged -= OnTextChanged;
            }
            base.OnApplyTemplate();
            if (GetTemplateChild(PART_TrackBackground) is Border b)
            {
                _background = b;
            }
            if ((_repeatButton = GetTemplateChild(PART_RepeatButton) as RepeatButton) is not null)
            {
                _repeatButton.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown_RepeatButton;
                _repeatButton.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp_RepeatButton;
                _repeatButton.Click += OnClick_RepeatButton;
            }
            if ((_text1 = GetTemplateChild(PART_TextBox1) as EditableTextBlock) is not null)
            {
                _text1.TextChanged += OnTextChanged;
            }
            if ((_text2 = GetTemplateChild(PART_TextBox2) as EditableTextBlock) is not null)
            {
                _text2.TextChanged += OnTextChanged;
            }
            if (GetTemplateChild(PART_Thumb1) is Thumb t1)
            {
                _thumb1.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown_Thumb1;
                t1.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown_Thumb1;
                _thumb1 = t1;
            }
            if (GetTemplateChild(PART_Thumb2) is Thumb t2)
            {
                _thumb2.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown_Thumb2;
                t2.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown_Thumb2;
                _thumb2 = t2;
            }
            _initialized = true;
            ReserveRefresh();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            ReserveRefresh();
        }

        public void ReserveRefresh()
        {
            _need_refresh = true;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_need_refresh)
            {
                Refresh();
            }
            base.OnRender(drawingContext);
        }

        private void Refresh()
        {
            _need_refresh = false;
            var range = _maximum - _minimum;
            var tw = _thumbWidth;
            var w = Math.Max(_background.ActualWidth, 0);
            if (range is > 0)
            {
                range = w / range;
                var x1 = Math.Round(Math.Clamp((_value1 - _minimum) * range, 0, w));
                var x2 = Math.Round(Math.Clamp((_value2 - _minimum) * range, 0, w));
                Thumb1Margin = new(x1, 0, 0, 0);
                Thumb2Margin = new(x2 + tw, 0, 0, 0);
                if (IsExclusive)
                {
                    Selection1Margin = default;
                    Selection1Width = x1;
                    Selection2Margin = new(x2, 0, 0, 0);
                    Selection2Width = w - x2;
                }
                else
                {
                    Selection1Margin = new(x1, 0, 0, 0);
                    Selection1Width = x2 - x1;
                    Selection2Width = 0;
                }
            }
            else
            {
                Thumb1Margin = default;
                Thumb2Margin = new(w + tw, 0, 0, 0);
                Selection1Width = 0;
                Selection1Width = 0;
            }
        }

        public void SetRange(double value1, double value2, bool exclusive = false)
        {
            _value1 = value1;
            _value2 = value2;
            Value1 = value1;
            Value2 = value2;
            IsExclusive = exclusive;
            RaiseValueChanged();
        }

        public (double Value1, double Value2, bool IsExclusive) GetRange() => (_value1, _value2, _isExclusive);

        private void OnMouseLeftButtonDown_Thumb1(object sender, MouseButtonEventArgs e)
        {
            ThumbDragMove(_thumb1!, _value1, Value1Property);
            e.Handled = true;
        }

        private void OnMouseLeftButtonDown_Thumb2(object sender, MouseButtonEventArgs e)
        {
            ThumbDragMove(_thumb2!, _value2, Value2Property);
            e.Handled = true;
        }

        protected virtual void ThumbDragMove(Thumb thumb, double initialValue, DependencyProperty target)
        {
            var pos = Mouse.GetPosition(_background);
            var initialX = pos.X;
            var w = (_maximum - _minimum) / _background.ActualWidth;

            thumb.CaptureMouse();
            thumb.MouseMove += MouseMove;
            thumb.MouseLeftButtonUp += MouseUp;

            void MouseMove(object sender, MouseEventArgs e)
            {
                var pos = e.GetPosition(_background);
                var value = initialValue + (pos.X - initialX) * w;
                if (value < _minimum)
                {
                    SetValue(target, _minimum);
                }
                else if (value > _maximum)
                {
                    SetValue(target, _maximum);
                }
                else
                {
                    value = _tick_frequency * (int)Math.Round(value / _tick_frequency);
                    SetValue(target, value);
                }
            }

            void MouseUp(object sender, MouseButtonEventArgs e)
            {
                thumb.MouseMove -= MouseMove;
                thumb.MouseLeftButtonUp -= MouseUp;
                thumb.ReleaseMouseCapture();
                RaiseValueChanged();
            }
        }

        protected (double current, DependencyProperty property, bool increase) GetTarget()
        {
            var x1 = Mouse.GetPosition(_thumb1).X - _thumbWidth;
            var x2 = Mouse.GetPosition(_thumb2).X;
            if (x1 < 0)
            {
                return (_value1, Value1Property, false);
            }
            else if (x2 > 0 || (x1 >= -x2))
            {
                return (_value2, Value2Property, x2 > 0);
            }
            else
            {
                return (_value1, Value1Property, true);
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            if (Mouse.Captured is null)
            {
                var (current, property, _) = GetTarget();
                MoveValue(current, property, e.Delta is > 0);
                e.Handled = true;
            }
        }

        private bool _changing_by_click;

        private void OnMouseLeftButtonDown_RepeatButton(object sender, MouseButtonEventArgs e)
        {
            _changing_by_click = true;
        }

        private void OnMouseLeftButtonUp_RepeatButton(object sender, MouseButtonEventArgs e)
        {
            if (_changing_by_click)
            {
                _changing_by_click = false;
                RaiseValueChanged();
            }
        }

        private void OnClick_RepeatButton(object sender, RoutedEventArgs e)
        {
            var (current, property, increase) = GetTarget();
            MoveValue(current, property, increase);
            e.Handled = true;
        }

        protected void MoveValue(double current, DependencyProperty target, bool increase)
        {
            var amount = _tick_frequency;
            if (current % amount is not 0)
            {
                current = amount * (increase ? Math.Ceiling(current / amount) : Math.Floor(current / amount));
            }
            else
            {
                if (KeyInput.IsShiftDown())
                {
                    amount *= 10;
                }
                if (increase)
                {
                    current += amount;
                }
                else
                {
                    current -= amount;
                }
            }
            SetValue(target, current);
            if (!_changing_by_click)
            {
                RaiseValueChanged();
            }
        }

        private void OnTextChanged(object sender, EditableTextChangedEventArgs e)
        {
            RaiseValueChanged();
        }
    }
}
