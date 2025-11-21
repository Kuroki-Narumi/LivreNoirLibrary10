using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IIconButton
    {
        static readonly double DefaultIconPadding = 2;
        const bool DefaultDisplayKeyGesture = false;

        static readonly DependencyProperty IconPaddingProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultIconPadding);
        static readonly DependencyProperty DisplayKeyGestureProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultDisplayKeyGesture);
        static readonly DependencyProperty KeyGestureTextProperty = PropertyUtils.RegisterAttachedTwoWay<string>(typeof(PropertyHolder));

        static void OverrideCommandProperty<T>()
            where T : DependencyObject, IIconButton
        {
            System.Windows.Controls.Primitives.ButtonBase.CommandProperty.OverrideMetadata(typeof(T), PropertyUtils.GetMetaTwoWay(null, OnCommandChanged));
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IIconButton b)
            {
                if (!BindingOperations.IsDataBound(d, KeyGestureTextProperty))
                {
                    b.KeyGestureText = (e.NewValue as RoutedCommand)?.GetKeyGestureText();
                }
            }
        }

        double IconPadding { get; set; }
        bool DisplayKeyGesture { get; set; }
        string? KeyGestureText { get; set; }
    }
}
