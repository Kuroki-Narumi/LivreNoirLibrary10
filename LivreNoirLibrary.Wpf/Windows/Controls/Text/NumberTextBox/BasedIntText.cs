using System;
using System.Windows;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IBasedIntText
    {
        public const int DefaultRadix = 16;
        public const int DefaultDigits = 0;

        public static readonly object MinRadixObject = BasedIndex.MinimumRadix;
        public static readonly object MaxRadixObject = BasedIndex.MaximumRadix;

        public static void OnRadixChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IBasedIntText)?.OnRadixChanged((int)e.NewValue);
        }

        public static object OnRadixCoerce(DependencyObject d, object baseValue)
        {
            var value = (int)baseValue;
            return value is < BasedIndex.MinimumRadix ? MinRadixObject
                 : value is > BasedIndex.MaximumRadix ? MaxRadixObject
                 : baseValue;
        }

        public static void OnDigitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as IBasedIntText)?.OnDigitsChanged((int)e.NewValue);
        }

        public int Radix { get; set; }
        public int Digits { get; set; }

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
            if (flag && _radix is >= BasedIndex.MinimumRadix)
            {
                Text = ToString(current);
            }
        }

        protected override string? ToString(int value) => BasedIndex.ToBased(value, _radix, _digits);
        protected override bool TryParse(string? text, out int value) => BasedIndex.TryParseToInt(text, _radix, out value);
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
            if (flag && _radix is >= BasedIndex.MinimumRadix)
            {
                Text = ToString(current);
            }
        }

        protected override string? ToString(int value) => BasedIndex.ToBased(value, _radix, _digits);
        protected override bool TryParse(string? text, out int value) => BasedIndex.TryParseToInt(text, _radix, out value);
    }
}
