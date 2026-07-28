using System;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class VerticalTextBlock : Control
    {
        static VerticalTextBlock()
        {
            PropertyUtils.OverrideDefaultStyleKey<VerticalTextBlock>();
        }

        [DependencyProperty]
        private string? _text;
        [DependencyProperty]
        private Thickness _charMargin;
    }
}
