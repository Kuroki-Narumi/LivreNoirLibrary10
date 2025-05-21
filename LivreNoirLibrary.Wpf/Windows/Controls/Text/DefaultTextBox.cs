using System;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    /// <summary>
    /// DefaultTextBox.xaml の相互作用ロジック
    /// </summary>
    public partial class DefaultTextBox : TextBox, IVerifyText, IDefaultText
    {
        static DefaultTextBox()
        {
            PropertyUtils.OverrideDefaultStyleKey<DefaultTextBox>();
        }

        public event VerifyTextEventHandler? Verify;

        public static readonly DependencyProperty DefaultTextProperty = IDefaultText.DefaultTextProperty.AddOwner(typeof(DefaultTextBox));

        [DependencyProperty(SetterScope = ObjectModel.Scope.Protected)]
        private bool _isTextValid = true;

        public string? DefaultText { get => GetValue(DefaultTextProperty) as string; set => SetValue(DefaultTextProperty, value); }

        public void ForceVerify()
        {
            var text = Text;
            IsTextValid = (Verify is null || Verify(text)) && OnVerify(text);
        }

        protected virtual bool OnVerify(string? text) => true;

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            ForceVerify();
            base.OnTextChanged(e);
        }
    }
}
