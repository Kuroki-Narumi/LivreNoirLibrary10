using System;
using System.Windows;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IBasedIntText
    {
        const int DefaultRadix = 16;
        const int DefaultDigits = 0;

        static readonly object MinRadixObject = BasedNumber.MinimumRadix;
        static readonly object MaxRadixObject = BasedNumber.MaximumRadix;

        static void OnRadixChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IBasedIntText)?.OnRadixChanged((int)e.NewValue);
        }

        static object OnRadixCoerce(DependencyObject d, object baseValue)
        {
            var value = (int)baseValue;
            return value is < BasedNumber.MinimumRadix ? MinRadixObject
                 : value is > BasedNumber.MaximumRadix ? MaxRadixObject
                 : baseValue;
        }

        static void OnDigitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IBasedIntText)?.OnDigitsChanged((int)e.NewValue);
        }

        int Radix { get; set; }
        int Digits { get; set; }

        void OnRadixChanged(int value) { }
        void OnDigitsChanged(int value) { }
    }

    public class BasedIntTextBlock : IntTextBlock, IBasedIntText
    {
        public static readonly DependencyProperty RadixProperty = IntPropertyHolder.RadixProperty.AddOwner(typeof(BasedIntTextBlock));
        public static readonly DependencyProperty DigitsProperty = IntPropertyHolder.DigitsProperty.AddOwner(typeof(BasedIntTextBlock));

        protected int _radix;
        protected int _digits;

        protected override void InitializeFields()
        {
            base.InitializeFields();
            _radix = (int)RadixProperty.GetMetadata(this).DefaultValue;
            _digits = (int)DigitsProperty.GetMetadata(this).DefaultValue;
        }

        public int Radix { get => _radix; set => SetValue(RadixProperty, value); }
        public int Digits { get => _digits; set => SetValue(DigitsProperty, value); }

        protected override int GetActualWheelStep() => KeyInput.IsShiftDown() ? _wheel_step : _wheel_step * _radix;

        void IBasedIntText.OnRadixChanged(int value)
        {
            var flag = TryParse(Text, out var current);
            _radix = value;
            if (flag)
            {
                Text = ToString(current);
            }
        }

        void IBasedIntText.OnDigitsChanged(int value)
        {
            var flag = TryParse(Text, out var current);
            _digits = value;
            if (flag && _radix is >= BasedNumber.MinimumRadix)
            {
                Text = ToString(current);
            }
        }

        protected override string? ToString(int value) => BasedNumber.ToBased(value, _radix, _digits);
        protected override bool TryParse(string? text, out int value) => BasedNumber.TryParseToInt(text, _radix, out value);
    }

    public class BasedIntTextBox : IntTextBox, IBasedIntText
    {
        public static readonly DependencyProperty RadixProperty = IntPropertyHolder.RadixProperty.AddOwner(typeof(BasedIntTextBox));
        public static readonly DependencyProperty DigitsProperty = IntPropertyHolder.DigitsProperty.AddOwner(typeof(BasedIntTextBox));

        protected int _radix;
        protected int _digits;

        protected override void InitializeFields()
        {
            base.InitializeFields();
            _radix = (int)RadixProperty.GetMetadata(this).DefaultValue;
            _digits = (int)DigitsProperty.GetMetadata(this).DefaultValue;
        }

        public int Radix { get => _radix; set => SetValue(RadixProperty, value); }
        public int Digits { get => _digits; set => SetValue(DigitsProperty, value); }

        protected override int GetActualWheelStep() => KeyInput.IsShiftDown() ? _wheel_step : _wheel_step * _radix;

        void IBasedIntText.OnRadixChanged(int value)
        {
            var flag = TryParse(Text, out var current);
            _radix = value;
            if (flag)
            {
                Text = ToString(current);
            }
        }

        void IBasedIntText.OnDigitsChanged(int value)
        {
            var flag = TryParse(Text, out var current);
            _digits = value;
            if (flag && _radix is >= BasedNumber.MinimumRadix)
            {
                Text = ToString(current);
            }
        }

        protected override string? ToString(int value) => BasedNumber.ToBased(value, _radix, _digits);
        protected override bool TryParse(string? text, out int value) => BasedNumber.TryParseToInt(text, _radix, out value);
    }
}
