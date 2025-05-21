using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ColorPicker : DropDownBase
    {
        public const string PART_TextBox = nameof(PART_TextBox);
        public const string PART_Indicator = nameof(PART_Indicator);
        public const string PART_PopupContent = nameof(PART_PopupContent);

        static ColorPicker()
        {
            PropertyUtils.OverrideDefaultStyleKey<ColorPicker>();
        }

        public static readonly RoutedEvent SelectedColorChangedEvent = EventRegister.Register<ColorPicker, ColorChangedEventHandler>();

        public event ColorChangedEventHandler SelectedColorChanged { add => AddHandler(SelectedColorChangedEvent, value); remove => RemoveHandler(SelectedColorChangedEvent, value); }

        [DependencyProperty(BindsTwoWayByDefault = true)]
        private Color _selectedColor;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private bool _isAlphaEnabled;
        [DependencyProperty]
        private bool _isPickerEnabled = true;
        [DependencyProperty(AffectsMeasure = true)]
        private double _indicatorWidth = double.NaN;
        [DependencyProperty(AffectsMeasure = true)]
        private double _separatorWidth = 0;
        [DependencyProperty(AffectsMeasure = true)]
        private bool _isTextBoxVisible = true;

        private SolidColorBrush? _indicator;
        private EditableTextBlock? _textBox;
        private Border? _selector_parent;
        private ColorSelector? _selector;

        private void OnSelectedColorChanged(Color value)
        {
            UpdateColorIndicate();
            RaiseEvent(new ColorChangedEventArgs(value, SelectedColorChangedEvent, this));
        }

        private void OnIsAlphaEnabledChanged(bool value)
        {
            UpdateColorIndicate();
        }

        public override void OnApplyTemplate()
        {
            if (_textBox is not null)
            {
                _textBox.GotFocus -= OnGotFocus_TextBox;
                _textBox.Verify -= OnVerify_TextBox;
                _textBox.TextChanged -= OnTextChanged_TextBox;
            }
            base.OnApplyTemplate();
            _indicator = GetTemplateChild(PART_Indicator) as SolidColorBrush;
            _textBox = GetTemplateChild(PART_TextBox) as EditableTextBlock;
            _selector_parent = GetTemplateChild(PART_PopupContent) as Border;
            if (_textBox is not null)
            {
                _textBox.GotFocus += OnGotFocus_TextBox;
                _textBox.Verify += OnVerify_TextBox;
                _textBox.TextChanged += OnTextChanged_TextBox;
            }
            UpdateColorIndicate();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var w = IndicatorWidth;
            if (!double.IsFinite(Width) && double.IsFinite(w) && w is >= 0) 
            {
                if (IsTextBoxVisible)
                {
                    w += SeparatorWidth;
                    if (_textBox is not null)
                    {
                        w += _textBox.Width;
                    }
                    return new(w, constraint.Height);
                }
            }
            return base.MeasureOverride(constraint);
        }

        private void UpdateColorIndicate()
        {
            if (_indicator is not null && !_indicator.IsFrozen)
            {
                _indicator.Color = _selectedColor;
            }
            if (_textBox is not null)
            {
                _textBox.Text = _selectedColor.GetColorCode(_isAlphaEnabled);
            }
        }

        private void OnGotFocus_TextBox(object sender, RoutedEventArgs e)
        {
            IsDropDownOpen = false;
        }

        private bool OnVerify_TextBox(string? text) => ColorUtils.IsValidColorCode(text);

        private void OnTextChanged_TextBox(object sender, EditableTextChangedEventArgs e)
        {
            var color = e.NewText!.ToColor();
            if (!_isAlphaEnabled)
            {
                color.A = 255;
            }
            if (color != _selectedColor)
            {
                SelectedColor = color;
            }
        }

        protected override void OnDropDownOpen()
        {
            base.OnDropDownOpen();
            if (_selector is null && _selector_parent is not null)
            {
                _selector = ColorSelectorPool.Rent(this);
                _selector_parent.Child = _selector;
            }
        }

        protected override void OnDropDownClosed()
        {
            base.OnDropDownClosed();
            if (_selector is not null && _selector_parent is not null)
            {
                _selector_parent.Child = null;
                ColorSelectorPool.Return(_selector);
                _selector = null;
            }
        }
    }
}
