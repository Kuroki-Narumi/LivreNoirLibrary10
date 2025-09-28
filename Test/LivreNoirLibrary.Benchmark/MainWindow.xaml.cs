using System.Buffers.Binary;
using System.Runtime.Intrinsics;
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
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Benchmark
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private unsafe void OnClick_Color(object sender, RoutedEventArgs e)
        {
            var vector = Vector128.Create(1f, 2f, 3f, 4f);
            var condition = Vector128.Create(2f);
            var result = Vector128.LessThanOrEqual(vector, condition);
            Console.WriteLine($"vector={vector}, condition={condition}, result={result}");

            Rect rect = new(1, 2, 3, 4);
            Console.WriteLine(*(Vector128<int>*)&rect);

            return;
            Console.WriteLine($"sizeof(Color) = {sizeof(Color)}");
            var color = Color.FromArgb(1, 2, 3, 4);
            var ptr = (byte*)&color;
            var span = new Span<byte>(ptr, 20); // ColorContext?
            var sc1 = *(float*)(ptr + 20);
            var sc2 = *(float*)(ptr + 24);
            var sc3 = *(float*)(ptr + 28);
            var sc4 = *(float*)(ptr + 32);
            var c1 = ptr[36];
            var c2 = ptr[37];
            var c3 = ptr[38];
            var c4 = ptr[39];
            Console.WriteLine($"Color = {color}");
            Console.WriteLine($"Sc = {sc1}, {sc2}, {sc3}, {sc4}");
            Console.WriteLine($"C = {c1}, {c2}, {c3}, {c4}");
        }
    }

    public readonly record struct Rect(int X, int Y, int Width, int Height);
}