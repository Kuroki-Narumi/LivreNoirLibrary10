using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.ComponentModel;
using System.Windows;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class CardInfoWindow : Window
    {
        public CardInfoWindow(Card card, Window? owner = null, LinkClickHandlers handlers = default)
        {
            Owner = owner;
            InitializeComponent();
            InfoView.AddLinkClickHandlers(handlers);
            InfoView.Source = card;
            Title = card.Name;
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
