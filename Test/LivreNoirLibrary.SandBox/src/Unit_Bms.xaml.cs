using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Controls.Bms;
using LivreNoirLibrary.Windows.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Button = LivreNoirLibrary.Windows.Controls.Button;
using LivreNoirLibrary.Windows.Media;

namespace LivreNoirLibrary.SandBox
{
    /// <summary>
    /// Unit_Bms.xaml の相互作用ロジック
    /// </summary>
    public partial class Unit_Bms : UserControl, IProgressReporter, ISkinOptionProvider
    {
        public static BmsVideoCreatorOptions BmsOptions => AppSettings.Instance.BmsVideoCreatorOptions;

        public static HsCorrectionMode[] HsCorrectionModes { get; } =
        [
            HsCorrectionMode.None,
            HsCorrectionMode.MaxBpm,
            HsCorrectionMode.MinBpm,
            HsCorrectionMode.AverageBpm,
            HsCorrectionMode.MainBpm,
            HsCorrectionMode.MainTimeBpm,
        ];

        public static Rational[] FpsList { get; } = [FrameRates.Fps24, FrameRates.Fps30, FrameRates.Fps60, FrameRates.Fps120];
        public static int[] SampleRateList { get; } = [22050, 24000, 44100, 48000, 96000];

        public BmsScreen BmsScreen { get; }
        public BmsVideoCreator BmsVideoCreator { get; }
        public SkinCollection BmsSkins { get; }

        UIElement IProgressReporter.MainElement => MainUI;
        TaskProgressBar IProgressReporter.ProgressBar => TaskProgressBar;
        Task? IProgressReporter.WorkingTask { get; set; }

        IDictionary<string, string>? ISkinOptionProvider.GetSkinOptions(Skin? skin) => skin?.Name is { } name ? AppSettings.Instance.BmsSkinOptions.GetOrAdd(name) : null;

        public Unit_Bms()
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

        private void OnClick_SkinOptions(object sender, RoutedEventArgs e)
        {
            if (BmsVideoCreator.IsPlaying)
            {
                return;
            }
            SkinOptionView.Open(BmsScreen.Skin, BmsScreen.SkinOptions);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            if (BmsVideoCreator.IsPlaying)
            {
                return;
            }
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
            if (BmsVideoCreator.IsPlaying)
            {
                return;
            }
            if (this.OpenFileDialog(FileDialogOptions.WithInitialPath(BmsScreen.BmsPath), Filters.Bms) is { } path)
            {
                BmsScreen.OpenBms(path);
            }
        }

        private void OnExecuted_Save(object sender, ExecutedRoutedEventArgs e)
        {
            if (BmsVideoCreator.IsPlaying)
            {
                return;
            }
            if (BmsScreen is { IsBmsReady: true, ViewModel.Data: BmsData data } &&
                this.SaveFileDialog(FileDialogOptions.WithInitialPath(BmsScreen.BmsPath), Filters.Bms_Save) is { } path)
            {
                data.Save(path, false, true);
            }
        }

        private void OnClick_Assemble(object sender, RoutedEventArgs e)
        {
            if (!BmsVideoCreator.IsPlaying && BmsScreen.IsBmsReady)
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
            if (!BmsVideoCreator.IsPlaying && BmsScreen.IsBmsReady)
            {
                var path = Path.ChangeExtension(BmsScreen.BmsPath, Exts.MP4);
                if (this.SaveFileDialog(FileDialogOptions.WithInitialPath(path), Filters.MP4) is { } savePath)
                {
                    BmsScreen.ShowDebugText = true;
                    this.StartTaskSynchronized((p, c) => BmsVideoCreator.CreateVideo(savePath, p, c));
                }
            }
        }

        private void OnClick_Play(object sender, RoutedEventArgs e)
        {
            if (BmsScreen.IsBmsReady)
            {
                if (!BmsVideoCreator.IsPlaying)
                {
                    BmsScreen.ShowDebugText = false;
                    var isReady = false;
                    this.StartTaskSynchronized((p, c) => isReady = BmsVideoCreator.SetupRealTimePlay(p, c));
                    if (isReady)
                    {
                        BmsVideoCreator.StartRealTimePlay();
                        (sender as Button)?.Icon = Icons.Stop;
                    }
                }
                else
                {
                    BmsVideoCreator.StopRealTimePlay();
                    (sender as Button)?.Icon = Icons.Play;
                }
            }
        }

        private void BmsOptions_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (BmsVideoCreator.IsPlaying)
            {
                var options = BmsOptions;
                var composer = BmsScreen.AudioComposer;
                switch (e.PropertyName)
                {
                    case nameof(BmsPlayOptions.MasterVolume):
                        composer.MasterVolume = options.MasterVolume;
                        break;
                    case nameof(BmsPlayOptions.KeyVolume):
                        composer.TagToVolume[BgmTimeline.Tag_KeySound] = options.KeyVolume;
                        break;
                    case nameof(BmsPlayOptions.BgmVolume):
                        composer.TagToVolume[BgmTimeline.Tag_BgmSound] = options.BgmVolume;
                        break;
                }
            }
        }
    }
}
