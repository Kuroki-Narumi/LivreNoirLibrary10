using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Windows.Controls;
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
    public partial class MainWindow : Window, IProgressReporter
    {
        private ConsoleWindow? _consoleWindow;

        UIElement IProgressReporter.MainElement => MainUI;
        TaskProgressBar IProgressReporter.ProgressBar => TaskProgressBar;
        Task? IProgressReporter.WorkingTask { get ; set; }

        public MainWindow()
        {
            BmsScreen = new(BmsOptions)
            {
                SkinOptionProvider = this
            };
            BmsVideoCreator = new(BmsScreen, BmsOptions);
            DataContext = this;
            InitializeComponent();
            this.RegisterCommand(ApplicationCommands.Open, OnExecuted_Open);
            this.RegisterCommand(ApplicationCommands.Save, OnExecuted_Save);

            BmsSkins = new();
            BmsSkins.Load(Path.GetFullPath(@"Themes\BmsSkin\", General.GetAssemblyDir()));
            ComboBox_Skin.ItemsSource = BmsSkins.PlaySkins[0];
            ComboBox_Skin.SelectedIndex = AppSettings.Instance.SkinIndex;

            BmsOptions.PropertyChanged += BmsOptions_PropertyChanged;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            AppSettings.Instance.SkinIndex = ComboBox_Skin.SelectedIndex;
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

        private void TabControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as TabControl)?.ChangeByWheel(e);
        }

        private void LabeledSlider_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as Slider)?.ChangeByWheel(e);
        }

        private void ComboBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as ComboBox)?.ChangeByWheel(e);
        }

        private void RadioContainer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as Panel)?.ChangeRadioButtonByWheel(e);
        }
    }
}