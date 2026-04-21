using LivreNoirLibrary.Windows.Media;
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

namespace LivreNoirLibrary.GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel = new();

        public MainWindow()
        {
            InitializeComponent();
            MainImage.Source = _viewModel.FieldBitmap;
            _viewModel.UpdateBitmap();
        }

        private void OnMouseDown_MainImage(object sender, MouseButtonEventArgs e) => _viewModel.StartDrawing(MainImage, e);

        private void OnClick_Start(object sender, RoutedEventArgs e) => _viewModel.Start();
        private void OnClick_Stop(object sender, RoutedEventArgs e) => _viewModel.Stop();
        private void OnClick_Clear(object sender, RoutedEventArgs e) => _viewModel.ClearField();
    }
}