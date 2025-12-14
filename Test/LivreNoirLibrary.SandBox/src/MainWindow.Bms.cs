using FFmpeg.AutoGen;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Controls.Bms;
using LivreNoirLibrary.Windows.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.SandBox
{
    public partial class MainWindow
    {
        public BmsVideoCreateOptions BmsOptions { get; }
        public BmsScreen BmsScreen { get; }
        public BmsVideoCreator BmsVideoCreator { get; }
        public SkinCollection BmsSkins { get; }

        public static HsCorrectionMode[] HsCorrectionModes => BmsExtensions.HsCorrectionModes;
        public static Rational[] FpsList { get; } = [FrameRates.Fps24, FrameRates.Fps30, FrameRates.Fps60, FrameRates.Fps120, FrameRates.Fps144];

        private void OnClick_SkinOptions(object sender, RoutedEventArgs e)
        {
            SkinOptionView.Open(BmsScreen.Skin!);
        }

        private void OnDragOver_Bms(object sender, DragEventArgs e)
        {
            //e.ApplyEffect(DragDropEffects.Copy, ExtRegs.BeMusic);
            e.Effects = DragDropEffects.Copy;
        }

        private void OnDrop_Bms(object sender, DragEventArgs e)
        {
            foreach (var path in e.GetFileList())
            {
                if (ExtRegs.BeMusic.IsMatch(path) && BmsScreen.OpenBms(path))
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
            if (this.OpenFileDialog(FileDialogOptions.WithInitialPath(BmsScreen.BmsPath), Filters.Bms) is { } path)
            {
                BmsScreen.OpenBms(path);
            }
        }

        private void OnExecuted_Save(object sender, ExecutedRoutedEventArgs e)
        {
            if (BmsScreen is { IsBmsReady: true, ViewModel.Data: BmsData data } &&
                this.SaveFileDialog(FileDialogOptions.WithInitialPath(BmsScreen.BmsPath), Filters.Bms_Save) is { } path)
            {
                data.Save(path, false, true);
            }
        }

        private void OnClick_Assemble(object sender, RoutedEventArgs e)
        {
            if (BmsScreen.IsBmsReady)
            {
                var path = Path.ChangeExtension(BmsScreen.BmsPath, Exts.Wav);
                if (this.SaveFileDialog(FileDialogOptions.WithInitialPath(path), Filters.Wave) is { } savePath)
                {
                    this.StartTask((p, c) => BmsVideoCreator.Assemble(savePath, p, c));
                }
            }
        }

        private void OnClick_Video(object sender, RoutedEventArgs e)
        {
            if (BmsScreen.IsBmsReady)
            {
                var path = Path.ChangeExtension(BmsScreen.BmsPath, Exts.MP4);
                if (this.SaveFileDialog(FileDialogOptions.WithInitialPath(path), Filters.MP4) is { } savePath)
                {
                    this.StartTaskSynchronized((p, c) => BmsVideoCreator.CreateVideo(savePath, p, c));
                }
            }
        }
    }
}
