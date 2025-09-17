using System;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class WithPanelTabControl : TabControl
    {
        static WithPanelTabControl()
        {
            PropertyUtils.OverrideDefaultStyleKey<WithPanelTabControl>();
        }

        public static readonly Thickness DefaultTabHeaderMargin = new(4, 2, 4, 0);

        [DependencyProperty]
        private object? _leftPanel;
        [DependencyProperty]
        private object? _rightPanel;
        [DependencyProperty]
        private Thickness _tabHeaderMargin = DefaultTabHeaderMargin;
    }
}
