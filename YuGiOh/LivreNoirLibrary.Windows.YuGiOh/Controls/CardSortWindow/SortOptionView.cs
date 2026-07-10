using LivreNoirLibrary.Windows.Controls;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class SortOptionView : Control
    {
        static SortOptionView()
        {
            PropertyUtils.OverrideDefaultStyleKey<SortOptionView>();
        }

        [DependencyProperty]
        private string? _header;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private SortSelectionItem? _sourceItem;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private bool _isAscending;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private bool _isDescending;

        public SortOptionView()
        {
            this.RegisterCommand(SearchCommands.Clear, Executed_Clear);
        }

        private void Executed_Clear(object sender, ExecutedRoutedEventArgs e)
        {
            SourceItem = SortSelectionItem.None;
            IsAscending = true;
            IsDescending = false;
        }
    }
}
