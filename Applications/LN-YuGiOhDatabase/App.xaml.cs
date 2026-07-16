using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh.Data;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoir.YuGiOhDatabase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void TabControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as TabControl)?.ChangeByWheel(e);
        }

        private void LabeledSlider_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as Slider)?.ChangeByWheel(e);
        }

        private void ComboBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as ComboBox)?.ChangeByWheel(e);
        }

        private void RadioContainer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as Panel)?.ChangeRadioButtonByWheel(e);
        }

        private void CardListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (KeyInput.IsShiftDown() && sender is ListViewItem { DataContext: ICard c })
            {
                try
                {
                    Clipboard.SetText(c.NameWithBracket());
                }
                catch { }
            }
        }

        private void CardListView_PreviewMouseLeftButtonDown_Alt(object sender, MouseButtonEventArgs e)
        {
            if (KeyInput.IsAltDown() && sender is ListViewItem { DataContext: ICard c })
            {
                try
                {
                    Clipboard.SetText(c.NameWithBracket());
                }
                catch { }
            }
        }
    }

}
