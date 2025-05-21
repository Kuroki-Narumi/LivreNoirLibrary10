using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class CenteringPanel : Panel
    {
        public static readonly SolidColorBrush DefaultBackground = MediaUtils.GetBrush(127, 0, 0, 0);

        static CenteringPanel()
        {
            PropertyUtils.OverrideDefaultStyleKey<CenteringPanel>();
            VisibilityProperty.OverrideMetadata(typeof(CenteringPanel), PropertyUtils.GetMetaTwoWay(Visibility.Collapsed, OnVisibilityChanged));
            FocusableProperty.OverrideMetadata(typeof(CenteringPanel), PropertyUtils.GetMetaTwoWay(true));
            BackgroundProperty.OverrideMetadata(typeof(CenteringPanel), PropertyUtils.GetMeta(DefaultBackground));
        }

        public static readonly RoutedEvent ClosedEvent = EventRegister.Register<CenteringPanel, RoutedEventHandler>();
        public static readonly RoutedEvent OpenedEvent = EventRegister.Register<CenteringPanel, RoutedEventHandler>();

        public static readonly DependencyProperty ClickModeProperty = ButtonBase.ClickModeProperty.AddOwner(typeof(CenteringPanel), ClickMode.Release, OnClickModeChanged);

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as CenteringPanel)!.OnVisibilityChanged(e.NewValue is Visibility.Visible);
        private static void OnClickModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as CenteringPanel)!._mode = (ClickMode)e.NewValue;

        public event RoutedEventHandler? Closed { add => AddHandler(ClosedEvent, value); remove => RemoveHandler(ClosedEvent, value); }
        public event RoutedEventHandler? Opened { add => AddHandler(OpenedEvent, value); remove => RemoveHandler(OpenedEvent, value); }

        [DependencyProperty]
        private int _visibleIndex = -1;
        private ClickMode _mode;
        [DependencyProperty]
        private bool _closeByClick = true;
        [DependencyProperty]
        private bool _closeByKey = true;

        public ClickMode ClickMode { get => _mode; set => SetValue(ClickModeProperty, value); }

        public CenteringPanel()
        {
            var type = GetType();
            _visibleIndex = (int)VisibleIndexProperty.GetMetadata(type).DefaultValue;
            _closeByClick = (bool)CloseByClickProperty.GetMetadata(type).DefaultValue;
            _closeByKey = (bool)CloseByKeyProperty.GetMetadata(type).DefaultValue;
            _mode = (ClickMode)ClickModeProperty.GetMetadata(type).DefaultValue;
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            if (visualAdded is UIElement element)
            {
                if (_visibleIndex is < 0 || Children.IndexOf(element) != _visibleIndex)
                {
                    element.Visibility = Visibility.Collapsed;
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var (w, h) = availableSize;
            var wIsFinite = double.IsFinite(w);
            var hIsFinite = double.IsFinite(h);
            var children = InternalChildren;
            var count = children.Count;
            for (var i = 0; i < count; i++)
            {
                var child = children[i];
                child.Measure(availableSize);
                var (cw, ch) = child.DesiredSize;
                if (double.IsFinite(cw) && (!wIsFinite || cw > w))
                {
                    w = cw;
                    wIsFinite = true;
                }
                if (double.IsFinite(ch) && (!hIsFinite || ch > h))
                {
                    h = ch;
                    hIsFinite = true;
                }
            }
            return new(w, h);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var (w, h) = finalSize;
            var round = UseLayoutRounding;
            var children = InternalChildren;
            var count = children.Count;
            for (var i = 0; i < count; i++)
            {
                var child = children[i];
                var (cw, ch) = child.DesiredSize;
                var x = (w - cw) / 2;
                var y = (h - ch) / 2;
                if (round)
                {
                    x = double.Round(x);
                    y = double.Round(y);
                }
                child.Arrange(new(x, y, cw, ch));
            }
            return finalSize;
        }

        private bool _clicked;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            _clicked = false;
            if (_closeByClick && e.OriginalSource == this)
            {
                if (_mode is ClickMode.Press)
                {
                    Close();
                    e.Handled = true;
                    return;
                }
                else
                {
                    _clicked = true;
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (_closeByClick && e.OriginalSource == this && _mode is ClickMode.Release && _clicked)
            {
                Close();
                e.Handled = true;
            }
            else
            {
                base.OnMouseUp(e);
            }
            _clicked = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (_closeByKey && e.Key is Key.Escape)
            {
                Close();
                e.Handled = true;
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        private void OnVisibilityChanged(bool visible)
        {
            if (visible)
            {
                var children = Children;
                var index = _visibleIndex;
                Focus();
                if ((uint)_visibleIndex < (uint)children.Count)
                {
                    var child = children[index];
                    if (child is ICenteringPanelChild c)
                    {
                        c.ReserveFocus();
                    }
                    else
                    {
                        child.SetDispatcher(() => child.Focus());
                    }
                }
                RaiseEvent(new RoutedEventArgs(OpenedEvent, this));
            }
            else
            {
                RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
            }
        }

        private void OnVisibleIndexChanged(int oldIndex, int newIndex)
        {
            _visibleIndex = newIndex;
            var children = Children;
            var uc = (uint)children.Count;
            if ((uint)newIndex < uc)
            {
                children[newIndex].Visibility = Visibility.Visible;
            }
            Visibility = newIndex is >= 0 ? Visibility.Visible : Visibility.Collapsed;
            if ((uint)oldIndex < uc)
            {
                children[oldIndex].Visibility = Visibility.Collapsed;
            }
            InvalidateMeasure();
        }

        public void Open()
        {
            if (_visibleIndex is < 0)
            {
                VisibleIndex = 0;
            }
            else
            {
                Visibility = Visibility.Visible;
            }
        }

        public void Close()
        {
            if (_visibleIndex is >= 0)
            {
                VisibleIndex = -1;
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
        }

        public void Open(int index)
        {
            if ((uint)index < (uint)Children.Count)
            {
                if (index == _visibleIndex)
                {
                    Open();
                }
                else
                {
                    VisibleIndex = index;
                }
            }
            else
            {
                Close();
            }
        }

        public void Open(UIElement element) => Open(Children.IndexOf(element));
    }
}
