using LivreNoirLibrary.IO;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// CardTableView.xaml の相互作用ロジック
    /// </summary>
    public partial class CardTableView : SaveImageBase, ICardSearch
    {
        public static CardSelector[] Selectors { get; } = 
        [
            new CardTypeSelector(), 
            new MonsterTypeSelector(), 
            new AttributeSelector(), 
            new LevelSelector(), 
            new AtkSelector(), 
            new DefSelector(),
            new LimitSelector(),
        ];

        public CardSearchConditions CardSearchConditions { get; } = new(CardSearchConditions.Usable);
        public CardSearchConditions DefaultCardSearchConditions => CardSearchConditions.Usable;

        protected override Visual SavingVisual => TableView;

        [DependencyProperty]
        private ICardProvider? _cardProvider;
        [DependencyProperty]
        private ITableDataSelector? _verticalSelector;
        [DependencyProperty]
        private ITableDataSelector? _horizontalSelector;

        public CardTableView()
        {
            InitializeComponent();
            MainGrid.DataContext = this;
            CardProvider ??= EmptyCardProvider.Instance;
            this.RegisterCardSearchCommands();
            VerticalSelector = Selectors[0];
            HorizontalSelector = Selectors[1];
            this.RegisterCommand(YgoCommands.RefreshItems, Executed_Refresh);
        }

        private void Executed_Refresh(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            this.UpdateCardFilter();
        }

        void ICardSearch.SetCardSearchText(string? text) => SearchBar.SearchText = text;
        void ICardSearch.OnCardSearchExecuted() => TableView.ItemsSource = CardProvider is { } c ? c.Where(CardSearchConditions.IsMatch) : null;

        private void ComboBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as ComboBox)?.ChangeByWheel(e);
        }

        protected override void SetExtraData(DataObject obj)
        {
            obj.SetText(TableView.CreateText());
        }

        private void ZeroText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TableView.ZeroText = (sender as TextBox)?.Text;
        }

        private void TableView_CellClick(object sender, RoutedEventArgs<TableDataCell> e)
        {
            var popup = Popup_NameList;
            var cell = e.Value;
            popup.DataContext = cell;
            popup.PlacementTarget = cell.RootElement;
            if (cell.Count > 0)
            {
                ListView_Popup.ScrollIntoView(ListView_Popup.Items[0]);
            }
            popup.IsOpen = true;
        }

        private void Popup_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Popup_NameList.IsOpen = false;
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            Popup_NameList.DataContext = null;
        }

        private void OnClick_Hyperlink(object sender, RoutedEventArgs e)
        {
            Popup_NameList.IsOpen = false;
        }
    }
}
