using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class EditableTextBlock : Control, IVerifyText, IDefaultText
    {
        public const string PART_TextBox = nameof(PART_TextBox);
        static EditableTextBlock()
        {
            PropertyUtils.OverrideDefaultStyleKey<EditableTextBlock>();
            EventManager.RegisterClassHandler(typeof(EditableTextBlock), Mouse.MouseDownEvent, new MouseButtonEventHandler(OnMouseButtonDown), true);
            EventManager.RegisterClassHandler(typeof(EditableTextBlock), Mouse.MouseUpEvent, new MouseButtonEventHandler(OnMouseButtonUp), true);
        }

        public static readonly DependencyProperty TextProperty = TextBlock.TextProperty.AddOwnerTwoWay<string>(typeof(EditableTextBlock), null, OnTextChanged);
        public static readonly DependencyProperty DefaultTextProperty = IDefaultText.DefaultTextProperty.AddOwner(typeof(EditableTextBlock));
        public static readonly DependencyProperty TextTrimmingProperty = TextBlock.TextTrimmingProperty.AddOwner(typeof(EditableTextBlock), TextTrimming.None);
        public static readonly DependencyProperty AcceptsReturnProperty = System.Windows.Controls.Primitives.TextBoxBase.AcceptsReturnProperty.AddOwner(typeof(EditableTextBlock));

        private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is EditableTextBlock s)
            {
                if (Mouse.Captured == s && e.OriginalSource == s)
                {
                    s.Cancel();
                    e.Handled = true;
                }
                else if (e.LeftButton == MouseButtonState.Pressed && !s._isEditing)
                {
                    s.OpenPopup();
                    e.Handled = true;
                }
            }
        }

        private static void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is EditableTextBlock s && Mouse.Captured != s)
            {
                s.CaptureIfNeed();
                e.Handled = true;
            }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EditableTextBlock t)
            {
                var old = t._text;
                t._text = e.NewValue as string;
                t.OnTextChanged(old, t._text);
            }
        }

        public event VerifyTextEventHandler? Verify;

        private string? _text;
        [DependencyProperty(SetterScope = ObjectModel.Scope.Protected)]
        private bool _isEditing;
        [DependencyProperty]
        private string? _decideText;
        [DependencyProperty]
        private string? _cancelText;
        [DependencyProperty]
        private string? _clearText;
        [DependencyProperty]
        private bool _isClearButtonVisible;
        [DependencyProperty(SetterScope = ObjectModel.Scope.Protected)]
        private bool _isTextValid = true;
        private TextBox? _textBox;

        public string? Text { get => _text; set => SetValue(TextProperty, value); }
        public string? DefaultText { get => GetValue(DefaultTextProperty) as string; set => SetValue(DefaultTextProperty, value); }
        public TextTrimming TextTrimming { get => (TextTrimming)GetValue(TextTrimmingProperty); set => SetValue(TextTrimmingProperty, value); }
        public bool AcceptsReturn { get => (bool)GetValue(AcceptsReturnProperty); set => SetValue(AcceptsReturnProperty, value); }

        public EditableTextBlock()
        {
            this.RegisterCommand(EditableTextCommands.Decide, OnExecuted_Decide, CanExecute_Decide);
            this.RegisterCommand(EditableTextCommands.CtrlDecide, OnExecuted_Decide, CanExecute_Decide);
            this.RegisterCommand(EditableTextCommands.Cancel, OnExecuted_Cancel);
            this.RegisterCommand(EditableTextCommands.Clear, OnExecuted_Clear, CanExecute_Clear);
        }

        private void OnIsEditingChanged(bool value)
        {
            Focusable = !value;
        }

        public override void OnApplyTemplate()
        {
            if (_textBox is not null)
            {
                _textBox.TextChanged -= OnTextChanged_TextBox;
                _textBox.LostFocus -= OnLostFocus_TextBox;
            }
            base.OnApplyTemplate();
            _textBox = GetTemplateChild(PART_TextBox) as TextBox;
            if (_textBox is not null)
            {
                _textBox.TextChanged += OnTextChanged_TextBox;
                _textBox.LostFocus += OnLostFocus_TextBox;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.OriginalSource == this)
            {
                if (e.Key is Key.Space)
                {
                    OpenPopup();
                    e.Handled = true;
                }
                else if (Keyboard.Modifiers is 0 or ModifierKeys.Shift && IsTextKey(e.Key))
                {
                    OpenPopup();
                }
            }
        }

        protected virtual bool IsTextKey(Key key) => KeyInput.IsTextKey(key);

        private void OpenPopup()
        {
            if (_textBox is not null)
            {
                _textBox.Text = _text;
                _textBox.SelectAll();
                _textBox.Visibility = Visibility.Visible;
            }
            IsEditing = true;
            OnPopupOpened();
            CaptureIfNeed();
        }

        private void CaptureIfNeed()
        {
            if (_isEditing)
            {
                _textBox?.Focus();
                Mouse.Capture(this, CaptureMode.SubTree);
            }
        }

        private void ClosePopup()
        {
            IsTextValid = true;
            if (_textBox is not null)
            {
                _textBox.Visibility = Visibility.Collapsed;
            }
            IsEditing = false;
            OnPopupClosed();
            ReleaseMouseCapture();
            if (IsKeyboardFocusWithin)
            {
                Focus();
            }
        }

        private void OnTextChanged_TextBox(object sender, TextChangedEventArgs e) => VerifyPrivate((sender as TextBox)!.Text);

        private void OnLostFocus_TextBox(object sender, RoutedEventArgs e)
        {
            if (!IsKeyboardFocusWithin)
            {
                if (IsTextValid)
                {
                    Decide();
                }
                else
                {
                    Cancel();
                }
                e.Handled = true;
            }
        }

        private void Decide() => VerifyAndClose(_textBox?.Text);
        private void Cancel() => ClosePopup();
        private void Clear() => VerifyAndClose(null);

        private bool VerifyPrivate(string? text)
        {
            var flag = (Verify is null || Verify(text)) && VerifyProtected(text);
            IsTextValid = flag;
            return flag;
        }

        protected virtual bool VerifyProtected(string? text) => true;

        private void VerifyAndClose(string? text)
        {
            if (VerifyPrivate(text))
            {
                ApplyText(text);
            }
            ClosePopup();
        }

        protected virtual void ApplyText(string? text)
        {
            var oldValue = _text;
            Text = text;
            RaiseTextChanged(oldValue, text);
        }

        protected virtual void OnTextChanged(string? oldValue, string? newValue) { }

        protected virtual void OnPopupOpened() { }
        protected virtual void OnPopupClosed() { }
    }
}
