using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Windows.Controls;

namespace LivreNoir.YuGiOhDatabase
{
    /// <summary>
    /// Unit_Deck.xaml の相互作用ロジック
    /// </summary>
    public partial class Unit_Deck : UserControl
    {
        public Unit_Deck()
        {
            InitializeComponent();
            DeckEditor.CardProvider = new CloningCardList(MainViewModel.Instance.CardPool.Cards);
        }
    }
}
