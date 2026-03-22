using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;
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

namespace LivreNoirLibrary.Benchmark
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel = new();

        public MainWindow()
        {
            DataContext = _viewModel;
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
        }

        private static void RangeSetTest()
        {
            RangeSet<int> set = [];
            void MatchTest(string text)
            {
                Console.WriteLine($"MatchTest text: \"{text}\"");
                if (BasedNumber.TryParseRangeSet(text, set, 10))
                {
                    Console.Write($"  Parse successed: ");
                    foreach (var range in set)
                    {
                        Console.Write($"{range}, ");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"  Parse failed!");
                }
            }

            ReadOnlySpan<string> texts = ["1 2 3 5 6 7", "1-6 3,4", "-9 4-15", "36-5 3 6 9", "1 5 2 4 3", "6 6 6 9..2"];
            foreach (var text in texts)
            {
                MatchTest(text);
            }
        }

        private void OnClick_Float_Plus(object sender, RoutedEventArgs e) => _viewModel.FloatApplyDelta(true, 1);
        private void OnClick_Float_Minus(object sender, RoutedEventArgs e) => _viewModel.FloatApplyDelta(false, 1);
        private void OnClick_Float_Plus100(object sender, RoutedEventArgs e) => _viewModel.FloatApplyDelta(true, 100);
        private void OnClick_Float_Minus100(object sender, RoutedEventArgs e) => _viewModel.FloatApplyDelta(false, 100);
        private void OnClick_Float_Plus10000(object sender, RoutedEventArgs e) => _viewModel.FloatApplyDelta(true, 10000);
        private void OnClick_Float_Minus10000(object sender, RoutedEventArgs e) => _viewModel.FloatApplyDelta(false, 10000);
    }

    public readonly record struct Rect(int X, int Y, int Width, int Height);
}