using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls
{
    public sealed partial class ImageRectSelectorView : ScrollViewerBase, IResize
    {
        public static readonly double[] ScaleList = [1.0 / 16, 1.0 / 8, 1.0 / 4, 1.0 / 2, 1, 1.5, 2, 2.5, 3, 4, 6, 8, 12, 16];
        public static IEnumerable<string> ScaleExpressionList => ScaleList.Select(v => v.ToString("0.##%"));

        public static readonly DependencyProperty MoveThresholdProperty = IResize.MoveThresholdProperty.AddOwner(typeof(ImageRectSelectorView));
        public static readonly DependencyProperty SnapDivisionProperty = IResize.SnapDivisionProperty.AddOwner(typeof(ImageRectSelectorView));
        public static readonly DependencyProperty SnapThresholdProperty = IResize.SnapThresholdProperty.AddOwner(typeof(ImageRectSelectorView));

        static ImageRectSelectorView()
        {
            PaddingProperty.OverrideMetadata(typeof(ImageRectSelectorView), PropertyUtils.GetMetaTwoWay(default(Thickness), OnPaddingChanged));
        }

        private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageRectSelectorView)?.UpdateScale();
        }

        public double MoveThreshold { get => (double)GetValue(MoveThresholdProperty); set => SetValue(MoveThresholdProperty, value); }
        public int SnapDivision { get => (int)GetValue(SnapDivisionProperty); set => SetValue(SnapDivisionProperty, value); }
        public double SnapThreshold { get => (double)GetValue(SnapThresholdProperty); set => SetValue(SnapThresholdProperty, value); }

        [DependencyProperty(BindsTwoWayByDefault = true)]
        private BitmapSource? _source;
        [DependencyProperty(SetterScope = Scope.Private)]
        private int _originalWidth = 0;
        [DependencyProperty(SetterScope = Scope.Private)]
        private int _originalHeight = 0;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private int _selectedLeft;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private int _selectedTop;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private int _selectedRight;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private int _selectedBottom;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private int _selectedWidth;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private int _selectedHeight;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private string _aspectRatioExpression = "-";
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private string _scaleExpression = "100%";

        private Rational _aspectRatio = Rational.Zero;
        private bool _needAutoUpdate = true;
        private bool _needAspectRatioUpdate = true;
        private bool _needScaleUpdate = true;
        private bool _scaleChanging;

        private int _cursorIndex;

        private readonly Canvas _background = new()
        {
            Background = MediaUtils.TransparentCheckerBrush,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };
        private readonly Image _image = new();
        private readonly IntRectPresenter _selection = new();

        protected override void InitializeScrollableContents()
        {
            base.InitializeScrollableContents();
            _background.Children.Add(_image);
            _background.Children.Add(_selection);
            AddScrollableContent(_background);
        }

        protected override void InitializeCommands()
        {
            base.InitializeCommands();
            this.RegisterCommand(ApplicationCommands.Copy, OnExecuted_Copy, CanExecute_Image);
            this.RegisterCommand(ApplicationCommands.SelectAll, OnExecuted_SelectAll, CanExecute_Image);
        }

        protected override void InitializeBindings()
        {
            base.InitializeBindings();
            RenderOptions.SetBitmapScalingMode(_image, BitmapScalingMode.NearestNeighbor);
            _background.SetBinding(WidthProperty, new Binding(nameof(Width)) { Source = _image });
            _background.SetBinding(HeightProperty, new Binding(nameof(Height)) { Source = _image });
            var s = _selection;
            s.SetBinding(IntRectPresenter.LeftProperty, new Binding(nameof(SelectedLeft)) { Source = this });
            s.SetBinding(IntRectPresenter.TopProperty, new Binding(nameof(SelectedTop)) { Source = this });
            s.SetBinding(IntRectPresenter.RightProperty, new Binding(nameof(SelectedRight)) { Source = this });
            s.SetBinding(IntRectPresenter.BottomProperty, new Binding(nameof(SelectedBottom)) { Source = this });
            s.SetBinding(IntRectPresenter.ScaleProperty, new Binding(nameof(ScaleX)) { Source = this });
        }

        private void OnSourceChanged(BitmapSource? value)
        {
            if (value is not null)
            {
                _image.Source = value;
                OriginalWidth = value.PixelWidth;
                OriginalHeight = value.PixelHeight;
                AutoScale(true);
            }
            else
            {
                _image.Source = null;
                OriginalWidth = 0;
                OriginalHeight = 0;
            }
            UpdateScale();
        }

        private void ProcessChange(Action action)
        {
            if (_needAutoUpdate)
            {
                _needAutoUpdate = false;
                action();
                UpdateAspectRatio();
                _needAutoUpdate = true;
            }
        }

        private void OnSelectedLeftChanged(int value) => ProcessChange(() => SelectedWidth = _selectedRight - value);
        private void OnSelectedRightChanged(int value) => ProcessChange(() => SelectedWidth = value - _selectedLeft);
        private void OnSelectedTopChanged(int value) => ProcessChange(() => SelectedHeight = _selectedBottom - value);
        private void OnSelectedBottomChanged(int value) => ProcessChange(() => SelectedHeight = value - _selectedTop);
        private void OnSelectedWidthChanged(int value) => ProcessChange(() => SelectedRight = _selectedLeft + value);
        private void OnSelectedHeightChanged(int value) => ProcessChange(() => SelectedBottom = _selectedTop + value);

        public static bool TryParseAspectRatio(string text, out Rational value)
        {
            var span = text.AsSpan();
            value = default;
            if (span.Length is 0)
            {
                return false;
            }
            var delimIndex = span.IndexOf(':');
            if ((uint)delimIndex < (uint)(span.Length - 1) && 
                int.TryParse(span[..delimIndex], out var w) && w is > 0 && 
                int.TryParse(span[(delimIndex + 1)..], out var h) && h is > 0)
            {
                value = new(w, h);
                return true;
            }
            return false;
        }

        private void OnAspectRatioExpressionChanged(string value)
        {
            if (_needAspectRatioUpdate)
            {
                if (TryParseAspectRatio(value, out var ratio))
                {
                    _aspectRatio = ratio;
                }
                else
                {
                    _aspectRatio = Rational.Zero;
                }
            }
        }

        private void OnScaleExpressionChanged(string value)
        {
            if (_needScaleUpdate)
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
                if (double.TryParse(span[..end], out var v))
                {
                    ScaleX = v * 0.01;
                }
            }
        }

        public Int32Rect GetRect() => new(
                Math.Clamp(Math.Min(_selectedLeft, _selectedRight), 0, _originalWidth),
                Math.Clamp(Math.Min(_selectedTop, _selectedBottom), 0, _originalHeight),
                Math.Clamp(Math.Abs(_selectedRight - _selectedLeft), 0, _originalWidth),
                Math.Clamp(Math.Abs(_selectedBottom - _selectedTop), 0, _originalHeight)
                );
        public void SetRect(Int32Rect rect) => SetRect(rect.X, rect.Y, rect.Width, rect.Height);
        public void SetRect(int x, int y, int width, int height)
        {
            ProcessChange(() =>
            {
                SelectedLeft = x;
                SelectedTop = y;
                SelectedRight = x + width;
                SelectedBottom = y + height;
                SelectedWidth = width;
                SelectedHeight = height;
            });
            UpdateAspectRatio();
        }

        private void UpdateAspectRatio()
        {
            if (!_aspectRatio.IsZero())
            {
                return;
            }
            var w = _selectedWidth;
            var h = _selectedHeight;
            string expr;
            if (w is > 0 && h is > 0)
            {
                if (w == h)
                {
                    expr = $"1:1";
                }
                else
                {
                    var (n, d) = Rational.LimitNumDen(w, h, 16);
                    if (w > h)
                    {
                        expr = $"{n}:{(double)h / w * n:0.000}";
                    }
                    else
                    {
                        expr = $"{(double)w / h * d:0.000}:{d}";
                    }
                }
            }
            else
            {
                expr = "-";
            }
            _needAspectRatioUpdate = false;
            AspectRatioExpression = expr;
            _needAspectRatioUpdate = true;
        }

        protected override bool ProcessWheel(int delta, bool ctrl, bool shift)
        {
            if (ctrl)
            {
                _scaleChanging = true;
                ScaleX = IScaleProperty.ChangeScaleCore(ScaleList, _scale_x, delta);
                _scaleChanging = false;
                return true;
            }
            else if (shift)
            {
                ScrollToHorizontalOffset(HorizontalOffset + delta);
                return true;
            }
            return false;
        }

        protected override void OnScaleXChanged(double oldValue, double newValue)
        {
            var padding = Padding;
            var pos = _scaleChanging ? Mouse.GetPosition(this) : new(ViewportWidth / 2, ViewportHeight / 2);
            ScrollToHorizontalOffset(AdjustOffset(HorizontalOffset - padding.Left, pos.X, oldValue, newValue) + padding.Left);
            ScrollToVerticalOffset(AdjustOffset(VerticalOffset - padding.Top, pos.Y, oldValue, newValue) + padding.Top);
            UpdateScale();
        }

        protected override void OnScaleYChanged(double oldValue, double newValue)
        {
            if (_scale_x != newValue)
            {
                ScaleX = newValue;
            }
        }

        private void UpdateScale()
        {
            var scale = _scale_x;
            var w = _originalWidth * scale;
            var h = _originalHeight * scale;
            _image.Width = w;
            _image.Height = h;
            ScaleY = scale;
            var padding = Padding;
            ContentWidth = w + padding.Left + padding.Right;
            ContentHeight = h + padding.Top + padding.Bottom;
            Canvas.SetLeft(_background, padding.Left);
            Canvas.SetTop(_background, padding.Top);
            _needScaleUpdate = false;
            ScaleExpression = scale.ToString("0.##%");
            _needScaleUpdate = true;
            RenderOptions.SetBitmapScalingMode(_image, scale < 1 ? BitmapScalingMode.HighQuality : BitmapScalingMode.NearestNeighbor);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            UpdateCursor();
            base.OnMouseMove(e);
        }

        private void UpdateCursor()
        {
            var index = _selection.GetCornerIndex(Mouse.GetPosition(_background));
            if (_cursorIndex != index)
            {
                _cursorIndex = index;
                Cursor = index switch
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
            var invScale = 1 / _scale_x;

            var rect = GetRect();
            var refSize = (rect.Width, rect.Height);
            var direction = _cursorIndex;
            if (e.ClickCount is > 1 || _cursorIndex is 0)
            {
                refSize = (0, 0);
                direction = 0;
            }
            RectSelectionInfo info = new(
                initialPos: (initX * invScale, initY * invScale),
                initial: (rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height),
                limit: (0, 0, _originalWidth, _originalHeight), 
                refSize: _aspectRatio.IsZero() ? refSize : _aspectRatio,
                direction, MoveThreshold * invScale, SnapDivision, SnapThreshold * invScale, initialSnap: KeyInput.IsCtrlDown(), checkArgs: false);

            element.MouseMove += MouseMove;
            element.MouseLeftButtonUp += MouseUp;

            void MouseMove(object sender, MouseEventArgs e)
            {
                e.Handled = true;
                var (mx, my) = e.GetPosition(element);
                RectSelection.Auto(ref info, mx * invScale, my * invScale, KeyInput.IsShiftDown(), KeyInput.IsCtrlDown());
                if (info.IsMoving)
                {
                    ProcessChange(() =>
                    {
                        SelectedLeft = info.Left.RoundToInt();
                        SelectedTop = info.Top.RoundToInt();
                        SelectedRight = info.Right.RoundToInt();
                        SelectedBottom = info.Bottom.RoundToInt();
                        SelectedWidth = _selectedRight - _selectedLeft;
                        SelectedHeight = _selectedBottom - _selectedTop;
                    });
                    UpdateAspectRatio();
                }
            }

            void MouseUp(object sender, MouseButtonEventArgs e)
            {
                e.Handled = true;
                element.ReleaseMouseCapture();
                element.MouseMove -= MouseMove;
                element.MouseLeftButtonUp -= MouseUp;
                UpdateCursor();
                this.RaiseModifiedEvent(info.IsModified);
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
                    ProcessChange(() =>
                    {
                        SelectedLeft = _selectedLeft + amount;
                        SelectedRight = _selectedRight + amount;
                    });
                }
                if (key is Key.Up or Key.Down)
                {
                    if (key is Key.Up)
                    {
                        amount = -amount;
                    }
                    ProcessChange(() =>
                    {
                        SelectedTop = _selectedTop + amount;
                        SelectedBottom = _selectedBottom + amount;
                    });
                }
                return true;
            }
            return false;
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            if (_key_moving)
            {
                _key_moving = false;
                this.RaiseModifiedEvent(true);
            }
            base.OnPreviewKeyUp(e);
        }

        public void AutoScale(bool shrinkOnly = false)
        {
            if (_source is not null)
            {
                var padding = Padding;
                var scale = Math.Min(
                    (ViewportWidth - padding.Left - padding.Right) / _originalWidth,
                    (ViewportHeight - padding.Top - padding.Bottom) / _originalHeight
                    );
                ScaleX = shrinkOnly ? Math.Min(scale, 1) : scale;
            }
        }

        public void SetOpaqueRect(byte threshold = 0)
        {
            if (_source is BitmapSource source)
            {
                SetRect(source.GetOpaqueRect(threshold));
            }
        }
        public void SetOriginalRect() => SetRect(0, 0, _originalWidth, _originalHeight);

        private void CanExecute_Image(object? sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _source is not null;
        }

        private void OnExecuted_SelectAll(object? sender, ExecutedRoutedEventArgs e)
        {
            if (_source is not null)
            {
                SetOriginalRect();
                e.Handled = true;
            }
        }

        private void OnExecuted_Copy(object? sender, ExecutedRoutedEventArgs e)
        {
            if (_source is not null)
            {
                BitmapSource bitmap;
                var rect = GetRect();
                if (rect.Width is 0)
                {
                    if (rect.Height is 0)
                    {
                        bitmap = _source;
                    }
                    else
                    {
                        bitmap = new CroppedBitmap(_source, new(0, rect.Y, _source.PixelWidth, rect.Height));
                    }
                }
                else if (rect.Height is 0)
                {
                    bitmap = new CroppedBitmap(_source, new(rect.X, 0, rect.Width, _source.PixelHeight));
                }
                else
                {
                    bitmap = new CroppedBitmap(_source, rect);
                }
                Clipboard.SetImage(bitmap);
                e.Handled = true;
            }
        }
    }
}
