using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// PackSearchWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class PackSearchWindow : Window, IToggleButtonContainer
    {
        public event EventHandler? Search;

        private PackSearchConditions? _conditions;
        private PackSearchConditions? _defaultConditions;

        public PackSearchConditionsViewModel ViewModel { get; } = new();

        bool IToggleButtonContainer.MousePressed { get; set; }
        bool IToggleButtonContainer.MouseToggleState { get; set; }

        public PackSearchWindow()
        {
            DataContext = ViewModel;
            InitializeComponent();
            this.InitializeIToggleButtonContainer();
            CardSearchWindow.CreateDateContextMenu(DatePicker_Since);
            CardSearchWindow.CreateDateContextMenu(DatePicker_Until);
        }

        public void Setup(PackSearchConditions conditions, PackSearchConditions defaultConditions)
        {
            _conditions = conditions;
            _defaultConditions = defaultConditions;
            ViewModel.CopyFrom(conditions);
            TextBox_Search.Text = ViewModel.SearchText;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void OnClick_Search(object sender, RoutedEventArgs e)
        {
            if (_conditions is { } cond)
            {
                ViewModel.CopyTo(cond);
                Search?.Invoke(this, EventArgs.Empty);
            }
            Close();
            e.Handled = true;
        }

        private void OnClick_Clear(object sender, RoutedEventArgs e)
        {
            ViewModel.CopyFrom(_defaultConditions ?? PackSearchConditions.Default);
            e.Handled = true;
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            _conditions = null;
            Close();
            e.Handled = true;
        }

        private bool SearchText_Verify(string text)
        {
            ViewModel.SearchText = text;
            return ViewModel.IsTextValid;
        }
    }
}
