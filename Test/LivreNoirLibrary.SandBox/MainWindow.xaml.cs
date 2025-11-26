using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Integrated;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LivreNoirLibrary.SandBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IProgressReporter
    {
        private ConsoleWindow? _consoleWindow;

        UIElement IProgressReporter.MainElement => MainUI;
        TaskProgressBar IProgressReporter.ProgressBar => TaskProgressBar;
        Task? IProgressReporter.WorkingTask { get ; set; }

        public MainWindow()
        {
            _creator = new();
            DataContext = _creator;
            InitializeComponent();
            this.RegisterCommand(ApplicationCommands.Open, OnExecuted_Open);
            this.RegisterCommand(ApplicationCommands.Save, OnExecuted_Save);

            _skins = new();
            _skins.Load(Path.GetFullPath(@"Themes\BmsSkin\Default\", IO.General.GetAssemblyDir()));
            if (_skins.PlaySkins[7] is { } enumer)
            {
                _creator.LoadSkin(enumer.First());
            }
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

        private void LabeledSlider_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as Slider)?.ChangeByWheel(e);
        }

        private void ComboBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as ComboBox)?.ChangeByWheel(e);
        }
    }
}