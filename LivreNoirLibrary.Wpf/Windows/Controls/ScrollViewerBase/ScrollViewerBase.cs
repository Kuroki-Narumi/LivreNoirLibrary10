using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;

namespace LivreNoirLibrary.Windows.Controls
{
    public abstract partial class ScrollViewerBase : ScrollViewer
    {
        public const string PART_MainArea = nameof(PART_MainArea);
        public const string PART_VerticalScrollBarArea = nameof(PART_VerticalScrollBarArea);
        public const string PART_HorizontalScrollBarArea = nameof(PART_HorizontalScrollBarArea);

        static ScrollViewerBase()
        {
            PropertyUtils.OverrideDefaultStyleKey<ScrollViewerBase>();
        }

        protected readonly Canvas _main_canvas = CreateSubCanvas(true);
        protected readonly Canvas _fixed_canvas = CreateSubCanvas(false);
        protected readonly Canvas _vertical_scrollbar = CreateSubCanvas(false);
        protected readonly Canvas _horizontal_scrollbar = CreateSubCanvas(false);
        protected readonly ScrollIcon _scroll_icon = new();
        private Panel? _fixed_area;
        private Panel? _vertical_area;
        private Panel? _horizontal_area;

        private static Canvas CreateSubCanvas(bool isHisTestVisible) => new()
        {
            ClipToBounds = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            IsHitTestVisible = isHisTestVisible,
        };

        public Canvas MainCanvas => _main_canvas;
        public Canvas FixedCanvas => _fixed_canvas;
        public Canvas VerticalScrollbar => _vertical_scrollbar;
        public Canvas HorizontalScrollbar => _horizontal_scrollbar;
        public ScrollIcon ScrollIcon => _scroll_icon;

        public ScrollViewerBase()
        {
            Content = _main_canvas;
            _scale_x = (double)GetValue(ScaleXProperty);
            _scale_y = (double)GetValue(ScaleYProperty);
            Grid.SetRow(_vertical_scrollbar, 1);
            Grid.SetColumn(_horizontal_scrollbar, 1);
            this.SetDispatcher(Initialize, System.Windows.Threading.DispatcherPriority.Send);
        }

        private Action? _refresh_action;
        private bool _initializing = true;
        protected void Initialize()
        {
            _main_canvas.Children.Clear();
            PreInitialize();
            InitializeScrollableContents();
            InitializeFixedContents();
            InitializeCommands();
            InitializeBindings();
            PostInitialize();
            _initializing = false;
            if (_refresh_action is not null)
            {
                this.SetDispatcher(_refresh_action, System.Windows.Threading.DispatcherPriority.Loaded);
                _refresh_action = null;
            }
        }

        protected virtual void PreInitialize() { }
        protected virtual void InitializeScrollableContents() { }
        protected virtual void InitializeFixedContents() => AddFixedContent(_scroll_icon);
        protected virtual void InitializeCommands() { }
        protected virtual void InitializeBindings()
        {
            FrameworkElement element = _main_canvas;
            element.SetBinding(SnapsToDevicePixelsProperty, new Binding(nameof(SnapsToDevicePixels)) { Mode = BindingMode.OneWay, Source = this });
            element.SetBinding(UseLayoutRoundingProperty, new Binding(nameof(UseLayoutRounding)) { Mode = BindingMode.OneWay, Source = this });
            element.SetBinding(WidthProperty, Binding_ContentWidth);
            element.SetBinding(HeightProperty, Binding_ContentHeight);
            element.SetBinding(VerticalAlignmentProperty, new Binding(nameof(VerticalContentAlignment)) { Source = this });
            element.SetBinding(HorizontalAlignmentProperty, new Binding(nameof(HorizontalContentAlignment)) { Source = this });

            element = _fixed_canvas;
            element.SetBinding(WidthProperty, Binding_ViewportWidth);
            element.SetBinding(HeightProperty, Binding_ViewportHeight);
        }
        protected virtual void PostInitialize() { }

        protected void AddScrollableContent(UIElement content) => _main_canvas.Children.Add(content);
        protected void AddFixedContent(UIElement content) => _fixed_canvas.Children.Add(content);

        protected bool CheckInitialized(Action action)
        {
            if (_initializing)
            {
                _refresh_action += action;
                return true;
            }
            return false;
        }

        public override void OnApplyTemplate()
        {
            _fixed_area?.Children.Remove(_fixed_canvas);
            _horizontal_area?.Children.Remove(_horizontal_scrollbar);
            _vertical_area?.Children.Remove(_vertical_scrollbar);

            base.OnApplyTemplate();

            _fixed_area = GetTemplateChild(PART_MainArea) as Panel;
            _horizontal_area = GetTemplateChild(PART_VerticalScrollBarArea) as Panel;
            _vertical_area = GetTemplateChild(PART_HorizontalScrollBarArea) as Panel;

            _fixed_area?.Children.Add(_fixed_canvas);
            _horizontal_area?.Children.Add(_horizontal_scrollbar);
            _vertical_area?.Children.Add(_vertical_scrollbar);

            Refresh();
        }

        public void Refresh()
        {
            if (CheckInitialized(Refresh))
            {
                return;
            }
            PreRefresh();
            RefreshContents();
            PostRefresh();
        }

        protected virtual void PreRefresh() { }
        protected virtual void RefreshContents() { }
        protected virtual void PostRefresh() { }

        protected void AdjustHorizontalScroll(double oldScale, double newScale) => AdjustHorizontalScroll(Mouse.GetPosition(this).X, oldScale, newScale);
        protected void AdjustHorizontalScroll(double pivot, double oldScale, double newScale) => ScrollToHorizontalOffset((HorizontalOffset + pivot) * newScale / oldScale - pivot);

        protected void AdjustVerticalScroll(double oldScale, double newScale) => AdjustVerticalScroll(Mouse.GetPosition(this).Y, oldScale, newScale);
        protected void AdjustVerticalScroll(double pivot, double oldScale, double newScale) => ScrollToVerticalOffset((VerticalOffset + pivot) * newScale / oldScale - pivot);

        public static double GetScrollUnit(double scale, double viewport)
        {
            var distance = scale;
            // スケールが表示幅の1/2より小さい場合、スクロール単位を整数倍で大きくする
            if (distance * 2 < viewport)
            {
                distance = scale * Math.Floor(viewport / (scale * 2));
            }
            // スケールが表示幅の1/2より大きい場合、2の累乗で割る
            else if (distance * 2 > viewport)
            {
                while (distance * 2 > viewport)
                {
                    distance /= 2;
                }
            }
            return distance;
        }

        protected static void SetVisibility_Where(DependencyPropertyChangedEventArgs e, bool? refer, params ReadOnlySpan<UIElement> elements)
            => SetVisibility_Where((bool?)e.NewValue, refer, elements);

        protected static void SetVisibility_Where(bool? value, bool? refer, params ReadOnlySpan<UIElement> elements)
        {
            var v = value == refer ? Visibility.Visible : Visibility.Collapsed;
            foreach (var element in elements)
            {
                element.Visibility = v;
            }
        }

        protected static void SetVisibility_Not(DependencyPropertyChangedEventArgs e, bool? refer, params ReadOnlySpan<UIElement> elements)
            => SetVisibility_Not((bool?)e.NewValue, refer, elements);

        protected static void SetVisibility_Not(bool? value, bool? refer, params ReadOnlySpan<UIElement> elements)
        {
            var v = value == refer ? Visibility.Collapsed : Visibility.Visible;
            foreach (var element in elements)
            {
                element.Visibility = v;
            }
        }
    }
}
