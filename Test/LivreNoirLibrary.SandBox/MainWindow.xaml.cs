using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Integrated;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
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
        private readonly BmsVideoCreator _viewModel;
        private ConsoleWindow? _consoleWindow;

        UIElement IProgressReporter.MainElement => MainUI;
        TaskProgressBar IProgressReporter.ProgressBar => TaskProgressBar;
        Task? IProgressReporter.WorkingTask { get ; set; }

        public MainWindow()
        {
            _viewModel = new();
            DataContext = _viewModel;
            InitializeComponent();
            this.RegisterCommand(ApplicationCommands.Open, OnExecuted_Open);
            this.RegisterCommand(ApplicationCommands.Save, OnExecuted_Save);
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
            e.ApplyEffect(DragDropEffects.Copy, ExtRegs.BeMusic);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            foreach (var path in e.EnumAvailable(ExtRegs.BeMusic))
            {
                if (_viewModel.OpenBms(path))
                {
                    e.Handled = true;
                    return;
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
            if (this.OpenFileDialog(FileDialogOptions.WithInitialPath(_viewModel.BmsPath), Filters.Bms) is { } path)
            {
                _viewModel.OpenBms(path);
            }
        }

        private void OnClick_Assemble(object sender, RoutedEventArgs e)
        {
            if (_viewModel.BmsData is not null)
            {
                _viewModel.AssembleOptions.Adjust = true;
                this.StartTask(_viewModel.Assemble, finished: Assemble_Finished);
            }
        }

        private void Assemble_Finished(bool aborted)
        {
            var path = Path.ChangeExtension(_viewModel.BmsPath, Exts.Wav);
            if (_viewModel.TryFlushAssembledData(out var data) &&
                this.SaveFileDialog(FileDialogOptions.WithInitialPath(path), Filters.Wave) is { } savePath)
            {
                data.Save(savePath);
            }
        }

        private void OnExecuted_Save(object sender, ExecutedRoutedEventArgs e)
        {
            if (_viewModel.BmsData is not null)
            {
                _viewModel.AssembleOptions.Adjust = false;
                this.StartTask(_viewModel.Assemble, finished: Construct_Assemble_Finished);
            }
        }

        private void Construct_Assemble_Finished(bool aborted)
        {
            var path = Path.ChangeExtension(_viewModel.BmsPath, Exts.MP4);
            if (_viewModel.TryFlushAssembledData(out var waveData) &&
                this.SaveFileDialog(FileDialogOptions.WithInitialPath(path), Filters.MP4) is { } savePath)
            {
                this.StartTaskSynchronized((p, c) => _viewModel.CreateVideo(savePath, waveData, p, c));
            }
        }
    }
}