using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Sandbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Windows.Input.InputManager.Initialize();
            VirtualKeyboard.Pressed += VirtualKeyboard_Pressed;
        }

        private void VirtualKeyboard_Pressed(Key key, ModifierKeys modifier)
        {
            //Console.WriteLine($"{key}, {modifier}");
        }

        private void OnDragOver_Image(object sender, DragEventArgs e)
        {
            e.ApplyEffect(Files.ExtRegs.Image);
        }

        private void OnDrop_Image(object sender, DragEventArgs e)
        {
            if (e.TryGetAvailable(out var path, Files.ExtRegs.Image))
            {
                BitmapImage bitmap = new(new Uri(path));
                ImageRectSelector.Source = bitmap;
                ImageRectSelector.InitialRect = new(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
            }
        }

        private void OnClick_NewWindow(object sender, RoutedEventArgs e)
        {
            SubWindow1 window = new() { Owner = this };
            window.Show();
        }

        private void RangeSlider_ValueChanged(object sender, Windows.Controls.RangeSliderValueChangedEventArgs e)
        {
            Console.WriteLine($"Value Changed: Value1={e.Value1}, Value2={e.Value2}");
        }
    }
}