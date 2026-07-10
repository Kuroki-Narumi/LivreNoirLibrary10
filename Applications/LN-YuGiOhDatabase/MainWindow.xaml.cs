using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.YuGiOh.Scraping;
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

namespace LivreNoir.YuGiOhDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IUpdateCheck
    {
        public static MainViewModel ViewModel => MainViewModel.Instance;

        bool IUpdateCheck.CheckUpdate { get => ViewModel.CheckUpdate; set => ViewModel.CheckUpdate = value; }
        string IUpdateCheck.VersionUrl => "";
        string IUpdateCheck.SettingName => MainViewModel.AppName;

        public MainWindow()
        {
            DataContext = ViewModel;
            InitializeComponent();
            TextBlock_Version.Text = $"Version {ViewModel.Version.ToStringAuto()}";
        }

        private void OnClick_OfficialDatabase(object sender, RoutedEventArgs e)
        {
            this.ShellOpen(Url.BaseUrl);
            e.Handled = true;
        }

        private void OnClick_Help(object sender, RoutedEventArgs e)
        {
            Area_Help.Open(true);
            e.Handled = true;
        }

        private void OnClick_Help_Manual(object sender, RoutedEventArgs e)
        {

        }

        private void OnClick_Help_Update(object sender, RoutedEventArgs e)
        {

        }
    }
}