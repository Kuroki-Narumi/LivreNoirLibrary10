using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Windows.Controls;
using System;
using System.Windows;
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

        private ComboBox? _comboBox;

        public SortOptionView()
        {
            this.RegisterCommand(YgoCommands.SearchClear, Executed_Clear);
        }

        public override void OnApplyTemplate()
        {
            _comboBox?.PreviewMouseWheel -= ComboBox_PreviewMouseWheel;
            base.OnApplyTemplate();
            (_comboBox = GetTemplateChild("ComboBox") as ComboBox)?.PreviewMouseWheel += ComboBox_PreviewMouseWheel;
        }

        private void ComboBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as ComboBox)?.ChangeByWheel(e, true);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Source is System.Windows.Controls.RadioButton)
            {
                if (IsAscending)
                {
                    IsDescending = true;
                    IsAscending = false;
                }
                else
                {
                    IsAscending = true;
                    IsDescending = false;
                }
            }
        }

        private void Executed_Clear(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            SourceItem = SortSelectionItem.None;
            IsAscending = true;
            IsDescending = false;
        }
    }
}
