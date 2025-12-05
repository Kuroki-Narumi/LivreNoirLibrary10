using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Controls.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.SandBox
{
    public partial class MainWindow
    {
        private readonly BmsVideoCreator _creator;
        private readonly SkinCollection _skins;

        public static readonly FixedHighSpeedMode[] FixedHighSpeedModeList =
        [
            FixedHighSpeedMode.None, FixedHighSpeedMode.Max, FixedHighSpeedMode.Min, FixedHighSpeedMode.Main, FixedHighSpeedMode.MainTime
        ];


        private void OnDragOver_Bms(object sender, DragEventArgs e)
        {
            //e.ApplyEffect(DragDropEffects.Copy, ExtRegs.BeMusic);
            e.Effects = DragDropEffects.Copy;
        }

        private void OnDrop_Bms(object sender, DragEventArgs e)
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
            if (_creator.Screen is { IsBmsReady: true, ViewModel.Data: BmsData data } &&
                this.SaveFileDialog(FileDialogOptions.WithInitialPath(_creator.Screen.BmsPath), Filters.Bms_Save) is { } path)
            {
                data.Save(path, false, true);
            }
        }

        private void OnClick_Assemble(object sender, RoutedEventArgs e)
        {
            _creator.AssembleOptions.AdjustBeginning = true;
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
            _creator.AssembleOptions.AdjustBeginning = false;
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
