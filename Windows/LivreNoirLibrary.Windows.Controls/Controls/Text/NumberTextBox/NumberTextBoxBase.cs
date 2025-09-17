using System;
using System.Numerics;
using System.Windows.Input;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public abstract class NumberTextBoxBase<T> : DefaultTextBox, INumberText<T>
        where T : struct, INumber<T>
    {
        private static readonly T _10 = T.CreateChecked(10);

        private bool _value_changing;
        private bool _default_enabled;
        protected T _default;
        protected T _value;
        protected T _minimum;
        protected T _maximum;
        protected T _wheel_step;

        public abstract T DefaultValue { get; set; }
        public abstract T Value { get; set; }
        public abstract T Minimum { get; set; }
        public abstract T Maximum { get; set; }
        public abstract T WheelStep { get; set; }

        public NumberTextBoxBase()
        {
            InitializeFields();
            UpdateDefaultEnabled();
            SetValue(InputMethod.IsInputMethodEnabledProperty, false);
            if (_default != default)
            {
                DefaultText ??= ToString(_default);
            }
            if (_value != _default)
            {
                Value = _default;
            }
            else
            {
                Text = ToString(_value);
            }
        }

        protected abstract void InitializeFields();

        void INumberText<T>.OnDefaultValueChanged(T value)
        {
            _default = value;
            DefaultText = ToString(value);
            UpdateDefaultEnabled();
        }

        void INumberText<T>.OnValueChanged(T value)
        {
            _value = value;
            var text = ToString(value);
            if (!_value_changing && Text != text)
            {
                _value_changing = true;
                Text = text;
                _value_changing = false;
            }
        }

        void INumberText<T>.OnMinimumChanged(T value)
        {
            _minimum = value;
            UpdateDefaultEnabled();
        }

        void INumberText<T>.OnMaximumChanged(T value)
        {
            _maximum = value;
            UpdateDefaultEnabled();
        }

        void INumberText<T>.OnWheelStepChanged(T value) => _wheel_step = value;

        private void UpdateDefaultEnabled()
        {
            _default_enabled = IsValueInRange(_default);
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            Value = e.Delta is > 0 ? GetNext(_value) : GetPrevious(_value);
            e.Handled = true;
        }

        protected virtual T GetActualWheelStep() => KeyInput.IsShiftDown() ? _wheel_step * _10 : _wheel_step;

        protected virtual T GetNext(T value)
        {
            value += GetActualWheelStep();
            if (value > _maximum)
            {
                value = _maximum;
            }
            return value;
        }

        protected virtual T GetPrevious(T value)
        {
            value -= GetActualWheelStep();
            if (value < _minimum)
            {
                value = _minimum;
            }
            return value;
        }

        protected override bool OnVerify(string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                if (_default_enabled)
                {
                    _value = _default;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (TryParse(text, out var value) && IsValueInRange(value))
            {
                if (!_value_changing && _value != value)
                {
                    _value_changing = true;
                    Value = value;
                    _value_changing = false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual bool IsValueInRange(T value) => value >= _minimum && value <= _maximum;
        protected virtual bool TryParse(string? text, out T value) => T.TryParse(text, null, out value);
        protected virtual string? ToString(T value) => value.ToString();
    }
}
