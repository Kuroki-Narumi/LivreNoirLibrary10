using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
    /// NumbersView.xaml の相互作用ロジック
    /// </summary>
    public partial class NumbersView : UserControl, IToggleButtonContainer, ITextSearch
    {
        public NumbersCollection NumbersCollection { get; } = new();
        public NumbersFlagCollection Flags { get; } = new();

        private readonly CardSearchConditions _searchConditions = new();

        bool IToggleButtonContainer.MousePressed { get; set; }
        bool IToggleButtonContainer.MouseToggleState { get; set; }

        TextSearchConditions? ITextSearch.TextSearchConditions => _searchConditions;

        [DependencyProperty]
        private ICardEnumerable? _itemsSource;
        [DependencyProperty]
        private NumbersCard? _selectedCandidate;
        [DependencyProperty]
        private NumbersKey? _selectedKey;
        [DependencyProperty]
        private MatchType _keyMatchType;

        public NumbersView()
        {
            InitializeComponent();
            MainGrid.DataContext = this;
            this.RegisterTextSearchCommands();
            this.RegisterCommand(YgoCommands.RefreshItems, OnExecuted_Refresh);
            this.InitializeIToggleButtonContainer();
        }

        private void OnExecuted_Refresh(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var numbers = NumbersCollection;
            if (ItemsSource is { } source)
            {
                numbers.RefreshCandidates(source);
                Flags.UpdateItems(numbers.Numbers);
            }
        }

        private void OnSelectedCandidateChanged(NumbersCard? value)
        {
            ListView_Keys.ItemsSource = NumbersCollection.GetKeys(value);
        }

        private void OnSelectedKeyChanged(NumbersKey? value)
        {
            var (e1, e2, e3, e4) = NumbersCollection.GetMaterials(value);
            ListView_Material1.ItemsSource = e1;
            ListView_Material2.ItemsSource = e2;
            ListView_Material3.ItemsSource = e3;
            ListView_Material4.ItemsSource = e4;
        }

        void ITextSearch.SetSearchText(string? text) => SearchBar.SearchText = text;

        void ITextSearch.OnTextSearchExecuted()
        {
            ListView_Candidates.Items.Filter = IsCandidateMatch;
            ListView_Candidates.ScrollSelectedItemIntoView();
        }

        private void Keys_DropDownClosed(object sender, EventArgs e)
        {
            UpdateKeyFilter();
        }

        private void Keys_Click_Clear(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Flags.Clear();
            UpdateKeyFilter();
        }

        private void UpdateKeyFilter()
        {
            ListView_Keys.Items.Filter = obj => obj is NumbersKey key && Flags.IsMatch(key, KeyMatchType);
            ListView_Keys.ScrollSelectedItemIntoView();
            this.UpdateTextFilter();
        }

        private bool IsCandidateMatch(object obj)
        {
            if (obj is not NumbersCard card || !_searchConditions.IsTextMatch(card))
            {
                return false;
            }
            var flags = Flags;
            if (flags.CheckedCount is 0)
            {
                return true;
            }
            var type = KeyMatchType;
            foreach (var key in (NumbersCollection.GetKeys(card) as List<NumbersKey>).AsSpan())
            {
                if (flags.IsMatch(key, type))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
