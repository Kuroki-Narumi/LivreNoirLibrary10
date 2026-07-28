using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class MatchTypeSelector : ComboBox
    {
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private MatchType _matchType;

        static MatchTypeSelector()
        {
            PropertyUtils.OverrideDefaultStyleKey<MatchTypeSelector>();
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            this.ChangeByWheel(e, true);
        }
    }
}
