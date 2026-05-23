using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Media;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.SandBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConsoleWindow? _consoleWindow;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            AppSettings.Instance.SkinIndex = Unit_Bms?.ComboBox_Skin?.SelectedIndex ?? 0;
            AppSettings.Save();
            base.OnClosing(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (_consoleWindow is null)
            {
                _consoleWindow = new()
                {
                    Owner = this,
                    Left = Left + ActualWidth,
                    Top = Top,
                    ShowInTaskbar = false,
                };
                _consoleWindow.Show();
            }
        }

        private void OnClick_TestOut(object sender, RoutedEventArgs e)
        {
            if (this.SaveFileDialog(null, Filters.Wave) is { } path)
            {
                SineWave.Generate(path, 20, 20000);
            }
        }
    }
}