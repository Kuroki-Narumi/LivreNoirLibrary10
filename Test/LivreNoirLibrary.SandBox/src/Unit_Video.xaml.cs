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
using Button = LivreNoirLibrary.Windows.Controls.Button;

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
            if (Player.IsPlaying)
            {
                return;
            }
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
            if (Player.IsPlaying)
            {
                return;
            }
            if (this.OpenFileDialog(FileDialogOptions.WithInitialPath(Player.Path), Filters.Media_Open) is { } path)
            {
                Player.Path = path;
            }
        }

        private void OnExecuted_Save(object sender, ExecutedRoutedEventArgs e)
        {
            if (Player.IsPlaying || !Player.IsPlayable)
            {
                return;
            }
            var path = Path.ChangeExtension(Player.Path, Exts.MP4);
            if (this.SaveFileDialog(FileDialogOptions.WithInitialPath(Player.Path), Filters.MP4) is { } savePath)
            {
                this.StartTaskSynchronized((p, c) => Player.CreateVideo(savePath, p, c));
            }
        }

        private void OnClick_Play(object sender, RoutedEventArgs e)
        {
            if (!Player.IsPlaying)
            {
                Player.Play();
                (sender as Button)?.Icon = Icons.Stop;
            }
            else
            {
                Player.Stop();
                (sender as Button)?.Icon = Icons.Play;
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
