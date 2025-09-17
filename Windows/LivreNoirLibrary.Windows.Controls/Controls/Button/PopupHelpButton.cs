using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace LivreNoirLibrary.Windows.Controls
{
    [ContentProperty(nameof(Content))]
    public class PopupHelpButton : Control
    {
        public static readonly DependencyProperty HeaderTextProperty = PropertyUtils.RegisterTwoWay<string>(typeof(PopupHelpButton));
        public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(typeof(PopupHelpButton));
        public static readonly DependencyProperty IconSizeProperty = IIconUI.IconSizeProperty.AddOwner(typeof(PopupHelpButton));
        public static readonly DependencyProperty IconPaddingProperty = PropertyUtils.RegisterTwoWay(typeof(PopupHelpButton), default(Thickness));
        public static readonly DependencyProperty CornerRadiusProperty = Border.CornerRadiusProperty.AddOwner(typeof(PopupHelpButton));
        public static readonly DependencyProperty PopupPlacementProperty = PropertyUtils.RegisterTwoWay(typeof(PopupHelpButton), PlacementMode.Bottom);

        static PopupHelpButton()
        {
            PropertyUtils.OverrideDefaultStyleKey<PopupHelpButton>();
        }

        public string? HeaderText { get => GetValue(HeaderTextProperty) as string; set => SetValue(HeaderTextProperty, value); }
        public object? Content { get => GetValue(ContentProperty); set => SetValue(ContentProperty, value); }
        public double IconSize { get => (double)GetValue(IconSizeProperty); set => SetValue(IconSizeProperty, value); }
        public Thickness IconPadding { get => (Thickness)GetValue(IconPaddingProperty); set => SetValue(IconPaddingProperty, value); }
        public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }
        public PlacementMode PopupPlacement { get => (PlacementMode)GetValue(PopupPlacementProperty); set => SetValue(PopupPlacementProperty, value); }
    }
}
