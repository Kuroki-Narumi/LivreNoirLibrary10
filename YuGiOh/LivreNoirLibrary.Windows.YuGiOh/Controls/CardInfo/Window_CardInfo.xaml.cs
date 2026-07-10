using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.ComponentModel;
using System.Windows;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// Window_CardInfo.xaml の相互作用ロジック
    /// </summary>
    public partial class Window_CardInfo : Window
    {
        public Window_CardInfo(Card card, Window? owner = null, LinkClickHandlers handlers = default)
        {
            Owner = owner;
            InitializeComponent();
            InfoView.AddLinkClickHandlers(handlers);
            InfoView.Source = card;
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
