using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Windows.Converters;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Separator = LivreNoirLibrary.Windows.Media.Bms.SkinInfo.Separator;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    /// <summary>
    /// SkinOptionView.xaml の相互作用ロジック
    /// </summary>
    public partial class SkinOptionView : CenteringPanelChildBase
    {
        public const string Header = nameof(Header);
        public const string Suffix = nameof(Suffix);

        private Skin? _skin;
        private IDictionary<string, string>? _options;
        private readonly List<LineCache> _cache = [];
        private readonly List<ComboBox> _comboCache = [];
        private readonly List<LabeledSlider> _sliderCache = [];
        private readonly List<Border> _separatorCache = [];
     
        public SkinOptionView()
        {
            InitializeComponent();
        }

        private void ClearCache()
        {
            foreach (var cache in _cache)
            {
                cache.Content = null;
            }
            MainPanel.Children.Clear();
        }

        private static T GetCacheCore<T>(List<T> list, Predicate<T> selector, Func<T> factory)
        {
            foreach (var item in list)
            {
                if (selector(item))
                {
                    return item;
                }
            }
            var ret = factory();
            list.Add(ret);
            return ret;
        }

        private LineCache GetCache(Style? headerStyle, Style? suffixStyle) => GetCacheCore(_cache, i => !i.IsUsing, () => new(headerStyle, suffixStyle));

        private static T GetElementCore<T>(List<T> list, Style? style)
            where T : FrameworkElement, new() => GetCacheCore(list, i => i.Parent is null, () => new() { Style = style });

        private ComboBox GetComboBox(Style? style) => GetElementCore(_comboCache, style);
        private LabeledSlider GetSlider(Style? style) => GetElementCore(_sliderCache, style);
        private Border GetSeparator(Style? style) => GetElementCore(_separatorCache, style);

        public void Open(Skin? skin, IDictionary<string, string>? options)
        {
            _skin = skin;
            _options = options;
            if (skin is not null)
            {
                var children = MainPanel.Children;
                var headerStyle = FindResource(Header) as Style;
                var suffixStyle = FindResource(Suffix) as Style;
                var listOptionStyle = FindResource(nameof(ListOption)) as Style;
                var rangeOptionStyle = FindResource(nameof(RangeOption)) as Style;
                var separatorStyle = FindResource(nameof(Separator)) as Style;
                foreach (var option in skin.Options)
                {
                    var line = GetCache(headerStyle, suffixStyle);
                    line.Header.Text = option.Key;
                    line.Suffix.Text = option.Suffix;
                    if (SetContent(line, option))
                    {
                        children.Add(line.Grid);
                    }
                }
                Open();

                bool SetContent(LineCache line, OptionBase option)
                {
                    switch (option)
                    {
                        case Separator s:
                            var separator = GetSeparator(separatorStyle);
                            line.Header.Text = "";
                            line.Suffix.Text = "";
                            line.Content = separator;
                            return true;
                        case ListOption op:
                            var comboBox = GetComboBox(listOptionStyle);
                            comboBox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(nameof(ListOption.Items)) { Source = op });
                            comboBox.SetBinding(Selector.SelectedIndexProperty, new Binding(nameof(ListOption.SelectedIndex))
                            {
                                Source = op,
                                Mode = BindingMode.TwoWay
                            });
                            line.Content = comboBox;
                            return true;
                        case RangeOption op:
                            op.ValidateValue();
                            var slider = GetSlider(rangeOptionStyle);
                            slider.Minimum = op.Minimum;
                            slider.Maximum = op.Maximum;
                            var snap = op.TickFrequency is > 0;
                            slider.IsSnapToTickEnabled = snap;
                            slider.SmallChange = slider.TickFrequency = snap ? op.TickFrequency : 0.1;
                            slider.LargeChange = slider.SmallChange * 10;
                            if (!string.IsNullOrEmpty(op.StringFormat))
                            {
                                slider.StringFormat = op.StringFormat;
                            }
                            slider.SetBinding(RangeBase.ValueProperty, new Binding(nameof(RangeOption.Value))
                            {
                                Source = op,
                                Mode = BindingMode.TwoWay,
                            });
                            line.Content = slider;
                            return true;
                        default:
                            return false;
                    }
                }
            }
        }

        private void OnPreviewMouseWheel_ComboBox(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            (sender as ComboBox)?.ChangeByWheel(e);
        }

        private void OnClick_Default(object sender, RoutedEventArgs e)
        {
            if (_skin is { } skin)
            {
                foreach (var option in skin.Options)
                {
                    option.SetDefaultValue();
                }
            }
        }

        protected override void OnClosed()
        {
            if (_skin is { } skin && _options is { } options)
            {
                foreach (var option in skin.Options)
                {
                    if (option.SelectedValue is { } value)
                    {
                        options[option.Key] = value;
                    }
                    else
                    {
                        options.Remove(option.Key);
                    }
                }
            }
            base.OnClosed();
            ClearCache();
            _skin = null;
            _options = null;
        }

        private class LineCache
        {
            public bool IsUsing { get; private set; }
            public Grid Grid { get; }
            public TextBlock Header { get; }
            public TextBlock Suffix { get; }
            public UIElement? Content
            {
                get;
                set
                {
                    var children = Grid.Children;
                    if (field is not null)
                    {
                        IsUsing = false;
                        BindingOperations.ClearAllBindings(field);
                        children.Remove(field);
                    }
                    field = value;
                    if (value is not null)
                    {
                        IsUsing = true;
                        children.Add(value);
                    }
                }
            }

            public LineCache(Style? headerStyle, Style? suffixStyle)
            {
                Grid line = new();
                var cols = line.ColumnDefinitions;
                cols.Add(new() { Width = GridLength.Auto, SharedSizeGroup = SkinOptionView.Header });
                cols.Add(new() { Width = new GridLength(1, GridUnitType.Star) });
                cols.Add(new() { Width = GridLength.Auto, SharedSizeGroup = SkinOptionView.Suffix });
                Grid = line;
                Header = new() { Style = headerStyle };
                Suffix = new() { Style = suffixStyle };

                var children = line.Children;
                children.Add(Header);
                children.Add(Suffix);
            }
        }
    }
}
