using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// FindRelayControl.xaml の相互作用ロジック
    /// </summary>
    public partial class FindRelayControl : UserControl, IToggleButtonContainer
    {
        private static readonly CardSearchConditions _defaultConditions = new(CardSearchConditions.Usable)
        {
            CardTypes = [CardType.Main_Monster, CardType.Ritual_Monster],
        };

        public CardSearchConditions DefaultCardSearchConditions => _defaultConditions;

        public SortedCheckableCardList CheckableList { get; } = [];
        public SortedCardList RelayCardList { get; } = [];

        public bool MousePressed { get; set; }
        public bool MouseToggleState { get; set; }

        [DependencyProperty]
        private ICardEnumerable? _itemsSource;
        [DependencyProperty]
        private ICardProvider? _cardProvider;

        public FindRelayControl()
        {
            InitializeComponent();
            MainGrid.DataContext = this;
            CardClipboard.RegisterCopy(DeckCardListView);
            CardClipboard.RegisterCopy(CardListView);
            this.InitializeIToggleButtonContainer();
        }

        private void OnItemsSourceChanged(ICardEnumerable? oldValue, ICardEnumerable? newValue)
        {
            (oldValue as INotifyCollectionChanged)?.CollectionChanged -= ItemsSource_CollectionChanged;
            CheckableList.Clear();
            if (newValue is not null)
            {
                CheckableList.AddRange(newValue.CardEnumerable.Where(card => card.IsMainDeckMonster()));
            }
            (newValue as INotifyCollectionChanged)?.CollectionChanged += ItemsSource_CollectionChanged;
        }

        private void ItemsSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (ItemsSource is not { } source)
            {
                return;
            }
            using var o = SmallWorld.RentFlagsArray();
            var array = o.Array;
            var list = CheckableList;
            foreach (var card in source.CardEnumerable.Where(card => card.IsMainDeckMonster()))
            {
                BitFlags.Set(array, card.Id);
                list.Add(card);
            }
            list.RemoveAll(Predicate);

            UpdateRelay();
            bool Predicate(int id, CheckableCard card) => !BitFlags.IsSet(array, id);
        }

        private void OnMouseLeftButtonDown_ToggleButton(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement { DataContext: CheckableCard item })
            {
                MouseToggleState = !item.IsChecked;
                MousePressed = true;
                UpdateCheck(item);
                e.Handled = true;
            }
        }

        private void OnMouseEnter_ToggleButton(object sender, MouseEventArgs e)
        {
            if (MousePressed && sender is FrameworkElement { DataContext: CheckableCard item })
            {
                UpdateCheck(item);
            }
        }

        private void UpdateCheck(CheckableCard card)
        {
            if (card.IsChecked != MouseToggleState)
            {
                card.IsChecked = MouseToggleState;
                UpdateRelay();
            }
            DeckCardListView.SelectedItem = card;
        }

        private void UpdateRelay()
        {
            if (CardProvider is { } provider)
            {
                SmallWorld.FindRelay(RelayCardList, CheckableList.Where(c => c.IsChecked).Select(c => c.ThisCard), provider);
            }
        }
    }
}
