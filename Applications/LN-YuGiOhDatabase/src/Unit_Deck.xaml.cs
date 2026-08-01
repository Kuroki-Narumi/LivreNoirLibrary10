using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace LivreNoir.YuGiOhDatabase
{
    /// <summary>
    /// Unit_Deck.xaml の相互作用ロジック
    /// </summary>
    public partial class Unit_Deck : UserControl, IProgressReporter
    {
        UIElement IProgressReporter.MainElement => MainUI;
        TaskProgressBar IProgressReporter.ProgressBar => TaskProgressBar;
        Task? IProgressReporter.WorkingTask { get; set; }

        public Unit_Deck()
        {
            InitializeComponent();

            this.RegisterCommand(YgoCommands.AddToDeck, OnExecuted_AddToDeck, CanExecute_AddToDeck);
            this.RegisterCommand(YgoCommands.RemoveFromDeck, OnExecuted_RemoveFromDeck, CanExecute_RemoveFromDeck);

            HandTestView.ProgressReporter = this;
        }

        private void CanExecute_AddToDeck(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter is ICard c && MainViewModel.Instance.Deck.MainDeck.CanAdd(c.ThisCard);
        }

        private void CanExecute_RemoveFromDeck(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = e.Parameter is ICard c && MainViewModel.Instance.Deck.MainDeck.CanRemove(c.ThisCard);
        }

        private void OnExecuted_AddToDeck(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is ICard c)
            {
                DeckEditor.AddCard(c.ThisCard, false, false);
            }
        }

        private void OnExecuted_RemoveFromDeck(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is ICard c)
            {
                DeckEditor.RemoveCard(c.ThisCard, true, false);
            }
        }
    }
}
