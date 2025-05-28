using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public sealed partial class ImageRectSelectorView : ScrollViewerBase
    {
        public const double MoveThreshold = 4;
        public static readonly double[] ScaleList = [1.0 / 16, 1.0 / 8, 1.0 / 4, 1.0 / 2, 1, 1.5, 2, 2.5, 3, 4, 6, 8, 12, 16];

        public static IEnumerable<string> ScaleExpressionList => ScaleList.Select(v => v.ToString("0.##%"));

        public event ValueChangedEventHandler<Int32Rect>? ValueChanged;

        [DependencyProperty(BindsTwoWayByDefault = true)]
        private BitmapSource? _source;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private Int32Rect _initialRect;
        [DependencyProperty(SetterScope = Scope.Private)]
        private Int32Rect _selectedRect;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private string _scaleExpression = "100%";

        private int _cursorIndex;

        private readonly Canvas _background = new()
        {
            Background = MediaUtils.TransparentCheckerBrush,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };
        private readonly Image _image = new();
        private readonly ImageSelectionRect _selection = new();

        public ImageSelectionRect Selection => _selection;

        protected override void InitializeScrollableContents()
        {
            base.InitializeScrollableContents();
            _background.Children.Add(_image);
            _background.Children.Add(_selection);
            AddScrollableContent(_background);
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();
            RenderOptions.SetBitmapScalingMode(_image, BitmapScalingMode.NearestNeighbor);
            _background.SetBinding(WidthProperty, new Binding(nameof(Width)) { Source = _image });
            _background.SetBinding(HeightProperty, new Binding(nameof(Height)) { Source = _image });
            _selection.SetBinding(ImageSelectionRect.ScaleProperty, new Binding(nameof(ScaleX)) { Source = this });
        }

        private void OnSourceChanged(BitmapSource? value)
        {
            if (value is not null)
            {
                _image.Source = value;
                _selection.OriginalWidth = value.PixelWidth;
                _selection.OriginalHeight = value.PixelHeight;
            }
            else
            {
                _image.Source = null;
                _selection.OriginalWidth = _selection.OriginalHeight = 1;
            }
            UpdateScale();
        }

        private void OnInitialRectChanged(Int32Rect value)
        {
            _selection.SetRect(value);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == PaddingProperty)
            {
                UpdateScale();
            }
        }

        private bool _scale_by_wheel;

        protected override bool ProcessWheel(int delta, bool ctrl, bool shift)
        {
            if (ctrl)
            {
                _scale_by_wheel = true;
                ScaleX = IScaleProperty.ChangeScaleCore(ScaleList, _scale_x, delta);
                _scale_by_wheel = false;
                return true;
            }
            return false;
        }

        protected override void OnScaleXChanged(double oldValue, double newValue)
        {
            var pos = _scale_by_wheel ? Mouse.GetPosition(this) : new(ViewportWidth / 2, ViewportHeight / 2);
            AdjustHorizontalScroll(pos.X, oldValue, newValue);
            AdjustVerticalScroll(pos.Y, oldValue, newValue);
            UpdateScale();
        }

        protected override void OnScaleYChanged(double oldValue, double newValue)
        {
            if (_scale_x != newValue)
            {
                ScaleX = newValue;
            }
        }

        private bool _scale_changing;

        private void OnScaleExpressionChanged(string value)
        {
            if (!_scale_changing)
            {
                var span = value.AsSpan();
                var end = 0;
                for (; end < value.Length; end++)
                {
                    var c = span[end];
                    if (!char.IsDigit(c) && c is not '.' or ',')
                    {
                        break;
                    }
                }
                if (!_scale_changing && double.TryParse(span[..end], out var v))
                {
                    ScaleX = v * 0.01;
                }
            }
        }

        private void UpdateScale()
        {
            var scale = _scale_x;
            var w = _selection.OriginalWidth * scale;
            var h = _selection.OriginalHeight * scale;
            _image.Width = w;
            _image.Height = h;
            ScaleY = scale;
            var padding = Padding;
            ContentWidth = w + padding.Left + padding.Right;
            ContentHeight = h + padding.Top + padding.Bottom;
            Canvas.SetLeft(_background, padding.Left);
            Canvas.SetTop(_background, padding.Top);
            _scale_changing = true;
            ScaleExpression = scale.ToString("0.##%");
            _scale_changing = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            UpdateCursor();
            base.OnMouseMove(e);
        }

        private void UpdateCursor()
        {
            var lastIndex = _cursorIndex;
            if (KeyInput.IsCtrlDown())
            {
                _cursorIndex = 0;
            }
            else
            {
                _cursorIndex = _selection.GetCornerIndex(Mouse.GetPosition(_background));
            }
            if (_cursorIndex != lastIndex)
            {
                Cursor = _cursorIndex switch
                {
                    1 or 9 => Cursors.SizeNESW,
                    2 or 8 => Cursors.SizeNS,
                    3 or 7 => Cursors.SizeNWSE,
                    4 or 6 => Cursors.SizeWE,
                    5 => Cursors.SizeAll,
                    _ => default,
                };
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            var element = _background;
            var selection = _selection;
            element.CaptureMouse();
            var (initX, initY) = Mouse.GetPosition(element);
            var scale = _scale_x;
            int Round(double value) => (value / scale).RoundToInt();
            var initIntX = Round(initX);
            var initIntY = Round(initY);
            var initLeft = selection.Left;
            var initTop = selection.Top;
            var initRight = selection.Right;
            var initBottom = selection.Bottom;
            var selecting = false;
            var index = _cursorIndex;
            var horizontal = index is not (2 or 8);
            var vertical = index is not (4 or 6);
            element.MouseMove += MouseMove;
            element.MouseLeftButtonUp += MouseUp;

            void MouseMove(object sender, MouseEventArgs e)
            {
                e.Handled = true;
                var (x, y) = e.GetPosition(element);
                var dx = x - initX;
                var dy = y - initY;
                if (!selecting)
                {
                    if ((horizontal && Math.Abs(dx) >= MoveThreshold) || (vertical && Math.Abs(dy) >= MoveThreshold))
                    {
                        selecting = true;
                    }
                    else
                    {
                        return;
                    }
                }
                this.AutoScroll(e);
                if (index is 0)
                {
                    selection.SetMovingHorizontal(initIntX, Round(x));
                    selection.SetMovingVertical(initIntY, Round(y));
                }
                else
                {
                    var dxi = Round(dx);
                    var dyi = Round(dy);
                    if (index is 5)
                    {
                        selection.SetMovingHorizontal(initLeft + dxi, initRight + dxi);
                        selection.SetMovingVertical(initTop + dyi, initBottom + dyi);
                    }
                    else
                    {
                        if (index is 1 or 4 or 7)
                        {
                            selection.SetMovingHorizontal(initLeft + dxi, initRight);
                        }
                        else if (index is 3 or 6 or 9)
                        {
                            selection.SetMovingHorizontal(initLeft, initRight + dxi);
                        }
                        if (index is 7 or 8 or 9)
                        {
                            selection.SetMovingVertical(initTop + dyi, initBottom);
                        }
                        else if (index is 1 or 2 or 3)
                        {
                            selection.SetMovingVertical(initTop, initBottom + dyi);
                        }
                    }
                }
            }

            void MouseUp(object sender, MouseButtonEventArgs e)
            {
                e.Handled = true;
                element.ReleaseMouseCapture();
                element.MouseMove -= MouseMove;
                element.MouseLeftButtonUp -= MouseUp;
                UpdateCursor();
                ValueChanged?.Invoke(this, _selectedRect);
            }
        }

        private bool _key_moving;

        protected override bool ProcessKey(Key key, bool ctrl, bool shift)
        {
            if (key is >= Key.Left and <= Key.Down)
            {
                _key_moving = true;
                var amount = shift ? 10 : 1;
                if (key is Key.Left or Key.Right)
                {
                    if (key is Key.Left)
                    {
                        amount = -amount;
                    }
                    _selection.OffsetHorizontal(amount);
                }
                if (key is Key.Up or Key.Down)
                {
                    if (key is Key.Up)
                    {
                        amount = -amount;
                    }
                    _selection.OffsetVertical(amount);
                }
                return true;
            }
            else if (key is Key.LeftCtrl or Key.RightCtrl)
            {
                UpdateCursor();
            }
            return false;
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            if (_key_moving)
            {
                _key_moving = false;
                ValueChanged?.Invoke(this, _selectedRect);
            }
            else if (e.Key is Key.LeftCtrl or Key.RightCtrl)
            {
                UpdateCursor();
            }
            base.OnPreviewKeyUp(e);
        }

        public void AutoScale()
        {
            if (_source is not null)
            {
                var padding = Padding;
                ScaleX = Math.Min(
                    (ViewportWidth - padding.Left - padding.Right) / _selection.OriginalWidth, 
                    (ViewportHeight - padding.Top - padding.Bottom) / _selection.OriginalHeight
                    );
            }
        }
    }
}
