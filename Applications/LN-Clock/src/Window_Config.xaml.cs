using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LivreNoirLibrary.Windows.Controls;

namespace LivreNoir.Clock
{
    /// <summary>
    /// Window_Config.xaml の相互作用ロジック
    /// </summary>
    public partial class Window_Config : FollowOwnerWindow
    {
        public Window_Config()
        {
            DataContext = MainViewModel.Instance;
            InitializeComponent();
        }

        private void OnMouseWheel_ComboBox(object sender, MouseWheelEventArgs e)
        {
            (sender as ComboBox)?.ChangeByWheel(e, true);
        }

        private void OnClick_Update(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow)?.ResetTimer();
        }

        private void OnClick_Reset(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.SetDefault();
        }
    }
}
