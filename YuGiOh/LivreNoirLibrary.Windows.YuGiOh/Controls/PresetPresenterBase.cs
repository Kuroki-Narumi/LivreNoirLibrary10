using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using P = System.Windows.Controls.Primitives;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract partial class PresetPresenterBase : Control
    {
        public const string PART_Name = nameof(PART_Name);

        public static RoutedCommand ApplyCommand { get; } = Commands.Create();
        public static RoutedCommand SetToDefaultCommand { get; } = Commands.Create();
        public static RoutedCommand DefaultChangedCommand { get; } = Commands.Create();

        static PresetPresenterBase()
        {
            PropertyUtils.OverrideDefaultStyleKey<PresetPresenterBase>();
        }

        private TextBox? _textBox;

        public PresetPresenterBase()
        {
            this.RegisterCommand(ApplyCommand, Executed_Apply);
            this.RegisterCommand(SetToDefaultCommand, Executed_Default);
        }

        public override void OnApplyTemplate()
        {
            _textBox?.GotFocus -= OnGotFocus_TextBox;
            base.OnApplyTemplate();
            _textBox = GetTemplateChild(PART_Name) as TextBox;
            _textBox?.GotFocus += OnGotFocus_TextBox;
        }

        private void OnGotFocus_TextBox(object sender, RoutedEventArgs e)
        {
            Select(sender);
        }

        private void Executed_Apply(object sender, ExecutedRoutedEventArgs e)
        {
            Select(sender);
            TryRaiseApplyEvent();
            e.Handled = true;
        }

        protected abstract void TryRaiseApplyEvent();

        private void Executed_Default(object sender, ExecutedRoutedEventArgs e)
        {
            Select(sender);
            if ((KeyInput.IsAltDown() || KeyInput.IsCtrlDown()) && e.OriginalSource is P.ToggleButton t)
            {
                t.IsChecked = false;
            }
            DefaultChangedCommand.Execute(DataContext, this);
            e.Handled = true;
        }

        private static void Select(object sender)
        {
            if ((sender as DependencyObject).TryGetAncestor<ListViewItem>(out var i))
            {
                i.IsSelected = true;
                if (!i.IsKeyboardFocusWithin)
                {
                    i.Focus();
                }
            }
        }
    }
}
