using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class RangeSlider : Control
    {
        public const string PART_TrackBackground = nameof(PART_TrackBackground);
        public const string PART_RepeatButton = nameof(PART_RepeatButton);
        public const string PART_Thumb1 = nameof(PART_Thumb1);
        public const string PART_Thumb2 = nameof(PART_Thumb2);

        static RangeSlider()
        {
            PropertyUtils.OverrideDefaultStyleKey<RangeSlider>();
        }

        private bool _initialized;
        private Border _background = new();
        private RepeatButton? _repeatButton;
        private Thumb _thumb1 = new();
        private Thumb _thumb2 = new();

        public override void OnApplyTemplate()
        {
            _initialized = false;
            if (_repeatButton is not null)
            {
                _repeatButton.Click -= OnClick_RepeatButton;
            }
            base.OnApplyTemplate();
            if (GetTemplateChild(PART_TrackBackground) is Border b)
            {
                _background = b;
            }
            if ((_repeatButton = GetTemplateChild(PART_RepeatButton) as RepeatButton) is not null)
            {
                _repeatButton.Click += OnClick_RepeatButton;
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

        private bool _need_refresh;

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
            var tw = _thumb1.Width;
            var w = _background.ActualWidth - tw;
            if (range is > 0)
            {
                range = w / range;
                var x1 = Math.Round(Math.Clamp((_value1 - _minimum) * range, 0, w));
                var x2 = Math.Round(Math.Clamp((_value2 - _minimum) * range, 0, w));
                ThumbLeft = x1 - tw;
                ThumbRight = x2;
                if (IsExclusive)
                {
                    Selection1Left = 0;
                    Selection1Width = Math.Max(x1, 0);
                    Selection2Left = x2 + tw;
                    Selection2Width = Math.Max(w - x2, 0);
                }
                else
                {
                    Selection1Left = x1;
                    Selection1Width = Math.Max(x2 - x1 + tw, 0);
                    Selection2Width = 0;
                }
            }
            else
            {
                ThumbLeft = -tw;
                ThumbRight = w;
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
            var w = (_maximum - _minimum) / (_background.ActualWidth - _thumb1.Width);

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
            }

            thumb.CaptureMouse();
            thumb.MouseMove += MouseMove;
            thumb.MouseLeftButtonUp += MouseUp;
        }

        protected (double current, DependencyProperty property, bool increase) GetTarget()
        {
            var x1 = Mouse.GetPosition(_thumb1).X - _thumb1.Width;
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
            var (current, property, _) = GetTarget();
            ChangeByWheel(current, property, e);
        }

        protected virtual void ChangeByWheel(double current, DependencyProperty target, MouseWheelEventArgs e)
        {
            MoveValue(current, target, e.Delta > 0);
            e.Handled = true;
        }

        private void OnClick_RepeatButton(object sender, RoutedEventArgs e)
        {
            var (current, property, increase) = GetTarget();
            MoveValue(current, property, increase);
            e.Handled = true;
        }

        protected void MoveValue(double current, DependencyProperty target, bool increase)
        {
            if (current % _tick_frequency != 0)
            {
                current = _tick_frequency * (increase ? (int)Math.Ceiling(current / _tick_frequency) : (int)Math.Floor(current / _tick_frequency));
            }
            else if (increase)
            {
                current += _tick_frequency;
            }
            else
            {
                current -= _tick_frequency;
            }
            SetValue(target, current);
        }
    }
}
