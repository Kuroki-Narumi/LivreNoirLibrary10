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

        private readonly BmsVideoCreator _creator;
        private readonly SkinCollection _skins;

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
            var s = _creator.Screen.BgaImageSource;
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

        protected override void OnDragOver(DragEventArgs e)
        {
            //e.ApplyEffect(DragDropEffects.Copy, ExtRegs.BeMusic);
            e.Effects = DragDropEffects.Copy;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            foreach (var path in e.GetFileList())
            {
                if (ExtRegs.BeMusic.IsMatch(path) && _creator.OpenBms(path))
                {
                    e.Handled = true;
                    return;
                }
                if (ExtRegs.Audio.IsMatch(path))
                {
                    var waveData = WaveBuffer.AutoOpen(path);
                    var peak = waveData.GetPeak();
                    var rms = waveData.GetRms();
                    var lufs = waveData.GetLufs();
                    Console.WriteLine($"peak={WaveBuffer.Value2Level(peak):0.###}dB, rms={WaveBuffer.Value2Level(rms):0.###}dB, lufs={lufs:0.###}dB");
                }
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

        private void OnExecuted_Open(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (this.OpenFileDialog(FileDialogOptions.WithInitialPath(_creator.Screen.BmsPath), Filters.Bms) is { } path)
            {
                _creator.OpenBms(path);
            }
        }

        private void OnExecuted_Save(object sender, ExecutedRoutedEventArgs e)
        {
            if (_creator.Screen.BmsData is { } data &&
                this.SaveFileDialog(FileDialogOptions.WithInitialPath(_creator.Screen.BmsPath), Filters.Bms_Save) is { } path)
            {
                data.Root.Save(path, false, true);
            }
        }

        private void OnClick_Assemble(object sender, RoutedEventArgs e)
        {
            _creator.AssembleOptions.Adjust = true;
            this.StartTask(_creator.Assemble, finished: Assemble_Finished);
        }

        private void Assemble_Finished(bool aborted)
        {
            var path = Path.ChangeExtension(_creator.Screen.BmsPath, Exts.Wav);
            if (_creator.TryFlushAssembledData(out var data) &&
                this.SaveFileDialog(FileDialogOptions.WithInitialPath(path), Filters.Wave) is { } savePath)
            {
                data.Save(savePath);
            }
        }

        private void OnClick_Video(object sender, RoutedEventArgs e)
        {
            _creator.AssembleOptions.Adjust = false;
            this.StartTask(_creator.Assemble, finished: Construct_Assemble_Finished);
        }

        private void Construct_Assemble_Finished(bool aborted)
        {
            var path = Path.ChangeExtension(_creator.Screen.BmsPath, Exts.MP4);
            if (_creator.TryFlushAssembledData(out var waveData) &&
                this.SaveFileDialog(FileDialogOptions.WithInitialPath(path), Filters.MP4) is { } savePath)
            {
                this.StartTaskSynchronized((p, c) => _creator.CreateVideo(savePath, waveData, p, c));
            }
        }
    }
}