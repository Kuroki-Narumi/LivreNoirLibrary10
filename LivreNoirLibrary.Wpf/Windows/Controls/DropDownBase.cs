using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Windows.Controls
{
    [TemplatePart(Name = PART_Popup, Type = typeof(Popup))]
    public abstract partial class DropDownBase : ContentControl
    {
        public const string PART_Popup = nameof(PART_Popup);

        static DropDownBase()
        {
            ToolTipService.IsEnabledProperty.OverrideMetadata(typeof(DropDownBase), new FrameworkPropertyMetadata(null, CoerceToolTipIsEnabled));
            EventManager.RegisterClassHandler(typeof(DropDownBase), Mouse.MouseDownEvent, new MouseButtonEventHandler(OnMouseButtonDown), true);
            EventManager.RegisterClassHandler(typeof(DropDownBase), Mouse.MouseUpEvent, new MouseButtonEventHandler(OnMouseButtonUp), true);
        }

        public static readonly RoutedEvent ClickEvent = EventRegister.Register<DropDownBase, RoutedEventHandler>();
        public static readonly DependencyProperty PlacementProperty = Popup.PlacementProperty.AddOwner(typeof(DropDownBase));

        private static object CoerceToolTipIsEnabled(DependencyObject d, object value) => !(d is DropDownBase { IsDropDownOpen: true });

        private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DropDownBase s && Mouse.Captured == s && e.OriginalSource == s)
            {
                s.IsDropDownOpen = false;
                e.Handled = true;
            }
        }

        private static void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is DropDownBase s && Mouse.Captured != s && s.IsDropDownOpen && !s.IsLogicalAncestorOf(Mouse.Captured))
            {
                s.Capture();
                e.Handled = true;
            }
        }

        public event EventHandler? DropDownOpened;
        public event EventHandler? DropDownClosed;

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        public PlacementMode Placement { get => (PlacementMode)GetValue(PlacementProperty); set => SetValue(PlacementProperty, value); }

        [DependencyProperty(BindsTwoWayByDefault = true)]
        protected bool _autoOpen;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        protected bool _isDropDownOpen;
        protected Popup? _popup;

        private void OnIsDropDownOpenChanged(bool value)
        {
            if (value)
            {
                OnDropDownOpen();
            }
            if (!AutoOpen && !value)
            {
                if (Mouse.Captured == this)
                {
                    if (this.FindAncestor<DropDownBase>(out var parent))
                    {
                        parent.Capture();
                    }
                    else
                    {
                        Mouse.Capture(null);
                    }
                }
                Focus();
            }
        }

        public override void OnApplyTemplate()
        {
            if (_popup is not null)
            {
                _popup.Opened -= OnOpened_Popup;
                _popup.Closed -= OnClosed_Popup;
            }
            base.OnApplyTemplate();
            _popup = GetTemplateChild(PART_Popup) as Popup;
            if (_popup is not null)
            {
                _popup.Opened += OnOpened_Popup;
                _popup.Closed += OnClosed_Popup;
            }
        }

        protected void RaiseClickEvent() => RaiseEvent(new RoutedEventArgs(ClickEvent, this));

        protected virtual void Capture()
        {
            if (!AutoOpen)
            {
                Mouse.Capture(this, CaptureMode.SubTree);
            }
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);
            if (!AutoOpen)
            {
                var captured = Mouse.Captured;
                var original = e.OriginalSource;
                if (captured != this)
                {
                    if (original == this)
                    {
                        if (captured is null || !this.IsLogicalAncestorOf(captured))
                        {
                            IsDropDownOpen = false;
                            return;
                        }
                    }
                    else if (this.IsLogicalAncestorOf(original) && IsDropDownOpen && captured is null)
                    {
                        Mouse.Capture(this, CaptureMode.SubTree);
                        e.Handled = true;
                        return;
                    }
                }
            }
        }

        private void OnOpened_Popup(object? sender, EventArgs e)
        {
            Focus();
            Capture();
            DropDownOpened?.Invoke(this, e);
        }

        private void OnClosed_Popup(object? sender, EventArgs e)
        {
            OnDropDownClosed();
            DropDownClosed?.Invoke(this, e);
        }

        protected virtual void OnDropDownOpen() { }
        protected virtual void OnDropDownClosed() { }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (AutoOpen)
            {
                IsDropDownOpen = true;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (AutoOpen && !this.IsLogicalAncestorOf(Mouse.DirectlyOver))
            {
                IsDropDownOpen = false;
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (!IsKeyboardFocusWithin && AutoOpen)
            {
                IsDropDownOpen = false;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (IsDropDownOpen)
            {
                switch (e.Key)
                {
                    case Key.Enter:
                    case Key.Escape:
                        IsDropDownOpen = false;
                        e.Handled = true;
                        break;
                }
            }
            else
            {
                if (e.Key is Key.Enter or Key.Space)
                {
                    RaiseClickEvent();
                    IsDropDownOpen = true;
                    e.Handled = true;
                }
            }
            base.OnKeyDown(e);
        }
    }
}
