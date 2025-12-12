using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using RButton = System.Windows.Controls.RadioButton;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Debug;

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
                d.TryGetAncestor<System.Windows.Controls.Primitives.TabPanel>(out var panel) &&
                panel.TryGetAncestor<TabControl>(out var tab) &&
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
            var initial = control.SelectedIndex;
            var index = initial;
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
            } while (index != initial);
            control.SelectedIndex = index;
        }

        public static void SelectPreviousTab(this TabControl control, bool wrap)
        {
            var max = control.Items.Count;
            var initial = control.SelectedIndex;
            var index = initial;
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
            } while (index != initial);
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

        public static void ChangeRadioButtonByWheel(this Panel panel, MouseWheelEventArgs e, bool wrap = true)
        {
            var cache = ObjectPool.Rent<List<RButton>>();
            try
            {
                var checkedIndex = 0;
                var delta = e.Delta is > 0;
                foreach (var child in panel.EnumerateDescendantsByStack())
                {
                    if (child is RButton button)
                    {
                        if (button.IsChecked is true)
                        {
                            checkedIndex = cache.Count;
                        }
                        cache.Add(button);
                    }
                }
                switch (cache.Count)
                {
                    case 0: // ラジオボタンが一つも見つからなかった
                        break;
                    case 1: // ラジオボタンが一つしかなかった
                        cache[0].IsChecked = true;
                        break;
                    default: // 複数のラジオボタンが見つかった
                        checkedIndex = checkedIndex + (e.Delta is > 0 ? -1 : 1);
                        if (checkedIndex is < 0)
                        {
                            if (wrap)
                            {
                                cache[^1].IsChecked = true;
                            }
                        }
                        else if (checkedIndex >= cache.Count)
                        {
                            if (wrap)
                            {
                                cache[0].IsChecked = true;
                            }
                        }
                        else
                        {
                            cache[checkedIndex].IsChecked = true;
                        }
                        break;
                }
            }
            finally
            {
                ObjectPool.Return(cache);
            }
        }
    }
}