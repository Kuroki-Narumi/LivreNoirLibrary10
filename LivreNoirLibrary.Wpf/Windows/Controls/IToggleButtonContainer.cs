using System;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IToggleButtonContainer
    {
        bool MousePressed { get; set; }
        bool MouseToggleState { get; set; }
    }

    public static class IToggleButtonContainerExtensions
    {
        public static void InitializeIToggleButtonContainer<T>(this T t)
            where T : UIElement, IToggleButtonContainer
        {
            t.MouseLeave += (s, e) => t.MousePressed = false;
            t.PreviewMouseLeftButtonUp += (s, e) => t.MousePressed = false;
        }

        public static void OnMouseLeftButtonDown_ToggleButton<T>(this T t, object sender, MouseButtonEventArgs e)
            where T : UIElement, IToggleButtonContainer
        {
            if (sender is System.Windows.Controls.Primitives.ToggleButton button)
            {
                t.MouseToggleState = button.IsChecked is not true;
                t.MousePressed = true;
                button.IsChecked = t.MouseToggleState;
                e.Handled = true;
            }
            else if (sender is ListBoxItem { DataContext: ICheckableObject item})
            {
                t.MouseToggleState = item.IsChecked is not true;
                t.MousePressed = true;
                item.IsChecked = t.MouseToggleState;
                e.Handled = true;
            }
        }

        public static void OnMouseEnter_ToggleButton<T>(this T t, object sender, MouseEventArgs e)
            where T : UIElement, IToggleButtonContainer
        {
            if (t.MousePressed)
            {
                if (sender is System.Windows.Controls.Primitives.ToggleButton button)
                {
                    button.IsChecked = t.MouseToggleState;
                }
                else if (sender is ListBoxItem { DataContext: ICheckableObject item })
                {
                    item.IsChecked = t.MouseToggleState;
                }
            }
        }
    }
}
