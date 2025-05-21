using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IIconButton
    {
        public static readonly double DefaultIconPadding = 2;
        public const bool DefaultDisplayKeyGesture = false;

        public static readonly DependencyProperty IconPaddingProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultIconPadding);
        public static readonly DependencyProperty DisplayKeyGestureProperty = PropertyUtils.RegisterAttachedTwoWay(typeof(PropertyHolder), DefaultDisplayKeyGesture);
        public static readonly DependencyProperty KeyGestureTextProperty = PropertyUtils.RegisterAttachedTwoWay<string>(typeof(PropertyHolder));

        public static void OverrideCommandProperty<T>()
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

        public double IconPadding { get; set; }
        public bool DisplayKeyGesture { get; set; }
        public string? KeyGestureText { get; set; }
    }
}
