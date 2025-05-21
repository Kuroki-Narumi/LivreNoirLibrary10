using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Controls
{
    public static partial class ControlExtensions
    {
        public static void ChangeByWheel(this ComboBox control, MouseWheelEventArgs e, bool wrap = false)
        {
            if (control.IsDropDownOpen)
            {
                return;
            }
            var index = control.SelectedIndex;
            var max = control.Items.Count;
            if (e.Delta < 0)
            {
                if (wrap || index < max - 1)
                {
                    control.SelectedIndex = (index + 1) % max;
                }
            }
            else if (e.Delta > 0)
            {
                if (wrap || index > 0)
                {
                    control.SelectedIndex = (index + max - 1) % max;
                }
            }
            e.Handled = true;
        }

        public static void ChangeByWheel(this Slider control, MouseWheelEventArgs e, double freq = double.NaN)
        {
            var value = control.Value;
            freq = freq.Validate(control.TickFrequency);
            var flag = freq > 0 && (int)freq == freq && value % freq != 0;
            if (e.Delta < 0 && value > control.Minimum)
            {
                if (flag)
                {
                    control.Value = freq * (int)Math.Floor(value / freq);
                }
                else
                {
                    control.Value = value - freq;
                }
            }
            else if (e.Delta > 0 && value < control.Maximum)
            {
                if (flag)
                {
                    control.Value = freq * (int)Math.Ceiling(value / freq);
                }
                else
                {
                    control.Value = value + freq;
                }
            }
            e.Handled = true;
        }

        public static void ChangeByWheel(this TabControl control, MouseWheelEventArgs e, bool wrap = true)
        {
            if (e.OriginalSource is DependencyObject d &&
                d.FindAncestor<System.Windows.Controls.Primitives.TabPanel>(out var panel) &&
                panel.FindAncestor<TabControl>(out var tab) &&
                tab == control)
            {
                if (e.Delta is > 0)
                {
                    SelectPreviousTab(control, wrap);
                }
                else
                {
                    SelectNextTab(control, wrap);
                }
                e.Handled = true;
            }
        }

        public static void SelectNextTab(this TabControl control, bool wrap)
        {
            var max = control.Items.Count;
            var init = control.SelectedIndex;
            var index = init;
            do
            {
                index++;
                if (index >= max)
                {
                    if (wrap)
                    {
                        index -= max;
                    }
                    else
                    {
                        index -= 1;
                        break;
                    }
                }
                if (control.Items[index] is not UIElement t || (t.Visibility is Visibility.Visible && t.IsEnabled))
                {
                    break;
                }
            } while (index != init);
            control.SelectedIndex = index;
        }

        public static void SelectPreviousTab(this TabControl control, bool wrap)
        {
            var max = control.Items.Count;
            var init = control.SelectedIndex;
            var index = init;
            do
            {
                index--;
                if (index is < 0)
                {
                    if (wrap)
                    {
                        index += max;
                    }
                    else
                    {
                        index += 1;
                        break;
                    }
                }
                if (control.Items[index] is not UIElement t || (t.Visibility is Visibility.Visible && t.IsEnabled))
                {
                    break;
                }
            } while (index != init);
            control.SelectedIndex = index;
        }

        public static void AdjustGridViewColumn(this ListView l)
        {
            if (l.View is GridView g)
            {
                foreach (var col in g.Columns)
                {
                    if (double.IsNaN(col.Width))
                    {
                        col.Width = col.ActualWidth;
                    }
                    col.Width = double.NaN;
                }
            }
        }
    }
}
