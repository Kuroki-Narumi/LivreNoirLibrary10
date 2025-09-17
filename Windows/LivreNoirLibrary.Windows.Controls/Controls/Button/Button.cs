using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.Controls
{
    public class Button : System.Windows.Controls.Button, IIconUI, IIconButton
    {
        public static readonly DependencyProperty IconProperty = IIconUI.IconProperty.AddOwner(typeof(Button));
        public static readonly DependencyProperty IconSizeProperty = IIconUI.IconSizeProperty.AddOwner(typeof(Button));
        public static readonly DependencyProperty IconStretchProperty = IIconUI.IconStretchProperty.AddOwner(typeof(Button));
        public static readonly DependencyProperty IconFontFamilyProperty = IIconUI.IconFontFamilyProperty.AddOwner(typeof(Button));

        public static readonly DependencyProperty IconPaddingProperty = IIconButton.IconPaddingProperty.AddOwner(typeof(Button));
        public static readonly DependencyProperty DisplayKeyGestureProperty = IIconButton.DisplayKeyGestureProperty.AddOwner(typeof(Button));
        public static readonly DependencyProperty KeyGestureTextProperty = IIconButton.KeyGestureTextProperty.AddOwner(typeof(Button));

        public static readonly DependencyProperty CornerRadiusProperty = Border.CornerRadiusProperty.AddOwner(typeof(Button));

        static Button()
        {
            PropertyUtils.OverrideDefaultStyleKey<Button>();
            IIconButton.OverrideCommandProperty<Button>();
        }

        public object? Icon { get => GetValue(IconProperty); set => SetValue(IconProperty, value); }
        public double IconSize { get => (double)GetValue(IconSizeProperty); set => SetValue(IconSizeProperty, value); }
        public Stretch IconStretch { get => (Stretch)GetValue(IconStretchProperty); set => SetValue(IconStretchProperty, value); }
        public FontFamily? IconFontFamily { get => GetValue(IconFontFamilyProperty) as FontFamily; set => SetValue(IconFontFamilyProperty, value); }
        public double IconPadding { get => (double)GetValue(IconPaddingProperty); set => SetValue(IconPaddingProperty, value); }
        public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }
        public bool DisplayKeyGesture { get => (bool)GetValue(DisplayKeyGestureProperty); set => SetValue(DisplayKeyGestureProperty, value); }
        public string? KeyGestureText { get => GetValue(KeyGestureTextProperty) as string; set => SetValue(KeyGestureTextProperty, value); }
    }
}
