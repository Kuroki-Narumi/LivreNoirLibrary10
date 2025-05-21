using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class AlignSelector : Control
    {
        const string PART_Vertical = nameof(PART_Vertical);
        const string PART_Horizontal = nameof(PART_Horizontal);
        const string Button_V1 = nameof(Button_V1);
        const string Button_V2 = nameof(Button_V2);
        const string Button_V3 = nameof(Button_V3);
        const string Button_V4 = nameof(Button_V4);
        const string Button_H1 = nameof(Button_H1);
        const string Button_H2 = nameof(Button_H2);
        const string Button_H3 = nameof(Button_H3);
        const string Button_H4 = nameof(Button_H4);

        static AlignSelector()
        {
            PropertyUtils.OverrideDefaultStyleKey<AlignSelector>();
        }

        public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(typeof(AlignSelector), Orientation.Vertical);

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        [DependencyProperty(BindsTwoWayByDefault = true)]
        private AlignSelectorMode _selectionMode;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private bool _isVerticalStretchEnabled = true;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private bool _isHorizontalStretchEnabled = true;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private VerticalAlignment _selectedVerticalAlignment = VerticalAlignment.Stretch;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private HorizontalAlignment _selectedHorizontalAlignment = HorizontalAlignment.Stretch;

        private StackPanel _vertical = new();
        private StackPanel _horizontal = new();
        private readonly Dictionary<VerticalAlignment, RadioButton> _vertical_buttons = [];
        private readonly Dictionary<HorizontalAlignment, RadioButton> _horizontal_buttons = [];

        private void OnSelectionModeChanged(AlignSelectorMode value)
        {
            UpdateStackPanelVisibility(value);
        }

        private void OnSelectedVerticalAlignmentChanged(VerticalAlignment value)
        {
            UpdateButtonChecked(value);
        }

        private void OnSelectedHorizontalAlignmentChanged(HorizontalAlignment value)
        {
            UpdateButtonChecked(value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _vertical.MouseWheel -= OnMouseWheel_Vertical;
            _horizontal.MouseWheel -= OnMouseWheel_Horizontal;
            if (GetTemplateChild(PART_Vertical) is StackPanel sv)
            {
                _vertical = sv;
                sv.MouseWheel += OnMouseWheel_Vertical;
            }
            if (GetTemplateChild(PART_Horizontal) is StackPanel sh)
            {
                _horizontal = sh;
                sh.MouseWheel += OnMouseWheel_Horizontal;
            }
            if (GetTemplateChild(Button_V1) is RadioButton v1)
            {
                RegisterVertical(v1, VerticalAlignment.Top);
            }
            if (GetTemplateChild(Button_V2) is RadioButton v2)
            {
                RegisterVertical(v2, VerticalAlignment.Center);
            }
            if (GetTemplateChild(Button_V3) is RadioButton v3)
            {
                RegisterVertical(v3, VerticalAlignment.Bottom);
            }
            if (GetTemplateChild(Button_V4) is RadioButton v4)
            {
                RegisterVertical(v4, VerticalAlignment.Stretch);
            }
            if (GetTemplateChild(Button_H1) is RadioButton h1)
            {
                RegisterHorizontal(h1, HorizontalAlignment.Left);
            }
            if (GetTemplateChild(Button_H2) is RadioButton h2)
            {
                RegisterHorizontal(h2, HorizontalAlignment.Center);
            }
            if (GetTemplateChild(Button_H3) is RadioButton h3)
            {
                RegisterHorizontal(h3, HorizontalAlignment.Right);
            }
            if (GetTemplateChild(Button_H4) is RadioButton h4)
            {
                RegisterHorizontal(h4, HorizontalAlignment.Stretch);
            }
            UpdateButtonChecked(SelectedVerticalAlignment);
            UpdateButtonChecked(SelectedHorizontalAlignment);
            UpdateStackPanelVisibility(SelectionMode);
        }

        private void OnMouseWheel_Vertical(object sender, MouseWheelEventArgs e)
        {
            SelectedVerticalAlignment = (VerticalAlignment)GetNextValue((int)SelectedVerticalAlignment, e.Delta, IsVerticalStretchEnabled);
        }

        private void OnMouseWheel_Horizontal(object sender, MouseWheelEventArgs e)
        {
            SelectedHorizontalAlignment = (HorizontalAlignment)GetNextValue((int)SelectedHorizontalAlignment, e.Delta, IsHorizontalStretchEnabled);
        }

        private static int GetNextValue(int current, int delta, bool stretchEnabled)
        {
            var max = stretchEnabled ? 4 : 3;
            return (current + (delta < 0 ? 1 : max - 1)) % max;
        }

        private void UpdateStackPanelVisibility(AlignSelectorMode mode)
        {
            _vertical.Visibility = (mode & AlignSelectorMode.Vertical) is not 0 ? Visibility.Visible : Visibility.Collapsed;
            _horizontal.Visibility = (mode & AlignSelectorMode.Horizontal) is not 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateButtonChecked(VerticalAlignment a)
        {
            if (_vertical_buttons.TryGetValue(a, out var button))
            {
                button.IsChecked = true;
            }
        }

        private void UpdateButtonChecked(HorizontalAlignment a)
        {
            if (_horizontal_buttons.TryGetValue(a, out var button))
            {
                button.IsChecked = true;
            }
        }

        private void RegisterVertical(RadioButton b, VerticalAlignment a)
        {
            _vertical_buttons.Add(a, b);
            b.Checked += (s, e) => SelectedVerticalAlignment = a;
        }

        private void RegisterHorizontal(RadioButton b, HorizontalAlignment a)
        {
            _horizontal_buttons.Add(a, b);
            b.Checked += (s, e) => SelectedHorizontalAlignment = a;
        }
    }

    public enum AlignSelectorMode
    {
        None,
        Vertical,
        Horizontal,
        Both,
    }
}
