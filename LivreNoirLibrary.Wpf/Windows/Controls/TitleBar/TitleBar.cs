using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using PToggleButton = System.Windows.Controls.Primitives.ToggleButton;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class TitleBar : Control
    {
        public const double DefaultButtonWidth = 40;
        public const double MoveThreshold = 8 * 8;

        private const string PART_LeftPanel = nameof(PART_LeftPanel);
        private const string PART_RightPanel = nameof(PART_RightPanel);
        private const string Button_Maximize = nameof(Button_Maximize);

        static TitleBar()
        {
            PropertyUtils.OverrideDefaultStyleKey<TitleBar>();
        }

        public static readonly DependencyProperty TitleProperty = Window.TitleProperty.AddOwner(typeof(TitleBar));
        public static readonly DependencyProperty ResizeModeProperty = Window.ResizeModeProperty.AddOwner(typeof(TitleBar));

        private static readonly TitlebarIconConverter IconConverter = new() { SmallIcon = true };

        [DependencyProperty]
        private ImageSource? _icon;
        [DependencyProperty]
        private double _buttonWidth = DefaultButtonWidth;
        [DependencyProperty]
        private bool _isMinimizeVisible = true;
        [DependencyProperty]
        private bool _isMaximizeVisible = true;
        [DependencyProperty]
        private bool _isCloseVisible = true;

        private Window? _owner;
        private PToggleButton? _maximize_button;

        public ObservableList<UIElement> LeftPanelItems { get; } = [];
        public ObservableList<UIElement> RightPanelItems { get; } = [];

        public string? Title
        {
            get => GetValue(TitleProperty) as string;
            set => SetValue(TitleProperty, value);
        }

        public ResizeMode ResizeMode
        {
            get => (ResizeMode)GetValue(ResizeModeProperty);
            set => SetValue(ResizeModeProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _maximize_button = GetTemplateChild(Button_Maximize) as PToggleButton;
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            if (_owner is not null)
            {
                _owner.StateChanged -= OnWindowStateChanged;
                BindingOperations.ClearBinding(this, IconProperty);
                BindingOperations.ClearBinding(this, TitleProperty);
                BindingOperations.ClearBinding(this, ResizeModeProperty);
            }
            base.OnVisualParentChanged(oldParent);
            if ((_owner = Window.GetWindow(VisualParent)) is Window w)
            {
                SetBinding(IconProperty, new Binding(nameof(Icon)) { Source = w, Converter = IconConverter });
                SetBinding(TitleProperty, new Binding(nameof(Title)) { Source = w });
                SetBinding(ResizeModeProperty, new Binding(nameof(ResizeMode)) { Source = w, Mode = BindingMode.TwoWay });
                w.StateChanged += OnWindowStateChanged;
                w.RegisterWindowCommands();
            }
        }

        private void OnWindowStateChanged(object? sender, EventArgs e)
        {
            if (_owner is not null && _maximize_button is not null)
            {
                _maximize_button.IsChecked = _owner.WindowState is WindowState.Maximized;
            }
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (_owner is not null && IsMaximizeVisible && _owner.ResizeMode >= ResizeMode.CanResize)
            {
                SystemCommands.MaximizeWindowCommand.Execute(null, this);
                e.Handled = true;
            }
            else
            {
                base.OnMouseDoubleClick(e);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (_owner is not null)
            {
                if (_owner.WindowState is WindowState.Maximized)
                {
                    DragMove_Maximized(_owner);
                }
                else
                {
                    _owner.DragMove();
                }
                e.Handled = true;
            }
            else
            {
                base.OnMouseLeftButtonDown(e);
            }
        }

        private void DragMove_Maximized(Window owner)
        {
            var initPos = this.GetCursorPos();
            var bounds = owner.GetScreenBounds();
            double initLeft = bounds.Left;
            double initTop = bounds.Top;
            double initWidth = bounds.Width;
            owner.CaptureMouse();

            void MouseMove(object sender, MouseEventArgs e)
            {
                var pos = this.GetCursorPos();
                if (owner.WindowState is WindowState.Maximized)
                {
                    var dy = pos.Y - initPos.Y;
                    if (dy * dy > MoveThreshold)
                    {
                        SystemCommands.MaximizeWindowCommand.Execute(null, this);
                        var th = owner.BorderThickness;
                        var offset = (initPos.X - initLeft) / initWidth * (owner.Width - th.Left - th.Right) + th.Left;
                        initLeft = -offset;
                    }
                    else
                    {
                        return;
                    }
                }
                owner.Left = initLeft + pos.X;
                owner.Top = initTop + pos.Y - initPos.Y;
                e.Handled = true;
            }

            void MouseUp(object sender, MouseEventArgs e)
            {
                owner.ReleaseMouseCapture();
                owner.MouseMove -= MouseMove;
                owner.MouseUp -= MouseUp;
                e.Handled = true;
            }

            owner.MouseMove += MouseMove;
            owner.MouseUp += MouseUp;
        }
    }
}
