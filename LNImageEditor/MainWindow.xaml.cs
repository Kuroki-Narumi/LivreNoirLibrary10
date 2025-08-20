using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace LNImageEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IProgressReporter
    {
        public ImageItemList ImageItems { get; } = [];

        UIElement IProgressReporter.MainElement => MainUI;
        TaskProgressBar IProgressReporter.ProgressBar => TaskProgressBar;
        Task? IProgressReporter.WorkingTask { get; set; }
        Dispatcher IProgressReporter.Dispatcher => Dispatcher;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            ExConsole.PipeFromConsole();
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            e.ApplyEffect(ExtRegs.Image);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            var lastAdded = -1;
            foreach (var path in e.EnumAvailable(ExtRegs.Image))
            {
                if (Path.Exists(path))
                {
                    ImageItems.Add(new(path), out lastAdded);
                }
            }
            ImageList.ProcessSelect(lastAdded);
        }

        private void Image_CanExecute_Delete(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is ListView lv)
            {
                e.CanExecute = lv.SelectedItem is not null;
            }
        }

        private void Image_Executed_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is ListView lv)
            {
                var index = lv.SelectedIndex;
                var item = ImageItems.GetItemAt(index);
                ImageItems.RemoveKey(item.FullPath);
                lv.ProcessSelect(index >= ImageItems.Count ? index - 1 : index);
                e.Handled = true;
            }
        }

        private void OnClick_Clip(object sender, RoutedEventArgs e)
        {
            var expression = "arg == 1 ? max(2, max(1, 3)) : 1 + +1 / (arg - 1)";
            LivreNoirLibrary.Numerics.ReversePolishNotation<int> rpn = new();
            if (rpn.TryParse(expression, out var ex))
            {
                ExConsole.Write(rpn);
                Dictionary<string, int> dic = new() { ["arg"] = 1 };
                void Try()
                {
                    if (rpn.TryEvaluate(dic, out var result))
                    {
                        ExConsole.Write($"args:{dic.ToListString()} result={result}");
                    }
                    else
                    {
                        ExConsole.Write($"evaluate failed");
                    }
                    if (rpn.TryLazyEvaluate(dic, out result))
                    {
                        ExConsole.Write($"args:{dic.ToListString()} result={result}");
                    }
                    else
                    {
                        ExConsole.Write($"evaluate failed");
                    }
                }
                Try();
                dic["arg"] = 2;
                Try();
            }
            else
            {
                ExConsole.Write($"RPN parse failed: {ex}");
            }

            if (ImageList.SelectedItem is ImageItem item)
            {
                var rect = ImageRectSelector.GetRect();
                if (item.Image.GetRect() != rect && this.SaveFileDialog(FileDialogOptions.WithInitialPath(item.FullPath), Filters.Image_Save) is string path)
                {
                    CroppedBitmap cropped = new(item.Image, rect);
                    cropped.SaveImage(path, BitmapEncodeType.Auto);
                    if (path == item.FullPath)
                    {
                        item.Reload();
                    }
                    WriteableBitmap bitmap = new(cropped);
                    using (var s = ExStopwatch.SaveProcessTime(path))
                    {
                        bitmap.DrawBorder(16, Color.FromArgb(128, 255, 0, 128), true);
                    }
                    bitmap.SaveImage(path.Replace(".png", "_2.png"), BitmapEncodeType.Auto);
                }
            }
        }

        private void OnClick_AutoClip(object sender, RoutedEventArgs e)
        {
            if (this.ShowMessage_YesNo("全ての画像を上書きしてよろしいですか？") is MessageBoxResult.Yes)
            {
                var max = ImageItems.Count;
                this.StartTask(new()
                {
                    InitialReport = ProgressReport.Initial("画像を自動切り抜きしています..."),
                    MainProcess = (p, c) =>
                    {
                        var i = 1;
                        foreach (var item in ImageItems)
                        {
                            c.ThrowIfCancellationRequested();
                            p.Report($"{item.Filename}: 境界の検出");
                            var rect = item.Image.GetOpaqueRect();
                            if (rect != item.Image.GetRect())
                            {
                                CroppedBitmap cropped = new(item.Image, rect);
                                c.ThrowIfCancellationRequested();
                                p.Report($"{item.Filename}: 保存中...", i, max);
                                cropped.SaveImage(item.FullPath, BitmapEncodeType.PNG);
                            }
                            i++;
                        }
                    },
                    Finished = a => ImageItems.Reload(),
                });
            }
        }
    }
}