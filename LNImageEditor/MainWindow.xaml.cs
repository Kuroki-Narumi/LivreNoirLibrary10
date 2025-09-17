using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Controls;

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

        private void Calc9D6(int target)
        {
            var span = (stackalloc int[6]);
            span = [1, 2, 3, 4, 5, 6];
            foreach (var a in span)
            {
                var a_total = a * a; // ^2
                a_total *= a_total; // ^4
                a_total *= a_total; // ^8
                a_total *= a; // ^9
                if (a_total > target)
                {
                    break;
                }
                if (a_total + 2015538 < target)
                {
                    continue;
                }
                foreach (var b in span)
                {
                    var b_total = b * b; // ^2
                    b_total *= b_total; // ^4
                    b_total *= b_total; // ^8
                    b_total += a_total;
                    if (b_total > target)
                    {
                        break;
                    }
                    if (b_total + 335922 < target)
                    {
                        continue;
                    }
                    foreach (var c in span)
                    {
                        var c_total = c * c; // ^2
                        c_total *= c_total * c_total * c; // ^7
                        c_total += b_total;
                        if (c_total > target)
                        {
                            break;
                        }
                        if (c_total + 55986 < target)
                        {
                            continue;
                        }
                        foreach (var d in span)
                        {
                            var d_total = d * d; // ^2
                            d_total *= d_total * d_total; // ^6
                            d_total += c_total;
                            if (d_total > target)
                            {
                                break;
                            }
                            if (d_total + 9330 < target)
                            {
                                continue;
                            }
                            foreach (var e in span)
                            {
                                var e_total = e * e; // ^2
                                e_total *= e_total; // ^4
                                e_total *= e; // ^5
                                e_total += d_total;
                                if (e_total > target)
                                {
                                    break;
                                }
                                if (e_total + 1554 < target)
                                {
                                    continue;
                                }
                                foreach (var f in span)
                                {
                                    var f_total = f * f; // ^2
                                    f_total *= f_total; // ^4
                                    f_total += e_total;
                                    if (f_total > target)
                                    {
                                        break;
                                    }
                                    if (f_total + 258 < target)
                                    {
                                        continue;
                                    }
                                    foreach (var g in span)
                                    {
                                        var g_total = g * g * g + f_total;
                                        if (g_total > target)
                                        {
                                            break;
                                        }
                                        if (g_total + 42 < target)
                                        {
                                            continue;
                                        }
                                        foreach (var h in span)
                                        {
                                            var h_total = h * h + g_total;
                                            if (h_total > target)
                                            {
                                                break;
                                            }
                                            if (h_total + 6 < target)
                                            {
                                                continue;
                                            }
                                            var i = target - h_total;
                                            ExConsole.Write($"target={target}, resol=[{i}, {h}, {g}, {f}, {e}, {d}, {c}, {b}, {a}]");
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            ExConsole.Write($"target={target}, not resolved");
        }

        private void OnClick_Clip(object sender, RoutedEventArgs e)
        {
            Calc9D6(10);
            Calc9D6(100);
            Calc9D6(1000);
            Calc9D6(10000);
            Calc9D6(100000);
            Calc9D6(1000000);
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
                    return;
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