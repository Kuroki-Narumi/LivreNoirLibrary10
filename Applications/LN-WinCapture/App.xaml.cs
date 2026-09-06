using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using LivreNoirLibrary.Windows.Controls;

namespace LivreNoir.WinCapture
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void TabControl_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            (sender as TabControl)?.ChangeByWheel(e);
        }

        private void RadioContainer_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            (sender as Panel)?.ChangeRadioButtonByWheel(e);
        }
    }

}
