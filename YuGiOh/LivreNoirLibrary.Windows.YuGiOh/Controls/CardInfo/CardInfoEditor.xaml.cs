using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.Windows.Controls;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// CardInfoEditor.xaml の相互作用ロジック
    /// </summary>
    public partial class CardInfoEditor : UserControl, IToggleButtonContainer
    {
        public static DoubleCollection StatusTicks { get; } = CreateStatusTicks();

        private static DoubleCollection CreateStatusTicks()
        {
            DoubleCollection c = [];
            c.Add(-1);
            for (var i = 0; i <= 5000; i += 50)
            {
                c.Add(i);
            }
            c.Freeze();
            return c;
        }

        private readonly CardInfoViewModel _viewModel = new(false);

        bool IToggleButtonContainer.MousePressed { get; set; }
        bool IToggleButtonContainer.MouseToggleState { get; set; }

        [DependencyProperty]
        private Card? _source;

        public CardInfoEditor()
        {
            InitializeComponent();
            MainGrid.DataContext = _viewModel;
            this.InitializeIToggleButtonContainer();
        }

        private void OnSourceChanged(Card? card)
        {
            if (card is not null)
            {
                _viewModel.CopyFrom(card);
            }
        }

        public void Save()
        {
            if (Source is { } card)
            {
                _viewModel.CopyTo(card);
            }
        }

        private void OnMouseWheel_ComboBox(object sender, MouseWheelEventArgs e)
        {
            (sender as ComboBox)?.ChangeByWheel(e, true);
        }
    }
}
