using LivreNoirLibrary.IO;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.SandBox
{
    /// <summary>
    /// Unit_Video.xaml の相互作用ロジック
    /// </summary>
    public partial class Unit_Video : UserControl, IProgressReporter
    {
        public FFmpegPlayer Player { get; } = new();

        UIElement IProgressReporter.MainElement => MainUI;
        TaskProgressBar IProgressReporter.ProgressBar => TaskProgressBar;
        Task? IProgressReporter.WorkingTask { get; set; }

        public Unit_Video()
        {
            DataContext = this;
            InitializeComponent();

            this.RegisterCommand(ApplicationCommands.Open, OnExecuted_Open);
            this.RegisterCommand(ApplicationCommands.Save, OnExecuted_Save);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            foreach (var path in e.GetFileList())
            {
                if (ExtRegs.Media.IsMatch(path))
                {
                    Player.Path = path;
                    e.Handled = true;
                    return;
                }
            }
        }

        private void OnExecuted_Open(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            Player.Stop();
            if (this.OpenFileDialog(FileDialogOptions.WithInitialPath(Player.Path), Filters.Media_Open) is { } path)
            {
                Player.Path = path;
            }
        }

        private void OnExecuted_Save(object sender, ExecutedRoutedEventArgs e)
        {
            if (!Player.IsPlayable)
            {
                return;
            }
            Player.Stop();
            var path = Path.ChangeExtension(Player.Path, Exts.MP4);
            if (this.SaveFileDialog(FileDialogOptions.WithInitialPath(Player.Path), Filters.MP4) is { } savePath)
            {
                var player = Player;
                var duration = player.Duration;
                var saveDuration = TextBox_Split.Value * 60;
                player.Position = 0;
                player.SaveDuration = saveDuration;
                var maxCount = (int)(duration / saveDuration) + 1;
                savePath = savePath[..^4];
                this.StartTaskSynchronized((p, c) =>
                {
                    var i = 1;
                    while (duration is > 0)
                    {
                        p?.Report($"Encoding {i}/{maxCount}", null);
                        player.CreateVideo($"{savePath}_{i:D3}.mp4", p, c);
                        duration -= saveDuration;
                        i++;
                    }
                });
            }
        }

        private void OnMouseWheel_VideoSeekbar(object sender, MouseWheelEventArgs e)
        {
            if (sender is Slider slider)
            {
                var value = slider.Value + (e.Delta is > 0 ? 1 : -1) * Player.SecondsPerFrame;
                slider.Value = Math.Clamp(value, 0, Player.Duration);
            }
        }
    }
}
