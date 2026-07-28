using LivreNoirLibrary.YuGiOh.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// DeckRecipeControl.xaml の相互作用ロジック
    /// </summary>
    public partial class DeckRecipeControl : SaveImageBase
    {
        public static readonly DependencyProperty DeckProperty = DeckRecipeView.DeckProperty.AddOwner(typeof(DeckRecipeControl));
        public static readonly DependencyProperty LineBreaksProperty = DeckRecipeView.LineBreaksProperty.AddOwner(typeof(DeckRecipeControl));

        public Deck? Deck { get => GetValue(DeckProperty) as Deck; set => SetValue(DeckProperty, value); }
        public IDictionary<int, int>? LineBreaks { get => GetValue(LineBreaksProperty) as IDictionary<int, int>; set => SetValue(LineBreaksProperty, value); }

        protected override Visual SavingVisual => DeckRecipeView;

        [DependencyProperty]
        private DeckRecipeOrderItem? _order;
        [DependencyProperty]
        private DeckRecipeSeparatorItem? _separator;
        [DependencyProperty]
        private bool _withBracket;

        public DeckRecipeControl()
        {
            InitializeComponent();
            MainGrid.DataContext = this;
            Order = DeckRecipeOrderItem.Items[0];
            Separator = DeckRecipeSeparatorItem.Items[0];
        }

        protected override void SetExtraData(DataObject obj)
        {
            obj.SetText(DeckRecipeView.GetText(Order!.IsNameFirst, WithBracket, Separator!.Value));
        }
    }
}
