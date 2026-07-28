using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class CardInfoWindow : Window
    {
        public int ReferenceId { get; }

        public CardInfoWindow(Card card, Window? owner = null)
        {
            ReferenceId = card.Id;
            Owner = owner;
            InitializeComponent();
            InfoView.Source = card;
            Title = card.Name;
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnRequestOpenCardUrl(object sender, CardLinkClickedEventArgs e)
        {
            e.Handled = true;
            this.OpenUrl_Card(e.Id, e.IsTcg);
        }
    }
}
